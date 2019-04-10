using System.Collections.Generic;
using SwimmingAPI.Models;

namespace SwimmingAPI.Repo.Interfaces
{
    public interface IEventRepo
    {
        Event GetEvent(int eventId);
        List<Event> GetEvents();
        bool AddEvent(EventAddModel model);
    }
}