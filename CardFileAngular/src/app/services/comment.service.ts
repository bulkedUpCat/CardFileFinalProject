import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Comment } from '../models/comment/Comment';
import { CreateCommentDTO } from '../models/comment/CreateCommentDTO';
import { UpdateCommentDTO } from '../models/comment/UpdateCommentDTO';

@Injectable({
  providedIn: 'root'
})
export class CommentService {

  constructor(private http: HttpClient) { }

  getCommentsByTextMaterialId(textMaterialId: number): Observable<Comment[]>{
    return this.http.get<Comment[]>(`${environment.apiUrl}/comments/textmaterials/` + textMaterialId);
  }

  postComment(comment: CreateCommentDTO){
    return this.http.post(`${environment.apiUrl}/comments`, comment);
  }

  updateComment(comment: UpdateCommentDTO){
    return this.http.put(`${environment.apiUrl}/comments`, comment);
  }

  deleteComment(id: number){
    return this.http.delete(`${environment.apiUrl}/comments/` + id);
  }
}
