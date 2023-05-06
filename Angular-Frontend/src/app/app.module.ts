import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AccountService } from './account.service';
import { EmployeeModule } from './employee/employee.module';
import { AuthServiceService } from './auth-service.service';
import { DataAccessService } from './data-access.service';
import { AuthGuard } from './auth.guard';
//import { JwtModule } from '@auth0/angular-jwt'
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http'
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    EmployeeModule,
    RouterModule,
    FormsModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot({
      timeOut:5000,
      preventDuplicates:true
    })
  ],
  providers: [DataAccessService,AuthServiceService,AuthGuard, AccountService], // not sure if AuthService is required here or not
  bootstrap: [AppComponent]
})
export class AppModule { }
