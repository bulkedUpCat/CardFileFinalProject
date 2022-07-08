import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { AuthService } from 'src/app/services/auth.service';
import { NotifierService } from 'src/app/services/notifier.service';

@Component({
  selector: 'app-confirm-email-dialog',
  templateUrl: './confirm-email-dialog.component.html',
  styleUrls: ['./confirm-email-dialog.component.css']
})
export class ConfirmEmailDialogComponent implements OnInit {
  confirmEmailForm: FormGroup;
  submitted: boolean;

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private notifier: NotifierService,
    private dialogRef: MatDialogRef<ConfirmEmailDialogComponent>) { }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(){
    this.confirmEmailForm = this.fb.group({
      email: [null, [Validators.required, Validators.email]]
    });
  }

  get email(){
    return this.confirmEmailForm.get('email');
  }

  onSubmit(){
    this.submitted = true;
    if (!this.confirmEmailForm.valid){
      return;
    }

    this.authService.sendEmailConfirmationLink(this.confirmEmailForm.value).subscribe(res => {
      this.notifier.showNotification('Check your email', 'OK', 'SUCCESS');
      this.dialogRef.close();
    }, err => {
      this.notifier.showNotification(err.error,'OK','ERROR');
    });
  }
}
