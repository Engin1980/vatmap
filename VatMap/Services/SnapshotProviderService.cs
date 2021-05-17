using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VatMap.Model.Back.Vatsim.JsonModel;
using VatMap.Model.Front;

namespace VatMap.Services
{
  public class SnapshotProviderService
  {
    private readonly VatsimProviderService vatsimProvider;
    private readonly Updater updater;
    private readonly LogService logService;
    private bool updateInProgress = false;

    public SnapshotProviderService([FromServices] VatsimProviderService vatsimProvider,
      [FromServices] AirportService airportService,
      [FromServices] LogService logService)
    {
      this.logService = logService;
      this.vatsimProvider = vatsimProvider;
      this.updater = new Updater(airportService);
    }
    private class Updater
    {
      private readonly AirportService airportService;

      public Updater(AirportService airportService)
      {
        this.airportService = airportService;
      }

      public void Update(Snapshot snapshot, Root jsonData)
      {
        snapshot.Date = jsonData.general.update_timestamp;
        this.UpdatePlanes(snapshot, jsonData);
        this.UpdateAtcs(snapshot, jsonData);
      }

      private void UpdatePlanes(Snapshot snapshot, Root jsonData)
      {
        ISet<string> existingCallsigns = snapshot.Planes.Select(q => q.Callsign).ToHashSet();

        foreach (var item in jsonData.pilots)
        {
          Plane plane;
          if (existingCallsigns.Contains(item.callsign) == false)
          {
            plane = this.BuildPlane(item);
            snapshot.Planes.Add(plane);
          }
          else
          {
            //TODO resolve the same callsign with flightplan change
            plane = snapshot.Planes.First(q => q.Callsign == item.callsign);
            existingCallsigns.Remove(plane.Callsign);
          }
          plane.GpsHistory.Insert(0, new Gps() { Latitude = item.latitude, Longitude = item.longitude });
        }
        snapshot.Planes.RemoveAll(q => existingCallsigns.Contains(q.Callsign));
      }

      private Plane BuildPlane(Pilot item)
      {
        Plane ret = new Plane()
        {
          Callsign = item.callsign,
          Altitude = item.altitude,          
          GpsHistory = new List<Gps>(),
          Heading = item.heading
        };
        if (item.flight_plan != null)
        {
          ret.ArrivalAirport = this.airportService.GetOrDefault(item.flight_plan.arrival);
          ret.DepartureAirport = this.airportService.GetOrDefault(item.flight_plan.departure);
        }
        return ret;
      }

      private void UpdateAtcs(Snapshot snapshot, Root jsonData)
      {
        snapshot.Atcs.Clear();
        foreach (var item in jsonData.controllers)
        {
          Atc atc = BuildAtc(item);
          snapshot.Atcs.Add(atc);
        }
      }

      private static Atc BuildAtc(Model.Back.Vatsim.JsonModel.Controller item)
      {
        Atc ret = new Atc()
        {
          Callsign = item.callsign,
          Frequency = item.frequency,
          Name = item.name
        };
        return ret;
      }
    }
    private Snapshot snapshot = new Snapshot();

    public Snapshot GetCurrent()
    {
      Snapshot ret;
      lock (this.snapshot)
      {
        ret = this.snapshot;
      }
      return ret;
    }

    private bool TryLockUpdate()
    {
      lock (this.updater)
      {
        if (this.updateInProgress)
        {
          this.logService.Log(LogService.Level.Verbose, "Updating already locked, skipping.");
          return false;
        }
        else
        {
          this.updateInProgress = true;
          this.logService.Log(LogService.Level.Verbose, "Locking updated.");
          return true;
        }
      }
    }

    public void UnlockUpdate()
    {
      lock (this.updater)
      {
        this.updateInProgress = false;
        this.logService.Log(LogService.Level.Verbose, "Updating unlocked.");
      }
    }

    public async Task UpdateByVatsimAsync()
    {
      if (TryLockUpdate() == false) return;

      this.logService.Log(LogService.Level.Info, "Updating by Vatsim.");
      Root jsonData = await Task.FromResult<Root>(this.vatsimProvider.Obtain());
      Snapshot tmp = this.snapshot.Clone();
      await Task.Run(() => this.updater.Update(tmp, jsonData));
      lock (this.snapshot)
      {
        this.snapshot = tmp;
      }

      UnlockUpdate();
    }

    public async void UpdateByVatsimIfObsoleteAsync()
    {
      this.logService.Log(LogService.Level.Verbose, "Update by Vatsim if obsolete.");
      if (this.snapshot.IsObsolete)
        await this.UpdateByVatsimAsync();
    }
  }
}
