import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedUserListParamsService {
  userName: string;
  email: string;
  isBanned: boolean;
  pageNumber: number = 1;
  pageSize: number = 5;

  constructor() { }
}
