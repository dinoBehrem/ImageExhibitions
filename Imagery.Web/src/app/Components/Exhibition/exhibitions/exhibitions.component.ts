import { Component, OnInit } from '@angular/core';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { FilterVM } from 'src/app/ViewModels/FilterVM';
import { TopicVM } from 'src/app/ViewModels/TopicVM';
import { PageEvent } from '@angular/material/paginator';

@Component({
  selector: 'app-exhibitions',
  templateUrl: './exhibitions.component.html',
  styleUrls: ['./exhibitions.component.css'],
})
export class ExhibitionsComponent implements OnInit {
  exhibitions: ExhibitionVM[] = [];
  imagePath: string = '../../../../../pexels.jpg';
  filters: any = {
    dateFrom: null,
    dateTo: null,
    avgPriceMin: null,
    avgPriceMax: null,
    topics: null,
    creatorName: null,
    description: null,
  };
  showExhibitionMessage: boolean = false;
  backgroundColor: string = '';
  exhibitionSubscriptionMessage: string = '';
  filter: string = '';

  length: number = 0;
  pageSize: number = 10;
  pageIndex: number = 1;

  constructor(private exhibitionService: ExhibitionService) {}

  ngOnInit(): void {
    this.loadExhibitions();
  }

  loadExhibitions() {
    this.exhibitionService
      .GetPagedExhibition(this.pageIndex, this.pageSize)
      .subscribe(
        (res: any) => {
          this.length = res.count;
          this.exhibitions = res.exhibitions;
        },
        (err: any) => {
          console.log(err);
        }
      );
  }

  filterByExhibitionName() {
    if (this.filter == '') {
      this.loadExhibitions();
    } else {
      this.exhibitionService
        .FilterByTitle(this.filter)
        .subscribe((res: any) => {
          this.exhibitions = res.exhibitions;
          this.length = res.count;
        });
    }
  }

  subscription(id: number) {
    this.exhibitionService.Subscribre(id).subscribe(
      (res: any) => {
        this.exhibitionSubscriptionMessage = res.message;
        this.backgroundColor = 'rgb(120, 57, 55)';
      },
      (err: any) => {
        console.log(err);
        this.exhibitionSubscriptionMessage = err?.error?.message;
        if (err.status == 401) {
          this.exhibitionSubscriptionMessage = 'You are not logged in!';
        }
        // this.backgroundColor = 'rgb(238, 78, 52)';
        this.backgroundColor = 'rgb(255, 0, 0)';
      }
    );

    this.showExhibitionMessage = true;
    setInterval(() => {
      this.exhibitionSubscriptionMessage = '';
      this.showExhibitionMessage = false;
    }, 3000);
  }

  getDateTimeString(dateTime: Date): string {
    let dateString: string;

    dateString = dateTime.toString().substring(0, 16);

    dateString = dateString.replace(/T/g, ' ');

    return dateString;
  }

  paginator(pageEvent: PageEvent) {
    this.length = pageEvent.length;
    this.pageSize = pageEvent.pageSize;
    this.pageIndex = pageEvent.pageIndex + 1;
    this.getByFilters(this.filters);
  }

  getByFilters(filters: FilterVM) {
    this.filters = filters;
    this.exhibitionService
      .Filter(filters, this.pageIndex, this.pageSize)
      .subscribe((res: any) => {
        this.exhibitions = res.exhibitions;
        this.length = res.count;
      });

    this.pageIndex = 1;
  }
}
