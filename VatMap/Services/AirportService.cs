using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using VatMap.Model.Back.DB;
using VatMap.Model.Front;
using DAirport = VatMap.Model.Back.DB.Airport;
using OAirport = VatMap.Model.Front.Airport;

namespace VatMap.Services
{
  public class AirportService
  {
    private readonly VatmapContext db;
    private readonly Dictionary<string, OAirport> proxy = new Dictionary<string, OAirport>();

    internal OAirport GetOrDefault(string icao)
    {
      OAirport ret;

      if (this.proxy.TryGetValue(icao, out ret) == false)
      {
        DAirport tmp = this.db.Airport.FirstOrDefault(q => q.Icao == icao);
        if (tmp != null)
        {
          ret = new OAirport()
          {
            City = tmp.City,
            Country = tmp.Country,
            ICAO = tmp.Icao,
            Name = tmp.Name,
            Gps = new Gps() { Latitude = tmp.Latitude, Longitude = tmp.Longitude }
          };
        }
        else
        {
          ret = new OAirport()
          {
            City = "",
            Country = "",
            Name = icao,
            ICAO = icao,
            Gps = null
          };
        }
        this.proxy[ret.ICAO] = ret;
      }
      return ret;
    }

    public AirportService([FromServices] VatmapContext vatmapContext)
    {
      this.db = vatmapContext;
    }
  }
}
