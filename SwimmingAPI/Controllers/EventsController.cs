using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using SwimmingAPI.Models;
using SwimmingAPI.Repo.Interfaces;

namespace SwimmingAPI.Controllers
{
    /// <inheritdoc />
    /// <summary>
    /// The event controller
    /// </summary>
    [RoutePrefix("api/Events")]
    public class EventsController : ApiController
    {
        private readonly IEventRepo _eventRepo;
        private readonly IEventResultsRepo _eventResultsRepo;
        private readonly IUserRepo _userRepo;

        private readonly List<string> EventCodes;
        private readonly List<string> Gender;
        private readonly List<string> Rounds;

        /// <inheritdoc />
        /// <summary>
        /// Handles Event and event results
        /// </summary>
        /// <param name="eventRepo">The event repository</param>
        /// <param name="eventResultsRepo">The event results repository</param>
        /// <param name="userRepo">The user repository</param>
        public EventsController(IEventRepo eventRepo, IEventResultsRepo eventResultsRepo, IUserRepo userRepo)
        {
            EventCodes = new List<string> { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12", "13", "14", "15", "16", "17", "29", "37" };
            Gender = new List<string> { "M", "F", "Mix" };
            Rounds = new List<string> { "H", "F", "C", "S", "B" };
            _eventRepo = eventRepo;
            _eventResultsRepo = eventResultsRepo;
            _userRepo = userRepo;
        }

        /// <summary>
        /// Adds Events
        /// </summary>
        /// <param name="model">The event add model</param>
        /// <returns>Whether OK or not</returns>
        [Route("AddEvent")]
        //POST api/Events/AddEvent
        public IHttpActionResult AddEvent (EventAddModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!Gender.Contains(model.EventGender))
            {
                return  BadRequest("Gender entered is not valid it must be either M, F, or Mix");
            }

            if (!EventCodes.Contains(model.EventCode))
            {
                return BadRequest("Event Code is not valid and must be match the neutral file format standard");
            }

            if (!Rounds.Contains(model.Round))
            {
                return  BadRequest("Round must be one of the following H, F, C, S, B according to the Neutral file format");
            }

            return _eventRepo.AddEvent(model)
                ?  (IHttpActionResult) Ok()
                :  BadRequest("Failed to add to database");
        }

        /// <summary>
        /// Edit Events
        /// </summary>
        /// <param name="model">The Event to edit</param>
        /// <returns>Whether successful or not</returns>
        [Route("EditEvent")]
        [Authorize(Roles = "Official")]
        //POST api/Events/EditEvent
        public IHttpActionResult EditEvent(EditEventModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!Gender.Contains(model.EventGender))
            {
                return BadRequest("Gender entered is not valid it must be either M, F, or Mix");
            }

            if (!EventCodes.Contains(model.EventCode))
            {
                return BadRequest("Event Code is not valid and must be match the neutral file format standard");
            }

            if (!Rounds.Contains(model.Round))
            {
                return BadRequest("Round must be one of the following H, F, C, S, B according to the Neutral file format");
            }

            var res = _eventRepo.EditEvent(model);
            return res ? (IHttpActionResult) Ok() : BadRequest("Editing event failed");
        }

        /// <summary>
        /// Gets Events
        /// </summary>
        /// <returns>The events as a list</returns>
        [Route("GetEvents")]
        [ResponseType(typeof(List<Event>))]
        //GET api/Events/GetEvents
        public IHttpActionResult GetEvents()
        {
            return Ok(_eventRepo.GetEvents());
        }

        /// <summary>
        /// Gets events by an event code
        /// </summary>
        /// <param name="eventCode">The event code</param>
        /// <returns>The events as a list</returns>
        [Route("GetEventsByEventCode")]
        [ResponseType(typeof(List<Event>))]
        //GET api/Events/GetEventsByEventCode
        public IHttpActionResult GetEventsByEventCode(string eventCode)
        {
            if (!EventCodes.Contains(eventCode))
            {
                return BadRequest("Event Code is invalid");
            }

            return Ok(_eventRepo.GetEventsByEventCode(eventCode));
        }

