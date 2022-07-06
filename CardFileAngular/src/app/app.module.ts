import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { TextMaterialsComponent } from './components/text-materials/text-materials.component';
import { NavBarComponent } from './components/nav-bar/nav-bar.component';
import { CategoryListComponent } from './components/category-list/category-list.component';
import { MaterialCategoryService } from './services/material-category.service';
import { MaterialCategoryComponent } from './components/material-category/material-category.component';
import { TextMaterialComponent } from './components/text-material/text-material.component';
import { SortingFormComponent } from './components/sorting-form/sorting-form.component';
import { MyJwtModule } from './modules/jwt/MyJwt.module';
import { UserLoginComponent } from './components/user-login/user-login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserSignupComponent } from './components/user-signup/user-signup.component';
import { AddTextMaterialComponent } from './components/add-text-material/add-text-material.component';
import { RichTextEditorModule } from '@syncfusion/ej2-angular-richtexteditor';
import { TextMaterialDetailComponent } from './components/text-material-detail/text-material-detail.component';
import { DatePipe } from '@angular/common';
import { HomePageComponent } from './components/home-page/home-page.component';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './modules/material/material.module';
import { EmailPdfComponent } from './components/dialogs/email-pdf/email-pdf.component';
import { UpdateTextMaterialComponent } from './components/dialogs/update-text-material/update-text-material.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { DeleteTextMaterialComponent } from './components/dialogs/delete-text-material/delete-text-material.component';
import { ForgotPasswordComponent } from './components/dialogs/forgot-password/forgot-password.component';
import { ResetPasswordComponent } from './components/reset-password/reset-password.component';
import { PersonalInfoComponent } from './components/personal-info/personal-info.component';
import { UserPageComponent } from './components/user-page/user-page.component';
import { UserListComponent } from './components/user-list/user-list.component';
import { UserItemComponent } from './components/user-item/user-item.component';
import { RoleComponent } from './components/dialogs/role/role.component';
import { CommentComponent } from './components/comment/comment/comment.component';
import { CommentListComponent } from './components/comment/comment-list/comment-list.component';
import { CommentFormComponent } from './components/comment/comment-form/comment-form.component';
import { AddCategoryComponent } from './components/dialogs/add-category/add-category.component';
import { DeleteCategoryComponent } from './components/dialogs/delete-category/delete-category.component';
import { SanitizeHtmlPipe } from './pipes/sanitize-html.pipe';
import { RejectTextMaterialComponent } from './components/dialogs/reject-text-material/reject-text-material.component';
import { UserListPaginationComponent } from './components/user-list-pagination/user-list-pagination.component';
import { ConfirmEmailComponent } from './components/confirm-email/confirm-email.component';
import { BanUserComponent } from './components/dialogs/ban-user/ban-user.component';
import { ChangeUserInfoComponent } from './components/dialogs/change-user-info/change-user-info.component';
import { UserSortingFormComponent } from './components/user-sorting-form/user-sorting-form.component';

@NgModule({
  declarations: [
    AppComponent,
    TextMaterialsComponent,
    NavBarComponent,
    CategoryListComponent,
    MaterialCategoryComponent,
    TextMaterialComponent,
    SortingFormComponent,
    UserLoginComponent,
    UserSignupComponent,
    AddTextMaterialComponent,
    TextMaterialDetailComponent,
    HomePageComponent,
    EmailPdfComponent,
    UpdateTextMaterialComponent,
    PaginationComponent,
    DeleteTextMaterialComponent,
    ForgotPasswordComponent,
    ResetPasswordComponent,
    PersonalInfoComponent,
    UserPageComponent,
    UserListComponent,
    UserItemComponent,
    RoleComponent,
    CommentComponent,
    CommentListComponent,
    CommentFormComponent,
    AddCategoryComponent,
    DeleteCategoryComponent,
    SanitizeHtmlPipe,
    RejectTextMaterialComponent,
    UserListPaginationComponent,
    ConfirmEmailComponent,
    BanUserComponent,
    ChangeUserInfoComponent,
    UserSortingFormComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    MyJwtModule,
    ReactiveFormsModule,
    RichTextEditorModule,
    FormsModule,
    NoopAnimationsModule,
    MaterialModule,
  ],
  providers: [MaterialCategoryService,DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
