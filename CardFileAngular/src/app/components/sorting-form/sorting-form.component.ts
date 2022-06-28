import { DatePipe } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TextMaterialParameters, TextMaterialParams } from 'src/app/models/parameters/TextMaterialParameters';
import { AuthService } from 'src/app/services/auth.service';
import { SharedHomeParamsService } from 'src/app/services/shared-home-params.service';
import { SharedParamsService } from 'src/app/services/shared-params.service';
import { TextMaterialService } from 'src/app/services/text-material.service';


@Component({
  selector: 'app-sorting-form',
  templateUrl: './sorting-form.component.html',
  styleUrls: ['./sorting-form.component.css']
})
export class SortingFormComponent implements OnInit {
  sortingParamsForm: FormGroup;
  textMaterialParams: TextMaterialParams = new TextMaterialParams();
  isManager: boolean;
  isAdmin: boolean;

  //@Input() userId: string;
  @Input() isHomePage: boolean;

  @Output() filter : EventEmitter<TextMaterialParameters> = new EventEmitter<TextMaterialParameters>();

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private sharedParams: SharedParamsService,
    private sharedHomeParams: SharedHomeParamsService,
    private datePipe: DatePipe) { }

  ngOnInit(): void {

    if (this.isHomePage){
      this.createHomeForm();
    }
    else{
      this.createForm();
    }

    if (this.isHomePage){
      this.configureHomeTextMaterialParams();
    }
    else{
      this.configureTextMaterialParams();
    }

    this.authService.claims.subscribe( c => {
      if (c){
        this.isManager = c.includes('Manager');
        this.isAdmin = c.includes('Admin');
      }
    });
  }

  configureTextMaterialParams(){
    this.textMaterialParams.orderBy = this.sharedParams.orderBy;
    this.textMaterialParams.filterFromDate = this.sharedParams.filterFromDate;
    this.textMaterialParams.filterToDate = this.sharedParams.filterToDate;
    //this.textMaterialParams.minLikesCount = this.sharedParams.minLikesCount;

    if (!this.isManager){
      this.textMaterialParams.approvalStatus = [];
      this.textMaterialParams.approvalStatus.push(1);
    }
    else if (!this.isAdmin){
      this.textMaterialParams.approvalStatus = [];
      this.textMaterialParams.approvalStatus.push(0,1);
    }
    else{
      this.textMaterialParams.approvalStatus = this.sharedParams.approvalStatus;
    }

    this.textMaterialParams.approvalStatus = this.sharedParams.approvalStatus;
    this.textMaterialParams.searchTitle = this.sharedParams.searchTitle;
    this.textMaterialParams.searchCategory = this.sharedParams.searchCategory;
    this.textMaterialParams.searchAuthor = this.sharedParams.searchAuthor;
  }

  configureHomeTextMaterialParams(){
    this.textMaterialParams.orderBy = this.sharedHomeParams.orderBy;
    this.textMaterialParams.filterFromDate = this.sharedHomeParams.filterFromDate;
    this.textMaterialParams.filterToDate = this.sharedHomeParams.filterToDate;

    if (!this.isManager){
      this.textMaterialParams.approvalStatus = [];
      this.textMaterialParams.approvalStatus.push(1);
    }
    else if (!this.isAdmin){
      this.textMaterialParams.approvalStatus = [];
      this.textMaterialParams.approvalStatus.push(0,1);
    }
    else{
      this.textMaterialParams.approvalStatus = this.sharedHomeParams.approvalStatus;
    }

    this.textMaterialParams.approvalStatus = this.sharedHomeParams.approvalStatus;
    this.textMaterialParams.searchTitle = this.sharedHomeParams.searchTitle;
    this.textMaterialParams.searchCategory = this.sharedHomeParams.searchCategory;
    this.textMaterialParams.searchAuthor = this.sharedHomeParams.searchAuthor;
  }

  createForm(){
    this.sortingParamsForm = this.fb.group({
      sortByTitle: [this.configureSortByTitle()],
      sortByDatePublished: [this.configureSortByDatePublished()],
      sortByRejectCount: [this.configureSortByRejectCount()],
      filterFromDate: [this.sharedParams.filterFromDate],
      filterToDate: [this.sharedParams.filterToDate],
      pending: [this.sharedParams.approvalStatus.includes(0)],
      approved: [this.sharedParams.approvalStatus.includes(1)],
      rejected: [this.sharedParams.approvalStatus.includes(2)],
      searchTitle: [this.sharedParams.searchTitle],
      searchCategory: [this.sharedParams.searchCategory],
      searchAuthor: [this.sharedParams.searchAuthor],
    });
  }

  createHomeForm(){
    this.sortingParamsForm = this.fb.group({
      sortByTitle: [this.configureSortByTitleHomePage()],
      sortByDatePublished: [this.configureSortByDatePublishedHome()],
      filterFromDate: [this.sharedHomeParams.filterFromDate],
      filterToDate: [this.sharedHomeParams.filterToDate],
      pending: [this.sharedHomeParams.approvalStatus.includes(0)],
      approved: [this.sharedHomeParams.approvalStatus.includes(1)],
      rejected: [this.sharedHomeParams.approvalStatus.includes(2)],
      searchTitle: [this.sharedHomeParams.searchTitle],
      searchCategory: [this.sharedHomeParams.searchCategory],
      searchAuthor: [this.sharedHomeParams.searchAuthor],
    });
  }

  configureSortByRejectCount(){
    var orderBy = this.sharedParams.orderBy;
    var parts = orderBy?.split(',');
    if (parts){
      for(let i = 0; i < parts.length; i++){
        if (parts[i] == 'rejectCount asc'){
          return true;
        }
        else if (parts[i] == 'rejectCount desc'){
          return false;
        }
      }
    }

    return null;
  }

  configureSortByTitle(){
    var orderBy = this.sharedParams.orderBy;
    var parts = orderBy?.split(',');
    if (parts){
      for(let i = 0; i < parts.length; i++){
        if (parts[i] == 'title asc'){
          return true;
        }
        else if (parts[i] == 'title desc'){
          return false;
        }
      }
    }

    return null;
  }

  configureSortByTitleHomePage(){
    var orderBy = this.sharedHomeParams.orderBy;
    var parts = orderBy?.split(',');
    if (parts){
      for(let i = 0; i < parts.length; i++){
        if (parts[i] == 'title asc'){
          return true;
        }
        else if (parts[i] == 'title desc'){
          return false;
        }
      }
    }

    return null;
  }

  configureSortByDatePublished(){
    var orderBy = this.sharedParams.orderBy;
    var parts = orderBy?.split(',');
    if (parts){
      for (let i = 0; i < parts.length; i++){
        if (parts[i] == 'datePublished asc'){
          return true;
        }
        else if (parts[i] == 'datePublished desc'){
          return false;
        }
      }
    }

    return null;
  }

  configureSortByDatePublishedHome(){
    var orderBy = this.sharedHomeParams.orderBy;
    var parts = orderBy?.split(',');
    if (parts){
      for (let i = 0; i < parts.length; i++){
        if (parts[i] == 'datePublished asc'){
          return true;
        }
        else if (parts[i] == 'datePublished desc'){
          return false;
        }
      }
    }

    return null;
  }

  validateFromDate(){
    let currentDate = new Date();
    var fromDate = new Date(this.sortingParamsForm.get('filterFromDate').value);
    var toDate = new Date(this.sortingParamsForm.get('filterToDate').value);

    if ((fromDate.getTime() > toDate.getTime() && this.sortingParamsForm.get('filterToDate').value) ||
        fromDate.getTime() > currentDate.getTime()){
      this.sortingParamsForm.get('filterFromDate').setValue(this.datePipe.transform(currentDate,'yyyy-MM-dd'));
    }
  }

  validateToDate(){
    let currentDate = new Date();

    var fromDate = new Date(this.sortingParamsForm.get('filterFromDate').value);
    var toDate = new Date(this.sortingParamsForm.get('filterToDate').value);

    if (toDate.getTime() < fromDate.getTime() ||
        toDate.getTime() > currentDate.getTime()){
      this.sortingParamsForm.get('filterToDate').setValue(this.datePipe.transform(currentDate,'yyyy-MM-dd'));
    }
  }

  validateMinLikesCount(){
    if (this.sortingParamsForm.get('minLikesCount').value < 0){
      this.sortingParamsForm.get('minLikesCount').setValue(0);
    }
  }

  onSubmit(){
    this.textMaterialParams.filterFromDate = this.sortingParamsForm.get('filterFromDate').value;

    this.textMaterialParams.filterToDate = this.sortingParamsForm.get('filterToDate').value;

    //this.textMaterialParams.minLikesCount = this.sortingParamsForm.get('minLikesCount').value;

    this.textMaterialParams.approvalStatus = [];

    if (this.sortingParamsForm.get('pending').value){
      this.textMaterialParams.approvalStatus.push(0);
    }

    if (this.sortingParamsForm.get('approved').value){
      this.textMaterialParams.approvalStatus.push(1);
    }

    if (this.sortingParamsForm.get('rejected').value){
      this.textMaterialParams.approvalStatus.push(2);
    }

    this.textMaterialParams.searchTitle = this.sortingParamsForm.get('searchTitle').value;

    this.textMaterialParams.searchCategory = this.sortingParamsForm.get('searchCategory').value;

    this.textMaterialParams.searchAuthor = this.sortingParamsForm.get('searchAuthor').value;

    if (this.isHomePage){
      this.textMaterialParams.pageNumber = this.sharedHomeParams.pageNumber;
      this.textMaterialParams.pageSize = this.sharedHomeParams.pageSize;
    }else{
      this.textMaterialParams.pageNumber = this.sharedParams.pageNumber;
      this.textMaterialParams.pageSize = this.sharedParams.pageSize;
    }

    this.filter.emit(this.textMaterialParams);
  }

  handleChangeSortByTitle(){
    if (this.textMaterialParams.orderBy == null){
      this.textMaterialParams.orderBy = '';
    }

    if (this.sortingParamsForm.get('sortByTitle').value){
      if (!this.textMaterialParams.orderBy.includes('title asc,')){
        if (this.textMaterialParams.orderBy.includes('title desc,')){
          this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('title desc,','');
        }
        this.textMaterialParams.orderBy += 'title asc,';
      }
    }
    else
    {
      if (!this.textMaterialParams.orderBy.includes('title desc,')){
        if (this.textMaterialParams.orderBy.includes('title asc,')){
          this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('title asc,','');
        }
        this.textMaterialParams.orderBy += 'title desc,';
      }
    }

    console.log(this.textMaterialParams.orderBy);

    this.onSubmit();
  }

  resetSortByTitle(status: boolean){
    if (status && this.textMaterialParams?.orderBy?.includes('title asc,')){
      this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('title asc,','');
      this.sortingParamsForm.get('sortByTitle').setValue(null);
    }
    else if (!status && this.textMaterialParams?.orderBy?.includes('title desc,')){
      this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('title desc,','');
      this.sortingParamsForm.get('sortByTitle').setValue(null);
    }

    this.onSubmit();
  }

  handleChangeSortByPublishmentDate(){
    if (this.textMaterialParams.orderBy == null){
      this.textMaterialParams.orderBy = '';
    }

    if (this.sortingParamsForm.get('sortByDatePublished').value){
      if (!this.textMaterialParams.orderBy.includes('datePublished asc,')){
        if (this.textMaterialParams.orderBy.includes('datePublished desc,')){
          this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('datePublished desc,','');
        }

        this.textMaterialParams.orderBy += 'datePublished asc,';
      }
    }
    else
    {
      if (!this.textMaterialParams.orderBy.includes('datePublished desc,')){
        if (this.textMaterialParams.orderBy.includes('datePublished asc,')){
          this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('datePublished asc,','');
        }

        this.textMaterialParams.orderBy += 'datePublished desc,';
      }
    }

    console.log(this.textMaterialParams.orderBy);

    this.onSubmit();
  }

  resetSortByPublishmentDate(status: boolean){
    if (status && this.textMaterialParams.orderBy.includes('datePublished asc,')){
      this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('datePublished asc,','');
      this.sortingParamsForm.get('sortByDatePublished').setValue(null);
    }
    else if (!status && this.textMaterialParams.orderBy.includes('datePublished desc,')){
      this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('datePublished desc,','');
      this.sortingParamsForm.get('sortByDatePublished').setValue(null);
    }

    this.onSubmit();
  }

  handleChangeSortByRejectCount(){
    if (this.textMaterialParams.orderBy == null){
      this.textMaterialParams.orderBy = '';
    }

    if (this.sortingParamsForm.get('sortByRejectCount').value){
      if (!this.textMaterialParams.orderBy.includes('rejectCount asc,')){
        if (this.textMaterialParams.orderBy.includes('rejectCount desc,')){
          this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('rejectCount desc,','');
        }

        this.textMaterialParams.orderBy += 'rejectCount asc,';
      }
    }
    else
    {
      if (!this.sortingParamsForm.get('sortByRejectCount').value){
        if (!this.textMaterialParams.orderBy.includes('rejectCount desc,')){
          if (this.textMaterialParams.orderBy.includes('rejectCount asc,')){
            this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('rejectCount asc,','');
          }

          this.textMaterialParams.orderBy += 'rejectCount desc,';
        }
      }
    }

    console.log(this.textMaterialParams.orderBy);

    this.onSubmit();
  }

  resetSortByRejectCount(status: boolean){
    if (status && this.textMaterialParams.orderBy.includes('rejectCount asc,')){
      this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('rejectCount asc,','');
      this.sortingParamsForm.get('sortByRejectCount').setValue(null);
    }
    else if (!status && this.textMaterialParams.orderBy.includes('rejectCount desc,')){
      this.textMaterialParams.orderBy = this.textMaterialParams.orderBy.replace('rejectCount desc,','');
      this.sortingParamsForm.get('sortByRejectCount').setValue(null);
    }

    this.onSubmit();
  }
}
