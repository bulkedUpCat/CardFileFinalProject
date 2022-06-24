import { ThisReceiver } from '@angular/compiler';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MaterialCategory } from 'src/app/models/MaterialCategory';
import { DeleteCategoryComponent } from '../dialogs/delete-category/delete-category.component';

@Component({
  selector: 'app-material-category',
  templateUrl: './material-category.component.html',
  styleUrls: ['./material-category.component.css']
})
export class MaterialCategoryComponent implements OnInit {
  @Input() category: MaterialCategory;
  @Output() deleted = new EventEmitter<boolean>();

  constructor(private dialog: MatDialog) { }

  ngOnInit(): void {

  }

  deleteCategory(){
    let dialogRef = this.dialog.open(DeleteCategoryComponent,{
      data: {
        category: this.category
      }
    });

    dialogRef.afterClosed().subscribe(x => {
      this.deleted.emit(true);
    });
  }
}
