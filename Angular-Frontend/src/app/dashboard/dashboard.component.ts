import { Observable, Subscriber } from 'rxjs';
import { Router } from '@angular/router';
import { AccountService } from './../account.service';
import { DataAccessService } from './../data-access.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Chart, ChartConfiguration, ChartItem, registerables, Colors } from 'node_modules/chart.js'
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

  allPendingData: any = []
  allApprovedData: any = []
  allDeclinedData: any = []
  currentUserEmail: string = "";
  cachedPendingRequests: any = [];
  cachedApprovedRequests: any = [];
  cachedDeclinedRequests: any = [];
  isApprover: boolean = false;//combination of isApprover and IsLoggedIn should be used
  isLoggedIn: boolean = false;
  isLoggedOut: boolean = true;
  currentUser: string = ""
  requestsByCurrentUser: any = [];
  constructor(private dataService: DataAccessService, private user: AccountService, private router: Router, private toastr: ToastrService) {

  }
  temp: any = ""
  ngOnInit(): void {

    this.myInitFunc();
    if(localStorage.getItem("isApprover")==="True")
    {
      this.dataService.GetGraphDataArrays().subscribe((arrays: any) => {

        this.myMonthChartFunc(arrays.arr2);
        this.myTypeChartFunc(arrays.arr1);
  
      })
    }
   

  }

  myMonthChartFunc(monthData: any) {
    Chart.register(...registerables);
    const data = {
      labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
      datasets: [{
        label: 'Month-wise',
        backgroundColor: [
          'rgba(255, 99, 132, 0.2)',
          'rgba(255, 159, 64, 0.2)',
          'rgba(255, 205, 86, 0.2)',
          'rgba(75, 192, 192, 0.2)',
          'rgba(54, 162, 235, 0.2)',
          'rgba(153, 102, 255, 0.2)',
          'rgba(201, 203, 207, 0.2)'
        ],
        borderColor: [
          'rgb(255, 99, 132)',
          'rgb(255, 159, 64)',
          'rgb(255, 205, 86)',
          'rgb(75, 192, 192)',
          'rgb(54, 162, 235)',
          'rgb(153, 102, 255)',
          'rgb(201, 203, 207)'
        ],
        borderWidth: 1,
        data: monthData,
      }]
    };
    const options = {
      scales: {
        y: {
          beginAtZero: true,
          display: true,
        }
      }
    }
    const config: ChartConfiguration = {
      type: 'bar',
      data: data,
      options: options
    }

    const chartItem: ChartItem = document.getElementById('myChart') as ChartItem
    new Chart(chartItem, config)



  }

  myTypeChartFunc(typeData: any) {
    Chart.register(...registerables, Colors);
    const data = {
      labels: ['Medical', 'Travel', 'Food', 'Entertainment', 'Miscellaneous'],
      datasets: [{
        label: 'Reimbursements',
        backgroundColor: [
          '#ff6384',
          '#36a2eb',
          '#cc65fe',
          '#ffce56',
          '#23d5ab'
        ],
        borderColor: 'rgb(255, 255, 255)',
        data: typeData,
        hoverOffset: 8
      }]
    };
    const options = {
      scales: {
        y: {
          beginAtZero: true,
          display: false
        }
      }
    }
    const config: ChartConfiguration = {
      type: 'doughnut',
      data: data,
      options: options
    }

    const chartItem2: ChartItem = document.getElementById('myChart2') as ChartItem
    new Chart(chartItem2, config)
  }

  // sortArrayByDate(data: any) {
  //   for (let i = 0; i < data.length; i++) {
  //     for (let j = 0; j < data.length; j++) {

  //       if (data[j].date < data[i].date) {
  //         let temp = data[j]
  //         data[j] = data[i]
  //         data[i] = temp
  //       }

  //     }
  //   }
  // }
  
  myInitFunc() {
    if (localStorage.getItem("JWT")) {
      if (localStorage.getItem("isApprover") === "True") {

        this.dataService.GetRequests(0).subscribe((data) => {
          //this.sortArrayByDate(data);
          this.allPendingData = data
          this.cachedPendingRequests = data
        });
        this.dataService.GetRequests(1).subscribe((data) => {
          //this.sortArrayByDate(data);
          this.allApprovedData = data
          this.cachedApprovedRequests = data
        });
        this.dataService.GetRequests(2).subscribe((data) => {
          //this.sortArrayByDate(data);
          this.allDeclinedData = data
          this.cachedDeclinedRequests = data
        });

        this.isApprover = true;
      }

      this.isLoggedIn = true;
      this.currentUserEmail = localStorage.getItem("email")!;

      if (this.isApprover === false) {

        this.dataService.GetRequestsForUser(this.currentUserEmail).subscribe((data: any) => {
          //this.sortArrayByDate(data);
          this.requestsByCurrentUser = data;
        })


      }
    }
  }

  // reactive Form for adding reimbursements
  newReimbursementForm = new FormGroup({
    Date: new FormControl("", Validators.required),
    ReimbursementType: new FormControl("Medical"), //drop down
    RequestedValue: new FormControl("", [Validators.required, Validators.min(1)]),
    Currency: new FormControl("INR"), //drop down
    ReceiptFile: new FormControl(),
    CreatedBy: new FormControl(this.currentUserEmail)
  })

  currency: string = "INR";
  reimbType: string = "Medical";
  SetCurrency(val: string) {
    this.currency = val;
  }
  SetReimbursementType(val: string) {
    this.reimbType = val;
  }

  CreateNewRequest(requestBody: any, creatorEmail: string) {
    if (requestBody != null) {
      requestBody.CreatedBy = creatorEmail

      this.dataService.CreateReimbursementRequest(requestBody).subscribe(() => {
        this.toastr.success("Created Reimbursement Successfully!")
        this.dataService.GetRequestsForUser(this.currentUserEmail).subscribe((data: any) => {
          //this.sortArrayByDate(data);
          this.requestsByCurrentUser = data;
        })
        this.newReimbursementForm.reset();
      })
    }
    else {

      alert("Error while creating the request")
    }
  }




  GetRequestsByuserEmail(email: string) {
    if (email != null && email != "") {
      let arrayT: any = []

      this.cachedPendingRequests.forEach((element: any) => {
        if (element.createdBy === email && element.requestPhase === 0) {
          arrayT.push(element)

        }
        this.allPendingData = arrayT;
      });
      // this.dataService.GetRequestsForUser(email).subscribe(data => {
      //   this.allPendingData = data;
      // })
    }
    else {
      this.allPendingData = this.cachedPendingRequests;
    }
  }

  requestedAmount: number = 0;
  sortSelect(value: any) {
    if (value != null) {
      let arrayT1: any = []
      switch (value) {
        case '1':
          //this.allPendingData= this.cachedApprovedRequests
          this.cachedPendingRequests.forEach((element: any) => {
            if (element.requestPhase === 0) {
              arrayT1.push(element)
            }

          });

          break;
        case '2':

          this.cachedPendingRequests.forEach((element: any) => {
            if (element.reimbursementType === "Medical" && element.requestPhase === 0) {
              arrayT1.push(element)
            }

          });
          break;
        case '3':
          this.cachedPendingRequests.forEach((element: any) => {
            if (element.reimbursementType === "Travel" && element.requestPhase === 0) {
              arrayT1.push(element)

            }
            // this.allPendingData = arrayT1;
          });
          // this.allPendingData=arrayT1;

          break;
        case '4':
          this.cachedPendingRequests.forEach((element: any) => {
            if (element.reimbursementType === "Food" && element.requestPhase === 0) {
              arrayT1.push(element)

            }
            // this.allPendingData = arrayT1;
          });
          // this.allPendingData=arrayT1;
          break;

        case '5':
          this.cachedPendingRequests.forEach((element: any) => {
            if (element.reimbursementType === "Entertainment" && element.requestPhase === 0) {
              arrayT1.push(element)

            }
            // this.allPendingData = arrayT1;
          });
          // this.allPendingData=arrayT1;

          break;
        case '6':
          this.cachedPendingRequests.forEach((element: any) => {
            if (element.reimbursementType === "Misc." && element.requestPhase === 0) {
              arrayT1.push(element)

            }
            // this.allPendingData = arrayT1;
          });
          // this.allPendingData=arrayT1;

          break;

        default: this.allPendingData = this.cachedApprovedRequests
          break;
      }
      this.allPendingData = arrayT1;
    }
  }

  approvalRequestId: number = 0;
  setApprovalRequestId(id: number, reqValue: number) {
    this.approvalRequestId = id;
    this.requestedAmount = reqValue;
  }

  editRequestBody: any = []
  setEditRequestBody(body: any) {
    this.editRequestBody = body;
  }

  editRequestForm = new FormGroup({
    Date: new FormControl(this.editRequestBody.Date, Validators.required),
    ReimbursementType: new FormControl(this.editRequestBody.ReimbursementType), //drop down
    RequestedValue: new FormControl(this.editRequestBody.RequestedValue, [Validators.required, Validators.min(1)]),
    Currency: new FormControl(this.editRequestBody.Currency), //drop down
    ReceiptFile: new FormControl(this.editRequestBody.ReceiptFile),
    //CreatedBy: new FormControl(this.editRequestBody.CreatedBy) 
  })

  submitEdit(putBody: any) {
    putBody.id = this.editRequestBody.id;
    putBody.CreatedBy = this.currentUserEmail
    this.dataService.PutEditRequest(putBody.id, putBody).subscribe(() => {
      this.toastr.success("Edited a Reimbursement Successfully!")
      this.dataService.GetRequestsForUser(this.currentUserEmail).subscribe((data: any) => {
        this.requestsByCurrentUser = data;
      })
    });
  }

  // reactive Form for approvals
  approvalForm = new FormGroup({
    //ApprovedOrRejectedBy: new FormControl(this.currentUserEmail, [Validators.required, Validators.email]),
    ApprovedValue: new FormControl("", [Validators.required, Validators.min(1)]),
    InternalNotes: new FormControl("", Validators.required),
  })

  ApproveRequest(approvedRequestBody: any) {
    //update pending requests and cached requests too
    //change pending-0 to approved-1
    //set internal notes, approvedorRejectedBy, approved amount
    let approvalPatchBody = [
      {
        "op": "replace",
        "path": "RequestPhase",
        "value": 1
      },
      {

        "op": "replace",
        "path": "ApprovedOrRejectedBy",
        "value": this.currentUserEmail

      },
      {

        "op": "replace",
        "path": "ApprovedValue",
        "value": approvedRequestBody.ApprovedValue

      },
      {

        "op": "replace",
        "path": "InternalNotes",
        "value": approvedRequestBody.InternalNotes

      }
    ]
    this.dataService.EditRequest(this.approvalRequestId, approvalPatchBody).subscribe(() => {
      this.toastr.success("Approval Successful!")
      this.myInitFunc();
      // this.router.navigate(['/dashboard'])
    });
  }

  DeclineRequest(id: number, declinedByEmail: string) {
    //update pending requests and cached requests too
    let declineBody = [
      {
        "op": "replace",
        "path": "RequestPhase",
        "value": 2
      },
      {

        "op": "replace",
        "path": "ApprovedOrRejectedBy",
        "value": declinedByEmail

      }
    ]
    this.dataService.EditRequest(id, declineBody).subscribe(() => {

      this.myInitFunc();
      this.dataService.GetRequestsForUser(this.currentUserEmail).subscribe((data: any) => {
        this.toastr.warning("Declined a Reimbursement")
        //this.sortArrayByDate(data);
        this.requestsByCurrentUser = data;
      })

    });


  }

  DeleteRequest(requestId: number) {
    this.dataService.DeleteRequest(requestId).subscribe(() => {
      this.dataService.GetRequestsForUser(this.currentUserEmail).subscribe((data: any) => {
        this.toastr.warning("Deleted a reimbursement!")
        //this.sortArrayByDate(data);
        this.requestsByCurrentUser = data;
      })

    })
  }
  Logout() {
    this.user.logout().subscribe(status => {
      this.isLoggedIn = false;
      this.isApprover = false;
      this.router.navigate(['/'])
    })
  }


}

//dumped code

//   myFileLink:any

// // imageUpload(event:any){
// //   var file = event.target.files[0]
// //   const formData = new FormData()
// //   formData.append('file',file,file.name)
// //   this.dataService.uploadImage(formData).subscribe((data:any)=>{
// //     this.myFileLink = data.path;
// //     console.warn(this.myFileLink);
// //   });

// // }
// img!:any;
//   onFileChange(event:Event){
//     const target = event.target as HTMLInputElement;
//     const file:File = (target.files as FileList)[0];
//     this.convertToBase64(file);
//   }
//   convertToBase64(file:File){
//     const observable = new Observable((subscriber:Subscriber<any>)=>{
//       this.readFile(file, subscriber)
//     })
//     observable.subscribe((d)=>{
//       this.img=d;
//       console.warn(this.img)
//     })
    
//   }
// readFile(file:File,subscriber:Subscriber<any>){
// const fileReader = new FileReader();
// fileReader.readAsDataURL(file)
// fileReader.onload = () =>{
//   subscriber.next(fileReader.result)
//   subscriber.complete();
// }
// fileReader.onerror = () =>{
//   subscriber.error()
//   subscriber.complete()
// }
// }
