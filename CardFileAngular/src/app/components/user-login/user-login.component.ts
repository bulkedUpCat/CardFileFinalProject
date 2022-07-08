import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { NotifierService } from 'src/app/services/notifier.service';
import { ConfirmEmailDialogComponent } from '../dialogs/confirm-email-dialog/confirm-email-dialog.component';
import { ForgotPasswordComponent } from '../dialogs/forgot-password/forgot-password.component';

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrls: ['./user-login.component.css']
})
export class UserLoginComponent implements OnInit {
  loginForm: FormGroup;
  submitted: boolean;

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private notifier: NotifierService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.createLoginForm();
  }

  createLoginForm(){
    this.loginForm = this.fb.group({
      email: [null,[Validators.required, Validators.email]],
      password: [null, [Validators.required]]
    });
  }

  get email(){
    return this.loginForm.get('email');
  }

  get password(){
    return this.loginForm.get('password');
  }

  onLogin(){
    this.submitted = true;

    if (!this.loginForm.valid){
      console.log('invalid login form');
      return;
    }

    const user = this.loginForm.value;

    this.authService.logIn(user).subscribe( u => {
      this.notifier.showNotification("You've successfully logged in!","OK",'SUCCESS');
      this.router.navigateByUrl('/main');
    }, err => {
      console.log(err);
      this.notifier.showNotification(err.error,"OK",'ERROR');
      this.loginForm.reset();
    });
  }

  onForgotPassword(){
    this.dialog.open(ForgotPasswordComponent,{
      width: '400px'
    });
  }

  onConfirmEmail(){
    this.dialog.open(ConfirmEmailDialogComponent, {
      width: '400px'
    });
  }
}
