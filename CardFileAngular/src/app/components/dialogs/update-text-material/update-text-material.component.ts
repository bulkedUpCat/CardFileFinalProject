import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { HtmlEditorService, ImageService, LinkService, ToolbarService } from '@syncfusion/ej2-angular-richtexteditor';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { TextMaterialService } from 'src/app/services/text-material.service';

@Component({
  selector: 'app-update-text-material',
  templateUrl: './update-text-material.component.html',
  styleUrls: ['./update-text-material.component.css'],
  providers: [ToolbarService, LinkService, ImageService, HtmlEditorService]
})
export class UpdateTextMaterialComponent implements OnInit {
  textMaterialForm: FormGroup;
  textMaterial: TextMaterial;
  submitted: boolean;
  public tools: object = {
    items: [
      'Undo','Redo','Bold','Italic','FontSize'
    ]
  };

  constructor(@Inject(MAT_DIALOG_DATA) public data,
    private dialogRef: MatDialogRef<UpdateTextMaterialComponent>,
    private fb: FormBuilder,
    private textMaterialService: TextMaterialService,
    private router: Router) { }

  ngOnInit(): void {
    this.textMaterial = this.data.textMaterial;
    this.createTextMaterialForm();
  }

  createTextMaterialForm(){
    this.textMaterialForm = this.fb.group({
      id: [this.data.textMaterialId],
      title: [this.data.textMaterial?.title, [Validators.required,Validators.minLength(5),Validators.maxLength(100)]],
      categoryTitle: [this.data.textMaterial?.categoryTitle,[Validators.required]],
      content: [this.data.textMaterial?.content,[Validators.required]],
      authorId: [this.data.textMaterial.authorId]
    });
  }

  get title(){
    return this.textMaterialForm.get('title');
  }

  get category(){
    return this.textMaterialForm.get('categoryTitle');
  }

  get content(){
    return this.textMaterialForm.get('content');
  }

  onUpdate(){
    this.submitted = true;
    console.log(this.textMaterialForm.value);

    if (!this.textMaterialForm.valid){
      console.log('update form is invalid');
      return;
    }

    this.textMaterial = this.textMaterialForm.value;

    this.textMaterialService.updateTextMaterial(this.textMaterial).subscribe(tm => {
      this.router.navigateByUrl('/home-page');
      this.dialogRef.close();
    }, err => {
      console.log(err);
    });
  }

}
