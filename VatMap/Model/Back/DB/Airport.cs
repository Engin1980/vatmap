using System;
using System.Collections.Generic;

namespace VatMap.Model.Back.DB
{
    public partial class Airport
    {
        public int AirportId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Iata { get; set; }
        public string Icao { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Altitude { get; set; }
        public string TimeZoneName { get; set; }
        public string TimeZoneSummerTimeType { get; set; }
        public double TimeZoneHourShift { get; set; }
        public int SourceType { get; set; }
    }
}
