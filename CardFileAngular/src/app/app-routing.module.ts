import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddTextMaterialComponent } from './components/add-text-material/add-text-material.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { HomePageComponent } from './components/home-page/home-page.component';
import { TextMaterialDetailComponent } from './components/text-material-detail/text-material-detail.component';
import { TextMaterialsComponent } from './components/text-materials/text-materials.component';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { UserSignupComponent } from './components/user-signup/user-signup.component';

const routes: Routes = [
  {path: '', redirectTo: '/main', pathMatch: 'full'},
  {path: 'main', component: TextMaterialsComponent},
  {path: 'main/:id', component: TextMaterialDetailComponent},
  {path: 'login', component: UserLoginComponent},
  {path: 'signup', component: UserSignupComponent},
  {path: 'home-page', component: HomePageComponent},
  {path: 'add-text-material', component: AddTextMaterialComponent},
  {path: '**', redirectTo: '/main'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
