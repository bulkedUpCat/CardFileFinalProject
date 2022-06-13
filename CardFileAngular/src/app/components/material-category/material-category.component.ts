import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-material-category',
  templateUrl: './material-category.component.html',
  styleUrls: ['./material-category.component.css']
})
export class MaterialCategoryComponent implements OnInit {
  @Input() category: any;
  constructor() { }

  ngOnInit(): void {

  }

}
