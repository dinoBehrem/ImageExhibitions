import { Component, OnInit } from '@angular/core';
import { SignService } from 'src/app/Services/Sign/sign.service';
import { UserService } from 'src/app/Services/User/user.service';
import { RoleManagerVM } from 'src/app/ViewModels/RoleManager';
import { UserVM } from 'src/app/ViewModels/UserVM';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css'],
})
export class UsersComponent implements OnInit {
  users: UserVM[] = [];
  constructor(
    private userService: UserService,
    private authService: SignService
  ) {}

  user!: UserVM;
  _role: string = '';
  roles!: string[];

  ngOnInit(): void {
    this.LoadUsers();
    this.LoadRoles();
  }

  LoadUsers() {
    this.userService.GetAll().subscribe((data: any) => {
      this.users = data;
    });
  }

  LoadRoles() {
    this.userService.GetRoles().subscribe((res: any) => {
      this.roles = res;
    });
  }

  Promote(user: string) {
    const prop = 'role';
    var isSuperAdmin = [...this.authService.GetJWTData(prop)];

    if (this._role === '') {
      alert('Please select a role?');
      return;
    }
    let roleManager = {
      userName: user,
      role: this._role,
    };

    this.userService
      .PromoteToRole(roleManager as RoleManagerVM)
      .subscribe((res: any) => {
        if (res.isSuccess) {
          alert(res?.message);
        }
      });
  }

  Demote(user: string) {
    const prop = 'role';
    var isSuperAdmin = [...this.authService.GetJWTData(prop)];

    if (this._role === '') {
      alert('Please select a role?');
      return;
    }
    let roleManager = {
      userName: user,
      role: this._role,
    };

    if (!isSuperAdmin.includes('SuperAdmin')) {
      alert("Denied, you don't have SuperAdmin permissions!");
      return;
    }

    this.userService
      .DemoteToRole(roleManager as RoleManagerVM)
      .subscribe((res: any) => {
        if (res.isSuccess) {
          alert(res.message);
        }
      });
  }

  SelectedUser(user: UserVM) {
    this.user = user;
  }
}
