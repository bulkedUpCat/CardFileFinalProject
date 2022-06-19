import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddTextMaterialComponent } from './components/add-text-material/add-text-material.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { PersonalInfoComponent } from './components/personal-info/personal-info.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { TextMaterialDetailComponent } from './components/text-material-detail/text-material-detail.component';
import { TextMaterialsComponent } from './components/text-materials/text-materials.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { UserPageComponent } from './components/user-page/user-page.component';
import { UserSignupComponent } from './components/user-signup/user-signup.component';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {path: '', redirectTo: '/main', pathMatch: 'full'},
  {path: 'main', component: TextMaterialsComponent},
  {path: 'main/:id', component: TextMaterialDetailComponent},
  {path: 'login', component: UserLoginComponent},
  {path: 'signup', component: UserSignupComponent},
  {path: 'reset-password', component: ResetPasswordComponent},
  {path: 'home-page', component: HomePageComponent, canActivate: [AuthGuard]},
  {path: 'home-page/personal-info', component: PersonalInfoComponent, canActivate: [AuthGuard]},
  {path: 'add-text-material', component: AddTextMaterialComponent, canActivate: [AuthGuard]},
  {path: 'user/:id', component: UserPageComponent},
  {path: 'users', component: UserListComponent, canActivate: [AuthGuard, AdminGuard]},
  {path: '**', redirectTo: '/main'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
