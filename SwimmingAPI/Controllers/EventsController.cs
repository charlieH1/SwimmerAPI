using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SwimmingAPI.Handlers.Interfaces;
using SwimmingAPI.Models;
using SwimmingAPI.Repo;
using SwimmingAPI.Repo.Interfaces;

namespace SwimmingAPI.Controllers
{
    [RoutePrefix("api/Events")]
    public class EventsController : ApiController
    {

        

        private readonly IEventHandler _eventHandler;
        public EventsController(IEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        [AllowAnonymous]
        [Route("GetEvents")]
        //GET api/Events/GetEvents
        public List<Event> GetEvents()
        {
            return _eventHandler.GetEvents();
        }
        
        [AllowAnonymous]
        [Route("GetEventsByAge")]
        //GET api/Events/GetEventsByAge
        public List<Event> GetEventsByAge(string age)
        {
            return _eventHandler.GetEventsByAge(age);
        }

        [AllowAnonymous]
        [Route("GetEventsByGender")]
        //GET api/Events/GetEventsByGender
        public List<Event> GetEventsByGender(string gender)
        {
            return _eventHandler.GetEventsByGender(gender);
        }

        [AllowAnonymous]
        [Route("GetEventsByEventCode")]
        //GET api/Events/GetEventsByEventCode
        public HttpResponseMessage GetEventsByEventCode(string eventCode)
        {
            try
            {
                var events = _eventHandler.GetEventsByEventCode(eventCode);
                return Request.CreateResponse(events);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
            

        }

        [AllowAnonymous]
        [Route("GetEventsByAgeAndEventCode")]
        //GET api/Events/GetEventsByAgeAndEventCode
        public HttpResponseMessage GetEventsByAgeAndEventCode(string age, string eventCode)
        {
            try
            {
                var events = _eventHandler.GetEventsByAgeAndEventCode(age, eventCode);
                return Request.CreateResponse(events);
            }
            catch(Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);

            }
        }

        [AllowAnonymous]
        [Route("GetEventsByAgeAndGender")]
        //GET api/Events/GetEventsByAgeAndGender
        public List<Event> GetEventsByAgeAndGender(string age, string gender)
        {
            return _eventHandler.GetEventsByAgeAndGender(age, gender);
        }

        [AllowAnonymous]
        [Route("GetEventsByEventCodeAndGender")]
        //GET api/Events/GetEventsByEventCodeAndGender
        public HttpResponseMessage GetEventsByEventCodeAndGender(string eventCode, string gender)
        {
            try
            {
                var events = _eventHandler.GetEventsByEventCodeAndGender(eventCode, gender); 
                return Request.CreateResponse(events);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);

            }
        }
        

        [AllowAnonymous]
        [Route("GetEventResultsForMeetFormatted")]
        //GET api/Events/GetResultsForMeetFormatted
        public List<string> GetResultsForMeetFormatted(int meetId)
        {
            return _eventHandler.GetResultsForMeetFormatted(meetId);
        }

        [AllowAnonymous]
        [Route("GetEventsResultsForMeet")]
        //GET api/Events/GetResultsForMeet
        public HttpResponseMessage GetResultsForMeet(int meetId)
        {
            try
            {
                var events = _eventHandler.GetResultsForMeet(meetId);
                return Request.CreateResponse(events);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest,e);
            }
        }


        [AllowAnonymous]
        [Route("GetEventResultsForEvent")]
        //GET api/Events/GetResultsForEvent
        public HttpResponseMessage GetResultsForEvent(int eventId)
        {
            try
            {
                var events = _eventHandler.GetResultsForEvent(eventId);
                return Request.CreateResponse(events);
            }
            catch (Exception e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, e);
            }
        }

        [AllowAnonymous]
        [Route("GetEventResultsForEventFormatted")]
        //GET api/Events/GetResultsForEventFormatted
        public List<string> GetResultsForEventFormatted(int eventId)
        {
            return _eventHandler.GetResultsForEventFormatted(eventId);
        }

        [Authorize(Roles = "Official")]
        [Route("AddEvent")]
        //POST api/Events/AddEvent
        public IHttpActionResult AddEvent(EventAddModel model)
        {
            return !ModelState.IsValid ? BadRequest(ModelState) : _eventHandler.AddEvent(model, this);
        }

        [Authorize(Roles = "Official")]
        [Route("AddResult")]
        //POST api/Events/AddResult
        public IHttpActionResult AddResult(AddResultModel model)
        {
            return !ModelState.IsValid ? BadRequest(ModelState) : _eventHandler.AddResult(model, this);
        }
    }
}
