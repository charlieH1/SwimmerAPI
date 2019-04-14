using System.Collections.Generic;
using System.Web.Http.Routing.Constraints;
using SwimmingAPI.Models;

namespace SwimmingAPI.Repo.Interfaces
{
    public interface IEventResultsRepo
    {
        List<EventResult> GetEventResults();
        List<EventResult> GetEventResultsFromEventId(int eventId);

        bool AddEventResult(AddResultModel model);
    }
}