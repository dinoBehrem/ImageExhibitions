import { Component, OnInit } from '@angular/core';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';

@Component({
  selector: 'app-my-exhibitions',
  templateUrl: './my-exhibitions.component.html',
  styleUrls: ['./my-exhibitions.component.css'],
})
export class MyExhibitionsComponent implements OnInit {
  constructor(
    private exhibitnioService: ExhibitionService,
    private signService: SignService
  ) {}

  exhibitions: ExhibitionVM[] = [];
  username: string = '';
  ngOnInit(): void {
    this.username = this.getUsername();
    this.loadExhibitions();
  }

  loadExhibitions() {
    this.exhibitnioService
      .GetUserExhibitions(this.username)
      .subscribe((res: any) => {
        this.exhibitions = res;
      });
  }

  getUsername() {
    const claim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const username: string = this.signService.GetJWTData(claim);

    if (username === '') {
      // this.router.navigateByUrl('Login');
      return '';
    }
    return username;
  }
}
