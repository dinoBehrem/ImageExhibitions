import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CollectionItemVM } from 'src/app/ViewModels/CollectionItemVM';
import { CollectionVM } from 'src/app/ViewModels/CollectionVM';
import { ImageServiceService } from '../Image/image-service.service';
import { SignService } from '../Sign/sign.service';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  cartItems: CollectionItemVM[] = [];

  constructor(
    private imageService: ImageServiceService,
    private signService: SignService,
    private router: Router
  ) {}

  addToCart(item: CollectionItemVM) {
    if (this.cartItems.includes(item)) {
      this.increaseQuantity(item);
    } else {
      this.cartItems.push(item);
    }
  }

  getItems() {
    return this.cartItems;
  }

  clearCart() {
    this.cartItems = [];
    return this.cartItems;
  }

  increaseQuantity(item: CollectionItemVM) {
    item.quantity++;
  }

  decreaseQuantity(item: CollectionItemVM) {
    item.quantity--;
    if (item.quantity <= 0) {
      this.removeItem(item);
    }
  }

  removeItem(item: CollectionItemVM) {
    let index = this.cartItems.indexOf(item);

    if (index > -1) {
      this.cartItems.splice(index, 1);
    }
  }

  purchase() {
    let collection = {
      collection: this.cartItems,
      username: this.getUsername(),
    };
    this.imageService
      .AddCollection(collection as CollectionVM)
      .subscribe((res: any) => {
        alert(res.message);
      });
    this.clearCart();
  }

  getUsername() {
    const claim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const username: string = this.signService.GetJWTData(claim);

    if (username === '') {
      alert('Login in to complete your purchase!');
      this.router.navigate(['Login']);
    }

    return username;
  }

  getItemCount(): number {
    let sum = 0;
    this.cartItems.forEach((item) => (sum += item.quantity));

    return sum;
  }

  getTotalPrice(): number {
    let total = 0;
    this.cartItems.forEach((item) => (total += item.quantity * item.price));

    return total;
  }
}
