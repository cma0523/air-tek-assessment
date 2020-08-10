using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using air_tek_assessment.Models;
using FlightService.Interfaces;
using FlightService.Models;
using Newtonsoft.Json.Linq;

namespace FlightService.Services
{
    public class FlightsService : IFlightsService
    {
        private readonly int _flightCapacity = 20; 

        private readonly List<Flight> _flights = new List<Flight>()
        {
            new Flight{FlightNumber = 1, Arrival = "YYZ", Departure = "YUL", Day = 1},
            new Flight{FlightNumber = 2, Arrival = "YYC", Departure = "YUL", Day = 1},
            new Flight{FlightNumber = 3, Arrival = "YVR", Departure = "YUL", Day = 1},
            new Flight{FlightNumber = 4, Arrival = "YYZ", Departure = "YUL", Day = 2},
            new Flight{FlightNumber = 5, Arrival = "YYC", Departure = "YUL", Day = 2},
            new Flight{FlightNumber = 6, Arrival = "YVR", Departure = "YUL", Day = 2}
        };

        private Dictionary<string, int> _destinationCapacities = new Dictionary<string, int>();
        private Dictionary<object, int> _remainCapacityForEachFlight = new Dictionary<object, int>();
        public FlightsService()
        {   
            //generate capacities for each des
            _destinationCapacities = _flights
                .GroupBy(_ => _.Arrival, (destination, group) => new {destination, count = group.Count()})
                .ToDictionary(_ => _.destination, _ => _.count * _flightCapacity);
            _remainCapacityForEachFlight = _flights.ToDictionary(_ => _.FlightNumber, _=>_flightCapacity);
        }


        //Get Flight Schedules
        public IEnumerable<Flight> GetFlightSchedules()
        {
            return _flights;
        }


        public IEnumerable<FlightItinerary> GetFlightItineraries()
        {   
            //Get all the orders from data source
            var orders = RetriveOrders();
         
            foreach (var order in orders)
            {
                //check if it still has capacity to ship the order 
                if (_destinationCapacities.ContainsKey(order.Destination) &&_destinationCapacities[order.Destination] > 0)
                {
                    _destinationCapacities[order.Destination]--;
                    var flight = GetNextAvailableFlight(order.Destination);
                    yield return new FlightItinerary{OrderId = order.OrderId, FlightNumber = flight.FlightNumber, Arrival = flight.Arrival, Departure = flight.Departure, Day = flight.Day};
                }

                else
                {
                    yield return new FlightItinerary{FlightNumber = "Not Scheduled", OrderId = order.OrderId};
                }
            }
        }

        private Flight GetNextAvailableFlight(string destination)
        {
            var flight = _flights.FirstOrDefault(_ =>
                _.Arrival == destination && _remainCapacityForEachFlight[_.FlightNumber] > 0);
            if (flight != null) _remainCapacityForEachFlight[flight.FlightNumber]--;
            return flight;
        }

        private IEnumerable<Order> RetriveOrders()
        {   
            //for web api usage only 
            var dataFilePath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\FlightService\\Data\\coding-assigment-orders.json";

            var jsonString = File.ReadAllText(dataFilePath);
            JObject jsonData = JObject.Parse(jsonString);
            foreach (var property in jsonData.Properties())
            {
                yield return new Order(){OrderId = property.Name, Destination = property.Value["destination"].ToString()};
            }

        }

    }
}
