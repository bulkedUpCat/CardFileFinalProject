import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { UserService } from 'src/app/services/user.service';
import { BanUserComponent } from '../dialogs/ban-user/ban-user.component';
import { RoleComponent } from '../dialogs/role/role.component';

@Component({
  selector: 'app-user-item',
  templateUrl: './user-item.component.html',
  styleUrls: ['./user-item.component.css']
})
export class UserItemComponent implements OnInit {
  @Input() user;
  @Output() reload = new EventEmitter();

  showInfo: boolean;

  constructor(private dialog: MatDialog,
    private userService: UserService) { }

  ngOnInit(): void {
  }

  onShow(){
    this.showInfo = true;
  }

  onHide(){
    this.showInfo = false;
  }

  assignRole(){
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      userId: this.user.id,
      user: this.user
    };

    dialogConfig.width = '400px';

    let dialogRef = this.dialog.open(RoleComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(res => {
      if (res?.data == 'update'){
        this.userService.getUserById(this.user.id).subscribe(u => {
          this.user = u;
        }, err => console.log(err));
      }
    })
  }

  banOrUnban(){
    const dialogConfig = new MatDialogConfig();

    dialogConfig.data = {
      user: this.user
    };

    dialogConfig.width = '400px';

    let dialogRef = this.dialog.open(BanUserComponent, dialogConfig);

    dialogRef.afterClosed().subscribe(res => {
      this.reload.emit();
    }, err => console.log(err));
  }
}
