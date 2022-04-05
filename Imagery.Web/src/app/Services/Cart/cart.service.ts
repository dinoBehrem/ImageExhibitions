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
    private signService: SignService
  ) {}

  addToCart(product: CollectionItemVM) {
    this.cartItems.push(product);
    console.log(this.cartItems);
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
    if (item.quantity == 0) {
      return;
    } else {
      item.quantity--;
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
        this.clearCart();
      });
  }

  getUsername() {
    const claim = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
    const username: string = this.signService.GetJWTData(claim);

    if (username === '') {
      alert('Login in to complete your purchase!');
      return '';
    }

    return username;
  }
}
