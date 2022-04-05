import { Component, OnInit } from '@angular/core';
import { CartService } from 'src/app/Services/Cart/cart.service';
import { ImageServiceService } from 'src/app/Services/Image/image-service.service';
import { CollectionItemVM } from 'src/app/ViewModels/CollectionItemVM';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  cartItems = this.cartService.getItems();

  constructor(private cartService: CartService) {}

  ngOnInit(): void {}

  clear() {
    this.cartItems = this.cartService.clearCart();
  }

  buy() {
    this.cartService.purchase();
    this.cartItems = this.cartService.clearCart();
  }
}
