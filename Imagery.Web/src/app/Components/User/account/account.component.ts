import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { ProfileVM } from 'src/app/ViewModels/ProfileVM';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css'],
})
export class AccountComponent implements OnInit {
  user!: ProfileVM;
  image!: File;
  oldImage: string = '';
  imageData: FormData = new FormData();
  imageFile: any = File;
  change: boolean = false;

  userDetails!: FormGroup;

  constructor(
    private signServices: SignService,
    private imageService: ImageServiceService,
    private formBuilder: FormBuilder,
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

    this.signServices.GetUser(userName).subscribe((res: any) => {
      if (res !== null) {
        this.user = res;
        this.userDetails = this.formBuilder.group({
          firstName: [
            this.user?.firstName,
            { validators: [Validators.required] },
          ],
          lastName: [
            this.user?.lastName,
            { validators: [Validators.required] },
          ],
          email: [this.user?.email, { validators: [Validators.required] }],
          phone: [this.user?.phone],
          biography: [this.user?.biography],
        });
      } else {
        alert('User is not found!');
      }
    });
  }

  fileInput(item: any) {
    if (item?.target?.files.length > 0) {
      this.imageFile = item?.target?.files[0];

      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.oldImage = this.user.profilePicture;
        this.user.profilePicture = event.target.result;
        this.change = true;
      };
      reader.readAsDataURL(this.imageFile);
    }
  }

  discardImage() {
    this.user.profilePicture = this.oldImage;
    this.change = false;
  }

  saveImage() {
    this.imageData.append('image', this.imageFile, this.imageFile.name);

    this.imageService
      .UploadProfilePicture(this.user.userName, this.imageData)
      .subscribe((res: any) => {
        this.user.profilePicture = res;
      });
  }

  saveChanges() {
    console.log(this.userDetails.value);
    this.signServices
      .EditProfile(this.user.userName, this.userDetails.value)
      .subscribe((res: any) => {
        this.user.firstName = res.firstname;
        this.user.lastName = res.lastname;
        this.user.email = res.email;
        this.user.phone = res.phone;
        this.user.biography = res.biography;
      });
  }
}
