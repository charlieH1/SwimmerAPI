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

        public IdentityRole GetRole(int roleId)
        {
            return _db.Roles.Find(roleId);
        }
    }
}