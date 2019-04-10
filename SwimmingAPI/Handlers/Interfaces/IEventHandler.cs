using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using SwimmingAPI.Controllers;
using SwimmingAPI.Models;

namespace SwimmingAPI.Handlers.Interfaces
{
    public interface IEventHandler
    {
        IHttpActionResult AddEvent(EventAddModel model, ApiController controller);
        List<string> GetResultsForEventFormatted(int eventId);
        IHttpActionResult AddResult(AddResultModel model, ApiController controller);
        List<EventResultView> GetResultsForEvent(int eventId);
        List<EventResultView> GetResultsForMeet(int meetId);
        List<string> GetResultsForMeetFormatted(int meetId);
        List<Event> GetEvents();
        List<Event> GetEventsByAgeAndEventCode(string age, string eventCode);
        List<Event> GetEventsByEventCode(string eventCode);
        List<Event> GetEventsByGender(string gender);
        List<Event> GetEventsByAge(string age);
        List<Event> GetEventsByAgeAndGender(string age, string gender);
        List<Event> GetEventsByEventCodeAndGender(string eventCode, string gender);
    }
}
