import { Observable, catchError } from 'rxjs';
import { ErrorHandler, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(private router: Router, private http: HttpClient) { 
  }


  baseUrl = "https://localhost:44345/api/Account/";

  // GetCurrentUserEmailWithRole() {
  //   return this.http.get(this.baseUrl + "currentRoleUser");
  // }
  token: any = ""
  Login(loginCreds: any) {
    let body = this.http.post<any>(this.baseUrl + "login", loginCreds)
    return body;

  }

  SignUp(signupCreds: any) {

    return this.http.post<any>(this.baseUrl + "Register", signupCreds);

  }
  logout() {
    if(localStorage.getItem("JWT")){
      localStorage.removeItem("JWT")
    }
    if(localStorage.getItem("email")){
      localStorage.removeItem("email");
    }
    if(localStorage.getItem("isApprover")){
    localStorage.removeItem("isApprover");

    }
    if(localStorage.getItem("isLoggedIn")){
      localStorage.removeItem("isLoggedIn");
    }
    localStorage.clear();
    return this.http.post<any>(this.baseUrl + "Logout", "")
  }
}
