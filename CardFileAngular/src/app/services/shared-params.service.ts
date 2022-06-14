import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedParamsService {
  pageNumber: number = 1;
  pageSize: number = 5;
  filterFromDate: Date = null;
  filterToDate: Date = null;
  searchTitle: string = null;
  searchCategory: string = null;
  searchAuthor: string = null;
  approvalStatus: Array<number> = new Array<number>();
  orderBy: string = null;

  constructor() { }
}
