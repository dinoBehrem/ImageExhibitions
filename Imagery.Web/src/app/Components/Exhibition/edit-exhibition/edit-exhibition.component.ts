import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { CoverImageVM } from 'src/app/ViewModels/CoverImageVM';
import { DimensionsVM } from 'src/app/ViewModels/DimensionsVM';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { ExponentItemVM } from 'src/app/ViewModels/ExponentItemVM';

@Component({
  selector: 'app-edit-exhibition',
  templateUrl: './edit-exhibition.component.html',
  styleUrls: ['./edit-exhibition.component.css'],
})
export class EditExhibitionComponent implements OnInit {
  id: number = -1;
  sub: any;

  exhibition!: ExhibitionVM;
  exhibitionDetails!: FormGroup;
  imageURL: string = '';
  imageData: FormData = new FormData();

  dimensionVM = { price: 0, dimension: '' };

  itemVM: any = {
    id: -1,
    image: File,
    name: '',
    creator: '',
    description: '',
    dimensions: [],
    newItem: true,
  };

  private imagePlaceholder: string = '../../../assets/imagePlaceholder.png';

  constructor(
    private exhibitionService: ExhibitionService,
    private auth: SignService,
    private imageService: ImageServiceService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sub = this.route.params.subscribe((param: any) => {
      this.id = +param['id'];

      this.loadExhbition();
    });
  }

  loadExhbition() {
    if (this.id == -1) {
      alert('Error, something went wrong!');
      return;
    }

    this.exhibitionService.GetSingle(this.id).subscribe((res: any) => {
      this.exhibition = res;
    });
  }

  fileInput(item: any) {
    if (item?.target?.files.length > 0) {
      this.itemVM.image = item?.target?.files[0];

      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.imageURL = event.target.result;
      };
      reader.readAsDataURL(this.itemVM.image);
    }
  }

  saveImage() {
    this.imageData.append('exhibitionId', this.exhibition.id.toString());
    this.imageData.append('image', this.itemVM.image, this.itemVM.image.name);
    this.imageData.append('name', this.itemVM.name);
    this.imageData.append('creator', this.itemVM.creator);
    this.imageData.append('imageDescription', this.itemVM.description);
    // this.imageData.append('dimensions', JSON.stringify(this.itemVM.dimensions));
    for (let index = 0; index < this.itemVM.dimensions.length; index++) {
      this.imageData.append(
        `dimensions[${index}]`,
        JSON.stringify(this.itemVM.dimensions[index])
      );
    }

    this.imageService
      .UploadItemImage(this.id, this.imageData)
      .subscribe((res: any) => {
        this.exhibition.items.push(res);
        this.clearModal();
      });
  }

  clearModal() {
    this.itemVM = {
      image: File,
      name: '',
      creator: '',
      description: '',
      dimensions: [],
      newItem: false,
    };

    this.imageURL = this.imagePlaceholder;
  }

  getUser() {
    const prop: string =
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const userName = this.auth.GetJWTData(prop);

    if (userName === '') {
      this.router.navigateByUrl('Login');
    }

    return userName;
  }

  setItem(event: ExponentItemVM) {
    this.itemVM = event;
    this.itemVM.newItem = false;
    this.imageURL = event.image;

    console.log(this.itemVM);
  }

  newItem() {
    this.imageURL = this.imagePlaceholder;
    this.itemVM = {
      image: File,
      name: '',
      creator: '',
      description: '',
      dimensions: [],
      newItem: true,
    };
  }

  setCover() {
    let imageCover = {
      exhibitionId: this.exhibition.id,
      coverImage: this.imageURL,
    };

    this.exhibitionService
      .UpdateCover(imageCover as CoverImageVM)
      .subscribe((res: any) => {
        alert(res);
        this.exhibition.cover = imageCover.coverImage;
      });
  }

  editDetails() {
    this.exhibitionService.Update(this.exhibition).subscribe((res: any) => {
      this.exhibition = res;
    });
  }

  addDimensions() {
    if (this.dimensionVM.dimension === '' || this.dimensionVM.price < 0) {
      alert('Dimensions value are incorrect!');
    }
    if (this.itemVM.dimensions.includes(this.dimensionVM)) {
      alert('Dimensions already exist!');
      return;
    }

    this.imageService
      .AddDimensions(this.itemVM.id, this.dimensionVM as DimensionsVM)
      .subscribe((res: any) => {
        this.itemVM.dimensions.push(res);
      });
  }
}
