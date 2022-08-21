using Busan_Hackathon_2022.Models.Request;
using Busan_Hackathon_2022.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace Busan_Hackathon_2022.Controllers
{
    [ApiController]
    public class CropsController : ControllerBase
    {        

        private readonly ILogger<CropsController> _logger;
        private readonly ICropsService _cropsService;

        public CropsController(ICropsService cropsService, ILogger<CropsController> logger)
        {
            _logger = logger;
            _cropsService = cropsService;
        }

        [HttpGet]
        [Route("/")]
        public async Task<ActionResult<IEnumerable<Crops>>> GetCropsData()
        {
            try
            {
                var crops = await _cropsService.GetCrops("SELECT * FROM c");
                var cropsList = crops.ToList();

                // check which plant must die if not watered
                for (int i = 0; i < cropsList.Count(); i++)
                {
                    if((cropsList[i].LastWater - DateTimeOffset.Now.ToUnixTimeSeconds()) > (60 * 3))
                    {
                        cropsList[i].Growth = -1;
                    }
                }
                crops = cropsList.AsEnumerable();
                return Ok(crops);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Some error occured while retreiving data");
            }
        } 
        [HttpGet]
        [Route("/plant-crops")]
        public async Task<ActionResult> PlantCrops(string UserId, int XCoordinate, int YCoordinate)
        {
            string queryString = $"SELECT * FROM c WHERE c.x_coordinate={XCoordinate.ToString()}" +
                $" AND c.y_coordinate={YCoordinate.ToString()}";

            var crops = await _cropsService.GetCrops(queryString);

            if (crops.Count() == 0)
            {
                try
                {
                    // compute unique id
                    string rawData = $"{UserId}{DateTimeOffset.Now.ToUnixTimeSeconds().ToString()}{XCoordinate}{YCoordinate}";
                    SHA256 sha256Hash = SHA256.Create();
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }

                    var id = builder.ToString();

                    var cropsInput = new Crops()
                    {
                        Id = id,
                        StartTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        LastWater = DateTimeOffset.Now.ToUnixTimeSeconds(),
                        UserId = UserId,
                        XCoordinate = XCoordinate,
                        YCoordinate = YCoordinate,                    
                    };
                    await _cropsService.CreateCrops(cropsInput);
                    return Ok(cropsInput);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return StatusCode(500, ex.Message);
                }
            }
            return StatusCode(500, "There is a crop that exists in the same coordinate");
        }

        [HttpGet]
        [Route("/get-crop-by-cord")]
        public async Task<IActionResult> GetCrops(int x, int y)
        {
            try
            {
                var crops = await _cropsService.GetCropsByCoordinate(x, y);
                return Ok(crops);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occured while finding crop");
            }
        }

        [HttpGet]
        [Route("/water-plant")]
        public async Task<IActionResult> WaterPlant(int x, int y)
        {
            try
            {
                var crops = await _cropsService.GetCropsByCoordinate(x, y);

                if (crops != null && crops.Growth<3)
                {
                    // update the start water 
                    crops.LastWater = DateTimeOffset.Now.ToUnixTimeSeconds();

                    // update growth
                    //var range = crops.LastWater - crops.StartTime;

                    //var magnitudeOfGrowth = range / (60);
                    //for (var i = 0; i < magnitudeOfGrowth; i++)
                    //{ 
                    //    if (crops.Growth < 4)
                    //    {
                    //        crops.Growth += 1; 
                    //    }
                    //}
                    crops.Growth += 1;

                    await _cropsService.UpdateCrops(crops);
                    return Ok(crops);            
                }
                return StatusCode(404, "Cannot water");
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("/remove-crop")]
        public async Task<IActionResult> RemoveCrop(int x, int y)
        {
            try
            {
                var crops = await _cropsService.GetCropsByCoordinate(x, y);
                await _cropsService.DeleteCrop(crops.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error occured while deleting");
            }
        }

        [HttpGet]
        [Route("/test")]
        public async Task<IActionResult> TestPost(string message)
        {
            return Ok(message);
        }
    }
}