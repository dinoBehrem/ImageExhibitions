import { Component, OnInit } from '@angular/core';
import { SignService } from 'src/app/Services/Sign/sign.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent implements OnInit {
  constructor(private signService: SignService) {}
  isLogged: boolean = this.signService.GetToken() == '';

  ngOnInit(): void {}

  Logout() {
    this.signService.SignOut();
    this.isLogged = false;
  }
}
