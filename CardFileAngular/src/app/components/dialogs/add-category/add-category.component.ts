import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MaterialCategoryService } from 'src/app/services/material-category.service';
import { NotifierService } from 'src/app/services/notifier.service';

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.css']
})
export class AddCategoryComponent implements OnInit {
  addCategoryForm: FormGroup;
  submitted: boolean;

  constructor(private fb: FormBuilder,
    private categoryService: MaterialCategoryService,
    private dialogRef: MatDialogRef<AddCategoryComponent>,
    private notifier: NotifierService) { }

  ngOnInit(): void {
    this.createAddCategoryForm();
  }

  createAddCategoryForm(){
    this.addCategoryForm = this.fb.group({
      title: [null, [Validators.required]]
    });
  }

  get title(){
    return this.addCategoryForm.get('title');
  }

  onSubmit(){
    this.submitted = true;

    if (!this.addCategoryForm.valid){
      return;
    }

    this.categoryService.createMaterialCategory({title: this.title.value}).subscribe(res => {
      this.dialogRef.close();
    }, err => this.notifier.showNotification(err.error,"OK","ERROR"));
  }
}
