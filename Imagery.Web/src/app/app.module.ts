import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatIconModule } from '@angular/material/icon';

import { AppComponent } from './app.component';
import { HeaderComponent } from './Components/Header/header/header.component';
import { SignupComponent } from './Components/Sign/signup/signup.component';
import { SigninComponent } from './Components/Sign/signin/signin.component';
import { UsersComponent } from './Components/User/users/users.component';
import { JwtInterceptorService } from './Services/Auth/jwt-interceptor.service';
import { SuperAdminAccessGuard } from './Guards/super-admin-access.guard';
import { AdminAccessGuard } from './Guards/admin-access.guard';
import { FilterComponent } from './Components/filter/filter.component';
import { ExhibitionsComponent } from './Components/Exhibition/exhibitions/exhibitions.component';
import { CreateExhibitionComponent } from './Components/Exhibition/create-exhibition/create-exhibition.component';
import { HeaderStatusComponent } from './Components/Header/header-status/header-status.component';
import { ProfileComponent } from './Components/User/profile/profile.component';
import { EditExhibitionComponent } from './Components/Exhibition/edit-exhibition/edit-exhibition.component';
import { ExhibitionComponent } from './Components/Exhibition/exhibition/exhibition.component';
import { UserAccessGuard } from './Guards/user-access.guard';
import { AccountComponent } from './Components/User/account/account.component';
import { MyExhibitionsComponent } from './Components/Exhibition/my-exhibitions/my-exhibitions.component';
import { CollectionComponent } from './Components/Collection/collection.component';
import { SubscriptionsComponent } from './Components/User/profile/subscriptions/subscriptions.component';
import { CartComponent } from './Components/cart/cart.component';
import { NotFoundComponent } from './Components/not-found/not-found.component';

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
    ProfileComponent,
    EditExhibitionComponent,
    ExhibitionComponent,
    AccountComponent,
    MyExhibitionsComponent,
    CollectionComponent,
    SubscriptionsComponent,
    CartComponent,
    NotFoundComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    MatPaginatorModule,
    MatIconModule,
    RouterModule.forRoot([
      { path: '', redirectTo: 'Home', pathMatch: 'full' },
      { path: 'Home', component: ExhibitionsComponent },
      { path: 'Login', component: SigninComponent },
      { path: 'Register', component: SignupComponent },
      { path: 'Create', component: CreateExhibitionComponent },
      {
        path: 'EditExhibition/:id',
        component: EditExhibitionComponent,
        canActivate: [UserAccessGuard],
      },
      {
        path: 'Exhibition/:id',
        component: ExhibitionComponent,
      },
      {
        path: 'Users',
        component: UsersComponent,
        canActivate: [AdminAccessGuard],
      },
      { path: 'Profile/:username', component: ProfileComponent },
      {
        path: 'Account',
        component: AccountComponent,
        canActivate: [UserAccessGuard],
      },
      { path: 'MyExhibitions', component: MyExhibitionsComponent },
      {
        path: 'Collection',
        component: CollectionComponent,
        canActivate: [UserAccessGuard],
      },
      {
        path: '**',
        component: NotFoundComponent,
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
