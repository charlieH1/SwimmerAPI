using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SwimmingAPI.Models;
using SwimmingAPI.Repo.Interfaces;

namespace SwimmingAPI.Repo
{
    public class EventResultsRepo : IEventResultsRepo
    {
        private readonly ApplicationDbContext _db;
        public EventResultsRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<EventResult> GetEventResults()
        {
            return _db.EventResults.ToList();
        }

        public bool AddEventResult(AddResultModel model)
        {
            var eventResult = _db.EventResults.Create();
            eventResult.EventId = model.EventId;
            eventResult.Time = new TimeSpan(0,0,model.Minutes,model.Seconds,model.Hundreths*10);
            eventResult.UserId = model.UserId;
            var res = _db.SaveChanges();
            return res > 0;
        }
    }
}