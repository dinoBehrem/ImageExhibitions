import { Component, OnInit } from '@angular/core';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { MyExhibitionVM } from 'src/app/ViewModels/MyExhibitionVM';

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

  exhibitionVM!: MyExhibitionVM;
  exhibitions: MyExhibitionVM[] = [];
  imagePlaceholder: string = '../../../../assets/imagePlaceholder.png';
  dateString: string = '';

  ngOnInit(): void {
    this.loadExhibitions();
  }

  loadExhibitions() {
    this.exhibitnioService
      .GetMyExhibitions(this.getUsername())
      .subscribe((res: any) => {
        this.exhibitions = res.exhibitions;
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

  deleteExhbition(exhibition: MyExhibitionVM) {
    if (exhibition.expired) {
      alert("Can't delete expired exhibition!");
      return;
    }

    if (exhibition.started) {
      alert("Can't delete active exhibition!");
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

  setExhibition(exhibition: MyExhibitionVM) {
    this.dateString = this.getDateTimeString(exhibition.date);
    this.exhibitionVM = exhibition;
  }

  getDateTimeString(dateTime: Date): string {
    let dateString: string;

    dateString = dateTime.toString().substring(0, 16);

    dateString = dateString.replace(/T/g, ' ');

    return dateString;
  }

  disableButton() {
    if (this.exhibitionVM?.started && this.exhibitionVM != null) {
      return true;
    } else {
      return false;
    }
  }
}
