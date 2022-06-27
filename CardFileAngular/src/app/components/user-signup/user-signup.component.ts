import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { NotifierService } from 'src/app/services/notifier.service';

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
    private router: Router,
    private notifier: NotifierService) { }

  ngOnInit(): void {
    this.createSignupForm();
  }

  createSignupForm(){
    this.signupForm = this.fb.group({
      email: [null,[Validators.required,Validators.email]],
      name: [null,[Validators.required, Validators.minLength(4)]],
      password: [null,[Validators.required,Validators.minLength(6),
        this.patternValidator(/\d/, { hasNumber: true }),
        this.patternValidator(/[a-z]/, { hasLowerCase: true }),
        this.patternValidator(/(?=.*?[#?!@$%^&*-])/, {hasSymbol: true})]],
      confirmPassword: [null,[Validators.required]]
    });
  }

  checkPasswords: ValidatorFn = (group: AbstractControl) : ValidationErrors | null => {
    let password = this.signupForm.get('password').value;
    let confirmPassword = this.signupForm.get('confirmPassword').value;
    return password === confirmPassword ? null : { notSame: true};
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
      return;
    }

    if (this.password.value != this.confirmPassword.value){
      this.notifier.showNotification("Passwords don't match","OK","ERROR");
      return;
    }

    const user = this.signupForm.value;

    this.authService.signUp(user).subscribe(u => {
      this.notifier.showNotification("You've successfully signed up!","OK","SUCCESS");
      this.router.navigateByUrl('login');
    },
    err => {
      this.notifier.showNotification(`${err.error}`,"OK","ERROR");
      console.log(err);
    });
  }
}
