using System;
using FlightService.Services;
using FlightService.Interfaces;

namespace FlightsConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new FlightsService();
            foreach (var item in service.GetFlightItineraries())
            {
                Console.WriteLine(@$"{item.OrderId}, {item.FlightNumber}");
            }

            Console.ReadLine();
        }
    }
}
