import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyExhibitionsComponent } from './my-exhibitions.component';

describe('MyExhibitionsComponent', () => {
  let component: MyExhibitionsComponent;
  let fixture: ComponentFixture<MyExhibitionsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MyExhibitionsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(MyExhibitionsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
