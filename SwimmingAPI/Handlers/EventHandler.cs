using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using SwimmingAPI.Controllers;
using SwimmingAPI.Handlers.Interfaces;
using SwimmingAPI.Models;
using SwimmingAPI.Repo.Interfaces;

namespace SwimmingAPI.Handlers
{
    public class EventHandler:IEventHandler
    {
        private readonly IEventRepo _eventRepo;
        private readonly IEventResultsRepo _eventResultsRepo;
        private readonly IUserRepo _userRepo;

        private readonly List<string> EventCodes;
        private readonly List<string> Gender;
        private readonly List<string> Rounds;
        public EventHandler(IEventRepo eventRepo, IEventResultsRepo eventResultsRepo, IUserRepo userRepo)
        {
            EventCodes = new List<string>{"01","02","03","04","05","06","07","08","09","10","11","12","13","14","15","16","17","29","37"};
            Gender = new List<string> {"M", "F", "Mix"};
            Rounds = new List<string> {"H", "F", "C", "S", "B"};
            _eventRepo = eventRepo;
            _eventResultsRepo = eventResultsRepo;
            _userRepo = userRepo;
        }

        public IHttpActionResult AddEvent(EventAddModel model, ApiController controller)
        {
            
            if (!Gender.Contains(model.EventGender))
            {
                return new BadRequestErrorMessageResult("Gender entered is not valid it must be either M, F, or Mix",controller);
            }

            if (!EventCodes.Contains(model.EventCode))
            {
                return new BadRequestErrorMessageResult("Event Code is not valid and must be match the neutral file format standard", controller);
            }

            if (!Rounds.Contains(model.Round))
            {
                return new BadRequestErrorMessageResult("Round must be one of the following H, F, C, S, B according to the Neutral file format", controller);
            }

            return _eventRepo.AddEvent(model)
                ? (IHttpActionResult) new OkResult(controller)
                : new BadRequestErrorMessageResult("Failed to add to database", controller);
        }

        public List<string> GetResultsForEventFormatted(int eventId)
        {
            var formattedResults = new List<string>();
            var resultsForEvent = GetResultsForEvent(eventId);
            foreach (var eventResult in resultsForEvent)
            {
                var formatted = eventResult.Gender + " , " + eventResult.FamilyName + " , " + eventResult.GivenName +
                                " , " + eventResult.Club + " , " + eventResult.DateOfBirth.ToString("DDMMyy") + " , " +
                                eventResult.Time + " , " + eventResult.EventCode + " , " + eventResult.Round;
                formattedResults.Add(formatted);
            }

            return formattedResults;
        }

        public IHttpActionResult AddResult(AddResultModel model, ApiController controller)
        {
            var res = _eventResultsRepo.AddEventResult(model);
            if (res)
            {
                return new OkResult(controller);
            }
            else
            {
                return new BadRequestErrorMessageResult("Could not add to db",controller);
            }
        }

        public List<EventResultView> GetResultsForEvent(int eventId)
        {
            var _event = _eventRepo.GetEvent(eventId);
            if (_event == null)
            {
                throw new ArgumentException("Invalid event Id event not found");
            }
            var eventResults = _eventResultsRepo.GetEventResults().Where(ER => ER.EventId == eventId).ToList();
            var eventResultsToView = new List<EventResultView>();
            
            foreach (var eventResult in eventResults)
            {
                var eventResultToView = new EventResultView();
                var user = _userRepo.GetUser(eventResult.UserId);
                eventResultToView.Gender = user.Gender;
                eventResultToView.Club = user.Club;
                eventResultToView.DateOfBirth = user.DateOfBirth;
                eventResultToView.EventCode = _event.EventCode;
                eventResultToView.Time = eventResult.Time.ToString("mmssff");
                eventResultToView.FamilyName = user.FamilyName;
                eventResultToView.GivenName = user.GivenName;
                eventResultToView.Round = _event.Round;
                eventResultsToView.Add(eventResultToView);
            }

            return eventResultsToView;
        }

        public List<EventResultView> GetResultsForMeet(int meetId)
        {
            var events = _eventRepo.GetEvents().Where(e => e.MeetId == meetId);

            return events.SelectMany(_event => GetResultsForEvent(_event.EventId)).ToList();
        }

        public List<string> GetResultsForMeetFormatted(int meetId)
        {
            var formattedResults = new List<string>();
            var resultsForMeet = GetResultsForMeet(meetId);
            foreach (var eventResult in resultsForMeet)
            {
                var formatted = eventResult.Gender + " , " + eventResult.FamilyName + " , " + eventResult.GivenName +
                                " , " + eventResult.Club + " , " + eventResult.DateOfBirth.ToString("DDMMyy") + " , " +
                                eventResult.Time + " , " + eventResult.EventCode + " , " + eventResult.Round;
                formattedResults.Add(formatted);
            }

            return formattedResults;
        }

        public List<Event> GetEvents()
        {
            return _eventRepo.GetEvents();
        }

        public List<Event> GetEventsByAgeAndEventCode(string age, string eventCode)
        {
            if (!EventCodes.Contains(eventCode))
            {
                throw new ArgumentException("Event Code is invalid");
            }

            return _eventRepo.GetEvents().Where(e => e.EventAge == age && e.EventCode == eventCode).ToList();
        }

        public List<Event> GetEventsByEventCode(string eventCode)
        {
            if (!EventCodes.Contains(eventCode))
            {
                throw new ArgumentException("Event Code is invalid");
            }
            
            return _eventRepo.GetEvents().Where(e => e.EventCode == eventCode).ToList();
        }

        public List<Event> GetEventsByGender(string gender)
        {
            
            if (!Gender.Contains(gender))
            {
                throw new ArgumentException("Gender entered is not valid it must be either M, F, or Mix");
            }

            return _eventRepo.GetEvents().Where(e => e.EventGender == gender).ToList();
        }

        public List<Event> GetEventsByAge(string age)
        {
            return _eventRepo.GetEvents().Where(e => e.EventAge == age).ToList();
        }

        public List<Event> GetEventsByAgeAndGender(string age, string gender)
        {
            
            if (!Gender.Contains(gender))
            {
                throw new ArgumentException("Gender entered is not valid it must be either M, F, or Mix");
            }

            return _eventRepo.GetEvents().Where(e => e.EventAge == age && e.EventGender == gender).ToList();
        }

        public List<Event> GetEventsByEventCodeAndGender(string eventCode, string gender)
        {
            if (!EventCodes.Contains(eventCode))
            {
                throw new ArgumentException("Event Code is invalid");
            }

            if (!Gender.Contains(gender))
            {
                throw new ArgumentException("Gender entered is not valid it must be either M, F, or Mix");
            }

            return _eventRepo.GetEvents().Where(e => e.EventCode == eventCode).ToList();
        }
    }
}