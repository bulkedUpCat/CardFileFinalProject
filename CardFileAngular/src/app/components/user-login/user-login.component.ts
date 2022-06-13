import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

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
    private router: Router) { }

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
      console.log(u);
      this.router.navigateByUrl('/main');
    }, err => {
      console.log(err);
      this.loginForm.reset();
    });
  }
}
