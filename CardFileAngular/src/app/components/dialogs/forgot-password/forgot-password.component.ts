import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ForgotPasswordDTO } from 'src/app/models/user/ForgotPasswordDTO';
import { AuthService } from 'src/app/services/auth.service';
import { NotifierService } from 'src/app/services/notifier.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  forgotPasswordForm: FormGroup;
  submitted: boolean;

  constructor(private fb: FormBuilder,
              private authService: AuthService,
              private notifier: NotifierService,
              private dialogRef: MatDialogRef<ForgotPasswordComponent>) { }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(){
    this.forgotPasswordForm = this.fb.group({
      email: [null,[Validators.required,Validators.email]]
    });
  }

  get email(){
    return this.forgotPasswordForm.get('email');
  }

  onSubmit(){
    this.submitted = true;
    if (!this.forgotPasswordForm.valid){
      return;
    }

    const forgotPasswordDTO : ForgotPasswordDTO = {
      email: this.email.value,
      clientURI: 'http://localhost:4200/reset-password'
    }

    this.authService.forgotPassword(forgotPasswordDTO).subscribe( p => {
      console.log(forgotPasswordDTO);
      this.notifier.showNotification('Password reset link has been sent to your email', 'Ok','SUCCESS');
      this.dialogRef.close();
    }, err => {
      this.notifier.showNotification(err.error,'Ok','ERROR');
      this.dialogRef.close();
    });
  }
}
