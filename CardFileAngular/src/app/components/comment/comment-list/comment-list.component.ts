import { not } from '@angular/compiler/src/output/output_ast';
import { Component, Input, OnInit } from '@angular/core';
import { ActiveComment } from 'src/app/models/comment/ActiveComment';
import { Comment } from 'src/app/models/comment/Comment';
import { UpdateCommentDTO } from 'src/app/models/comment/UpdateCommentDTO';
import { CommentService } from 'src/app/services/comment.service';
import { NotifierService } from 'src/app/services/notifier.service';

@Component({
  selector: 'app-comment-list',
  templateUrl: './comment-list.component.html',
  styleUrls: ['./comment-list.component.css']
})
export class CommentListComponent implements OnInit {
  comments: Comment[];
  mainComments: Comment[];
  activeComment: ActiveComment = null;
  commentsLoaded: boolean = false;

  @Input() currentUserId: string;
  @Input() textMaterialId: number;
  @Input() isAdmin: boolean;

  constructor(private commentService: CommentService,
    private notifier: NotifierService) { }

  ngOnInit(): void {
    this.getComments();
  }

  getComments(){
    this.commentService.getCommentsByTextMaterialId(this.textMaterialId).subscribe(c => {
      this.comments = c.reverse();
      this.mainComments = this.comments.filter(c => c.parentCommentId == null);
      this.commentsLoaded = true;
    }, err => {
      console.log(err);
    });
  }

  addComment({commentContent, parentId}){
    let comment = {
      userId: this.currentUserId,
      textMaterialId: this.textMaterialId,
      parentCommentId: parentId,
      content: commentContent
    }


    this.commentService.postComment(comment).subscribe(x => {
      this.commentService.getCommentsByTextMaterialId(this.textMaterialId).subscribe(c => {
        this.comments = c.reverse();
        this.mainComments = this.comments.filter(comment => comment.parentCommentId == null);
        this.activeComment = null;
      });
    }, err => console.log(err));
  }

  updateComment({content, id}){

    const comment = {
      id: id,
      content: content
    };

    this.commentService.updateComment(comment).subscribe(x => {
      this.commentService.getCommentsByTextMaterialId(this.textMaterialId).subscribe(c => {
        this.comments = c.reverse();
        this.mainComments = this.comments.filter(comment => comment.parentCommentId == null);
        this.activeComment = null;
      });
    }, err => {
      this.notifier.showNotification(err.error.errors.Content,"OK","ERROR");
      this.activeComment = null;
    });
  }

  deleteComment(id: number){
    this.commentService.deleteComment(id).subscribe(x => {
      this.commentService.getCommentsByTextMaterialId(this.textMaterialId).subscribe(c => {
        this.comments = c.reverse();
        this.mainComments = this.comments.filter(comment => comment.parentCommentId == null);
        this.activeComment = null;
      });
    }, err => {
      console.log(err);
    });
  }

  setActiveComment(activeComment: ActiveComment){
    this.activeComment = activeComment;
  }

  getReplies(commentId: number): Comment[]{
    let replies = this.comments.filter(c => c.parentCommentId == commentId)
      .sort((a,b) => new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime());

    return replies;
  }
}
