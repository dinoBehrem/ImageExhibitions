import { Component, OnInit } from '@angular/core';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { FilterVM } from 'src/app/ViewModels/FilterVM';
import { TopicVM } from 'src/app/ViewModels/TopicVM';

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

  filter: string = '';

  constructor(private exhibitionService: ExhibitionService) {}

  ngOnInit(): void {
    this.loadExhibitions();
  }

  loadExhibitions() {
    this.exhibitionService
      .GetAll()
      .subscribe((data: any) => (this.exhibitions = data));
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

  hasStarted(date: Date) {
    if (date > new Date()) {
      return false;
    }

    return true;
  }
}
