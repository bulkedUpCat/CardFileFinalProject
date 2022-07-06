import { ThisReceiver } from '@angular/compiler';
import { Component, Inject, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user/User';
import { AuthService } from 'src/app/services/auth.service';
import { NotifierService } from 'src/app/services/notifier.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-change-user-info',
  templateUrl: './change-user-info.component.html',
  styleUrls: ['./change-user-info.component.css']
})
export class ChangeUserInfoComponent implements OnInit {
  user: User;
  changeUserName: boolean;
  changeUserEmail: boolean;
  changeUserNameForm: FormGroup;
  changeEmailForm: FormGroup;
  submittedUserNameForm: boolean;
  submittedEmailForm: boolean;

  constructor(private authService: AuthService,
    private userService: UserService,
    private dialogRef: MatDialogRef<ChangeUserInfoComponent>,
    @Inject(MAT_DIALOG_DATA) private data,
    private notifier: NotifierService,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.user = this.data.user;

    this.createForms();
  }

  createForms(){
    this.changeUserNameForm = this.fb.group({
      newUserName: [null,[Validators.required, Validators.minLength(4),
        this.patternValidator(/^[a-zA-Z]+$/, {isAllLetters: true})]]
    });

    this.changeEmailForm = this.fb.group({
      newEmail: [null,[Validators.required,Validators.email]]
    });
  }

  get newUserName(){
    return this.changeUserNameForm.get('newUserName');
  }

  get newEmail(){
    return this.changeEmailForm.get('newEmail');
  }

  patternValidator(regex: RegExp, error: ValidationErrors): ValidatorFn{
    return (control: AbstractControl): { [key: string]: any } => {
      if (!control.value){
        return null;
      }

      const valid = regex.test(control.value);

      return valid ? null : error;
    }
  }

  toggleReceiveNotifications(){
    this.user.receiveNotifications = !this.user.receiveNotifications;

    this.userService.toggleReceiveNotifications(this.user.id, this.user.receiveNotifications).subscribe(res => {
      console.log('done');
    }, err => console.log(err));
  }

  openFieldForChangingUserName(){
    this.changeUserName = !this.changeUserName;
  }

  openFieldForChangingEmail(){
    this.changeUserEmail = !this.changeUserEmail;
  }

  onChangeUserName(){
    this.submittedUserNameForm = true;

    if (!this.changeUserNameForm.valid){
      return;
    }

    const newUserName = this.changeUserNameForm.value;

    this.authService.changeUsername(this.user.id, newUserName).subscribe(res => {
      this.notifier.showNotification("User name was successfully changed", 'OK', 'SUCCESS');
      this.dialogRef.close();
    }, err => {
      console.log(err);
      this.notifier.showNotification(err.error, 'OK', 'ERROR');
    });
  }

  onChangeEmail(){
    this.submittedEmailForm = true;

    if (!this.changeEmailForm.valid){
      return;
    }

    const newEmail = this.changeEmailForm.value;

    this.authService.changeEmail(this.user.id, newEmail).subscribe(res => {
      this.notifier.showNotification("Email was changed. Confirm it please before logging in", 'OK', 'SUCCESS');
      this.dialogRef.close();
      this.authService.logOut();
    }, err => {
      console.log(err);
      this.notifier.showNotification(err.error, 'OK','ERROR');
    });
  }
}
