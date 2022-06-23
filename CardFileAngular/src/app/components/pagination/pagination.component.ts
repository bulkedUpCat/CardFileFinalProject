import { Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges } from '@angular/core';
import { SharedHomeParamsService } from 'src/app/services/shared-home-params.service';
import { SharedParamsService } from 'src/app/services/shared-params.service';
import { SharedUserPageParamsService } from 'src/app/services/shared-user-page-params.service';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {
  currentPage: number = 1;

  @Input() isHomePage: boolean;
  @Input() isUserPage: boolean;
  @Input() totalPages: number;

  @Output() goTo: EventEmitter<number> = new EventEmitter<number>();
  @Output() next: EventEmitter<number> = new EventEmitter<number>();
  @Output() previous: EventEmitter<number> = new EventEmitter<number>();

  constructor(private sharedParams: SharedParamsService,
    private sharedHomeParams: SharedHomeParamsService,
    private sharedUserPageParams: SharedUserPageParamsService) { }

  ngOnInit(): void {
    if (this.isHomePage){
      this.currentPage = this.sharedHomeParams.pageNumber;
    }
    else if(this.isUserPage){
      this.currentPage = this.sharedUserPageParams.pageNumber;
    }
    else{
      this.currentPage = this.sharedParams.pageNumber;
    }
  }

  onGoTo(page: number){
    this.goTo.emit(page);
  }

  onNext(){
    this.currentPage++;
    this.next.emit(this.currentPage);
  }

  onPrevious(){
    this.currentPage--;
    this.previous.emit(this.currentPage);
  }
}