using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SwimmingAPI.Models
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {

        public DbSet<Meet> Meets { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventResult> EventResults { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}