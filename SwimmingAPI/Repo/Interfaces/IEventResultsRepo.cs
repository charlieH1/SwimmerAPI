using System.Collections.Generic;
using System.Web.Http.Routing.Constraints;
using SwimmingAPI.Models;

namespace SwimmingAPI.Repo.Interfaces
{
    public interface IEventResultsRepo
    {
        List<EventResult> GetEventResults();

        bool AddEventResult(AddResultModel model);
    }
}