import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatDialogModule } from '@angular/material/dialog'
import { MatSnackBarModule } from '@angular/material/snack-bar'


const materialModules = [
  MatIconModule,
  MatDialogModule,
  MatSnackBarModule
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
