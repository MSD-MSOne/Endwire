using AutoMapper;
using EndWire.Domain.Models.API;
using EndWire.Infrastructure.Repositories.Mobile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EndWire.API.Controllers
{
    [Route("api/1.0")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationRepository _locationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TimerController> _logger;
        public LocationController(IMapper mapper, ILocationRepository locationRepository, ILogger<TimerController> logger)
        {
            _locationRepository = locationRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger;   
        }

        [HttpPost]
        [Route("locationselect")]
        public async Task<IActionResult> LocationselectAsync([FromHeader(Name = "authtoken")] string authToken, [FromBody] LocationselectRequest request)
        {
            try
            {
                var response = await _locationRepository.GetLocationselectAsync(authToken, request.locationid);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LocationController.LocationselectAsync Locationid = {request.locationid} with message {ex.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Locationid {request.locationid} LocationselectAsync failed.");
            }
        }

        [HttpGet]
        [Route("locationlist")]
        public async Task<IActionResult> LocationlistAsync([FromHeader(Name = "authtoken")] string authToken)
        {
            AuthToken request = new AuthToken { authtoken = authToken };
            try
            {
                var response = await _locationRepository.GetLocationlistAsync(request.authtoken);
                if (response == null)
                {
                    return NotFound("Invalid or Expired Authorization Token");
                }
             
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in LocationController.LocationlistAsync with message: {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
    }

    public class LocationselectRequest
    {
        //public string authtoken { get; set; }
        public int locationid { get; set; }
    }

    public class AuthToken
    {
        public string authtoken { get; set; }
    }
}
