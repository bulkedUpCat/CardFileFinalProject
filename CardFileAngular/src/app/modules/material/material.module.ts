import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog'


const materialModules = [
  MatIconModule,
  MatDialogModule
]

@NgModule({
  declarations: [],
  exports: [
    ...materialModules,
  ],
  imports: [
    CommonModule,
    ...materialModules
  ]
})
export class MaterialModule { }
