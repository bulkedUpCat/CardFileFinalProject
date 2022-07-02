import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Ban } from '../models/user/Ban';
import { CreateBanDTO } from '../models/user/CreateBanDTO';

@Injectable({
  providedIn: 'root'
})
export class BanService {

  constructor(private http: HttpClient) { }

  getAllBans(): Observable<Ban[]>{
    return this.http.get<Ban[]>(`${environment.apiUrl}/bans`);
  }

  getBanByUserId(id: string): Observable<Ban>{
    return this.http.get<Ban>(`${environment.apiUrl}/bans/users/${id}`);
  }

  banUser(ban: CreateBanDTO): any{
    return this.http.post(`${environment.apiUrl}/bans`, ban);
  }

  unbanUser(id: number){
    return this.http.delete(`${environment.apiUrl}/bans/${id}`);
  }
}
