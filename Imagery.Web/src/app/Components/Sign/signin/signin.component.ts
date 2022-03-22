import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { Router } from '@angular/router';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { SignInVM } from 'src/app/ViewModels/SignInVM';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css'],
})
export class SigninComponent implements OnInit {
  loginDetails: FormGroup = new FormGroup({
    username: new FormControl('', Validators.required),
    password: new FormControl('', Validators.required),
  });

  errorMessage?: string;

  constructor(
    private signServices: SignService,
    private formBuilder: FormBuilder,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loginDetails = this.formBuilder.group({
      username: ['', { validators: [Validators.required] }],
      password: ['', { validators: [Validators.required] }],
    });
  }

  Login() {
    this.signServices.SignIn(this.loginDetails.value as SignInVM).subscribe(
      (data: any) => {
        this.signServices.SaveToken(data);
        this.router.navigateByUrl('');
      },
      (err: any) => {
        this.errorMessage = err.error.message;
      }
    );
  }
}
