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
export class SuperAdminGuard implements CanActivate {
  constructor(private signService: SignService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | boolean
    | UrlTree
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree> {
    if (this.isSuperAdmin()) {
      return true;
    }

    if (this.signService.isAuthenticated()) {
      this.router.navigateByUrl('');
      return false;
    }

    this.router.navigateByUrl('Login');
    return false;
  }

  isSuperAdmin() {
    let role = this.signService.GetJWTData('role');

    if (role === 'SuperAdmin') {
      return true;
    } else {
      return false;
    }
  }
}
