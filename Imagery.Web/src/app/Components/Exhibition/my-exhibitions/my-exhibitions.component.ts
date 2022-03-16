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

  exhibitionVM!: ExhibitionVM;
  exhibitions: ExhibitionVM[] = [];
  username: string = '';
  imagePlaceholder: string = '../../../../assets/imagePlaceholder.png';

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

  deleteExhbition(exhibition: ExhibitionVM) {
    if (!exhibition.expired) {
      alert("Can't delete expired exhibition!");
      return;
    }

    console.log(this.exhibitions.indexOf(exhibition) + ' ' + exhibition.id);

    this.exhibitnioService
      .RemoveExhbition(exhibition.id)
      .subscribe((res: any) => {
        if (res.isSuccess) {
          let index = this.exhibitions.indexOf(exhibition);
          if (index != -1) {
            this.exhibitions.splice(index, 1);
          }
        }

        alert(res.message);
      });
  }

  setExhibition(exhibition: ExhibitionVM) {
    this.exhibitionVM = exhibition;
  }
}
