import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { ExponentItemVM } from 'src/app/ViewModels/ExponentItemVM';

@Component({
  selector: 'app-edit-item',
  templateUrl: './edit-item.component.html',
  styleUrls: ['./edit-item.component.css'],
})
export class EditItemComponent implements OnInit {
  @Input() editItem?: any;
  @Input() exhId?: number;

  imageData: FormData = new FormData();
  imageURL?: string;
  isChanged: boolean = false;
  isNewItem: boolean = false;
  imagePlaceholder: string = '../../../../../assets/imagePlaceholder.png';
  constructor() {}

  ngOnInit(): void {}

  fileInput(item: any) {
    if (item?.target?.files.length > 0) {
      this.newItem();
      if (!this.isNewItem) {
        this.imageURL = this.editItem.image;
      }

      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.editItem.image = event.target.result;
        this.isChanged = true;
      };
      reader.readAsDataURL(item?.target?.files[0]);
    }
  }

  closeModal() {
    if (this.isChanged) {
      this.editItem.image = this.imageURL;
    }

    this.isChanged = false;
  }

  newItem() {
    if (this.editItem == null) {
      this.isNewItem = true;
    } else {
      this.isNewItem = false;
    }
  }

  imageDisplay() {
    this.newItem();
    if (this.isNewItem) {
      return this.imagePlaceholder;
    }
    return this.editItem?.image;
  }
}
