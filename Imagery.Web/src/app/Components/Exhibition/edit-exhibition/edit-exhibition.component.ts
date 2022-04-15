import { Component, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { CoverImageVM } from 'src/app/ViewModels/CoverImageVM';
import { DimensionsVM } from 'src/app/ViewModels/DimensionsVM';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { ExponentItemVM } from 'src/app/ViewModels/ExponentItemVM';
import { TopicVM } from 'src/app/ViewModels/TopicVM';

@Component({
  selector: 'app-edit-exhibition',
  templateUrl: './edit-exhibition.component.html',
  styleUrls: ['./edit-exhibition.component.css'],
})
export class EditExhibitionComponent implements OnInit {
  id: number = -1;
  sub: any;
  exhibitionDetails!: FormGroup;

  exhibition!: ExhibitionVM;
  imageURL: string = '../../../../assets/imagePlaceholder.png';
  imageData: FormData = new FormData();

  topics: TopicVM[] = [];
  selectedTopics: TopicVM[] = [];

  dimensionVM: any;
  newDimensions: any = { price: 0, dimension: '', id: 0 };
  dimensionId: number = -1;

  imageFile: any = File;

  itemVM: any = {
    id: -1,
    image: '',
    name: '',
    creator: '',
    description: '',
    dimensions: [],
  };

  isNewItem: boolean = true;
  change: boolean = false;

  topicVM: any = {
    id: -1,
    name: '',
    isAssigned: false,
  };

  editSuccessMessage: string = '';
  showExhibitionEditMessage: boolean = false;
  backgroundColor: string = 'transparent';

  imagePlaceholder: string = '../../../../assets/imagePlaceholder.png';

  constructor(
    private exhibitionService: ExhibitionService,
    private auth: SignService,
    private imageService: ImageServiceService,
    private formBuilder: FormBuilder,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sub = this.route.params.subscribe((param: any) => {
      this.id = +param['id'];

      this.loadTopics();
      this.loadExhbition();
    });
  }

  loadExhbition() {
    if (this.id == -1) {
      alert('Error, something went wrong!');
      return;
    }

    this.exhibitionService.GetSingle(this.id).subscribe((res: any) => {
      this.exhibition = res;
      this.exhibitionDetails = this.formBuilder.group({
        title: [this.exhibition.title, { validators: [Validators.required] }],
        description: [this.exhibition.description],
        date: [
          this.getDateTimeString(this.exhibition.date),
          { validators: [Validators.required] },
        ],
      });
      this.selectedTopics = res.topics;
      this.configureTopics();
    });
  }

  loadTopics() {
    this.exhibitionService.GetTopics().subscribe((res: any) => {
      this.topics = res;
    });
  }

  fileInput(item: any) {
    if (item?.target?.files.length > 0) {
      this.imageFile = item?.target?.files[0];

      var reader = new FileReader();
      reader.onload = (event: any) => {
        this.imageURL = event.target.result;
        this.change = true;
      };
      reader.readAsDataURL(this.imageFile);
    }
  }

  saveImage() {
    this.imageData.set('exhibitionId', this.exhibition.id.toString());
    this.imageData.set('name', this.itemVM.name);
    if (this.itemVM.creator == '') {
      this.itemVM.creator =
        this.exhibition.organizer.firstname +
        ' ' +
        this.exhibition.organizer.lastname;
    }
    this.imageData.set('creator', this.itemVM.creator);
    this.imageData.set('imageDescription', this.itemVM.description);

    if (this.isNewItem) {
      this.imageData.set('image', this.imageFile, this.imageFile.name);
      this.imageService
        .UploadItemImage(this.id, this.imageData)
        .subscribe((res: any) => {
          this.exhibition.items.push(res);
          this.setItem(res as ExponentItemVM);
          this.isNewItem = false;
          this.change = false;
        });
    } else {
      if (this.change) {
        this.imageData.set('image', this.imageFile, this.imageFile.name);
      }
      this.editItem();
    }
  }

  itemStatus() {
    return this.isNewItem;
  }

  clearModal() {
    this.itemVM = {
      image: File,
      name: '',
      creator: '',
      description: '',
      dimensions: [],
      newItem: false,
    };

    this.imageURL = this.imagePlaceholder;
  }

  getUser() {
    const prop: string =
      'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const userName = this.auth.GetJWTData(prop);

    if (userName === '') {
      this.router.navigateByUrl('Login');
    }

    return userName;
  }

  setItem(event: ExponentItemVM) {
    this.itemVM = event;
    this.imageURL = event.image;
    this.isNewItem = false;
    this.newDimensions = { price: 0, dimension: '', id: 0 };
    this.dimensionVM = { price: 0, dimension: '', id: 0 };
  }

  editItem() {
    return this.imageService
      .EditItem(this.itemVM.id, this.imageData)
      .subscribe((res: any) => {
        this.itemVM.name = res.name;
        this.itemVM.imageDescription = res.imageDescription;
        this.itemVM.creator = res.creator;
        this.itemVM.image = res.imagePath;
        this.imageURL = res.imagePath;

        this.change = false;
      });
  }

  newItem() {
    this.imageURL = this.imagePlaceholder;
    this.itemVM = {
      id: -1,
      image: File,
      name: '',
      creator: '',
      description: '',
      dimensions: [],
    };
    this.newDimensions = { price: 0, dimension: '', id: 0 };
    this.dimensionVM = { price: 0, dimension: '', id: 0 };
    this.isNewItem = true;
    this.change = false;
  }

  removeItem() {
    if (this.itemVM.id != -1) {
      this.imageService.DeleteItem(this.itemVM.id).subscribe((res: any) => {
        if (res.isSuccess) {
          let index = this.exhibition.items.indexOf(
            this.itemVM as ExponentItemVM
          );

          if (index != -1) {
            this.exhibition.items.splice(index, 1);
          }

          this.newItem();
        }
      });
    }
  }

  setCover() {
    let imageCover = {
      exhibitionId: this.exhibition.id,
      coverImage: this.imageURL,
    };

    this.exhibitionService
      .UpdateCover(imageCover as CoverImageVM)
      .subscribe((res: any) => {
        this.exhibition.cover = res.coverImage;
      });
  }

  editDetails() {
    this.exhibitionDetails.addControl(
      'id',
      new FormControl(this.exhibition.id)
    );
    this.exhibitionDetails.addControl(
      'topics',
      new FormControl(this.selectedTopics)
    );

    this.exhibitionService.Update(this.exhibitionDetails.value).subscribe(
      (res: any) => {
        this.exhibition.title = res.title;
        this.exhibition.description = res.description;
        this.exhibition.date = res.date;
        this.selectedTopics = res.topics;

        this.editSuccessMessage = 'Successfully edited!';
        this.backgroundColor = 'rgb(120, 57, 55)';
      },
      (err: any) => {
        console.log(err);
        this.editSuccessMessage = 'Error while editing, try again!';

        if (err.error.errors.hasOwnProperty('Title')) {
          this.editSuccessMessage = err.error.errors['Title'][0];
        } else if (err.error.errors.hasOwnProperty('Id')) {
          this.editSuccessMessage = err.error.errors['Id'][0];
        } else if (err.error.errors.hasOwnProperty('$.date')) {
          this.editSuccessMessage = 'Date is required';
        }

        this.backgroundColor = 'rgb(238, 78, 52)';
      }
    );
    this.showExhibitionEditMessage = true;

    clearInterval();
    setInterval(() => {
      this.editSuccessMessage = '';
      this.showExhibitionEditMessage = false;
    }, 3000);
  }

  addDimensions() {
    console.log(this.itemVM);

    if (this.newDimensions.dimension === '' || this.newDimensions.price < 0) {
      alert('Dimensions value are incorrect!');
    }
    if (
      this.itemVM?.dimensions.length > 0 &&
      this.itemVM?.dimensions.includes(this.newDimensions as DimensionsVM)
    ) {
      alert('Dimensions already exist!');
      return;
    }

    this.imageService
      .AddDimensions(this.itemVM.id, this.newDimensions as DimensionsVM)
      .subscribe((res: any) => {
        this.itemVM.dimensions.push(res);
      });
  }

  removeDimensions() {
    if (this.dimensionId == -1) {
      alert("Dimensions doesn't exist!");
    }

    this.imageService
      .DeleteDimensions(this.dimensionId)
      .subscribe((res: any) => {
        if (res.isSuccess) {
          let index = this.itemVM.dimensions.indexOf(this.dimensionVM);

          if (index != -1) {
            this.itemVM.dimensions.splice(index, 1);
          }
          this.dimensionId = -1;
          this.dimensionVM = null;
        }
      });
  }

  dimensionsPrice() {
    this.itemVM?.dimensions.forEach((dimen: any) => {
      if (dimen.id == this.dimensionId) {
        this.dimensionVM = dimen as DimensionsVM;
      }
    });
  }

  getDateTimeString(dateTime: Date): string {
    let dateString: string;
    dateString = dateTime.toString().substring(0, 16);

    return dateString;
  }

  configureTopics() {
    this.topics.forEach((topic) => {
      this.selectedTopics.forEach((selected) => {
        if (topic.name == selected.name) {
          topic.isAssigned = true;
        }
      });
    });
  }

  addTopic(topic: TopicVM) {
    let exist = false;
    this.selectedTopics.forEach((selectedTopic, index) => {
      if (topic.id == selectedTopic.id) {
        topic.isAssigned = false;
        this.selectedTopics.splice(index, 1);
        exist = true;
      }
    });

    if (!exist) {
      topic.isAssigned = true;
      this.selectedTopics.push(topic);
    }
  }
}
