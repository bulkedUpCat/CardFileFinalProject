import { Component, OnInit } from '@angular/core';
import { MaterialCategory } from 'src/app/models/MaterialCategory';
import { MaterialCategoryService } from 'src/app/services/material-category.service';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  categories: MaterialCategory[] = [];

  constructor(private materialCategoryService: MaterialCategoryService) { }

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(){
    this.materialCategoryService.getMaterialCategories().subscribe( c => {
      this.categories = c;
    })
  }
}
