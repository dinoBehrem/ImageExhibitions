import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EditExhibitionComponent } from './edit-exhibition.component';

describe('EditExhibitionComponent', () => {
  let component: EditExhibitionComponent;
  let fixture: ComponentFixture<EditExhibitionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EditExhibitionComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(EditExhibitionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
