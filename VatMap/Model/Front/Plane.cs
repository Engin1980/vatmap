using System.Collections.Generic;

namespace VatMap.Model.Front
{
  public class Plane
  {
    public string Callsign { get; set; }
    public List<Gps> GpsHistory { get; set; }
    public int Altitude { get; set; }
    public int Heading { get; set; }
    public Airport DepartureAirport { get; set; }
    public Airport ArrivalAirport { get; set; }
  }
}
