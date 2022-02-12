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

  constructor(private exhibitionService: ExhibitionService) {}

  ngOnInit(): void {
    this.loadExhibitions();
  }

  loadExhibitions() {
    this.exhibitionService
      .GetAll()
      .subscribe((data: any) => (this.exhibitions = data));
  }

  getByFilters(filters: FilterVM): void {
    this.exhibitionService.Filter(filters).subscribe((data: any) => {
      this.exhibitions = data;
    });
  }
}
