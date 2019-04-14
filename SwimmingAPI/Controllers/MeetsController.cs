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
    [RoutePrefix("api/Meets")]
    public class MeetsController : ApiController
    {
        private readonly IMeetRepo _meetRepo;
        private readonly List<string> ValidPoolCodes;

        /// <inheritdoc />
        public MeetsController(IMeetRepo meetRepo)
        {
            _meetRepo = meetRepo;
            ValidPoolCodes = new List<string>{"01","02","03","11","12","13","21","22","23","31","32","33","41","42","43","51","52","53","61","62","63","71","72","73","81","82","83","91","92","93"};
        }
        
        /// <summary>
        /// The method for adding a meet - must be authorized as a official
        /// </summary>
        /// <param name="model">The model for adding a meet</param>
        /// <returns>Whether successful or not</returns>
        [Authorize(Roles = "Official")]
        [Route("AddMeet")]
        //POST api/Meets/AddMeet
        public IHttpActionResult AddMeet(AddMeetModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!ValidPoolCodes.Contains(model.PoolLength))
            {
                return BadRequest("Pool Length must match the neutral file format code");
            }

            var res = _meetRepo.AddMeet(model);
            return res ? Ok() : (IHttpActionResult)BadRequest("Not able to add meet to db");


        }

        /// <summary>
        /// The Method for updating a meet - must be authorized as a official
        /// </summary>
        /// <param name="model">The meet model</param>
        /// <returns>Whether successful or not</returns>
        [Authorize(Roles = "Official")]
        [Route("UpdateMeet")]
        public IHttpActionResult UpdateMeet(Meet model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!ValidPoolCodes.Contains(model.PoolLength))
            {
                return BadRequest("Pool Length must match the neutral file format code");
            }

            var res = _meetRepo.UpdateMeet(model);

            return res ? Ok() : (IHttpActionResult)BadRequest("Meet failed to be updated");
        }

        /// <summary>
        /// Gets all meets
        /// </summary>
        /// <returns>All meets</returns>
        [AllowAnonymous]
        [Route("GetMeets")]
        [ResponseType(typeof(List<Meet>))]
        //GET api/Meets/GetMeets
        public IHttpActionResult GetMeets()
        {
            return Ok(_meetRepo.GetAllMeets());
        }

        /// <summary>
        /// Gets all meets in the neutral file format
        /// </summary>
        /// <returns>All the meets in the neutral file format</returns>
        [AllowAnonymous]
        [Route("GetMeetsFormatted")]
        [ResponseType(typeof(List<string>))]
        //Get api/Meets/GetMeetsFormatted
        public IHttpActionResult GetMeetsFormatted()
        {
            var meets = _meetRepo.GetAllMeets();
            var formattedMeets = FormatMeets(meets);
            

            return Ok(formattedMeets);
        }

        /// <summary>
        /// Gets the meets in the neutral file format for a specific date period
        /// </summary>
        /// <param name="startDate">the start date</param>
        /// <param name="endDate">the end date</param>
        /// <returns>The meets in the neutral file format</returns>
        [AllowAnonymous]
        [Route("GetMeetsFormatted")]
        [ResponseType(typeof(List<string>))]
        //Get api/Meets/GetMeetsFormatted
        public IHttpActionResult GetMeetsFormatted(DateTime startDate, DateTime endDate)
        {
            var meets = _meetRepo.GetMeets(startDate, endDate);
            var formattedMeets = FormatMeets(meets);

            return Ok(formattedMeets);
        }

        

        /// <summary>
        /// Gets all the meets for a specific date in the neutral file format
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The meets for the date in the neutral file format</returns>
        [AllowAnonymous]
        [Route("GetMeetsFormatted")]
        [ResponseType(typeof(List<string>))]
        //Get api/Meets/GetMeetsFormatted
        public IHttpActionResult GetMeetsFormatted(DateTime date)
        {
            var meets = _meetRepo.GetMeets(date);
            var formattedMeets = FormatMeets(meets);

            return Ok(formattedMeets);
        }

        /// <summary>
        /// Gets Meets for a specific venue in the neutral file format
        /// </summary>
        /// <param name="venue">The venue</param>
        /// <returns>The meets in the neutral file format</returns>
        [AllowAnonymous]
        [Route("GetMeetsFormatted")]
        [ResponseType(typeof(List<string>))]
        //Get api/Meets/GetMeetsFormatted
        public IHttpActionResult GetMeetsFormatted(string venue)
        {
            var meets = _meetRepo.GetMeets(venue);
            var formattedMeets = FormatMeets(meets);

            return Ok(formattedMeets);
        }


        /// <summary>
        /// Gets meets for a specific date range
        /// </summary>
        /// <param name="startDate">The start date</param>
        /// <param name="endDate">The end date</param>
        /// <returns>The meets</returns>
        [AllowAnonymous]
        [Route("GetMeets")]
        [ResponseType(typeof(List<Meet>))]
        //Get api/Meets/GetMeets
        public IHttpActionResult GetMeets(DateTime startDate, DateTime endDate)
        {
            var meets = _meetRepo.GetMeets(startDate, endDate);
            

            return Ok(meets);
        }

        /// <summary>
        /// Gets meets for a specific date
        /// </summary>
        /// <param name="date">The date</param>
        /// <returns>The meets</returns>
        [AllowAnonymous]
        [Route("GetMeets")]
        [ResponseType(typeof(List<Meet>))]
        //Get api/Meets/GetMeets
        public IHttpActionResult GetMeets(DateTime date)
        {
            var meets = _meetRepo.GetMeets(date);
            

            return Ok(meets);
        }

        /// <summary>
        /// Gets the meets for a specific venue
        /// </summary>
        /// <param name="venue">The venue</param>
        /// <returns>The meets</returns>
        [AllowAnonymous]
        [Route("GetMeets")]
        [ResponseType(typeof(List<Meet>))]
        //Get api/Meets/GetMeets
        public IHttpActionResult GetMeets(string venue)
        {
            var meets = _meetRepo.GetMeets(venue);
            
            return Ok(meets);
        }

        private static List<string> FormatMeets(IEnumerable<Meet> meets)
        {
            return meets.Select(meet => meet.MeetName + " , " + meet.MeetVenue + " , " + meet.MeetDate.ToString("ddMMy") + " , " + meet.PoolLength).ToList();
        }

    }
}
