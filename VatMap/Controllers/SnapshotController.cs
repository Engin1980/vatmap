using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VatMap.Model.Front;
using VatMap.Services;

namespace VatMap.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SnapshotController : ControllerBase
  {
    private readonly SnapshotProviderService snapshotProviderService;

    public SnapshotController(
      [FromServices] SnapshotProviderService snapshotProviderService
      )
    {
      this.snapshotProviderService = snapshotProviderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetSnapshotAsync()
    {
      //await Task.Run(() => this.snapshotProviderService.UpdateByVatsimIfRequiredAsync());
      this.snapshotProviderService.UpdateByVatsimIfObsoleteAsync();
      Snapshot tmp = this.snapshotProviderService.GetCurrent();

      Snapshot ret = new Snapshot();
      foreach (var plane in tmp.Planes)
      {
        ret.Planes.Add(plane);
      }

      //while(ret.Planes.Count > 0)
      //{
      //  ret.Planes.RemoveAt(ret.Planes.Count - 1);
      //}

      //Plane p = ret.Planes[0];
      //ret = new Snapshot();
      //ret.Planes.Add(new Plane()
      //{
      //  Altitude = 1,
      //  ArrivalAirport = new Airport()
      //  {
      //    City = "Ostrava",
      //    Country = "CR",
      //    Gps = new Gps()
      //    {
      //      Latitude = 50,
      //      Longitude = 17
      //    },
      //    ICAO = "LKMT",
      //    Name = "Mošnov"
      //  },
      //  DepartureAirport = null,
      //  Callsign = "EZY001",
      //  GpsHistory = new System.Collections.Generic.List<Gps>(),
      //  Heading = 0,
      //});
      //ret.Planes[0].GpsHistory.Add(new Gps() { Latitude = 50, Longitude = 17 });
      //ret.Planes.Add(p);
      return this.Ok(ret);
    }
  }
}