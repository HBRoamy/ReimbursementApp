import { AuthGuard } from './auth.guard';
import { DashboardComponent } from './dashboard/dashboard.component';
import { SignUpComponent } from './employee/sign-up/sign-up.component';
import { LoginComponent } from './employee/login/login.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [

  {path:"", component:LoginComponent},
  {path:"login", component:LoginComponent},
  {path:"signup", component:SignUpComponent},
  {path:"dashboard", component:DashboardComponent,canActivate:[AuthGuard]},
  {path:"**", component:LoginComponent}

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
