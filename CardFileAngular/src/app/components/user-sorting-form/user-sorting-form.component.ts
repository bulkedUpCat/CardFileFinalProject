import { ThisReceiver } from '@angular/compiler';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserParameters, UserParams } from 'src/app/models/parameters/UserParameters';
import { SharedUserListParamsService } from 'src/app/services/shared-user-list-params.service';

@Component({
  selector: 'app-user-sorting-form',
  templateUrl: './user-sorting-form.component.html',
  styleUrls: ['./user-sorting-form.component.css']
})
export class UserSortingFormComponent implements OnInit {
  userSortingForm: FormGroup;
  userParams: UserParameters = new UserParams();

  @Output() filter = new EventEmitter<UserParameters>();

  constructor(private fb: FormBuilder,
    private sharedParams: SharedUserListParamsService) { }

  ngOnInit(): void {
    this.createForm();
  }

  createForm(){
    this.userSortingForm = this.fb.group({
      userName: [this.sharedParams.userName],
      email: [this.sharedParams.email],
      isBanned: [this.sharedParams.isBanned]
    });
  }

  get userName(){
    return this.userSortingForm.get('userName');
  }

  get email(){
    return this.userSortingForm.get('email');
  }

  get isBanned(){
    return this.userSortingForm.get('isBanned');
  }

  onSubmit(){
    this.userParams.userName = this.userName.value;
    this.userParams.email = this.email.value;
    this.userParams.isBanned = this.isBanned.value;

    this.filter.emit(this.userParams);
  }
}
