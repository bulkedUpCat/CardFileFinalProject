import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { User } from 'src/app/models/user/User';
import { NotifierService } from 'src/app/services/notifier.service';
import { RoleService } from 'src/app/services/role.service';
import { UserService } from 'src/app/services/user.service';
import { UserItemComponent } from '../../user-item/user-item.component';

@Component({
  selector: 'app-role',
  templateUrl: './role.component.html',
  styleUrls: ['./role.component.css']
})
export class RoleComponent implements OnInit {
  showAddRoleOptions: boolean = false;
  addUserToRoleForm: FormGroup;
  submitted: boolean;
  user: User;
  roles: Array<string>;

  constructor(private userService: UserService,
    private roleService: RoleService,
    private notifier: NotifierService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<RoleComponent>,
    @Inject(MAT_DIALOG_DATA) private data) { }

  ngOnInit(): void {
    this.user = this.data.user;

    this.getAllRoles();
    this.createAddUserToRoleForm();
  }

  createAddUserToRoleForm(){
    this.addUserToRoleForm = this.fb.group({
      userId: [this.user.id],
      roleName: [null,[Validators.required]]
    });
  }

  get roleName(){
    return this.addUserToRoleForm.get('roleName');
  }

  getAllRoles(){
    this.roleService.getAllRoles().subscribe(r => {
      this.roles = r;
    }, err => {
      console.log(err);
    });
  }

  addRole(){
    this.showAddRoleOptions = !this.showAddRoleOptions;
  }

  removeRole(roleName: string){
    const userRole = {
      userId: this.user.id,
      roleName: roleName
    };

    this.roleService.removeRole(userRole.userId, userRole.roleName).subscribe(r => {
      this.notifier.showNotification(`${this.user.userName} is no longer a ${roleName}`,'OK','SUCCESS');

      this.dialogRef.close({data: 'update'});
    }, err => {
      this.notifier.showNotification(err.error,'OK','ERROR');
    })
  }

  onSubmit(){
    this.submitted = true;
    if (!this.addUserToRoleForm.valid){
      return;
    }

    const userRole = this.addUserToRoleForm.value;

    this.roleService.addRole(userRole.userId, userRole.roleName).subscribe(r => {
      this.notifier.showNotification(`${this.user.userName} is now a ${userRole.roleName}`, 'OK','SUCCESS');

      this.dialogRef.close({data: 'update'});
    }, err => {
      this.notifier.showNotification(err.error,'OK','ERROR');
    });
  }
}
