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
export class AdminAccessGuard implements CanActivate {
  constructor(private signService: SignService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (this.isAdmin()) {
      return true;
    }

    if (this.signService.isAuthenticated()) {
      this.router.navigateByUrl('');
      return false;
    }

    this.router.navigateByUrl('Login');
    return false;
  }

  isAdmin() {
    let role: string[] = [...this.signService.GetJWTData('role')];

    if (role.includes('Admin') || role.includes('SuperAdmin')) {
      return true;
    } else {
      return false;
    }
  }
}
