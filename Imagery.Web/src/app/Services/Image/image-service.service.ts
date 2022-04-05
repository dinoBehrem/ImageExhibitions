import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CollectionItemVM } from 'src/app/ViewModels/CollectionItemVM';
import { CollectionVM } from 'src/app/ViewModels/CollectionVM';
import { DimensionsVM } from 'src/app/ViewModels/DimensionsVM';

@Injectable({
  providedIn: 'root',
})
export class ImageServiceService {
  url: string = 'https://localhost:44395/Image';
  options = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  constructor(private http: HttpClient) {}

  UploadProfilePicture(username: string, picture: FormData) {
    return this.http.post(
      this.url + '/ProfilePictureUpload?username=' + username,
      picture
    );
  }

  UploadItemImage(id: number, itemData: FormData) {
    return this.http.post(this.url + '/ItemUpload/' + id, itemData);
  }

  AddDimensions(id: number, dimensions: DimensionsVM) {
    return this.http.post(
      this.url + '/AddDimension/' + id,
      dimensions,
      this.options
    );
  }

  DeleteDimensions(id: number) {
    return this.http.delete(this.url + '/DeleteDimension/' + id, this.options);
  }

  EditItem(id: number, itemData: FormData) {
    return this.http.put(this.url + '/ItemUpdate/' + id, itemData);
  }

  DeleteItem(id: number) {
    return this.http.delete(this.url + '/DeleteItem/' + id, this.options);
  }

  GetCollectionItems(username: string): any {
    return this.http.get(this.url + '/GetCollection/' + username, this.options);
  }

  AddCollection(collection: CollectionVM) {
    return this.http.post(this.url + '/BuyItem', collection, this.options);
  }
}
