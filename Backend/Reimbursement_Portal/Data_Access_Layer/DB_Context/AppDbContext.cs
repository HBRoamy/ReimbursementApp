using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using Shared.User;
using Microsoft.EntityFrameworkCore;
using Data_Access_Layer.Entities;

namespace Data_Access_Layer.DbContext
{
    public class AppDbContext: IdentityDbContext<Employee>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {

        }

        public DbSet<ReimbursementEntity> Reimbursements { get; set; }
    }
}
