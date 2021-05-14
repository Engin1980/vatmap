using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using VatMap.Model.Back.Vatsim.JsonModel;
using VatMap.Model.Front;

namespace VatMap.Services
{
  public class SnapshotProviderService
  {
    private readonly Updater updater;
    public SnapshotProviderService([FromServices] AirportService airportService)
    {
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
            plane = BuildPlane(item);
            snapshot.Planes.Add(plane);
          }
          else
          {
            //TODO resolve the same callsign with flightplan change
            plane = snapshot.Planes.First(q => q.Callsign == item.callsign);
            existingCallsigns.Remove(plane.Callsign);
          }
          plane.GpsHistory.Insert(0, new Gps() { Latitude = item.latitude, Longitude = item.longitude });

          snapshot.Planes.RemoveAll(q => existingCallsigns.Contains(q.Callsign));
        }
      }

      private Plane BuildPlane(Pilot item)
      {
        Plane ret = new Plane()
        {
          Callsign = item.callsign,
          Altitude = item.altitude,
          ArrivalAirport = airportService.GetOrDefault(item.flight_plan.arrival),
          DepartureAirport = airportService.GetOrDefault(item.flight_plan.departure),
          GpsHistory = new List<Gps>(),
          Heading = item.heading
        };
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

    public void UpdateByVatsim(Root jsonData)
    {
      Snapshot tmp = this.snapshot.Clone();
      this.updater.Update(this.snapshot, jsonData);
      lock (this.snapshot)
      {
        this.snapshot = tmp;
      }
    }

  }
}
