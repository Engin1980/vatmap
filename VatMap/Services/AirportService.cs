using Microsoft.AspNetCore.Mvc;
using System;
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

    private readonly Dictionary<string, OAirport> proxy = new Dictionary<string, OAirport>();
    private readonly IServiceProvider serviceProvider;

    public AirportService(IServiceProvider serviceProvider)
    {
      this.serviceProvider = serviceProvider;
    }

    private class AirportDbProvider
    {
      private readonly VatmapContext db;

      public AirportDbProvider([FromServices] VatmapContext db)
      {
        this.db = db;
      }

      internal DAirport Get(string icao)
      {
        DAirport ret = this.db.Airport.FirstOrDefault(q => q.Icao == icao);
        return ret;
      }
    }

    internal OAirport GetOrDefault(string icao)
    {
      VatmapContext db = (VatmapContext)this.serviceProvider.GetService(typeof(VatmapContext));
      OAirport ret;

      if (this.proxy.TryGetValue(icao, out ret) == false)
      {
        DAirport tmp = db.Airport.FirstOrDefault(q => q.Icao == icao);
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

  }
}
