import { _getFocusedElementPierceShadowDom } from '@angular/cdk/platform';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../models/user/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getUsers(): Observable<User[]>{
    return this.http.get<User[]>(`${environment.apiUrl}/users`);
  }

  getUserById(id: string): Observable<User>{
    return this.http.get<User>(`${environment.apiUrl}/users/` + id);
  }

  toggleReceiveNotifications(id: string, receiveNotifications: boolean){
    return this.http.put(`${environment.apiUrl}/users/${id}/notifications`,receiveNotifications);
  }
}
