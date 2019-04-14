using System.Collections.Generic;
using SwimmingAPI.Models;

namespace SwimmingAPI.Repo.Interfaces
{
    public interface IEventRepo
    {
        Event GetEvent(int eventId);
        List<Event> GetEvents();
        List<Event> GetEventsByAge(string age);
        List<Event> GetEventsByEventCode(string eventCode);
        List<Event> GetEventsByGender(string gender);
        bool AddEvent(EventAddModel model);
        List<Event> GetEventsByAgeAndGender(string age, string gender);
        List<Event> GetEventsByAgeAndEventCode(string age, string eventCode);
        List<Event> GetEventsByEventCodeAndGender(string eventCode, string gender);
        List<Event> GetEventsByAgeGenderAndEventCode(string age, string gender, string eventCode);

        List<Event> GetEventsByMeetId(int meetId);
        bool EditEvent(EditEventModel model);
    }
}