import { ThisReceiver } from '@angular/compiler';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { EmailParams } from 'src/app/models/parameters/EmailParameters';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { AuthService } from 'src/app/services/auth.service';
import { TextMaterialService } from 'src/app/services/text-material.service';
import { EmailPdfComponent } from '../dialogs/email-pdf/email-pdf.component';

@Component({
  selector: 'app-text-material-detail',
  templateUrl: './text-material-detail.component.html',
  styleUrls: ['./text-material-detail.component.css']
})
export class TextMaterialDetailComponent implements OnInit {
  textMaterial: TextMaterial;
  isManager: boolean = false;
  loadedStatus: boolean = false;
  isLoggedIn: boolean;

  constructor(private route: ActivatedRoute,
    private textMaterialService: TextMaterialService,
    private authService: AuthService,
    private router: Router,
    private dialog: MatDialog) { }

  ngOnInit(): void {
    const id = parseInt(this.route.snapshot.paramMap.get('id'));

    this.textMaterialService.getTextMaterialById(id).subscribe( tm => {
      this.loadedStatus = true;
      this.textMaterial = tm;
    });

    this.checkIfUserIsLoggedIn();

    this.authService.claims.subscribe(c => {
      if (c){
        this.isManager = c.includes('Manager');
      }
    });
  }

  checkIfUserIsLoggedIn(){
    this.authService.isLoggedIn.subscribe(u => {
      this.isLoggedIn = u;
    }, err => {
      console.log(err);
    });
  }

  approveTextMaterial(){
    this.textMaterial.approvalStatusId = 1;
    this.textMaterialService.approveTextMaterial(this.textMaterial.id).subscribe(x => {
      this.textMaterialService.getTextMaterialById(this.textMaterial.id).subscribe(tm => {
        this.textMaterial = tm;
        this.router.navigateByUrl('/main');
      }, err => {
        console.log(err);
      });
    },err => {
      console.log(err);
    });
  }

  rejectTextMaterial(){
    this.textMaterial.approvalStatusId = 2;
    this.textMaterialService.rejectTextMaterial(this.textMaterial.id).subscribe(x => {
      this.textMaterialService.getTextMaterialById(this.textMaterial.id).subscribe(tm => {
        this.textMaterial = tm;
        this.router.navigateByUrl('/main');
      }, err => {
        console.log(err);
      })
    }, err => {
      console.log(err);
    });
  }

  sendTextMaterialAsPdf(){
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data  = {
      textMaterialId: this.textMaterial.id
    }

    this.dialog.open(EmailPdfComponent,dialogConfig);
  }
}
