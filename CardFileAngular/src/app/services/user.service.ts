import { _getFocusedElementPierceShadowDom } from '@angular/cdk/platform';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserParameters } from '../models/parameters/UserParameters';
import { User } from '../models/user/User';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  getUsers(userParams: UserParameters): Observable<any>{
    var parameters = {}

    if (userParams.pageNumber) parameters['pageNumber'] = userParams.pageNumber;
    if (userParams.pageSize) parameters['pageSize'] = userParams.pageSize;

    return this.http.get(`${environment.apiUrl}/users`, {
      responseType: 'json',
      observe: 'response',
      params: parameters
    });
  }

  getUserById(id: string): Observable<User>{
    return this.http.get<User>(`${environment.apiUrl}/users/` + id);
  }

  toggleReceiveNotifications(id: string, receiveNotifications: boolean){
    return this.http.put(`${environment.apiUrl}/users/${id}/notifications`,receiveNotifications);
  }

  getLikedTextMaterials(id: string): any{
    return this.http.get(`${environment.apiUrl}/users/${id}/textMaterials/liked`);
  }

  addTextMaterialToLiked(id: string, textMaterialId: number){
    return this.http.post(`${environment.apiUrl}/users/${id}/textMaterials/liked`, textMaterialId);
  }

  removeTextMaterialFromLiked(id: string, textMaterialId: number){
    return this.http.delete(`${environment.apiUrl}/users/${id}/textMaterials/liked`, {
      body: textMaterialId
    });
  }
}
