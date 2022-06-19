import { Component, OnInit } from '@angular/core';
import { TextMaterialParameters, TextMaterialParams } from 'src/app/models/parameters/TextMaterialParameters';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { AuthService } from 'src/app/services/auth.service';
import { SharedHomeParamsService } from 'src/app/services/shared-home-params.service';
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
    private sharedHomeParams: SharedHomeParamsService) { }

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
}
