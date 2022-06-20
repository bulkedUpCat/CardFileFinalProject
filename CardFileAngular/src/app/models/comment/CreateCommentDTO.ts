export interface CreateCommentDTO{
  parentCommentId: number;
  userId: string;
  textMaterialId: number;
  content: string;
}
