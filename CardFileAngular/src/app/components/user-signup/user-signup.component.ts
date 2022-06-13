import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-user-signup',
  templateUrl: './user-signup.component.html',
  styleUrls: ['./user-signup.component.css']
})
export class UserSignupComponent implements OnInit {
  signupForm: FormGroup;
  submitted: boolean;

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private router: Router) { }

  ngOnInit(): void {
    this.createSignupForm();
  }

  createSignupForm(){
    this.signupForm = this.fb.group({
      email: [null,[Validators.required,Validators.email]],
      name: [null,[Validators.required, Validators.minLength(4)]],
      password: [null,[Validators.required,Validators.minLength(6)]],
      confirmPassword: [null,[Validators.required]]
    });
  }

  checkPasswords: ValidatorFn = (group: AbstractControl) : ValidationErrors | null => {
    let password = this.signupForm.get('password').value;
    let confirmPassword = this.signupForm.get('confirmPassword').value;
    return password === confirmPassword ? null : { notSame: true};
  }

  get email(){
    return this.signupForm.get('email');
  }

  get name(){
    return this.signupForm.get('name');
  }

  get password(){
    return this.signupForm.get('password');
  }

  get confirmPassword(){
    return this.signupForm.get('confirmPassword');
  }

  onSignup(){
    this.submitted = true;

    if(!this.signupForm.valid){
      console.log('sign up form is not valid');
      return;
    }

    if (this.password.value != this.confirmPassword.value){
      console.log('passwords do not match');
      return;
    }

    const user = this.signupForm.value;

    this.authService.signUp(user).subscribe(u => {
      this.router.navigateByUrl('login');
    },
    err => {
      console.log(err);
    });
  }
}
