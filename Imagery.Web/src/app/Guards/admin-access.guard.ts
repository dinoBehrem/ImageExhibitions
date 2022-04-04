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
    if (!this.signService.isAuthenticated()) {
      this.router.navigateByUrl('Login');
      return false;
    }

    if (this.isAdmin()) {
      return true;
    }

    this.router.navigateByUrl('');
    return false;
  }

  isAdmin() {
    const field = 'role';
    let role: string[] = [...this.signService.GetJWTData(field)];

    const adminRole = 'Admin';
    const superAdminRole = 'SuperAdmin';

    if (role.includes(adminRole) || role.includes(superAdminRole)) {
      return true;
    } else {
      return false;
    }
  }
}
