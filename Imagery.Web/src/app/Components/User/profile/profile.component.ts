import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { ProfileVM } from 'src/app/ViewModels/ProfileVM';
import { UserVM } from 'src/app/ViewModels/UserVM';
import { MatIcon } from '@angular/material/icon';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css'],
})
export class ProfileComponent implements OnInit {
  profile?: ProfileVM;
  users?: UserVM[];
  exhibitionSubscriptionMessage?: string;
  showExhibitionMessage: boolean = false;

  subCategory: string = 'followers';

  userSubscriptionMessage?: string;
  backgroundColor: string = 'transparent';
  showUserMessage: boolean = false;

  constructor(
    private signService: SignService,
    private exhibitionService: ExhibitionService,
    private route: ActivatedRoute
  ) {}

  sub: any;
  username: string = '';

  ngOnInit(): void {
    this.sub = this.route.params.subscribe((param) => {
      this.username = param['username'];
      if (this.username != '') {
        this.loadProfile();
      }
    });
  }

  loadProfile() {
    this.signService.GetProfile(this.username).subscribe((res: any) => {
      this.profile = res;
      this.setUsers('followers');
    });
  }

  subscribe() {
    this.signService.Subscribre(this.username).subscribe(
      (res: any) => {
        this.userSubscriptionMessage = res.message;
        this.backgroundColor = 'rgb(120, 57, 55)';
      },
      (err: any) => {
        this.userSubscriptionMessage = err?.error?.message;
        if (err.status == 401) {
          this.userSubscriptionMessage = 'You are note logged in!';
        }
        this.backgroundColor = 'rgb(238, 78, 52)';
      }
    );
    this.showUserMessage = true;
    setInterval(() => {
      this.userSubscriptionMessage = '';
      this.showUserMessage = false;
    }, 3000);
  }

  unsubscribe() {
    this.signService.Unsubscribre(this.username).subscribe(
      (res: any) => {
        this.userSubscriptionMessage = res.message;
        this.backgroundColor = 'green';
      },
      (err: any) => {
        this.userSubscriptionMessage = err?.error?.message;
        if (err.status == 401) {
          this.userSubscriptionMessage = 'You are note logged in!';
        }
        this.backgroundColor = 'red';
      }
    );
    this.showUserMessage = true;
    setInterval(() => {
      this.userSubscriptionMessage = '';
      this.showUserMessage = false;
    }, 3000);
  }

  setUsers(type: string) {
    if (type === 'following') {
      this.users = this.profile?.following;
    } else if (type === 'followers') {
      this.users = this.profile?.followers;
    }

    this.subCategory = type;
  }

  exhibitionSubscription(id: number) {
    this.exhibitionService.Subscribre(id).subscribe((res: any) => {
      alert(res);
    });
  }

  getDateTimeString(dateTime: Date): string {
    let dateString: string;
    dateString = dateTime.toString().substring(0, 16);

    dateString = dateString.replace(/T/g, ' ');

    return dateString;
  }

  subscription(id: number) {
    this.exhibitionService.Subscribre(id).subscribe(
      (res: any) => {
        this.exhibitionSubscriptionMessage = res.message;
        this.backgroundColor = 'rgb(120, 57, 55)';
      },
      (err: any) => {
        this.exhibitionSubscriptionMessage = err?.error?.message;
        if (err.status == 401) {
          this.exhibitionSubscriptionMessage = 'You are note logged in!';
        }
        this.backgroundColor = 'rgb(238, 78, 52)';
      }
    );

    this.showExhibitionMessage = true;
    setInterval(() => {
      this.exhibitionSubscriptionMessage = '';
      this.showExhibitionMessage = false;
    }, 3000);
  }

  unsubscription(id: number) {
    this.exhibitionService.Unsubscribre(id).subscribe(
      (res: any) => {
        this.exhibitionSubscriptionMessage = res.message;
        this.backgroundColor = 'rgb(120, 57, 55)';
      },
      (err: any) => {
        this.exhibitionSubscriptionMessage = err?.error?.message;
        if (err.status == 401) {
          this.exhibitionSubscriptionMessage = 'You are note logged in!';
        }
        this.backgroundColor = 'rgb(238, 78, 52)';
      }
    );

    this.showExhibitionMessage = true;
    setInterval(() => {
      this.exhibitionSubscriptionMessage = '';
      this.showExhibitionMessage = false;
    }, 3000);
  }

  displayUsers(): UserVM[] {
    if (this.users == null) {
      return [];
    }
    return this.users;
  }
}
