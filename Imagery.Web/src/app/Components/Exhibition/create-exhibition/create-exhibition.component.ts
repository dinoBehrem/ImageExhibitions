import { Component, OnInit, NgModule } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { ExhibitionCreationVM } from 'src/app/ViewModels/ExhibitionCreationVM';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { UserVM } from 'src/app/ViewModels/UserVM';

@Component({
  selector: 'app-create-exhibition',
  templateUrl: './create-exhibition.component.html',
  styleUrls: ['./create-exhibition.component.css'],
})
export class CreateExhibitionComponent implements OnInit {
  exhibitionDetails!: FormGroup;
  // itemDetails = new FormGroup({
  //   name: new FormControl(''),
  //   creator: new FormControl(''),
  //   imageDescription: new FormControl(''),
  //   dimensions: new FormControl(''),
  //   price: new FormControl(0),
  // });

  user!: UserVM;
  exhibition!: ExhibitionVM;

  constructor(
    private exhibitionService: ExhibitionService,
    private auth: SignService,
    private imageService: ImageServiceService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.exhibitionDetails = this.formBuilder.group({
      title: '',
      description: '',
      startingDate: Date.now,
    });
  }

  save() {
    if (this.auth.isAuthenticated()) {
      let userName = this.getUser();
      this.exhibitionDetails.addControl('organizer', new FormControl(userName));

      this.exhibitionService
        .Create(this.exhibitionDetails.value as ExhibitionCreationVM)
        .subscribe((res: any) => {
          this.exhibition = res;
          this.router.navigate(['EditExhibition', this.exhibition.id]);
        });
    } else {
      alert('You are not signed user, please sign in!');
    }
  }

  // saveImage() {
  //   this.imageData.append('exhbitionId', this.exhibition.id.toString());
  //   this.imageData.append('image', this.image, this.image.name);
  //   this.imageData.append('name', this.name);
  //   this.imageData.append('creator', this.creator);
  //   this.imageData.append('imageDescritpion', this.imageDescription);
  //   this.imageData.append('price', this.price.toString());
  //   this.imageData.append('dimensions', this.dimensions);
  //   this.imageData.append('exhibitionTitle', this.exhibition?.title);

  //   this.imageService
  //     .UploadItemImage(this.getUser(), this.imageData)
  //     .subscribe((res: any) => {
  //       alert(res);
  //     });
  // }

  getUser() {
    const prop: string =
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const userName = this.auth.GetJWTData(prop);

    if (userName === '') {
      this.router.navigateByUrl('Login');
    }

    return userName;
  }
}
