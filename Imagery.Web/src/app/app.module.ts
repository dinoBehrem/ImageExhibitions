import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppComponent } from './app.component';
import { HeaderComponent } from './Components/Header/header/header.component';
import { SignupComponent } from './Components/Sign/signup/signup.component';
import { SigninComponent } from './Components/Sign/signin/signin.component';
import { UsersComponent } from './Components/User/users/users.component';
import { SuperAdminGuard } from './Guards/superAdmin.guard';
import { JwtInterceptorService } from './Services/Auth/jwt-interceptor.service';
import { SuperAdminAccessGuard } from './Guards/super-admin-access.guard';
import { AdminAccessGuard } from './Guards/admin-access.guard';
import { FilterComponent } from './Components/filter/filter.component';
import { ExhibitionsComponent } from './Components/Exhibition/exhibitions/exhibitions.component';
import { CreateExhibitionComponent } from './Components/Exhibition/create-exhibition/create-exhibition.component';
import { HeaderStatusComponent } from './Components/Header/header-status/header-status.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    SignupComponent,
    SigninComponent,
    UsersComponent,
    FilterComponent,
    ExhibitionsComponent,
    CreateExhibitionComponent,
    HeaderStatusComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    RouterModule.forRoot([
      { path: 'Home', component: ExhibitionsComponent },
      { path: 'Login', component: SigninComponent },
      { path: 'Register', component: SignupComponent },
      { path: 'Create', component: CreateExhibitionComponent },
      {
        path: 'Users',
        component: UsersComponent,
        canActivate: [SuperAdminAccessGuard],
      },
    ]),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptorService,
      multi: true,
    },
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
