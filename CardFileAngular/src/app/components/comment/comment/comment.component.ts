import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActiveComment } from 'src/app/models/comment/ActiveComment';
import { Comment } from 'src/app/models/comment/Comment';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.css']
})
export class CommentComponent implements OnInit {
  @Input() comment: Comment;
  @Input() parentId: number = null;
  @Input() currentUserId: string;
  @Input() replies: Comment[];
  @Input() activeComment: ActiveComment;

  @Output() setActiveComment = new EventEmitter<ActiveComment>();
  @Output() addComment = new EventEmitter<{content: string, parentId: number}>();
  @Output() updateComment = new EventEmitter<{content: string, id: number}>();
  @Output() deleteComment = new EventEmitter<number>();

  replyId: number = null;
  canReply: boolean;
  canEdit: boolean;
  canDelete: boolean;

  constructor() { }

  ngOnInit(): void {
    this.replyId = this.parentId ? this.parentId : this.comment.id;
    this.setUp();
  }

  setUp(){
    this.canReply = !!this.currentUserId;
    this.canEdit = this.comment.userId == this.currentUserId;
    this.canDelete = this.comment.userId == this.currentUserId && this.replies.length == 0;
  }

  isReplying(){
    if (!this.activeComment){
      return false;
    }
    else{
      return this.activeComment.id == this.comment.id &&
        this.activeComment.type == 'replying';
    }
  }

  isEditing(){
    if (!this.activeComment){
      return false;
    }
    else{
      return this.activeComment.id == this.comment.id &&
        this.activeComment.type == 'editing';
    }
  }
}
