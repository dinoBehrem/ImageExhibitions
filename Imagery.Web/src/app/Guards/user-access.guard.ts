import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { SignService } from '../Services/Sign/sign.service';

@Injectable({
  providedIn: 'root',
})
export class UserAccessGuard implements CanActivate {
  constructor(private signService: SignService, private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (!this.signService.isAuthenticated()) {
      this.router.navigateByUrl('Login');
      return false;
    }
    if (this.isUser()) {
      return true;
    }

    this.router.navigateByUrl('');
    return false;
  }

  isUser() {
    const userRole = 'User';
    const field = 'role';

    let role: string[] = [];
    let userRoles = this.signService.GetJWTData(field);

    if (userRoles instanceof Array) {
      role = [...userRoles];
    } else {
      role.push(userRoles);
    }

    if (role.includes(userRole)) {
      return true;
    } else {
      return false;
    }
  }
}
