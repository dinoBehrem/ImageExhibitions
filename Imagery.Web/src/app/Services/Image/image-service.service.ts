import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ImageServiceService {
  url: string = 'https://localhost:44367/Image';

  constructor(private http: HttpClient) {}

  UploadProfilePicture(username: string, picture: FormData) {
    return this.http.post(
      this.url + '/UploadImage?username=' + username,
      picture
    );
  }
}
