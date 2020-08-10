using System;
using System.Collections.Generic;
using System.Text;
using air_tek_assessment.Models;
using FlightService.Models;

namespace FlightService.Interfaces
{
    public interface IFlightsService
    {
        IEnumerable<Flight> GetFlightSchedules();

        IEnumerable<FlightItinerary> GetFlightItineraries();
    }
}
