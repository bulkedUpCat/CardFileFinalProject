import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HtmlEditorService, ImageService, LinkService, ToolbarService } from '@syncfusion/ej2-angular-richtexteditor';
import { AuthService } from 'src/app/services/auth.service';
import { TextMaterialService } from 'src/app/services/text-material.service';

@Component({
  selector: 'app-add-text-material',
  templateUrl: './add-text-material.component.html',
  styleUrls: ['./add-text-material.component.css'],
  providers: [ToolbarService, LinkService, ImageService, HtmlEditorService]
})
export class AddTextMaterialComponent implements OnInit {
  textMaterialForm: FormGroup;
  userId: string;
  public tools: object = {
    items: [
      'Undo','Redo','Bold','Italic','FontSize'
    ]
  };

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private textMaterialService: TextMaterialService) { }

  ngOnInit(): void {
    this.createTextMaterial();

    this.authService.getUserInfo().subscribe(u => {
      if (u){
        this.userId = u.jti;
      }
    })
  }

  createTextMaterial(){
    this.textMaterialForm = this.fb.group({
      title: [null, [Validators.required]],
      categoryTitle: [null,[Validators.required]],
      content: [null,[Validators.required]]
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

  onSubmit(){
    const textMaterial = this.textMaterialForm.value;
    textMaterial.authorId = this.userId;

    this.textMaterialService.createTextMaterial(textMaterial).subscribe( tm => {
      console.log('created');
      console.log(tm);
    }, err => {
      console.log(err);
    });
  }
}
