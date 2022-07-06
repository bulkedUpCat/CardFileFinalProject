import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable, of, tap } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ConfirmEmailDTO } from '../models/user/ConfirmEmailDTO';
import { ForgotPasswordDTO } from '../models/user/ForgotPasswordDTO';
import { ResetPasswordDTO } from '../models/user/ResetPasswordDTO';
import { UserLogin } from '../models/user/UserLogin';
import { UserSignUp } from '../models/user/UserSignup';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  public loggedIn = new BehaviorSubject<boolean>(!!localStorage.getItem('TokenInfo'));
  public claims = new BehaviorSubject<string[]>(null);

  constructor(private http: HttpClient,
    private jwtHelper: JwtHelperService,
    private router: Router) {
      const token = localStorage.getItem('TokenInfo');
      let payload;

      if (token && !this.jwtHelper.isTokenExpired(token)){
        payload = token.split('.')[1];
        payload = window.atob(payload);
        payload = JSON.parse(payload);

        this.claims.next(payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);
      }
     }

  get isLoggedIn(){
    const token = localStorage.getItem('TokenInfo');

    if (token && this.jwtHelper.isTokenExpired(token)){
      this.loggedIn.next(false);
      this.claims.next([]);
    }

    return this.loggedIn.asObservable();
  }

  getUserInfo(){
    const token = localStorage.getItem('TokenInfo');
    let payload;

    if (token && !this.jwtHelper.isTokenExpired(token)){
      payload = token.split('.')[1];
      payload = window.atob(payload);
      payload = JSON.parse(payload);

      this.claims.next(payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);
      return of(payload);
    }
    return of(null);
  }

  logIn(user: UserLogin) : Observable<any>{
    const headers = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };

    return this.http.post<UserLogin>(`${environment.apiUrl}/auth/login`,user,headers)
      .pipe(tap(user => {
        if (user.token){
          localStorage.setItem("TokenInfo",user.token);

          const token = localStorage.getItem('TokenInfo');
          let payload;

          payload = token.split('.')[1];
          payload = window.atob(payload);
          payload = JSON.parse(payload);
          this.claims.next(payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']);
          this.loggedIn.next(true);
        }
      }));
  }

  signUp(user: UserSignUp){
    const headers = {
      headers: new HttpHeaders({
        'Content-Type' : 'application/json'
      })
    };

    return this.http.post<UserSignUp>(`${environment.apiUrl}/auth/signup`,user,headers);
  }

  logOut(){
    this.loggedIn.next(false);
    localStorage.removeItem("TokenInfo");
    this.claims.next([]);
    this.router.navigate(['/login']);
  }

  confirmEmail(model: ConfirmEmailDTO){
    return this.http.post(`${environment.apiUrl}/auth/confirmEmail`, model);
  }

  changeUsername(userId: string, newUserName: string){

    return this.http.put<string>(`${environment.apiUrl}/auth/${userId}/userName`, newUserName);
  }

  changeEmail(userId: string, newEmail: string){

    return this.http.put<string>(`${environment.apiUrl}/auth/${userId}/email`, newEmail);
  }

  forgotPassword(model: ForgotPasswordDTO){
    const headers = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };

    return this.http.post(`${environment.apiUrl}/auth/forgotPassword`, model, headers)
  }

  resetPassword(model: ResetPasswordDTO){
    const headers = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      })
    };

    return this.http.post(`${environment.apiUrl}/auth/passwordReset`, model, headers)
  }
}
