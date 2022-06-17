import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { HtmlEditorService, ImageService, LinkService, ToolbarService } from '@syncfusion/ej2-angular-richtexteditor';
import { MaterialCategory } from 'src/app/models/MaterialCategory';
import { AuthService } from 'src/app/services/auth.service';
import { MaterialCategoryService } from 'src/app/services/material-category.service';
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
  submitted: boolean;
  categories: MaterialCategory[];
  public tools: object = {
    items: [
      'Undo','Redo','Bold','Italic','FontSize'
    ]
  };

  constructor(private fb: FormBuilder,
    private authService: AuthService,
    private textMaterialService: TextMaterialService,
    private categoryService: MaterialCategoryService,
    private router: Router) { }

  ngOnInit(): void {
    this.createTextMaterialForm();
    this.getCategories();

    this.authService.getUserInfo().subscribe(u => {
      if (u){
        this.userId = u.sub;
      }
    })
  }

  getCategories(){
    this.categoryService.getMaterialCategories().subscribe(c => {
      this.categories = c;
    }, err => {
      console.log(err);
    });
  }

  createTextMaterialForm(){
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
    this.submitted = true;
    if (!this.textMaterialForm.valid){
      return;
    }

    const textMaterial = this.textMaterialForm.value;
    textMaterial.authorId = this.userId;

    this.textMaterialService.createTextMaterial(textMaterial).subscribe( tm => {
      console.log('created');
      console.log(tm);
      this.router.navigateByUrl('/home-page');
    }, err => {
      console.log(err);
    });
  }
}
