export class UserParams implements UserParameters{
  email: string;
  userName: string;
  isBanned: boolean;
  pageNumber: number = 1;
  pageSize: number = 2;
}

export interface UserParameters{
  email: string;
  userName: string;
  isBanned: boolean;
  pageNumber: number;
  pageSize: number;
}
