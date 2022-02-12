import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateExhibitionComponent } from './create-exhibition.component';

describe('CreateExhibitionComponent', () => {
  let component: CreateExhibitionComponent;
  let fixture: ComponentFixture<CreateExhibitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CreateExhibitionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(CreateExhibitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
