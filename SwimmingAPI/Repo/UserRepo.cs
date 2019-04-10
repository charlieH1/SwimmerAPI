using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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

        
    }
}