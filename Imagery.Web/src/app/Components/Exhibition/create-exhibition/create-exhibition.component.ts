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
  user!: UserVM;
  errorMessage?: string;

  constructor(
    private exhibitionService: ExhibitionService,
    private auth: SignService,
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
        .subscribe(
          (res: any) => {
            this.router.navigate(['EditExhibition', res]);
          },
          (err: any) => {
            alert(err.error.message);
            this.errorMessage = err.error.message;
          }
        );
    } else {
      alert('You are not signed user, please sign in!');
      this.errorMessage = 'You are not signed user, please sign in!';
    }
  }

  getUser() {
    const prop: string =
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const userName = this.auth.GetJWTData(prop);

    // if (userName === '') {
    //   this.router.navigateByUrl('Login');
    // }

    return userName;
  }

  myExhibitions() {
    if (this.getUser() == '') {
      alert('You are not signed user, please sign in!');
      this.errorMessage = 'You are not signed user, please sign in!';
    } else {
      this.router.navigateByUrl('/MyExhibitions');
    }
  }
}
