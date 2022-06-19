import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(private router: Router,
    private jwtHelper: JwtHelperService){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      const token = localStorage.getItem('TokenInfo');
      let payload;

      payload = token.split('.')[1];
      payload = window.atob(payload);
      payload = JSON.parse(payload);

      if (payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
       && payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'].includes('Admin')){
        return true;
      }

      this.router.navigate(['/main']);
      return false;
  }
}
