import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
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
  loginDetails: FormGroup = new FormGroup({
    firstname: new FormControl('', Validators.required),
    lastname: new FormControl('', Validators.required),
    username: new FormControl('', Validators.required),
    email: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  });
  constructor(
    private signServices: SignService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.registrationDetails = this.formBuilder.group({
      firstname: ['', { validators: [Validators.required] }],
      lastname: ['', { validators: [Validators.required] }],
      email: ['', { validators: [Validators.required] }],
      username: ['', { validators: [Validators.required] }],
      password: ['', { validators: [Validators.required] }],
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
