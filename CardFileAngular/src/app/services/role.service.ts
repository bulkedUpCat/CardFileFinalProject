import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RoleService {

  constructor(private http: HttpClient) { }

  getAllRoles(): Observable<string[]>{
    return this.http.get<string[]>(`${environment.apiUrl}/roles`);
  }

  addRole(userId: string, role: string){
    return this.http.post(`${environment.apiUrl}/roles`,{
      userId: userId,
      roleName: role
    });
  }

  removeRole(userId: string, roleName: string){
    return this.http.delete(`${environment.apiUrl}/roles`,{
      body: {
        userId: userId,
        roleName: roleName
      }
    });
  }
}
