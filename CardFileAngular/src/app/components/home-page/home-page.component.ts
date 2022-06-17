import { Component, OnInit } from '@angular/core';
import { TextMaterialParameters, TextMaterialParams } from 'src/app/models/parameters/TextMaterialParameters';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { AuthService } from 'src/app/services/auth.service';
import { SharedParamsService } from 'src/app/services/shared-params.service';
import { TextMaterialService } from 'src/app/services/text-material.service';

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

  constructor(private authService: AuthService,
    private textMaterialService: TextMaterialService,
    private sharedParams: SharedParamsService) { }

  ngOnInit(): void {
    this.authService.getUserInfo().subscribe( u => {
      if (u){
        this.userId = u.sub;
        this.userName = u.name;
      }
    });

    this.configureTextMaterialParams();
    this.getOwnTextMaterials();
  }

  configureTextMaterialParams(){
    this.textMaterialParams.orderBy = this.sharedParams.orderBy;
    this.textMaterialParams.filterFromDate = this.sharedParams.filterFromDate;
    this.textMaterialParams.filterToDate = this.sharedParams.filterToDate;
    this.textMaterialParams.approvalStatus = this.sharedParams.approvalStatus;
    this.textMaterialParams.searchTitle = this.sharedParams.searchTitle;
    this.textMaterialParams.searchCategory = this.sharedParams.searchCategory;
    this.textMaterialParams.searchAuthor = this.sharedParams.searchAuthor;
    this.textMaterialParams.pageNumber = this.sharedParams.pageNumber;
    this.textMaterialParams.pageSize = this.sharedParams.pageSize;
  }

  // getAllTextMaterials(){
  //   this.textMaterialService.showApproved.next(null);
  // }

  // getApprovedMaterials(){
  //   this.textMaterialService.showApproved.next(true.toString());
  // }

  // getRejectedMaterials(){
  //   this.textMaterialService.showApproved.next(false.toString());
  // }

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
    this.textMaterialService.getTextMaterialsByUserId(this.userId, parameters).subscribe( tm => {
      this.ownTextMaterials = tm.body;
      this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
    }, err => {
      console.log(err);
    });
  }

  onNextPage(page: number){
    this.textMaterialParams.pageNumber = page;
    this.sharedParams.pageNumber = page;

    this.textMaterialService.getTextMaterials(this.textMaterialParams).subscribe(tm => {
      this.ownTextMaterials = tm.body;
    }, err => {
      console.log(err);
    });
  }

  onPreviousPage(page: number){
    this.textMaterialParams.pageNumber = page;
    this.sharedParams.pageNumber = page;

    this.textMaterialService.getTextMaterials(this.textMaterialParams).subscribe(tm => {
      this.ownTextMaterials = tm.body;
    }, err => {
      console.log(err);
    });
  }
}
