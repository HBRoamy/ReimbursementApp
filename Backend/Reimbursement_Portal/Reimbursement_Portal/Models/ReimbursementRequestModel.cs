using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_Portal_API.Models
{
    public class ReimbursementRequestModel
    {
        public int id { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; } // set by creater

        [Required]
        public string ReimbursementType { get; set; } // use enum maybe // set by creater

        [Required]
        public int RequestedValue { get; set; }// set by creater
        public int ApprovedValue { get; set; }


        //[DataType(DataType.Currency)]

        [Required]
        public string Currency { get; set; }// set by creater
        public int RequestPhase { get; set; } // make it as 0 for pending (bcoz int default is 0) 1 for processed

        public bool ReceiptAttached { get; set; }

        //public IFormFile ReceiptFile { get; set; }
        public string ReceiptFile { get; set; }//create a bit array // has to be a file

        public string CreatedBy { get; set; } //employee ID
        public string ApprovedOrRejectedBy { get; set; } //employee email

        public string InternalNotes { get; set; }
    }
}
