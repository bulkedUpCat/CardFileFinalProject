import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { TextMaterialParameters, TextMaterialParams } from 'src/app/models/parameters/TextMaterialParameters';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { User } from 'src/app/models/user/User';
import { AuthService } from 'src/app/services/auth.service';
import { SharedHomeParamsService } from 'src/app/services/shared-home-params.service';
import { TextMaterialService } from 'src/app/services/text-material.service';
import { UserService } from 'src/app/services/user.service';
import { ChangeUserInfoComponent } from '../dialogs/change-user-info/change-user-info.component';

@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit {
  ownTextMaterials: TextMaterial[];
  textMaterialParams: TextMaterialParameters = new TextMaterialParams();
  showApproved: string;
  paginator: any;

  userId: string;
  userName: string;
  user: User;

  constructor(private authService: AuthService,
    public router: Router,
    private userService: UserService,
    private textMaterialService: TextMaterialService,
    private sharedHomeParams: SharedHomeParamsService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.authService.getUserInfo().subscribe( u => {
      if (u){
        this.userId = u.sub;
      }
    });
    this.userService.getUserById(this.userId).subscribe(u => {
      if (u){
        this.userName = u.userName;
        this.user = u;
      }
    });

    this.configureTextMaterialParams();
    this.getOwnTextMaterials();
  }

  configureTextMaterialParams(){
    this.textMaterialParams.orderBy = this.sharedHomeParams.orderBy;
    this.textMaterialParams.filterFromDate = this.sharedHomeParams.filterFromDate;
    this.textMaterialParams.filterToDate = this.sharedHomeParams.filterToDate;
    this.textMaterialParams.approvalStatus = this.sharedHomeParams.approvalStatus;
    this.textMaterialParams.searchTitle = this.sharedHomeParams.searchTitle;
    this.textMaterialParams.searchCategory = this.sharedHomeParams.searchCategory;
    this.textMaterialParams.searchAuthor = this.sharedHomeParams.searchAuthor;
    this.textMaterialParams.pageNumber = this.sharedHomeParams.pageNumber;
    this.textMaterialParams.pageSize = this.sharedHomeParams.pageSize;
  }

  getOwnTextMaterials(){
    this.textMaterialService.getTextMaterialsByUserId(this.userId, this.textMaterialParams).subscribe(tm => {
      this.ownTextMaterials = tm.body;
      this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
    }, err => {
      console.log(err);
    });
  }

  onFilter(parameters: TextMaterialParameters){
    this.textMaterialParams = parameters;

    this.sharedHomeParams.orderBy = parameters.orderBy;

    this.sharedHomeParams.filterFromDate = parameters.filterFromDate;
    this.sharedHomeParams.filterToDate = parameters.filterToDate;
    this.sharedHomeParams.approvalStatus = parameters.approvalStatus;
    this.sharedHomeParams.searchTitle = parameters.searchTitle;
    this.sharedHomeParams.searchCategory = parameters.searchCategory;
    this.sharedHomeParams.searchAuthor = parameters.searchAuthor;

    this.textMaterialService.getTextMaterialsByUserId(this.userId, parameters).subscribe( tm => {
      this.ownTextMaterials = tm.body;
      this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
    }, err => {
      console.log(err);
    });
  }

  onNextPage(page: number){
    this.textMaterialParams.pageNumber = page;
    this.sharedHomeParams.pageNumber = page;

    this.textMaterialService.getTextMaterialsByUserId(this.userId, this.textMaterialParams).subscribe(tm => {
      this.ownTextMaterials = tm.body;
    }, err => {
      console.log(err);
    });
  }

  onPreviousPage(page: number){
    this.textMaterialParams.pageNumber = page;
    this.sharedHomeParams.pageNumber = page;

    this.textMaterialService.getTextMaterialsByUserId(this.userId, this.textMaterialParams).subscribe(tm => {
      this.ownTextMaterials = tm.body;
    }, err => {
      console.log(err);
    });
  }

  toggleReceiveNotifications(){
    this.user.receiveNotifications = !this.user.receiveNotifications;

    this.userService.toggleReceiveNotifications(this.userId, this.user.receiveNotifications).subscribe(res => {
      console.log('done');
    }, err => console.log(err));
  }

  changeUserInfo(){
    const config = new MatDialogConfig();

    config.data = {
      user: this.user
    };

    config.width = '630px';

    this.dialog.open(ChangeUserInfoComponent, config);
  }
}
