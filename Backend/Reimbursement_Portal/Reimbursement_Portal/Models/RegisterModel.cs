using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Reimbursement_Portal_API.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        //retype password too
        [Required]
        [Compare("ConfirmPassword")]
        public string Password { get; set; }
        
        [Required]
        public string ConfirmPassword { get; set; }

        //[RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$")]
        [Required]
        public string FullName { get; set; }

        [Required]
        public string PanNumber { get; set; }

        [Required]
        public string BankName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        public string BankAccountNumber { get; set; }

        public bool IsApprover { get; set; } = false;


    }
}
