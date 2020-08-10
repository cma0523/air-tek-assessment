using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FlightService.Interfaces;
using FlightService.Services;


namespace FlightsAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    [Produces("application/json")]
    public class FlightController : ControllerBase
    {
        private readonly ILogger<FlightController> _logger;
        private readonly IFlightsService _flightService;

        public FlightController(ILogger<FlightController> logger, IFlightsService flightService)
        {
            _logger = logger;
            _flightService = flightService;
        }

        [HttpGet]
        public IActionResult GetFlightSchedule()
        {
            var result = _flightService.GetFlightSchedules();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetFlightItineraries()
        {
            var result = _flightService.GetFlightItineraries();
            return Ok(result);
        }

    }
}
