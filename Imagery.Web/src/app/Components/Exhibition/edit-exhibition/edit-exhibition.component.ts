import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { CoverImageVM } from 'src/app/ViewModels/CoverImageVM';
import { DimensionsVM } from 'src/app/ViewModels/DimensionsVM';
import { EditExhibitionVM } from 'src/app/ViewModels/EditExhibitionVM';
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

  exhibition!: ExhibitionVM;
  editExhibition!: EditExhibitionVM;
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

  imagePlaceholder: string = '../../../../assets/imagePlaceholder.png';

  constructor(
    private exhibitionService: ExhibitionService,
    private auth: SignService,
    private imageService: ImageServiceService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sub = this.route.params.subscribe((param: any) => {
      this.id = +param['id'];

      this.loadExhbition();
    });

    this.loadTopics();
  }

  loadExhbition() {
    if (this.id == -1) {
      alert('Error, something went wrong!');
      return;
    }

    this.exhibitionService.GetSingle(this.id).subscribe((res: any) => {
      this.exhibition = res;
      this.selectedTopics = res.topics;
    });
  }

  loadTopics() {
    this.exhibitionService.GetTopics().subscribe((res: any) => {
      this.topics = res;

      this.topics.forEach((topic) => {
        let index = false;

        this.selectedTopics.forEach((stopic) => {
          if (stopic.id == topic.id) {
            index = true;
          }
        });

        if (index) {
          topic.isAssigned = true;
          // console.log('------');
        }
      });
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
    this.imageData.append('exhibitionId', this.exhibition.id.toString());
    this.imageData.append('name', this.itemVM.name);
    this.imageData.append('creator', this.itemVM.creator);
    this.imageData.append('imageDescription', this.itemVM.description);

    this.itemStatus();

    if (this.isNewItem) {
      this.imageData.append('image', this.imageFile, this.imageFile.name);
      this.imageService
        .UploadItemImage(this.id, this.imageData)
        .subscribe((res: any) => {
          this.exhibition.items.push(res);
          this.setItem(res as ExponentItemVM);
          this.isNewItem = false;
        });
      console.log('New image');
    } else {
      if (this.change) {
        this.imageData.append('image', this.imageFile, this.imageFile.name);
      }
      this.editItem();
      console.log('Edit image');
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

        console.log(res.isSuccess);
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
    this.editExhibition = {
      id: this.exhibition.id,
      title: this.exhibition.title,
      description: this.exhibition.description,
      date: this.exhibition.date,
    };

    this.exhibitionService.Update(this.editExhibition).subscribe((res: any) => {
      this.exhibition.title = res.title;
      this.exhibition.description = res.description;
      this.exhibition.date = res.date;
    });
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

  addTopic() {
    this.topicVM = this.topics.filter((topic) => topic.id == this.topicVM.id);
    console.log(this.topicVM);
    let existAsFalse = this.exhibition.topics.includes(
      this.topicVM[0] as TopicVM
    );
    let index = this.exhibition.topics.indexOf(this.topicVM[0]);

    this.topicVM[0].isAssigned = true;
    let existAsTrue = this.exhibition.topics.includes(
      this.topicVM[0] as TopicVM
    );

    if (existAsTrue) {
      console.log('---/---');

      return;
    }

    if (existAsFalse) {
      this.exhibition.topics[index].isAssigned = true;
      console.log('---+---');
      return;
    }

    if (!existAsFalse && !existAsTrue) {
      this.exhibition.topics.push(this.topicVM[0]);
      console.log('---=---');
    }
  }

  selectTopic(topic: TopicVM) {
    if (!topic.isAssigned) {
      topic.isAssigned = !topic.isAssigned;
      if (this.selectedTopics.includes(topic)) {
        return;
      }
      this.selectedTopics.push(topic);
    } else {
      let index = this.selectedTopics.indexOf(topic);
      if (index === -1) {
        return;
      }
      this.selectedTopics[index].isAssigned =
        !this.selectedTopics[index].isAssigned;
    }
  }

  removeTopic(topic: TopicVM) {
    let index = this.exhibition.topics.indexOf(topic);

    if (index != -1) {
      console.log('---+---');
      this.exhibition.topics[index].isAssigned = false;
    }
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
    dateString = dateTime.toString().substring(0, 19);

    //dateString = dateString.replace(/T/g, ' ');
    let date = new Date(dateString);

    return date.toLocaleString();
  }
}
