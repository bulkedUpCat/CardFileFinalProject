import { DatePipe } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Ban } from 'src/app/models/user/Ban';
import { BanService } from 'src/app/services/ban.service';
import { NotifierService } from 'src/app/services/notifier.service';

@Component({
  selector: 'app-ban-user',
  templateUrl: './ban-user.component.html',
  styleUrls: ['./ban-user.component.css']
})
export class BanUserComponent implements OnInit {
  ban: Ban;
  banUserClicked: boolean;
  banForm: FormGroup;
  submitted: boolean;

  constructor(private banService: BanService,
    @Inject(MAT_DIALOG_DATA) private data,
    private dialogRef: MatDialogRef<BanUserComponent>,
    private fb: FormBuilder,
    private notifier: NotifierService,
    private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.getBanByUserId();
    this.createBanForm();
  }

  createBanForm(){
    this.banForm = this.fb.group({
      reason: [null,[Validators.required,Validators.maxLength(100)]],
      days: [null,[Validators.required,Validators.min(1),Validators.max(100)]],
      userId: [this.data.user.id]
    });
  }

  get reason(){
    return this.banForm.get('reason');
  }

  get days(){
    return this.banForm.get('days');
  }

  getBanByUserId(){
    this.banService.getBanByUserId(this.data.user.id).subscribe(b => {
      this.ban = b;
    }, err => console.log(err));
  }

  banUser(){
    this.banUserClicked = !this.banUserClicked;
  }

  unbanUser(){
    this.banService.unbanUser(this.ban.id).subscribe(res => {
      this.notifier.showNotification(`User with email ${this.data.user.email} was unbanned`, 'OK','SUCCESS');
      this.dialogRef.close();
    }, err => {
      this.notifier.showNotification(err.error,'OK','ERROR');
    });
  }

  onSubmit(){
    this.submitted = true;
    if (!this.banForm.valid){
      return;
    }

    const ban = this.banForm.value;

    if (!this.ban){
      this.banService.banUser(ban).subscribe(res => {
        this.dialogRef.close();
        this.notifier.showNotification(`User with email ${this.data.user.email} is banned till ${this.datePipe.transform(res.expires, 'MMM, d, y')}`,'OK','SUCCESS');
      }, err => {
        this.notifier.showNotification(err.error,'OK','ERROR');
      });
    }
    else{
      this.banService.updateBan(this.ban.id, ban).subscribe(res => {
        this.dialogRef.close();
        this.notifier.showNotification(`User with email ${this.data.user.email} is banned till ${this.datePipe.transform(res.expires, 'MMM, d, y')}`,'OK','SUCCESS');
      }, err => {
        this.notifier.showNotification(err.error,'OK','ERROR');
      });
    }

  }
}
