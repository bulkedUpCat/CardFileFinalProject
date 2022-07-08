import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TextMaterialParameters, TextMaterialParams } from 'src/app/models/parameters/TextMaterialParameters';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { User } from 'src/app/models/user/User';
import { AuthService } from 'src/app/services/auth.service';
import { SharedUserPageParamsService } from 'src/app/services/shared-user-page-params.service';
import { TextMaterialService } from 'src/app/services/text-material.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.css']
})
export class UserPageComponent implements OnInit {
  textMaterials: TextMaterial[];
  textMaterialParams: TextMaterialParameters = new TextMaterialParams();
  totalCount: number;
  paginator: any;
  loggedUserEmail: string;
  userId: string;
  user: User;
  totalLikeCount: number = 0;

  constructor(private route: ActivatedRoute,
    private userService: UserService,
    private textMaterialService: TextMaterialService,
    private sharedParams: SharedUserPageParamsService,
    private authService: AuthService,
    public router: Router) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.userId = id;

    this.getLoggedUserId();
    this.getUserById(id);
    this.configureTextMaterialParams();
    this.getTextMaterialsOfUser(id);
  }

  getLoggedUserId(){
    this.authService.getUserInfo().subscribe(res => {
      if (res){
        this.loggedUserEmail = res.email;
      }
    }, err => console.log(err));
  }

  getUserById(id: string){
    this.userService.getUserById(id).subscribe(u => {
      this.user = u;
    }, err => {
      console.log(err);
    });
  }

  configureTextMaterialParams(){
    this.textMaterialParams.pageSize = this.sharedParams.pageSize;
    this.textMaterialParams.pageNumber = this.sharedParams.pageNumber;
    this.textMaterialParams.approvalStatus = [1];
  }

  getTextMaterialsOfUser(userId: string){
    this.textMaterialService.getTextMaterialsByUserId(userId, this.textMaterialParams).subscribe(tm => {
      this.textMaterials = tm.body;
      this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
      this.totalCount = this.paginator.TotalCount;
      this.getTotalLikeCount();
    }, err => {
      console.log(err);
    });
  }

  getTotalLikeCount(){
    for (let i = 0; i < this.textMaterials.length; i++){
      this.totalLikeCount += this.textMaterials[i].likesCount;
    }
  }

  sendTextMaterialsOfUser(){
    this.userService.sendListOfTextMaterials(this.userId, this.loggedUserEmail).subscribe(res => {
      console.log('sent');
    }, err => console.log(err));
  }

  onNextPage(page: number){
    this.textMaterialParams.pageNumber = page;
    this.sharedParams.pageNumber = page;

    this.textMaterialService.getTextMaterialsByUserId(this.userId, this.textMaterialParams).subscribe(tm => {
      this.textMaterials = tm.body;
    }, err => {
      console.log(err);
    });
  }

  onPreviousPage(page: number){
    this.textMaterialParams.pageNumber = page;
    this.sharedParams.pageNumber = page;

    this.textMaterialService.getTextMaterialsByUserId(this.userId, this.textMaterialParams).subscribe(tm => {
      this.textMaterials = tm.body;
    }, err => {
      console.log(err);
    });
  }
}
