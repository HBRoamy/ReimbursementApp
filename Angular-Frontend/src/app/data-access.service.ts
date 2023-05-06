import { AccountService } from './account.service';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
@Injectable({
  providedIn: 'root'
})
export class DataAccessService {

  constructor(private router: Router, private http: HttpClient, private user: AccountService) { }

  baseUrl = "https://localhost:44345/api/";

  GetRequests(type: number): Observable<any> {

    let token = localStorage.getItem("JWT");
    // if (!localStorage.getItem("JWT")) {
    //   // then run the get part so as to handle the 401
    // }
    //console.warn(token)
    let headerObject = new HttpHeaders().set("Authorization", "Bearer " + token)
    //load the below details only if user is an approver, so use account service
    return this.http.get(this.baseUrl + "reimbursements/lookfor/" + type, { headers: headerObject });

  }

  GetAllRequests() {
    return this.http.get(this.baseUrl + "reimbursements/");
  }
  GetRequestsForUser(email: string) {
//console.warn(localStorage.getItem("JWT"))
    return this.http.get(this.baseUrl + "reimbursements/Search?requestedForEmail=" + email,
    {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + localStorage.getItem("JWT")
      })
    });

    
  }
 
  GetGraphDataArrays(){
    return this.http.get(this.baseUrl+"reimbursements/DataArrays", {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + localStorage.getItem("JWT")
      })
    });
  }

  CreateReimbursementRequest(requestBody: any) {
    return this.http.post<any>(this.baseUrl + "reimbursements/", requestBody, {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + localStorage.getItem("JWT")
      })
    });
  }

  EditRequest(id: number, patchBody: any) {
    //can be used for decline, approve etc.
    return this.http.patch(this.baseUrl + "reimbursements/" + id, patchBody, {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + localStorage.getItem("JWT")
      })
    });
  }
  PutEditRequest(id: number, putBody: any) {
    return this.http.put<any>(this.baseUrl + "reimbursements/" + id, putBody, {
      headers: new HttpHeaders({
        "Authorization": "Bearer " + localStorage.getItem("JWT")
      })
    });
  }
  DeleteRequest(id: number) {
    return this.http.delete(this.baseUrl + "reimbursements/" + id,{
      headers: new HttpHeaders({
        "Authorization": "Bearer " + localStorage.getItem("JWT")
      })
    });
  }

// uploadImage(formData:any){
//   return this.http.post(this.baseUrl + "reimbursements/file",formData);
// }

}
