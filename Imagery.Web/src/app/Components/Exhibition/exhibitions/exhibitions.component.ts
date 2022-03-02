import { Component, OnInit } from '@angular/core';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { FilterVM } from 'src/app/ViewModels/FilterVM';

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
  }

  filterExhibitions() {
    if (this.exhibitions == null) {
      return [];
    }

    return this.exhibitions.filter(
      (exhibition) =>
        exhibition.title.toLowerCase().includes(this.filter.toLowerCase()) &&
        (exhibition.date >= this.filters?.dateFrom ||
          this.filters.dateFrom == null) &&
        (exhibition.date <= this.filters?.dateTo ||
          this.filters.dateTo == null) &&
        (exhibition.averagePrice >= this.filters?.avgPriceMin ||
          this.filters.avgPriceMin == null) &&
        (exhibition.averagePrice <= this.filters?.avgPriceMax ||
          this.filters.avgPriceMax == null)
    );
  }

  getByFilters(filters: FilterVM): void {
    this.exhibitionService.Filter(filters).subscribe((data: any) => {
      this.exhibitions = data;
    });
  }

  filterByExhibitionName() {
    return this.exhibitions.filter((exhibition) =>
      exhibition.title.toLowerCase().includes(this.filter.toLowerCase())
    );
  }
}
