using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SwimmingAPI.Repo;

namespace SwimmingAPI.Controllers
{
    public class EventsController : ApiController
    {
        private readonly IEventRepo _eventRepo;
        public EventsController(IEventRepo eventRepo)
        {
            _eventRepo = eventRepo;
        }
    }
}
