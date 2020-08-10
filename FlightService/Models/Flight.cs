using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace air_tek_assessment.Models
{
    public class Flight 
    {   

        //Flight Number
        public object FlightNumber { get; set; }
        //Departure Airport
        public string Departure { get; set; }
        //Arrival Airport
        public string Arrival { get; set; }
       
        public int Day { get; set; }
    }
}
