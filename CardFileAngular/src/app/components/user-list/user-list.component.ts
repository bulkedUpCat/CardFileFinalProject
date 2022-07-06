import { Component, OnInit } from '@angular/core';
import { UserParameters, UserParams } from 'src/app/models/parameters/UserParameters';
import { User } from 'src/app/models/user/User';
import { SharedUserListParamsService } from 'src/app/services/shared-user-list-params.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.css']
})
export class UserListComponent implements OnInit {
  users: User[];
  userParams: UserParameters = new UserParams();
  paginator: any;

  constructor(private userService: UserService,
    private sharedUserListParams: SharedUserListParamsService) { }

  ngOnInit(): void {
    this.configureUserParams();
    this.getAllUsers();
  }

  reload(){
    this.userService.getUsers(this.userParams).subscribe(u => {
      this.users = u.body;
      this.paginator = JSON.parse(u.headers.get('X-Pagination'));
    }, err => {
      console.log(err);
    });
  }

  configureUserParams(){
    this.userParams.email = this.sharedUserListParams.email;
    this.userParams.userName = this.sharedUserListParams.userName;
    this.userParams.isBanned = this.sharedUserListParams.isBanned;
    this.userParams.pageNumber = this.sharedUserListParams.pageNumber;
    this.userParams.pageSize = this.sharedUserListParams.pageSize;
  }

  getAllUsers(){
    this.userService.getUsers(this.userParams).subscribe(u => {
      this.users = u.body;
      this.paginator = JSON.parse(u.headers.get('X-Pagination'));
    }, err => {
      console.log(err);
    });
  }

  onFilter(params: UserParameters){
    this.sharedUserListParams.email = params.email;
    this.userParams.email = params.email;

    this.sharedUserListParams.userName = params.userName;
    this.userParams.userName = params.userName;

    this.sharedUserListParams.isBanned = params.isBanned;
    this.userParams.isBanned = params.isBanned;

    this.userService.getUsers(this.userParams).subscribe(u => {
      this.users = u.body;
      this.paginator = JSON.parse(u.headers.get('X-Pagination'));;
    }, err => {
      console.log(err);
    });
  }

  onNextPage(page: number){
    this.userParams.pageNumber = page;
    this.sharedUserListParams.pageNumber = page;

    this.userService.getUsers(this.userParams).subscribe(u => {
      this.users = u.body;
    }, err => {
      console.log(err);
    });
  }

  onPreviousPage(page: number){
    this.userParams.pageNumber = page;
    this.sharedUserListParams.pageNumber = page;

    this.userService.getUsers(this.userParams).subscribe(u => {
      this.users = u.body;
    }, err => {
      console.log(err);
    });
  }
}
