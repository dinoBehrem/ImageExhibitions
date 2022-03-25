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
    topics: [],
    creatorName: '',
    description: '',
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
    this.exhibitionsCount();
    this.loadExhibitions();
  }

  loadExhibitions() {
    // this.exhibitionService
    //   .GetAll()
    //   .subscribe((data: any) => (this.exhibitions = data));

    this.exhibitionService
      .GetPagedExhibition(this.pageIndex, this.pageSize)
      .subscribe(
        (res: any) => {
          this.exhibitions = res;
        },
        (err: any) => {
          console.log(err);
        }
      );
  }

  setFilters(filters: FilterVM) {
    this.filters = filters;
    this.filterExhibitions();
  }

  filterExhibitions() {
    if (this.exhibitions == null) {
      return [];
    }

    return this.exhibitions.filter(
      (exhibition) =>
        this.checkForText(exhibition.title, this.filter) &&
        (exhibition.date >= this.filters?.dateFrom ||
          this.filters?.dateFrom == null) &&
        (exhibition.date <= this.filters?.dateTo ||
          this.filters?.dateTo == null) &&
        (exhibition.averagePrice >= this.filters?.avgPriceMin ||
          this.filters.avgPriceMin == null) &&
        (exhibition.averagePrice <= this.filters?.avgPriceMax ||
          this.filters.avgPriceMax == null) &&
        (this.checkForText(
          exhibition.organizer.firstname + ' ' + exhibition.organizer.lastname,
          this.filters.creatorName
        ) ||
          this.filters.creatorName == '') &&
        (this.checkForText(exhibition.description, this.filters.description) ||
          this.filters.description == '') &&
        (this.checkForTopic(exhibition.topics, this.filters.topics) ||
          this.filters.topics?.length == 0)
    );
  }

  checkForText(content: string, keyword: string) {
    return content.toLowerCase().includes(keyword.toLowerCase());
  }

  checkForTopic(exhibitionTopics: TopicVM[], topics: TopicVM[]) {
    if (
      exhibitionTopics == null ||
      exhibitionTopics.length == 0 ||
      topics == null ||
      topics.length == 0
    ) {
      return false;
    }

    let containes = false;

    for (let i = 0; i < topics.length; i++) {
      for (let j = 0; j < exhibitionTopics.length; j++) {
        if (exhibitionTopics[j].id == topics[i].id) {
          containes = true;
          i = topics.length;
          j = exhibitionTopics.length;
        }
      }
    }

    return containes;
  }

  filterByExhibitionName() {
    this.loadExhibitions();
    return this.exhibitions.filter((exhibition) =>
      exhibition.title.toLowerCase().includes(this.filter.toLowerCase())
    );
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

    console.log(pageEvent);
    this.loadExhibitions();
  }

  exhibitionsCount() {
    this.exhibitionService.GetTotalPageCount().subscribe((res: any) => {
      this.length = res;
      console.log(this.length);
    });
  }
}