        /// <summary>
        /// Gets Events By Gender
        /// </summary>
        /// <param name="gender">The gender must be M, F or Mix</param>
        /// <returns>The events</returns>
        [Route("GetEventsByGender")]
        [ResponseType(typeof(List<Event>))]
        //GET api/Events/GetEventsByGender
        public IHttpActionResult GetEventsByGender(string gender)
        {
            if (!Gender.Contains(gender))
            {
                return BadRequest("Gender entered is not valid it must be either M, F, or Mix");
            }

            return Ok(_eventRepo.GetEventsByGender(gender));
        }

        /// <summary>
        /// Gets events by age
        /// </summary>
        /// <param name="age">The age</param>
        /// <returns>The events as a list</returns>
        [Route("GetEventsByAge")]
        [ResponseType(typeof(List<Event>))]
        //GET api/Events/GetEventsByAge
        public IHttpActionResult GetEventsByAge(string age)
        {
            return Ok(_eventRepo.GetEventsByAge(age));
        }

        /// <summary>
        /// Gets events by age and gender
        /// </summary>
        /// <param name="age">The age</param>
        /// <param name="gender">The gender</param>
        /// <returns>The events as a list</returns>
        [Route("GetEventsByAgeAndGender")]
        [ResponseType(typeof(List<Event>))]
        //GET api/Events/GetEventsByAgeAndGender
        public IHttpActionResult GetEventsByAgeAndGender(string age, string gender)
        {
            if (!Gender.Contains(gender))
            {
                return BadRequest("Gender entered is not valid it must be either M, F, or Mix");
            }

            return Ok(_eventRepo.GetEventsByAgeAndGender(age,gender));
        }

        /// <summary>
        /// Gets Events By Age and Event Code
        /// </summary>
        /// <param name="age">The age</param>
        /// <param name="eventCode">The event code</param>
        /// <returns></returns>
        [Route("GetEventsByAgeAndEventCode")]
        [ResponseType(typeof(List<Event>))]
        //GET api/Events/GetEventsByAgeAndEventCode
        public IHttpActionResult GetEventsByAgeAndEventCode(string age, string eventCode)
        {
            if (!EventCodes.Contains(eventCode))
            {
                return BadRequest("Event Code is not valid and must be match the neutral file format standard");
            }

            return Ok(_eventRepo.GetEventsByAgeAndEventCode(age,eventCode));

        }

        /// <summary>
        /// Get Events by Event Code and Gender
        /// </summary>
        /// <param name="eventCode">The event code</param>
        /// <param name="gender">The gender</param>
        /// <returns>The Events as a list</returns>
        [Route("GetEventsByEventCodeAndGender")]
        [ResponseType(typeof(List<Event>))]
        //GET api/Events/GetEventsByAgeAndEventCode
        public IHttpActionResult GetEventsByEventCodeAndGender(string eventCode, string gender)
        {
            if (!EventCodes.Contains(eventCode))
            {
                return BadRequest("Event Code is not valid and must be match the neutral file format standard");
            }
            if (!Gender.Contains(gender))
            {
                return BadRequest("Gender entered is not valid it must be either M, F, or Mix");
            }

            return Ok(_eventRepo.GetEventsByEventCodeAndGender(eventCode,gender));
        }

        /// <summary>
        /// Gets Events by Age, Gender and Event Code
        /// </summary>
        /// <param name="age">The Age</param>
        /// <param name="gender">The Gender</param>
        /// <param name="eventCode">The Event Code</param>
        /// <returns>The Events as a List</returns>
        [Route("GetEventsByAgeGenderAndEventCode")]
        [ResponseType(typeof(List<Event>))]
        //GET api/Events/GetEventsByAgeGenderAndEventCode
        public IHttpActionResult GetEventsByAgeGenderAndEventCode(string age, string gender, string eventCode)
        {
            if (!EventCodes.Contains(eventCode))
            {
                return BadRequest("Event Code is not valid and must be match the neutral file format standard");
            }
            if (!Gender.Contains(gender))
            {
                return BadRequest("Gender entered is not valid it must be either M, F, or Mix");
            }

            return Ok(_eventRepo.GetEventsByAgeGenderAndEventCode(age,gender,eventCode));
        }

        /// <summary>
        /// Gets Results for a specific Event
        /// </summary>
        /// <param name="eventId">The event Id</param>
        /// <returns>The results for the event</returns>
        [Route("GetResultsForEvent")]
        [ResponseType(typeof(List<EventResultView>))]
        //GET api/Events/GetResultsForEvent
        public IHttpActionResult GetResultsForEvent(int eventId)
        {
            var _event = _eventRepo.GetEvent(eventId);
            if (_event == null)
            {
                return BadRequest("Invalid event Id, event not found");
            }
            var eventResultsToView = EventResultsToView(_event);

            return Ok(eventResultsToView);
        }

        

