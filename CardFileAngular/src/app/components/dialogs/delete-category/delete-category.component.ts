import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MaterialCategory } from 'src/app/models/MaterialCategory';
import { MaterialCategoryService } from 'src/app/services/material-category.service';
import { NotifierService } from 'src/app/services/notifier.service';

@Component({
  selector: 'app-delete-category',
  templateUrl: './delete-category.component.html',
  styleUrls: ['./delete-category.component.css']
})
export class DeleteCategoryComponent implements OnInit {
  category: MaterialCategory;

  constructor(private fb: FormBuilder,
    public dialogRef: MatDialogRef<DeleteCategoryComponent>,
    @Inject(MAT_DIALOG_DATA) private data,
    private categoryService: MaterialCategoryService,
    private notifier: NotifierService) { }

  ngOnInit(): void {
    this.category = this.data.category;
  }

  deleteCategory(){
    this.categoryService.deleteMaterialCategory(this.category.id).subscribe(res => {
      this.notifier.showNotification(`Category ${this.category.title} was deleted`,'Ok','SUCCESS');
      this.dialogRef.close();
    }, err => this.notifier.showNotification(err.error,'OK','ERROR'));
  }
}
