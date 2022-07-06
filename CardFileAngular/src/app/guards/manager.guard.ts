import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { TextMaterialService } from '../services/text-material.service';

@Injectable({
  providedIn: 'root'
})
export class ManagerGuard implements CanActivate {

  constructor(private textMaterialService: TextMaterialService,
    private router: Router){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

    let id = +route.paramMap.get('id');
    let textMaterial;
    this.textMaterialService.getTextMaterialById(id).subscribe(tm => {
      textMaterial = tm;

      if (textMaterial.approvalStatusId == 1){
        return true;
      }
    });

    const token = localStorage.getItem('TokenInfo');
    let payload;

    payload = token.split('.')[1];
    payload = window.atob(payload);
    payload = JSON.parse(payload);

    if (payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      && payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'].includes('Manager')){
      return true;
    }

    //this.router.navigate(['/main']);
    return true;
  }

}
