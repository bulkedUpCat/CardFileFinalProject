export class TextMaterialParams implements TextMaterialParameters{
  pageNumber: number;
  pageSize: number;
  filterFromDate: Date;
  filterToDate: Date;
  //minLikesCount: number;
  searchTitle: string;
  searchCategory: string;
  searchAuthor: string;
  approvalStatus: Array<number>;
  orderBy: string;
}

export interface TextMaterialParameters{
  pageNumber: number;
  pageSize: number;
  filterFromDate: Date;
  filterToDate: Date;
  //minLikesCount: number;
  searchTitle: string;
  searchCategory: string;
  searchAuthor: string;
  approvalStatus: Array<number>;
  orderBy: string;
}
