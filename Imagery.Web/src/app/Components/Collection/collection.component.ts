import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { CollectionItemVM } from 'src/app/ViewModels/CollectionItemVM';

@Component({
  selector: 'app-collection',
  templateUrl: './collection.component.html',
  styleUrls: ['./collection.component.css'],
})
export class CollectionComponent implements OnInit {
  collectionItems: CollectionItemVM[] = [];
  collectionItem!: CollectionItemVM;
  imagePlaceholder: string = '../../assets/imagePlaceholder.png';

  errorMessage?: string;

  constructor(
    private imageService: ImageServiceService,
    private signService: SignService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadCollectionItems();
  }

  loadCollectionItems() {
    let username = this.getUsername();
    if (username == '') {
      this.router.navigateByUrl('Home');
    }

    this.imageService.GetCollectionItems(username).subscribe(
      (res: any) => {
        this.collectionItems = res;
      },
      (err: any) => {
        alert(err.error.message);
      }
    );
  }

  getUsername() {
    const claim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const username: string = this.signService.GetJWTData(claim);

    if (username === '') {
      // this.router.navigateByUrl('Login');
      return '';
    }
    return username;
  }

  setItem(item: CollectionItemVM) {
    this.collectionItem = item;
    this.collectionItem.dimensions;
  }
}
