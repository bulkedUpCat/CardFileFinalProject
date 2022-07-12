import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CreateCommentDTO } from 'src/app/models/comment/CreateCommentDTO';

@Component({
  selector: 'app-comment-form',
  templateUrl: './comment-form.component.html',
  styleUrls: ['./comment-form.component.css']
})
export class CommentFormComponent implements OnInit {
  submitted: boolean;

  @Input() submitLabel: string;
  @Input() initialText: string = '';
  @Input() hasCancelButton: boolean = false;

  @Output() handleSubmit: EventEmitter<string> = new EventEmitter<string>();
  @Output() handleCancel: EventEmitter<void> = new EventEmitter<void>();

  commentForm: FormGroup;

  constructor(private fb: FormBuilder) { }

  ngOnInit(): void {
    this.createForm();
    this.activateButtons();
  }

  createForm(){
    this.commentForm = this.fb.group({
      //content: [this.initialText,[Validators.required]]
      content: [this.initialText,[Validators.required, Validators.minLength(1), Validators.maxLength(400)]]
    });
  }

  get content(){
    return this.commentForm.get('content');
  }

  onSubmit(){
    this.submitted = true;
    if (!this.commentForm.valid){
      return;
    }

    this.handleSubmit.emit(this.commentForm.value.content);
    this.commentForm.reset();
  }

  activateButtons(){
    let elements = document.querySelectorAll('.btn');

    elements.forEach(el => {
      el.addEventListener('click', () => {
        let command = (el as HTMLElement).dataset['element'];

        document.execCommand(command, false, null);
      })
    })
  }
}
