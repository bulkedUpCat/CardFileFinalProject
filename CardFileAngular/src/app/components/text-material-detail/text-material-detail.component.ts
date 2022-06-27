import { Location } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { TextMaterialParams } from 'src/app/models/parameters/TextMaterialParameters';
import { TextMaterial } from 'src/app/models/TextMaterial';
import { AuthService } from 'src/app/services/auth.service';
import { NotifierService } from 'src/app/services/notifier.service';
import { TextMaterialService } from 'src/app/services/text-material.service';
import { UserService } from 'src/app/services/user.service';
import { DeleteTextMaterialComponent } from '../dialogs/delete-text-material/delete-text-material.component';
import { EmailPdfComponent } from '../dialogs/email-pdf/email-pdf.component';
import { RejectTextMaterialComponent } from '../dialogs/reject-text-material/reject-text-material.component';
import { UpdateTextMaterialComponent } from '../dialogs/update-text-material/update-text-material.component';

@Component({
  selector: 'app-text-material-detail',
  templateUrl: './text-material-detail.component.html',
  styleUrls: ['./text-material-detail.component.css']
})
export class TextMaterialDetailComponent implements OnInit {
  textMaterial: TextMaterial;
  savedTextMaterials: TextMaterial[];
  isSaved: boolean;
  isManager: boolean = false;
  isAdmin: boolean = false;
  loadedStatus: boolean = false;
  isLoggedIn: boolean;
  isAuthor: boolean;
  userId: string;
  isLiked: boolean;

  constructor(private route: ActivatedRoute,
    private textMaterialService: TextMaterialService,
    private authService: AuthService,
    private userService: UserService,
    private router: Router,
    private dialog: MatDialog,
    private location: Location,
    private notifier: NotifierService) { }

  ngOnInit(): void {
    const id = parseInt(this.route.snapshot.paramMap.get('id'));

    this.textMaterialService.getTextMaterialById(id).subscribe( tm => {
      this.loadedStatus = true;
      this.textMaterial = tm;

      this.checkIsUserIsAuthor();
      this.getSavedTextMaterials();
      this.getLikedTextMaterials();
    });

    this.checkIfUserIsLoggedIn();

    this.authService.claims.subscribe(c => {
      if (c){
        this.isManager = c.includes('Manager');
        this.isAdmin = c.includes('Admin');
      }
    });
  }

  checkIsUserIsAuthor(){
    this.authService.getUserInfo().subscribe(u => {
      if (u){
        this.userId = u.sub;
        this.isAuthor = u.sub == this.textMaterial?.authorId;
      }
    }, err => {
      console.log(err);
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
    let dialogRef = this.dialog.open(RejectTextMaterialComponent, {
      data: {
        id: this.textMaterial.id
      }
    });

    dialogRef.afterClosed().subscribe(res => {
      this.textMaterial.approvalStatusId = 2;
      this.router.navigateByUrl('/main');
    })
  }

  sendTextMaterialAsPdf(){
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data  = {
      textMaterial: this.textMaterial,
      textMaterialId: this.textMaterial.id
    };

    this.dialog.open(EmailPdfComponent,dialogConfig);
  }

  updateTextMaterial(){
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      textMaterial: this.textMaterial,
      textMaterialId: this.textMaterial.id
    };

    this.dialog.open(UpdateTextMaterialComponent, dialogConfig);
  }

  deleteTextMaterial(){
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      textMaterial: this.textMaterial,
      textMaterialId: this.textMaterial.id
    };

    this.dialog.open(DeleteTextMaterialComponent, dialogConfig);
  }

  getSavedTextMaterials(){
    if (this.userId){
      this.textMaterialService.getSavedTextMaterials(this.userId, new TextMaterialParams()).subscribe(res => {
      this.savedTextMaterials = res.body;

      if (res){
        if (this.savedTextMaterials.filter(tm => tm.id == this.textMaterial.id).length != 0){
        this.isSaved = true;
        }
      }
      }, err => {
        console.log(err);
      });
    }
  }

  addToSaved(){
    this.textMaterialService.addTextMaterialToSaved(this.userId,this.textMaterial.id).subscribe(res => {
      this.notifier.showNotification("Text material saved!","OK","SUCCESS");
      this.isSaved = true;
    }, err => {
      console.log(err);
      this.notifier.showNotification(err.error,"OK","ERROR");
    });
  }

  removeFromSaved(){
    this.textMaterialService.removeTextMaterialFromSaved(this.userId,this.textMaterial.id).subscribe(res => {
      this.notifier.showNotification("Text material removed from saved!","OK","SUCCESS");
      this.isSaved = false;
    }, err => {
      console.log(err);
      this.notifier.showNotification(err.error,"OK","ERROR");
    });
  }

  getLikedTextMaterials(){
    if (this.userId){
      this.userService.getLikedTextMaterials(this.userId).subscribe(res => {
        if (res.filter(tm => tm.id == this.textMaterial.id).length != 0){
          this.isLiked = true;
        }
      }, err => console.log(err));
    }
  }

  addToLiked(){
    this.userService.addTextMaterialToLiked(this.userId, this.textMaterial.id).subscribe(res => {
      this.isLiked = true;
      this.textMaterial.likesCount++;
    }, err => console.log(err));
  }

  removeFromLiked(){
    this.userService.removeTextMaterialFromLiked(this.userId, this.textMaterial.id).subscribe(res => {
      this.isLiked = false;
      this.textMaterial.likesCount--;
    }, err => console.log(err));
  }

  goBack(){
    this.location.back();
  }
}
