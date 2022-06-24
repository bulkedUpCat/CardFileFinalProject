import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedUserPageParamsService {
  pageNumber: number = 1;
  pageSize: number = 5;

  constructor() { }
}
