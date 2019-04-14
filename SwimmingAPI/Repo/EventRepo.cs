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

        public List<Event> GetEventsByAge(string age)
        {
            return _db.Events.Where(e => e.EventAge == age).ToList();
        }

        public List<Event> GetEventsByEventCode(string eventCode)
        {
            return _db.Events.Where(e => e.EventCode == eventCode).ToList();
        }

        public List<Event> GetEventsByGender(string gender)
        {
            return _db.Events.Where(e => e.EventGender == gender).ToList();
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

        public List<Event> GetEventsByAgeAndGender(string age, string gender)
        {
            return _db.Events.Where(e => e.EventAge == age && e.EventGender == gender).ToList();
        }

        public List<Event> GetEventsByAgeAndEventCode(string age, string eventCode)
        {
            return _db.Events.Where(e => e.EventCode == eventCode && e.EventAge == age).ToList();
        }

        public List<Event> GetEventsByEventCodeAndGender(string eventCode, string gender)
        {
            return _db.Events.Where(e => e.EventCode == eventCode && e.EventGender == gender).ToList();
        }

        public List<Event> GetEventsByAgeGenderAndEventCode(string age, string gender, string eventCode)
        {
            return _db.Events.Where(e => e.EventCode == eventCode && e.EventAge == age && e.EventGender == gender)
                .ToList();
        }

        public List<Event> GetEventsByMeetId(int meetId)
        {
            return _db.Events.Where(e => e.MeetId == meetId).ToList();
        }

        public bool EditEvent(EditEventModel model)
        {
            var _eventToUpdate = _db.Events.Single(e => e.EventId == model.EventId);
            _eventToUpdate.EventAge = model.EventAge;
            _eventToUpdate.EventCode = model.EventCode;
            _eventToUpdate.EventGender = model.EventGender;
            _eventToUpdate.MeetId = model.MeetId;
            _eventToUpdate.Round = model.Round;
            var res = _db.SaveChanges();
            return res > 0;
        }
    }
}