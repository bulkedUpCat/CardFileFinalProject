export interface Comment{
  id: number;
  parentCommentId: number;
  userId: string;
  userName: string;
  textMaterialId: number;
  content: string;
  createdAt: Date;
}
