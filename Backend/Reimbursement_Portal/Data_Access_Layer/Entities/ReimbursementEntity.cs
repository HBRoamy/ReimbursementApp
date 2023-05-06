using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data_Access_Layer.Entities
{
    public class ReimbursementEntity
    {
        [Key]
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
        public int RequestPhase { get; set; } // make it as 0 for pending (bcoz int default is 0) 1 for processed or 1 for approved, 2 for declined

        public bool ReceiptAttached { get; set; } 

        //public IFormFile ReceiptFile { get; set; }
        public string ReceiptFile { get; set; }//create a bit array // has to be a file

        public string CreatedBy { get; set; } //employee email //its real name should be requestedBy
        public string ApprovedOrRejectedBy { get; set; } //approver employee email

        public string InternalNotes { get; set; }



    }
}
