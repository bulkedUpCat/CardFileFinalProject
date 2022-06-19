import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TextMaterialParameters, TextMaterialParams } from 'src/app/models/parameters/TextMaterialParameters';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { User } from 'src/app/models/user/User';
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
  userId: string;
  user: User;

  constructor(private route: ActivatedRoute,
    private userService: UserService,
    private textMaterialService: TextMaterialService,
    private sharedParams: SharedUserPageParamsService) { }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    this.userId = id;

    this.getUserById(id);
    this.configureTextMaterialParams();
    this.getTextMaterialsOfUser(id);
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
    }, err => {
      console.log(err);
    });
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
