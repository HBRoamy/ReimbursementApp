import { Component } from '@angular/core';
import { loginModel } from 'src/app/Models/UserModels'
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/account.service';
import { catchError } from 'rxjs';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  constructor(private user: AccountService, private router: Router) {

  }

  loginForm = new FormGroup({
    Email: new FormControl("", [Validators.required, Validators.email]),
    Password: new FormControl("", [Validators.required, Validators.minLength(1)]),
  });


  invalidCreds = false;
  onLoginSubmit(values: any) {

    if (values != null) {
      this.user.Login(values)
        .subscribe((obj: any) => {

          if (obj != null) {

            localStorage.setItem("JWT", obj.token);
            localStorage.setItem("email", obj.email);
            localStorage.setItem("isApprover", obj.isApprover);
            //console.warn(localStorage.getItem("email")+" "+ localStorage.getItem("isApprover")+" " +localStorage.getItem("JWT"))
            localStorage.setItem("isLoggedIn", "true");
            this.isLoggedIn = true;
            this.router.navigate(["/dashboard"]);
            
          }
        }, (error)=>{
          //if(error.status===401)
         
          // console.warn(error.status)
          this.invalidCreds = true;
        });

    }
   
  }
  isLoggedIn = false;
  isLoggedOut = false;
  Logout() {
    // localStorage.removeItem("JWT")
    // localStorage.removeItem("email");
    // localStorage.removeItem("isApprover");
    // localStorage.removeItem("isLoggedIn");
    //localStorage.clear();
    this.user.logout().subscribe(status => {
      this.isLoggedOut = status;
      this.isLoggedIn = !status;
      this.invalidCreds = false;
    })


  }

  clearLs() {
    localStorage.clear();
  }
  
  clickAudio(){
    const audio = new Audio()
    audio.src = "./assets/notif.mp3"
    audio.play()
  }
}
