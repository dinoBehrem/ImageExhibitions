import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { FilterVM } from 'src/app/ViewModels/FilterVM';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.css'],
})
export class FilterComponent implements OnInit {
  params!: FormGroup;
  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.params = this.formBuilder.group({
      creatorName: '',
      dateFrom: null,
      dateTo: null,
    });
  }
  @Output() filters = new EventEmitter<FilterVM>();

  sendFilters() {
    this.filters.emit(this.params.value as FilterVM);
  }
}
