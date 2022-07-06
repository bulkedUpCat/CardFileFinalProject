
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TextMaterialParameters, TextMaterialParams } from 'src/app/models/parameters/TextMaterialParameters';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { AuthService } from 'src/app/services/auth.service';
import { SharedParamsService } from 'src/app/services/shared-params.service';
import { TextMaterialService } from 'src/app/services/text-material.service';

@Component({
  selector: 'app-text-materials',
  templateUrl: './text-materials.component.html',
  styleUrls: ['./text-materials.component.css']
})
export class TextMaterialsComponent implements OnInit {
  textMaterials: TextMaterial[] = [];
  paginator: any;
  textMaterialParams: TextMaterialParameters = new TextMaterialParams();
  isManager: boolean;
  isAdmin: boolean;
  showSaved: boolean = this.sharedParams.showSaved;
  userId: string;

  constructor(private textMaterialService: TextMaterialService,
    private authService: AuthService,
    public sharedParams: SharedParamsService,
    public router: Router) { }

  ngOnInit(): void {
    this.authService.getUserInfo().subscribe(u => {
      if (u){
        this.userId = u.sub;
      }
    });

    this.authService.claims.subscribe( c => {
      if (c){
        this.isManager = c.includes('Manager');
        this.isAdmin = c.includes('Admin');
      }

      this.configureTextMaterialParams();
      this.getTextMaterials();
    });
  }

  configureTextMaterialParams(){
    this.textMaterialParams.orderBy = this.sharedParams.orderBy;
    this.textMaterialParams.filterFromDate = this.sharedParams.filterFromDate;
    this.textMaterialParams.filterToDate = this.sharedParams.filterToDate;

    if (!this.isManager){
      this.textMaterialParams.approvalStatus = [];
      this.textMaterialParams.approvalStatus.push(1);
    }
    else if (!this.isAdmin && this.sharedParams.approvalStatus.length == 0){
      this.textMaterialParams.approvalStatus = [];
      this.textMaterialParams.approvalStatus.push(0,1);
    }
    else{
      this.textMaterialParams.approvalStatus = this.sharedParams.approvalStatus;
    }

    this.textMaterialParams.searchTitle = this.sharedParams.searchTitle;
    this.textMaterialParams.searchCategory = this.sharedParams.searchCategory;
    this.textMaterialParams.searchAuthor = this.sharedParams.searchAuthor;
    this.textMaterialParams.pageNumber = this.sharedParams.pageNumber;
    this.textMaterialParams.pageSize = this.sharedParams.pageSize;
  }

  getTextMaterials(){
    if (!this.showSaved){
      this.textMaterialService.getTextMaterials(this.textMaterialParams).subscribe( tm => {
        this.textMaterials = tm.body;
        this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
      });
    }
    else{
      this.textMaterialService.getSavedTextMaterials(this.userId,this.textMaterialParams).subscribe( tm => {
        this.textMaterials = tm.body;
        this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
      });
    }
  }

  onShowSaved(){
    this.showSaved = true;
    this.sharedParams.showSaved = true;

    this.textMaterialService.getSavedTextMaterials(this.userId,this.textMaterialParams).subscribe( tm => {
      this.textMaterials = tm.body;
      this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
    });
  }

  onShowAll(){
    this.showSaved = false;
    this.sharedParams.showSaved = false;

    this.textMaterialService.getTextMaterials(this.textMaterialParams).subscribe( tm => {
      this.textMaterials = tm.body;
      this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
    });
  }

  onFilter(parameters: TextMaterialParameters){
      this.textMaterialParams = parameters;

      this.sharedParams.orderBy = parameters.orderBy;

      this.sharedParams.filterFromDate = parameters.filterFromDate;
      this.sharedParams.filterToDate = parameters.filterToDate;
      this.sharedParams.approvalStatus = parameters.approvalStatus;
      this.sharedParams.searchTitle = parameters.searchTitle;
      this.sharedParams.searchCategory = parameters.searchCategory;
      this.sharedParams.searchAuthor = parameters.searchAuthor;

      if (!this.isManager){
        if (this.textMaterialParams.approvalStatus.includes(0)){
          const index = this.textMaterialParams.approvalStatus.indexOf(0);
          this.textMaterialParams.approvalStatus.splice(index,1);
        }
        this.textMaterialParams.approvalStatus.push(1);
        this.sharedParams.approvalStatus = this.textMaterialParams.approvalStatus;
      }
      else if(!this.isAdmin && this.textMaterialParams.approvalStatus.includes(2)){
        const index = this.textMaterialParams.approvalStatus.indexOf(2);
        this.textMaterialParams.approvalStatus.splice(index,1);
        this.sharedParams.approvalStatus = this.textMaterialParams.approvalStatus;
      }
      else if(!this.isAdmin && this.textMaterialParams.approvalStatus.length == 0){
        this.textMaterialParams.approvalStatus.push(0,1);
        this.sharedParams.approvalStatus = this.textMaterialParams.approvalStatus;
      }
      else{
        this.sharedParams.approvalStatus = this.textMaterialParams.approvalStatus;
      }

      if (!this.sharedParams.showSaved){
        this.textMaterialService.getTextMaterials(this.textMaterialParams).subscribe( tm => {
          this.textMaterials = tm.body;
          this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
        }, err => {
            console.log(err);
        });
      }
      else{
        this.textMaterialService.getSavedTextMaterials(this.userId,this.textMaterialParams).subscribe( tm => {
          this.textMaterials = tm.body;
          this.paginator = JSON.parse(tm.headers.get('X-Pagination'));
        }, err => {
            console.log(err);
        });
      }

  }

  onNextPage(page: number){
    this.textMaterialParams.pageNumber = page;
    this.sharedParams.pageNumber = page;

    if (!this.showSaved){
      this.textMaterialService.getTextMaterials(this.textMaterialParams).subscribe(tm => {
      this.textMaterials = tm.body;
      }, err => {
        console.log(err);
      });
    }
    else{
      this.textMaterialService.getSavedTextMaterials(this.userId,this.textMaterialParams).subscribe( tm => {
        this.textMaterials = tm.body;
      }, err => {
          console.log(err);
      });
    }
  }

  onPreviousPage(page: number){
    this.textMaterialParams.pageNumber = page;
    this.sharedParams.pageNumber = page;

    if (!this.showSaved){
      this.textMaterialService.getTextMaterials(this.textMaterialParams).subscribe(tm => {
      this.textMaterials = tm.body;
      }, err => {
        console.log(err);
      });
    }
    else{
      this.textMaterialService.getSavedTextMaterials(this.userId,this.textMaterialParams).subscribe( tm => {
        this.textMaterials = tm.body;
      }, err => {
          console.log(err);
      });
    }
  }
}
