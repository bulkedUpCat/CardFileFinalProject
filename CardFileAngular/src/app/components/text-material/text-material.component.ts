import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MaterialCategory } from 'src/app/models/MaterialCategory';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { MaterialCategoryService } from 'src/app/services/material-category.service';

@Component({
  selector: 'app-text-material',
  templateUrl: './text-material.component.html',
  styleUrls: ['./text-material.component.css']
})
export class TextMaterialComponent implements OnInit {
  @Input() textMaterial: TextMaterial;

  constructor(private router: Router) { }

  ngOnInit(): void {

  }

  onShow(){
    console.log('show');
    this.router.navigateByUrl('main/' + this.textMaterial.id);
  }
}
