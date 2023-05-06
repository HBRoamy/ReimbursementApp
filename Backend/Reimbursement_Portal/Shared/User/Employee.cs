using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Shared.User
{
    public class Employee :IdentityUser
    {
        
        [Required]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$")]
        public string FullName { get; set; }

        [Required]
        [StringLength(10,MinimumLength = 10)]
        //[RegularExpression("/^([a-zA-Z]{5,5})([0-9]{4,4})([0-9]{1,1})$/")]
        public string PanNumber { get; set; }

        [Required]
        public string BankName { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 12)]
        public string BankAccountNumber { get; set; } //12 digits

        public bool IsApprover { get; set; } = false; // change it in database
    }
}
