import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { CollectionItemVM } from 'src/app/ViewModels/CollectionItemVM';
import { DimensionsVM } from 'src/app/ViewModels/DimensionsVM';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { ExponentItemVM } from 'src/app/ViewModels/ExponentItemVM';

@Component({
  selector: 'app-exhibition',
  templateUrl: './exhibition.component.html',
  styleUrls: ['./exhibition.component.css'],
})
export class ExhibitionComponent implements OnInit {
  exhibition!: ExhibitionVM;
  id: number = -1;
  sub: any;

  // itemVM: any = {
  //   image: '',
  //   name: '',
  //   creator: '',
  //   description: '',
  //   dimensions: '',
  //   price: 0.0,
  // };

  itemVM!: ExponentItemVM;

  dimensionVM: any;
  dimensionId: number = -1;
  imageURL: string = '../../../../assets/imagePlaceholder.png';

  constructor(
    private exhibitionService: ExhibitionService,
    private imageService: ImageServiceService,
    private signService: SignService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sub = this.route.params.subscribe((param: any) => {
      this.id = +param['id'];

      this.loadExhibition();
      this.loadItem();
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  loadExhibition() {
    if (this.id === -1) {
      this.router.navigateByUrl('');
    }
    this.exhibitionService.GetSingle(this.id).subscribe((res: any) => {
      this.exhibition = res;
    });
  }

  loadItem() {
    this.itemVM = this.exhibition?.items[0];
  }

  setItem(event: ExponentItemVM) {
    this.itemVM = event;
    this.imageURL = event.image;
    if (this.itemVM.dimensions.length > 0) {
      this.dimensionVM = this.itemVM.dimensions[0];
      this.dimensionId = this.itemVM.dimensions[0].id;
    } else {
      this.dimensionVM = null;
      this.dimensionId = -1;
    }
  }

  addToCart() {
    if (this.itemVM == undefined) {
      alert('Please select image!');
      return;
    }

    if (this.dimensionId == -1) {
      alert('Please select dimensions!');
      return;
    }

    if (!this.getUsername()) {
      alert('You are not logged in!');

      return;
    }

    let collectionItem = {
      image: this.itemVM.image,
      name: this.itemVM.name,
      description: this.itemVM.description,
      price: this.dimensionVM.price,
      dimensions: this.dimensionVM.dimension,
      exhibition: this.exhibition.title,
      organizer: this.exhibition.organizer.username,
      creator: this.itemVM.creator,
      customer: this.getUsername(),
    };

    this.imageService
      .AddCollectionItem(collectionItem as CollectionItemVM)
      .subscribe((res: any) => {
        alert(res.message);

        this.dimensionVM = null;
        this.dimensionId = -1;
      });
  }

  getUsername() {
    const claim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const username: string = this.signService.GetJWTData(claim);

    if (username === '') {
      // this.router.navigateByUrl('Login');
      return '';
    }
    return username;
  }

  dimensionsPrice() {
    this.itemVM?.dimensions.forEach((dimen: any) => {
      if (dimen.id == this.dimensionId) {
        this.dimensionVM = dimen as DimensionsVM;
      }
    });
    console.log(this.dimensionVM);
  }
}
