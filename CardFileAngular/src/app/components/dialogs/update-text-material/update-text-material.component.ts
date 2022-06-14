import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { HtmlEditorService, ImageService, LinkService, ToolbarService } from '@syncfusion/ej2-angular-richtexteditor';
import { TextMaterial } from 'src/app/models/TextMaterial';

@Component({
  selector: 'app-update-text-material',
  templateUrl: './update-text-material.component.html',
  styleUrls: ['./update-text-material.component.css'],
  providers: [ToolbarService, LinkService, ImageService, HtmlEditorService]
})
export class UpdateTextMaterialComponent implements OnInit {
  textMaterialForm: FormGroup;
  textMaterial: TextMaterial;
  public tools: object = {
    items: [
      'Undo','Redo','Bold','Italic','FontSize'
    ]
  };

  constructor(@Inject(MAT_DIALOG_DATA) public data,
    private dialogRef: MatDialogRef<UpdateTextMaterialComponent>,
    private fb: FormBuilder) { }

  ngOnInit(): void {
    this.textMaterial = this.data.textMaterial;
    console.log(this.textMaterial);
    this.createTextMaterialForm();
  }

  createTextMaterialForm(){
    this.textMaterialForm = this.fb.group({
      id: [this.data.textMaterialId],
      title: [this.data.textMaterial?.title, [Validators.required,Validators.minLength(5),Validators.maxLength(100)]],
      categoryTitle: [this.data.textMaterial?.categoryTitle,[Validators.required]],
      content: [this.data.textMaterial?.content,[Validators.required]],
      authorId: [this.data.authorId]
    });
  }

  get content(){
    return this.textMaterialForm.get('content');
  }

  onUpdate(){
    //console.log(this.textMaterialForm.value);
  }

}
