using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.DTOs
{
    public class ReimbursementDTO
    {

        public int id { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        public string ReimbursementType { get; set; } // use enum maybe

        public int RequestedValue { get; set; }
        public int ApprovedValue { get; set; }

        public string Currency { get; set; }
        public int RequestPhase { get; set; }

        public bool ReceiptAttached { get; set; }
        public string ReceiptFile { get; set; }//create a bit array // has to be a file

        public string CreatedBy { get; set; } //employee ID // should be called requested by
        public string ApprovedOrRejectedBy { get; set; } //employee email

        public string InternalNotes { get; set; }
    }
}
