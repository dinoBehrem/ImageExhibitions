import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { UserVM } from 'src/app/ViewModels/UserVM';

@Component({
  selector: 'app-subscriptions',
  templateUrl: './subscriptions.component.html',
  styleUrls: ['./subscriptions.component.css'],
})
export class SubscriptionsComponent implements OnInit {
  @Input() subscriptions?: UserVM[] = [];

  constructor(private signService: SignService) {}

  ngOnInit(): void {}

  subscribe(username: string) {
    this.signService.Subscribre(username).subscribe((res: any) => {
      alert(res.message);
    });
  }
}
