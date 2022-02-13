import { Component, OnInit, NgModule } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
} from '@angular/forms';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { ExhibitionCreationVM } from 'src/app/ViewModels/ExhibitionCreationVM';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';

@Component({
  selector: 'app-create-exhibition',
  templateUrl: './create-exhibition.component.html',
  styleUrls: ['./create-exhibition.component.css'],
})
export class CreateExhibitionComponent implements OnInit {
  exhibitionDetails!: FormGroup;
  itemDetails = new FormGroup({
    name: new FormControl(''),
    creator: new FormControl(''),
    imageDescription: new FormControl(''),
    dimensions: new FormControl(''),
    price: new FormControl(0),
  });
  // exhibition: ExhibitionVM;
  // exhTitle: string;

  constructor(
    private exhibitionService: ExhibitionService,
    private auth: SignService,
    private formBuilder: FormBuilder
  ) {}

  ngOnInit(): void {
    this.exhibitionDetails = this.formBuilder.group({
      title: '',
      description: '',
      startingDate: Date.now,
    });
  }

  create() {
    if (this.auth.isAuthenticated()) {
      this.exhibitionService.Create(
        this.exhibitionDetails.value as ExhibitionCreationVM
      );
    } else {
      alert('You are not signed user, please sign in!');
    }
  }
}
