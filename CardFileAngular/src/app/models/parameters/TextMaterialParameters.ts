export class TextMaterialParams implements TextMaterialParameters{
  pageNumber: number;
  pageSize: number;
  filterFromDate: Date;
  filterToDate: Date;
  searchTitle: string;
  searchCategory: string;
  searchAuthor: string;
  userId: string;
  approvalStatus: Array<number>;
  orderBy: string;
}

export interface TextMaterialParameters{
  pageNumber: number;
  pageSize: number;
  filterFromDate: Date;
  filterToDate: Date;
  searchTitle: string;
  searchCategory: string;
  searchAuthor: string;
  userId: string;
  approvalStatus: Array<number>;
  orderBy: string;
}
