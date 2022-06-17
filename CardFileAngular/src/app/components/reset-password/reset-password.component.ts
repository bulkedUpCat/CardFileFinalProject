import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPasswordDTO } from 'src/app/models/user/ResetPasswordDTO';
import { AuthService } from 'src/app/services/auth.service';
import { NotifierService } from 'src/app/services/notifier.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  passwordResetForm: FormGroup;
  submitted: boolean;

  constructor(private fb: FormBuilder,
              private authService: AuthService,
              private notifier: NotifierService,
              private route: ActivatedRoute,
              private router: Router) { }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(){
    this.passwordResetForm = this.fb.group({
      password: [null,[Validators.required]],
      confirmPassword: [null,[Validators.required]]
    })
  }

  get password(){
    return this.passwordResetForm.get('password');
  }

  get confirmPassword(){
    return this.passwordResetForm.get('confirmPassword');
  }

  onSubmit(){
    this.submitted = true;
    if (!this.passwordResetForm.valid){
      this.notifier.showNotification("All fields are required", 'Ok', 'ERROR');
      return;
    }

    if (this.password.value != this.confirmPassword.value){
      this.notifier.showNotification("Passwords don't match", 'Ok', 'ERROR');
      return;
    }

    let email = this.route.snapshot.queryParams['email'];
    let token = this.route.snapshot.queryParams['token'];

    const model: ResetPasswordDTO = {
      password: this.password.value,
      confirmPassword: this.confirmPassword.value,
      email: email,
      token: token
    };

    console.log(model);

    this.authService.resetPassword(model).subscribe( p => {
      this.notifier.showNotification('Password has been successfully reset', 'Ok', 'SUCCESS');
      this.router.navigateByUrl('/login');
    }, err => {
      this.notifier.showNotification(err.error,'Ok','ERROR');
      this.passwordResetForm.reset();
    })
  }
}
