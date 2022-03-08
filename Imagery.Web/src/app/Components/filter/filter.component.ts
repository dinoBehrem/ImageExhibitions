import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { FilterVM } from 'src/app/ViewModels/FilterVM';
import { TopicVM } from 'src/app/ViewModels/TopicVM';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css'],
})
export class FilterComponent implements OnInit {
  params!: FormGroup;
  constructor(
    private formBuilder: FormBuilder,
    private exhibitionService: ExhibitionService
  ) {}

  topics: TopicVM[] = [];

  ngOnInit(): void {
    this.loadTopics();
    this.params = this.formBuilder.group({
      creatorName: '',
      dateFrom: null,
      dateTo: null,
      avgPriceMax: null,
      avgPriceMin: null,
    });
  }
  @Output() filters = new EventEmitter<FilterVM>();

  sendFilters() {
    this.filters.emit(this.params.value as FilterVM);
  }

  resetFilters() {
    this.params = this.formBuilder.group({
      creatorName: '',
      dateFrom: null,
      dateTo: null,
      avgPriceMax: null,
      avgPriceMin: null,
    });

    this.sendFilters();
  }

  loadTopics() {
    this.exhibitionService.GetTopics().subscribe((res: any) => {
      this.topics = res;
    });
  }

  test(topic: TopicVM) {
    console.log(topic.name);
  }
}
