import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { NotifierService } from 'src/app/services/notifier.service';
import { TextMaterialService } from 'src/app/services/text-material.service';
import { TextMaterialsComponent } from '../../text-materials/text-materials.component';

@Component({
  selector: 'app-delete-text-material',
  templateUrl: './delete-text-material.component.html',
  styleUrls: ['./delete-text-material.component.css']
})
export class DeleteTextMaterialComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) private data,
    public dialogRef: MatDialogRef<DeleteTextMaterialComponent>,
    private textMaterialService: TextMaterialService,
    private router: Router,
    private notifier: NotifierService) { }

  ngOnInit(): void {
  }

  deleteTextMaterial(){
    this.textMaterialService.deleteTextMaterial(this.data.textMaterialId).subscribe(tm => {
      this.dialogRef.close();
      this.notifier.showNotification(`You've just delete a text material with id ${this.data.textMaterialId}`,
                                     "OK","SUCCESS");
      this.router.navigateByUrl('/main');
    }, err => {
      this.notifier.showNotification("Something went wrong. Please try again", "OK", "ERROR");
      console.log(err);
    });
  }
}