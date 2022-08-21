//using Busan_Hackathon_2022.Models;
//using Busan_Hackathon_2022.Models.Request;
//using Busan_Hackathon_2022.Services.Interfaces;
//using Microsoft.AspNetCore.Mvc;
//using System.Security.Cryptography;
//using System.Text;

//namespace Busan_Hackathon_2022.Controllers
//{
//    [ApiController]
//    public class EventController : ControllerBase
//    {        

//        private readonly ILogger<CropsController> _logger;
//        private readonly IEventsServices _eventsServices;

//        public EventController(IEventsServices eventsServices, ILogger<CropsController> logger)
//        {
//            _logger = logger;
//            _eventsServices = eventsServices;
//        }

//        [HttpGet]
//        [Route("/")]
//        public async Task<ActionResult<IEnumerable<Event>>> GetCropsData()
//        {
//            try
//            {
//                var events = await _eventsServices.GetAllEvents("SELECT * FROM c");
//                var eventsList = events.ToList();
//                var eventDict = new Dictionary<long, Event>();
//                var newEventList = new List<Event>();

//                // check which plant must die if not watered
//                for (int i = 0; i < eventsList.Count(); i++)
//                {
//                    if((eventsList[i].StartTime - DateTimeOffset.Now.ToUnixTimeSeconds()) > 0)
//                    {
//                        eventDict.Add(eventsList[i].StartTime, eventsList[i]);
//                    }
//                }
//                var sortedDict = eventDict.OrderBy(x => x.Key);
                
//                foreach (var e in sortedDict)
//                {

//                }
//                return Ok();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex.Message);
//                return StatusCode(500, "Some error occured while retreiving data");
//            }
//        }
//    }
//}