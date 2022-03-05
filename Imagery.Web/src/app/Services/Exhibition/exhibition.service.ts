import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AssignTopicVM } from 'src/app/ViewModels/AssignTopicVM';
import { CoverImageVM } from 'src/app/ViewModels/CoverImageVM';
import { ExhibitionCreationVM } from 'src/app/ViewModels/ExhibitionCreationVM';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { FilterVM } from 'src/app/ViewModels/FilterVM';
import { ItemUploadVM } from 'src/app/ViewModels/ItemUploadVM';
import { SignService } from '../Sign/sign.service';

@Injectable({
  providedIn: 'root',
})
export class ExhibitionService {
  url: string = 'https://localhost:44395/Exhibition';

  constructor(private http: HttpClient, private signServices: SignService) {}

  GetAll(): Observable<ExhibitionVM[]> {
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.get<ExhibitionVM[]>(this.url + '/GetAll', options);
  }

  GetSingle(id: number): Observable<ExhibitionVM> {
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.get<ExhibitionVM>(
      this.url + '/GetExhbition/' + id,
      options
    );
  }

  Create(exhibition: ExhibitionCreationVM): any {
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.post<ExhibitionCreationVM>(
      this.url + '/Create',
      exhibition,
      options
    );
  }

  ItemUpload(title: string, item: ItemUploadVM): any {
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
        'Authentication-token': this.signServices.GetToken() + '',
      }),
    };
    return this.http.post<ItemUploadVM>(
      this.url + '/ItemUpload' + `?title=${title}`,
      item,
      options
    );
  }

  Filter(filter: FilterVM): Observable<ExhibitionVM[]> {
    let dateFromString: string;
    let dateToString: string;

    if (filter.dateFrom == null) {
      filter.dateFrom = new Date(new Date().toLocaleDateString());

      dateFromString = this.getDateTimeString(filter.dateFrom);
    } else {
      dateFromString = filter.dateFrom.toString();
    }

    if (filter.dateTo == null) {
      filter.dateTo = new Date(new Date(2050, 1, 1).toLocaleDateString());
      dateToString = this.getDateTimeString(filter.dateTo);
    } else {
      dateToString = filter.dateTo.toString();
    }

    console.log(dateFromString + ' ------------ ' + dateToString);

    const httpParams = new HttpParams({
      fromObject: {
        creatorName: filter.creatorName,
        dateFrom: dateFromString,
        dateTo: dateToString,
      },
    });

    console.log(httpParams);

    return this.http.get<ExhibitionVM[]>(this.url + '/GetByFilter', {
      params: httpParams,
    });
  }

  getDateTimeString(dateTime: Date): string {
    let dateString: string;

    dateTime = new Date(new Date(2050, 1, 1).toLocaleDateString());
    dateString = dateTime.getFullYear().toString() + '-';

    if (dateTime.getMonth() < 9) {
      dateString += '0' + (dateTime.getMonth() + 1).toString() + '-';
    } else {
      dateString +=
        (dateTime.getMonth() + 1).toString() +
        '-' +
        dateTime.getDate().toString();
    }

    if (dateTime.getDate() < 10) {
      dateString += '0' + dateTime.getDate().toString();
    } else {
      dateString += dateTime.getDate().toString();
    }

    if (dateTime.getHours() < 10) {
      dateString += 'T0' + dateTime.getHours().toString() + ':';
    } else {
      dateString += dateTime.getHours().toString();
    }

    if (dateTime.getMinutes() < 10) {
      dateString += '0' + dateTime.getMinutes().toString();
    } else {
      dateString += dateTime.getMinutes().toString();
    }

    return dateString;
  }

  UpdateCover(cover: CoverImageVM): any {
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.post<CoverImageVM>(
      this.url + '/UpadteCoverImage',
      cover,
      options
    );
  }

  Update(exhibition: ExhibitionVM): any {
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.post(this.url + '/Update', exhibition, options);
  }

  AssignTopic(assignTopic: AssignTopicVM) {
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.post(this.url + '/AssignTopic', assignTopic, options);
  }

  GetTopics() {
    let options = {
      headers: new HttpHeaders({
        'Content-Type': 'application/json',
      }),
    };
    return this.http.get(this.url + '/GetTopics', options);
  }
}
