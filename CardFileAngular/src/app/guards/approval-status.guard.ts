import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { TextMaterialService } from '../services/text-material.service';

@Injectable({
  providedIn: 'root'
})
export class ApprovalStatusGuard implements CanActivate {
  constructor(private textMaterialService: TextMaterialService,
    private router: Router){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

      let isManager = false;
      let isAdmin = false;

      const token = localStorage.getItem('TokenInfo');

      if (token){
        let payload;

        payload = token.split('.')[1];
        payload = window.atob(payload);
        payload = JSON.parse(payload);

        if (payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']){
          if (payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'].includes('Manager')){
            isManager = true;
          }
          if (payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'].includes('Admin')){
            isAdmin = true;
          }
        }
      }

      return new Observable<boolean>(obs => {
        let id = +route.paramMap.get('id');

        let textMaterial;
        this.textMaterialService.getTextMaterialById(id).subscribe(tm => {
          textMaterial = tm;
          if (!tm){
            this.router.navigateByUrl('/main');
            obs.next(false);
          }
          if (textMaterial.approvalStatusId == 1){
            obs.next(true);
          }
          else if (textMaterial.approvalStatusId == 0 && isManager){
            obs.next(isManager);
          }
          else if (textMaterial.approvalStatusId == 2 && isAdmin){
            obs.next(isAdmin);
          }
          else{
            this.router.navigateByUrl('/main');
            obs.next(false);
          }
        }, err => {
          console.log(err);
          this.router.navigateByUrl('/main');
          obs.next(false);
        });
      })
  }

}
