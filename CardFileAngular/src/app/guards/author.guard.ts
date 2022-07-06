import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { TextMaterialService } from '../services/text-material.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorGuard implements CanActivate {
  constructor(private textMaterialService: TextMaterialService,
    private router: Router){}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
      let id = +route.paramMap.get('id');
      let currentUserId: string;

      const token = localStorage.getItem('TokenInfo');

      if (token){
        let payload;

        payload = token.split('.')[1];
        payload = window.atob(payload);
        payload = JSON.parse(payload);

        currentUserId = payload.sub;
      }


      return new Observable<boolean>(obs => {
        this.textMaterialService.getTextMaterialById(id).subscribe(tm => {
          //obs.next(true);
          if (tm.authorId == currentUserId){
            obs.next(true);
          }
          else{
            this.router.navigateByUrl('/home-page');
            obs.next(false);
          }
        }, err => {
          this.router.navigateByUrl('/home-page');
          console.log(err);
          obs.next(false);
        })
      });
  }

}