        /// <summary>
        /// Gets Results for a meet
        /// </summary>
        /// <param name="meetId">The meet id</param>
        /// <returns>The results for the meet</returns>
        [Route("GetResultsForMeet")]
        [ResponseType(typeof(List<EventResultView>))]
        //GET api/Events/GetResultsForMeet
        public IHttpActionResult GetResultsForMeet(int meetId)
        {
            var events = _eventRepo.GetEventsByMeetId(meetId);
            if (events.Count == 0)
            {
                return BadRequest("No events for that meet");
            }

            var eventResultsToView = new List<EventResultView>();
            foreach (var _event in events)
            {
                eventResultsToView.AddRange(EventResultsToView(_event));
            }

            return Ok(eventResultsToView);
        }

        /// <summary>
        /// Gets the events for a event in the neutral file format
        /// </summary>
        /// <param name="eventId">The event id</param>
        /// <returns>The Results as a list of strings in the format of the neutral file format</returns>
        [Route("GetEventResultsForEventFormatted")]
        [ResponseType(typeof(List<string>))]
        //GET api/Events/GetResultsForEventFormatted
        public IHttpActionResult GetResultsForEventFormatted(int eventId)
        {
            var _event = _eventRepo.GetEvent(eventId);
            if (_event == null)
            {
                return BadRequest("Invalid event Id, event not found");
            }
            var eventResultsToView = EventResultsToView(_event);
            return Ok(FormatEventResults(eventResultsToView));
        }

        /// <summary>
        /// Gets the results for a meet in the neutral file format
        /// </summary>
        /// <param name="meetId">The meet id</param>
        /// <returns>The results as a list of strings in the format of the neutral file format</returns>
        [Route("GetEventResultsForMeetFormatted")]
        [ResponseType(typeof(List<string>))]
        //GET api/Events/GetResultsForMeetFormatted
        public IHttpActionResult GetResultsForMeetFormatted(int meetId)
        {
            var events = _eventRepo.GetEventsByMeetId(meetId);
            if (events.Count == 0)
            {
                return BadRequest("No events for that meet");
            }

            var eventResultsToView = new List<EventResultView>();
            foreach (var _event in events)
            {
                eventResultsToView.AddRange(EventResultsToView(_event));
            }

            return Ok(FormatEventResults(eventResultsToView));
        }


        /// <summary>
        /// Adds Results
        /// </summary>
        /// <param name="model">The result to be added</param>
        /// <returns>Whether it was successfully able to add it</returns>
        [Route("AddResult")]
        //POST api/Events/AddResult
        public IHttpActionResult AddResult(AddResultModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var res = _eventResultsRepo.AddEventResult(model);
            return res ? (IHttpActionResult) Ok() : BadRequest("Unable To add to database");
        }


        private static List<string> FormatEventResults(IEnumerable<EventResultView> eventResultViews)
        {
            return eventResultViews.Select(eventResult => eventResult.Gender + " , " + eventResult.FamilyName + " , " + eventResult.GivenName + " , " + eventResult.Club + " , " + eventResult.DateOfBirth.ToString("DDMMyy") + " , " + eventResult.Time + " , " + eventResult.EventCode + " , " + eventResult.Round).ToList();
        }

        private List<EventResultView> EventResultsToView(Event _event)
        {
            var eventResults = _eventResultsRepo.GetEventResultsFromEventId(_event.EventId);
            var eventResultsToView = new List<EventResultView>();

            foreach (var eventResult in eventResults)
            {
                var eventResultToView = new EventResultView();
                var user = _userRepo.GetUser(eventResult.UserId);
                eventResultToView.Gender = user.Gender;
                eventResultToView.Club = user.Club;
                eventResultToView.DateOfBirth = user.DateOfBirth;
                eventResultToView.EventCode = _event?.EventCode;
                eventResultToView.Time = eventResult.Time.ToString("mmssff");
                eventResultToView.FamilyName = user.FamilyName;
                eventResultToView.GivenName = user.GivenName;
                eventResultToView.Round = _event?.Round;
                eventResultsToView.Add(eventResultToView);
            }

            return eventResultsToView;
        }


        



    }
}
