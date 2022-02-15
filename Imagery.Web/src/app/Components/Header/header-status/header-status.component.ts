import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { SignService } from 'src/app/Services/Sign/sign.service';

@Component({
  selector: 'app-header-status',
  templateUrl: './header-status.component.html',
  styleUrls: ['./header-status.component.css'],
})
export class HeaderStatusComponent implements OnInit {
  constructor(private singService: SignService, private router: Router) {}

  ngOnInit(): void {}

  isLogged() {
    return this.singService.isAuthenticated();
  }
}
