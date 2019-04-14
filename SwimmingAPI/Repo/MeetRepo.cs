using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SwimmingAPI.Models;
using SwimmingAPI.Repo.Interfaces;

namespace SwimmingAPI.Repo
{
    
    public class MeetRepo : IMeetRepo
    {
        private readonly ApplicationDbContext _db;

        public MeetRepo(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool AddMeet(AddMeetModel model)
        {
            var meet = _db.Meets.Create();
            meet.MeetDate = model.MeetDate;
            meet.MeetName = model.MeetName;
            meet.PoolLength = model.PoolLength;
            meet.MeetVenue = model.MeetVenue;
            var added = _db.SaveChanges();
            return added > 0;
        }

        public List<Meet> GetAllMeets()
        {
            return _db.Meets.ToList();
        }

        public bool UpdateMeet(Meet model)
        {
            var meet = _db.Meets.Single(m => m.MeetId == model.MeetId);
            meet.MeetDate = model.MeetDate;
            meet.MeetName = model.MeetName;
            meet.MeetVenue = model.MeetVenue;
            meet.PoolLength = model.PoolLength;
            var updated = _db.SaveChanges();
            return updated > 0;
        }

        public List<Meet> GetMeets(DateTime startDate, DateTime endDate)
        {
            return _db.Meets.Where(m => m.MeetDate <= endDate && m.MeetDate >= startDate).ToList();
        }

        public List<Meet> GetMeets(DateTime date)
        {
            return _db.Meets.Where(m => m.MeetDate == date).ToList();
        }

        public List<Meet> GetMeets(string venue)
        {
            return _db.Meets.Where(m => m.MeetVenue == venue).ToList();
        }
    }
}