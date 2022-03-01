import { toBase64String } from '@angular/compiler/src/output/source_map';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { PictureVM } from 'src/app/ViewModels/PictureVM';
import { UserVM } from 'src/app/ViewModels/UserVM';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  user!: UserVM;
  image!: File;
  imageURL: string = '';
  imageData: FormData = new FormData();
  constructor(
    private signServices: SignService,
    private imageService: ImageServiceService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadUser();
  }

  loadUser() {
    const prop: string =
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const userName = this.signServices.GetJWTData(prop);

    if (userName === '') {
      this.router.navigateByUrl('Login');
    }

    return this.signServices.GetUser(userName).subscribe((res: any) => {
      if (res !== null) {
        this.user = res;
        this.imageURL = res.picture;
      } else {
        alert('User is not found!');
      }
    });
  }

  fileInput(item: any) {
    if (item?.target?.files.length > 0) {
      this.image = item?.target?.files[0];

      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.imageURL = event.target.result;
      };
      reader.readAsDataURL(this.image);
      console.log(this.image);
    }
  }

  saveImage() {
    this.imageData.append('image', this.image, this.image.name);
    this.imageService
      .UploadProfilePicture(this.user.username, this.imageData)
      .subscribe((res: any) => {
        this.user.picture = res;
        this.imageURL = res;
      });
  }
}
