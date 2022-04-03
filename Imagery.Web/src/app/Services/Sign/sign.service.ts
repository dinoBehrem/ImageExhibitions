import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SignUpVM } from 'src/app/ViewModels/SignUpVM';
import { AuthResponse } from 'src/app/ViewModels/AuthResponse';
import { SignInVM } from 'src/app/ViewModels/SignInVM';
import { Observable } from 'rxjs';
import { UserVM } from 'src/app/ViewModels/UserVM';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class SignService {
  url: string = 'https://localhost:44395/User';
  options = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  constructor(private http: HttpClient, private router: Router) {}

  isAuthenticated() {
    const token = this.GetToken();

    if (token === '') {
      return false;
    }

    const expiriation = this.GetExpiriation();
    const expiriationToDate = new Date(expiriation as string);

    if (expiriationToDate <= new Date()) {
      this.SignOut();
      return false;
    }

    return true;
  }

  SignUp(register: SignUpVM): any {
    return this.http.post<SignUpVM>(
      this.url + '/Register',
      register,
      this.options
    );
  }

  SignIn(login: SignInVM): any {
    return this.http.post<SignInVM>(this.url + '/Login', login, this.options);
  }

  SignOut() {
    const tokenKey: string = 'token';
    const expiriation: string = 'token-expiriation';
    localStorage.removeItem(tokenKey);
    localStorage.removeItem(expiriation);

    // this.router.navigateByUrl('Login');
  }

  SaveToken(authResponse: AuthResponse) {
    const tokenKey: string = 'token';
    const expiriation: string = 'token-expiriation';
    localStorage.setItem(tokenKey, authResponse.token);
    localStorage.setItem(expiriation, authResponse.expiriation.toString());
  }

  GetToken() {
    const tokenKey: string = 'token';
    return localStorage.getItem(tokenKey);
  }

  GetExpiriation() {
    const expiriation: string = 'token-expiriation';
    return localStorage.getItem(expiriation);
  }

  GetJWTData(prop: string) {
    const tokenKey: string = 'token';
    const token = localStorage.getItem(tokenKey);

    if (!token || prop === '') {
      return '';
    }

    const dataProps = token.split('.');
    const dataValue = JSON.parse(atob(dataProps[1]));

    return dataValue[prop];
  }

  GetUser(username: string) {
    return this.http.get(this.url + '/GetUser/' + username, this.options);
  }

  GetProfile(username: string) {
    return this.http.get(this.url + '/GetProfile/' + username, this.options);
  }

  EditProfile(username: string, accountData: FormData) {
    return this.http.put(this.url + '/EditAccount/' + username, accountData);
  }

  Subscribre(creator: string): any {
    const claim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const subscriber: string = this.GetJWTData(claim);

    return this.http.post(
      this.url + '/Subscribe',
      { creator, subscriber },
      this.options
    );
  }

  Unsubscribre(creator: string): any {
    const claim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const subscriber: string = this.GetJWTData(claim);

    return this.http.post(
      this.url + '/Unsubscribe',
      { creator, subscriber },
      this.options
    );
  }
}
