using SwimmingAPI.Models;

namespace SwimmingAPI.Repo.Interfaces
{
    public interface IUserRepo
    {
        ApplicationUser GetUser(string userId);
    }
}