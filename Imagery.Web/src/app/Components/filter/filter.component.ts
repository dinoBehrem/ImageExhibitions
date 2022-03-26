import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup } from '@angular/forms';
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
  selectedTopics: string[] = [];

  ngOnInit(): void {
    this.loadTopics();
    this.params = this.formBuilder.group({
      creatorName: '',
      dateFrom: null,
      dateTo: null,
      avgPriceMax: null,
      avgPriceMin: null,
      description: '',
    });
  }
  @Output() filters = new EventEmitter<FilterVM>();

  sendFilters() {
    this.params.addControl('topics', new FormControl(this.selectedTopics));

    this.filters.emit(this.params.value as FilterVM);
  }

  resetFilters() {
    this.params = this.formBuilder.group({
      creatorName: '',
      dateFrom: null,
      dateTo: null,
      avgPriceMax: null,
      avgPriceMin: null,
      description: '',
    });
    // this.topics.forEach((topic) => (topic.isAssigned = false));
    this.loadTopics();
    this.selectedTopics = [];
    this.sendFilters();
  }

  loadTopics() {
    this.exhibitionService.GetTopics().subscribe((res: any) => {
      this.topics = res;
    });
  }

  addTopic(topic: TopicVM) {
    if (!topic.isAssigned) {
      topic.isAssigned = !topic.isAssigned;
      if (this.selectedTopics.includes(topic.name)) {
        return;
      }

      this.selectedTopics.push(topic.name);
    } else {
      let index = this.selectedTopics.indexOf(topic.name);

      if (index === -1) {
        return;
      }

      topic.isAssigned = !topic.isAssigned;
      this.selectedTopics.splice(index, 1);
    }
  }
}
