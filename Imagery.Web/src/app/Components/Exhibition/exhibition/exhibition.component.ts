import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ExhibitionService } from 'src/app/Services/Exhibition/exhibition.service';
import { ExhibitionVM } from 'src/app/ViewModels/ExhibitionVM';
import { ExponentItemVM } from 'src/app/ViewModels/ExponentItemVM';

@Component({
  selector: 'app-exhibition',
  templateUrl: './exhibition.component.html',
  styleUrls: ['./exhibition.component.css'],
})
export class ExhibitionComponent implements OnInit {
  exhibition!: ExhibitionVM;
  id: number = -1;
  sub: any;
  itemVM: any = {
    image: '',
    name: '',
    creator: '',
    description: '',
    dimensions: '',
    price: 0.0,
  };
  constructor(
    private exhibitionService: ExhibitionService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.sub = this.route.params.subscribe((param: any) => {
      this.id = +param['id'];

      this.loadExhibition();
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  loadExhibition() {
    if (this.id === -1) {
      this.router.navigateByUrl('');
    }
    this.exhibitionService.GetSingle(this.id).subscribe((res: any) => {
      this.exhibition = res;
    });
  }

  setItem(event: ExponentItemVM) {
    this.itemVM = event;
  }

  addToCart() {
    alert('Image added to cart!');
  }
}
