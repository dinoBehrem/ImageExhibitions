import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { SignUpVM } from 'src/app/ViewModels/SignUpVM';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent implements OnInit {
  registrationDetails!: FormGroup;
  constructor(
    private signServices: SignService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registrationDetails = this.formBuilder.group({
      firstname: '',
      lastname: '',
      username: '',
      email: '',
      password: '',
    });
  }

  Register() {
    this.registrationDetails = this.signServices
      .SignUp(this.registrationDetails.value as SignUpVM)
      .subscribe((data: any) => {
        this.router.navigateByUrl('Login');
      });
  }
}
