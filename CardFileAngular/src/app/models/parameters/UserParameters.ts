export class UserParams implements UserParameters{
  pageNumber: number = 1;
  pageSize: number = 2;
}

export interface UserParameters{
  pageNumber: number;
  pageSize: number;
}
