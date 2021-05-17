using System;

namespace VatMap.Model.Front
{
  [Serializable]
  public class Airport
  {
    public string ICAO { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public Gps Gps { get; set; }
  }
}
