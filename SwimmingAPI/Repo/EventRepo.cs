using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SwimmingAPI.Models;
using SwimmingAPI.Repo.Interfaces;

namespace SwimmingAPI.Repo
{
    public class EventRepo : IEventRepo
    {
        private readonly ApplicationDbContext _db;
        public EventRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public Event GetEvent(int eventId)
        {
            return _db.Events.Find(eventId);
        }

        public List<Event> GetEvents()
        {
            return _db.Events.ToList();
        }

        public bool AddEvent(EventAddModel model)
        {
            var eventToAdd = _db.Events.Create();
            eventToAdd.MeetId = model.MeetId;
            eventToAdd.EventAge = model.EventAge;
            eventToAdd.EventCode = model.EventCode;
            eventToAdd.EventGender = model.EventGender;
            eventToAdd.EventCode = model.EventCode;
            eventToAdd.Round = model.Round;
            var res = _db.SaveChanges();
            return res > 0;
        }
    }
}