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
import { MatIconModule } from '@angular/material/icon';
import { EmailPdfComponent } from './components/dialogs/email-pdf/email-pdf.component';

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
