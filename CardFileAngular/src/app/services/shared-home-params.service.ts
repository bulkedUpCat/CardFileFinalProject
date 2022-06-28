import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SharedHomeParamsService {
  pageNumber: number = 1;
  pageSize: number = 5;
  filterFromDate: Date = null;
  filterToDate: Date = null;
  //minLikesCount: number = 0;
  searchTitle: string = null;
  searchCategory: string = null;
  searchAuthor: string = null;
  approvalStatus: Array<number> = new Array<number>();
  orderBy: string = null;

  constructor() { }
}
