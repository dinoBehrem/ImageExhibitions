import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ResponseVM } from 'src/app/ViewModels/ResponseVM';
import { RoleManagerVM } from 'src/app/ViewModels/RoleManager';
import { UserVM } from 'src/app/ViewModels/UserVM';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  url: string = 'https://localhost:44395/Authentication';
  options = {
    headers: new HttpHeaders({
      'Content-Type': 'application/json',
    }),
  };
  constructor(private http: HttpClient, private router: Router) {}

  GetAll(): Observable<UserVM[]> {
    return this.http.get<UserVM[]>(this.url + '/GetUsers', this.options);
  }

  GetRoles(): Observable<string> {
    return this.http.get<string>(this.url + '/GetRoles', this.options);
  }

  PromoteToRole(roleManager: RoleManagerVM) {
    return this.http.post<RoleManagerVM>(
      this.url + '/PromoteTo',
      roleManager,
      this.options
    );
  }

  DemoteToRole(roleManager: RoleManagerVM) {
    return this.http.post<RoleManagerVM>(
      this.url + '/DemoteTo',
      roleManager,
      this.options
    );
  }
}
