using System.Collections;
using System.Collections.Generic;
using SwimmingAPI.Models;

namespace SwimmingAPI.Repo.Interfaces
{
    public interface IUserRepo
    {
        ApplicationUser GetUser(string userId);
        List<ApplicationUser> GetUserByName(string GivenName, string FamilyName);
        List<ApplicationUser> GetUserByAge(int age);
    }
}