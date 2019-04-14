using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;
using SwimmingAPI.Models;
using SwimmingAPI.Repo.Interfaces;

namespace SwimmingAPI.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _db;

        public UserRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public ApplicationUser GetUser(string userId)
        {
            return _db.Users.Find(userId);
        }

        public List<ApplicationUser> GetUserByName(string GivenName, string FamilyName)
        {
            return _db.Users.Where(U => U.FamilyName == FamilyName && U.GivenName == GivenName).ToList();
        }

        public List<ApplicationUser> GetUserByAge(int age)
        {
            var earliestDob = new DateTime().AddYears(-age-1);
            var latestDob = new DateTime().AddYears(-age);
            return _db.Users.Where(U => U.DateOfBirth > earliestDob && U.DateOfBirth < latestDob).ToList();
        }
    }
}