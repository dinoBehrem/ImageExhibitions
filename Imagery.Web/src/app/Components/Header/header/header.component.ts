import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SignService } from 'src/app/Services/Sign/sign.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  constructor(private signService: SignService, private router: Router) {}
  isLogged: boolean = false;
  isAdmin: boolean = false;

  ngOnInit(): void {
    this.adminPermission();
    this.userIsLogged();
  }

  Logout() {
    this.signService.SignOut();
    this.isLogged = false;
  }

  adminPermission() {
    const claim = 'role';
    const permission: string[] = [...this.signService.GetJWTData(claim)];

    if (permission.includes('Admin') || permission.includes('SuperAdmin')) {
      return true;
    } else {
      return false;
    }
  }

  getUsername() {
    const claim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const username: string = this.signService.GetJWTData(claim);
    return username;
  }

  userIsLogged() {
    return this.signService.isAuthenticated();
  }
}
