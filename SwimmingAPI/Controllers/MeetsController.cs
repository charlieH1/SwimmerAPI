using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SwimmingAPI.Models;
using SwimmingAPI.Repo;
using SwimmingAPI.Repo.Interfaces;

namespace SwimmingAPI.Controllers
{
    [RoutePrefix("api/Meets")]
    public class MeetsController : ApiController
    {
        private readonly IMeetRepo _meetRepo;

        public MeetsController(IMeetRepo meetRepo)
        {
            _meetRepo = meetRepo;
        }
        
        [Authorize(Roles = "Official")]
        [Route("AddMeet")]
        //POST api/Meets/AddMeet
        public IHttpActionResult AddMeet(AddMeetModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.PoolLength < 0 || model.PoolLength > 99)
            {
                return BadRequest("Pool length must be between 0 and 99");
            }

            var res = _meetRepo.AddMeet(model);
            return res ? Ok() : (IHttpActionResult)BadRequest("Not able to add meet to db");


        }

        [Authorize(Roles = "Official")]
        [Route("UpdateMeet")]
        public IHttpActionResult UpdateMeet(Meet model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.PoolLength < 0 || model.PoolLength > 99)
            {
                return BadRequest("Pool length must be between 0 and 99");
            }

            var res = _meetRepo.UpdateMeet(model);

            return res ? Ok() : (IHttpActionResult)BadRequest("Meet failed to be updated");
        }

        [AllowAnonymous]
        [Route("GetMeets")]
        //GET api/Meets/GetMeets
        public List<Meet> GetMeets()
        {
            return _meetRepo.GetAllMeets();
        }

        [AllowAnonymous]
        [Route("GetMeetsFormatted")]
        //Get api/Meets/GetMeetsFormatted
        public List<string> GetMeetsFormatted()
        {
            var meets = _meetRepo.GetAllMeets();
            var formattedMeets = new List<string>();
            foreach (var meet in meets)
            {
                var formattedMeet = meet.MeetName + " , " + meet.MeetVenue + " , " + meet.MeetDate.ToString("ddMMy") +
                                    " , " + meet.PoolLength;
                formattedMeets.Add(formattedMeet);

            }

            return formattedMeets;
        }

        [AllowAnonymous]
        [Route("GetMeetsFormatted")]
        //Get api/Meets/GetMeetsFormatted
        public List<string> GetMeetsFormatted(DateTime startDate, DateTime endDate)
        {
            var meets = _meetRepo.GetMeets(startDate, endDate);
            var formattedMeets = FormatMeets(meets);

            return formattedMeets;
        }

        

        [AllowAnonymous]
        [Route("GetMeetsFormatted")]
        //Get api/Meets/GetMeetsFormatted
        public List<string> GetMeetsFormatted(DateTime date)
        {
            var meets = _meetRepo.GetMeets(date);
            var formattedMeets = FormatMeets(meets);

            return formattedMeets;
        }

        [AllowAnonymous]
        [Route("GetMeetsFormatted")]
        //Get api/Meets/GetMeetsFormatted
        public List<string> GetMeetsFormatted(string venue)
        {
            var meets = _meetRepo.GetMeets(venue);
            var formattedMeets = FormatMeets(meets);

            return formattedMeets;
        }


        [AllowAnonymous]
        [Route("GetMeets")]
        //Get api/Meets/GetMeets
        public List<Meet> GetMeets(DateTime startDate, DateTime endDate)
        {
            var meets = _meetRepo.GetMeets(startDate, endDate);
            

            return meets;
        }

        [AllowAnonymous]
        [Route("GetMeets")]
        //Get api/Meets/GetMeets
        public List<Meet> GetMeets(DateTime date)
        {
            var meets = _meetRepo.GetMeets(date);
            

            return meets;
        }

        [AllowAnonymous]
        [Route("GetMeets")]
        //Get api/Meets/GetMeets
        public List<Meet> GetMeets(string venue)
        {
            var meets = _meetRepo.GetMeets(venue);
            
            return meets;
        }

        private static List<string> FormatMeets(IEnumerable<Meet> meets)
        {
            return meets.Select(meet => meet.MeetName + " , " + meet.MeetVenue + " , " + meet.MeetDate.ToString("ddMMy") + " , " + meet.PoolLength).ToList();
        }

    }
}
