import { ThisReceiver } from '@angular/compiler';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TextMaterialParameters, TextMaterialParams } from 'src/app/models/parameters/TextMaterialParameters';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { AuthService } from 'src/app/services/auth.service';
import { MaterialCategoryService } from 'src/app/services/material-category.service';
import { TextMaterialService } from 'src/app/services/text-material.service';

@Component({
  selector: 'app-text-materials',
  templateUrl: './text-materials.component.html',
  styleUrls: ['./text-materials.component.css']
})
export class TextMaterialsComponent implements OnInit {
  textMaterials: TextMaterial[];
  textMaterialParams: TextMaterialParameters = new TextMaterialParams();
  isManager: boolean;
  isAdmin: boolean;

  @Input() userId: string;

  constructor(private textMaterialService: TextMaterialService,
    private authService: AuthService) { }

  ngOnInit(): void {
    this.textMaterialParams.userId = this.userId;

    this.authService.claims.subscribe( c => {
      if (c){
        this.isManager = c.includes('Manager');
      }
    })

    this.getTextMaterials();
  }

  getTextMaterials(){
    return this.textMaterialService.getTextMaterials(this.textMaterialParams).subscribe( tm => {
      this.textMaterials = tm;

      if (this.isAdmin){
        return;
      }

      if (this.isManager){
        this.textMaterials = this.textMaterials.filter(x => x.approvalStatusId == 0 || x.approvalStatusId == 1);
        return;
      }

      if (!this.userId && !this.isManager){
        this.textMaterials = this.textMaterials.filter(x => x.approvalStatusId == 1);
        return;
      }
    });
  }

  onFilter(parameters: TextMaterialParameters){
    this.textMaterialService.getTextMaterials(parameters).subscribe( tm => {

      if (this.userId){
        this.textMaterials = tm;
        return;
      }

      if (!this.isManager){
        this.textMaterials = tm.filter(x => x.approvalStatusId == 1);
        return;
      }

      if (!this.isAdmin){
        this.textMaterials = tm.filter(x => x.approvalStatusId != 2);
        return;
      }

      this.textMaterials = tm;
    });
  }
}
