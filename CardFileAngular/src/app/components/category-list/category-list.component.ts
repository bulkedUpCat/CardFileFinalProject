import { Component, OnInit, Optional } from '@angular/core';
import { MatOptionSelectionChange } from '@angular/material/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { MaterialCategory } from 'src/app/models/MaterialCategory';
import { MaterialCategoryService } from 'src/app/services/material-category.service';
import { AddCategoryComponent } from '../dialogs/add-category/add-category.component';

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.css']
})
export class CategoryListComponent implements OnInit {
  categories: MaterialCategory[] = [];

  constructor(private materialCategoryService: MaterialCategoryService,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories(){
    this.materialCategoryService.getMaterialCategories().subscribe( c => {
      this.categories = c;
    })
  }

  addCategory(){
    let dialogRef = this.dialog.open(AddCategoryComponent);
    dialogRef.afterClosed().subscribe(x => {
      this.materialCategoryService.getMaterialCategories().subscribe(c => {
        this.categories = c;
      }, err => console.log(err));
    });
  }

  refresh(){
    this.materialCategoryService.getMaterialCategories().subscribe(c => {
      this.categories = c;
    }, err => console.log(err));
  }
}
