import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AccountService } from 'src/app/account.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {

  constructor(private user: AccountService, private router: Router) {

  }

  bankArray = ["SBI", "ICICI", "HDFC", "IDFC", "Kotak", "IDBI"]
  selectedbank = this.bankArray[0];
  signUpForm = new FormGroup({
    FullName: new FormControl("", [Validators.required, Validators.pattern('^([a-zA-Z ]+)$')]),
    Email: new FormControl("", [Validators.required, Validators.email]),
    PanNumber: new FormControl("", [Validators.required]),
    BankName: new FormControl("SBI", [Validators.required]),
    BankAccountNumber: new FormControl("", [Validators.required, Validators.pattern('^([1-9])([0-9]+)$'), Validators.minLength(12), Validators.maxLength(12)]),
    Password: new FormControl("", [Validators.required, Validators.pattern('^.*(?=.{8,})(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[@?#$%^&+=]).*$'), Validators.minLength(8)]),
    ConfirmPassword: new FormControl("", [Validators.required, Validators.pattern('^.*(?=.{8,})(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[@?#$%^&+=]).*$'), Validators.minLength(8)]),
    IsApprover: new FormControl(false)
  });
  invalidCreds = false;
  signupSuccess = false;
  userAlreadyPresent = false;
  passwordsMismatch = false;
  signUp(values: any) {
    if (values != null) {
      if (values.Password === values.ConfirmPassword && values.Password != null) {
        this.user.SignUp(values).subscribe(status => {
          this.signupSuccess = status
          this.router.navigate(['/login']);
        }, (error) => {
          this.invalidCreds = true;
          this.userAlreadyPresent = true;

        })
        this.signupSuccess=true;
      } else {
        this.invalidCreds = true;
        this.passwordsMismatch = true;
      }
    }

  }

  pop() { // just a testing function
    console.warn(this.selectedbank)
  }
}
