import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
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

  itemVM: any = {
    image: File,
    name: '',
    creator: '',
    imageDescription: '',
    dimensions: '',
    price: 0.0,
  };

  image!: File;
  name: string = '';
  creator: string = '';
  imageDescription: string = '';
  dimensions: string = '';
  price: number = 0.0;

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

  // save() {
  //   if (this.auth.isAuthenticated()) {
  //     let userName = this.getUser();
  //     this.exhibitionDetails.addControl('organizer', new FormControl(userName));

  //     this.exhibitionService
  //       .Create(this.exhibitionDetails.value as ExhibitionCreationVM)
  //       .subscribe((res: any) => {
  //         this.exhibition = res;
  //         this.router.navigate(['EditExhibition', this.exhibition.id]);
  //       });
  //   } else {
  //     alert('You are not signed user, please sign in!');
  //   }
  // }

  fileInput(item: any) {
    // if (item?.target?.files.length > 0) {
    //   this.image = item?.target?.files[0];

    //   var reader = new FileReader();
    //   reader.onload = (event: any) => {
    //     this.imageURL = event.target.result;
    //   };
    //   reader.readAsDataURL(this.image);
    //   console.log(this.image);
    // }

    if (item?.target?.files.length > 0) {
      this.itemVM.image = item?.target?.files[0];

      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.imageURL = event.target.result;
      };
      reader.readAsDataURL(this.itemVM.image);
      console.log(this.image);
    }
  }

  saveImage() {
    // this.imageData.append('exhibitionId', this.exhibition.id.toString());
    // this.imageData.append('image', this.image, this.image.name);
    // this.imageData.append('name', this.name);
    // this.imageData.append('creator', this.creator);
    // this.imageData.append('imageDescription', this.imageDescription);
    // this.imageData.append('price', this.price.toString());
    // this.imageData.append('dimensions', this.dimensions);

    this.imageData.append('exhibitionId', this.exhibition.id.toString());
    this.imageData.append('image', this.itemVM.image, this.itemVM.image.name);
    this.imageData.append('name', this.itemVM.name);
    this.imageData.append('creator', this.itemVM.creator);
    this.imageData.append('imageDescription', this.itemVM.imageDescription);
    this.imageData.append('price', this.itemVM.price.toString());
    this.imageData.append('dimensions', this.itemVM.dimensions);

    this.imageService
      .UploadItemImage(this.id, this.imageData)
      .subscribe((res: any) => {
        this.exhibition.items.push(res);
        this.clearModal();
      });
  }

  clearModal() {
    this.imageURL = '';
    this.name = '';
    this.creator = '';
    this.imageDescription = '';
    this.price = 0.0;
    this.dimensions = '';
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
    this.imageURL = event.image;
  }
}
