using System;
using System.Collections.Generic;
using VatMap.Model.Back.Shared;
using VatMap.Model.Back.Vatsim.JsonModel;
using VatMap.Model.Front;

namespace VatMap.Model.Back.Vatsim
{
  public class Parser
  {
    public Parser()
    {

    }

    internal Snapshot Parse(Root jsonData)
    {

      DateTime updateTime = jsonData.general.update_timestamp;

      List<Plane> planes = ReadPlanes(jsonData);
      List<Atc> atcs = ReadAtcs(jsonData);

      Snapshot ret = new Snapshot()
      {
        Atcs = atcs,
        Planes = planes,
        Date = updateTime
      };
      return ret;
    }

    private List<Plane> ReadPlanes(Root jsonData)
    {
      List<Plane> ret = new List<Plane>();

      jsonData.pilots.ForEach(q =>
      {
        Plane p = new Plane()
        {
          Altitude = q.altitude,
          Callsign = q.callsign,
          Heading = q.heading,
          Gps = new Gps()
          {
            Latitude = q.latitude,
            Longitude = q.longitude
          }
        };
        ret.Add(p);
      });

      return ret;
    }

    private List<Atc> ReadAtcs(Root jsonData)
    {
      List<Atc> ret = new List<Atc>();

      jsonData.controllers.ForEach(q =>
      {
        Atc atc = new Atc()
        {
          Callsign = q.callsign,
          Frequency = q.frequency,
          Name = q.name
        };
        ret.Add(atc);
      });

      return ret;
    }



    #region Record Parsing Utils

    [Flags]
    public enum ParseFlag
    {
      None = 0,
      ZeroAsNull = 1,
      IllegalAsNull = 2
    }

    private static Position? ParseToPosition(string lat, string lon)
    {
      if (string.IsNullOrEmpty(lat) || string.IsNullOrEmpty(lon))
        return null;
      else
      {
        double dlat = ParseToDouble(lat).Value;
        double dlon = ParseToDouble(lon).Value;

        return new Position(dlat, dlon);
      }
    }

    private static Record.RecordClientType ParseToClientType(string s)
    {
      if (string.IsNullOrEmpty(s))
        return Record.RecordClientType.Empty;

      switch (s)
      {
        case "PILOT":
          return Record.RecordClientType.Pilot;
        case "ATC":
          return Record.RecordClientType.Atc;
        default:
          throw new NotSupportedException();
      }
    }

    private static System.Globalization.CultureInfo enUs = System.Globalization.CultureInfo.GetCultureInfo("en-US");

    private static double? ParseToDouble(string s)
    {
      if (string.IsNullOrEmpty(s))
        return null;
      else
        return double.Parse(s, enUs);
    }

    private static int? ParseToInt(string s)
    {
      if (string.IsNullOrEmpty(s))
        return null;
      else
        return int.Parse(s);
    }

    private static TimeSpan? ParseToTimeSpan(string s, ParseFlag flags = ParseFlag.None)
    {
      TimeSpan ret;
      if (string.IsNullOrEmpty(s)) return null;
      if ((flags & ParseFlag.ZeroAsNull) > 0 && s == "0") return null;
      if ((flags & ParseFlag.IllegalAsNull) > 0 && (s.Length < 3 || s.Length > 4)) return null;
      if (s.Length == 3 || s.Length == 4)
      {
        int h = int.Parse(s.Substring(0, s.Length - 2));
        int m = int.Parse(s.Substring(s.Length - 2));
        ret = new TimeSpan(h, m, 0);
      }
      else
        throw new NotImplementedException();

      return ret;
    }

    private static TimeSpan? ParseToTimeSpan(string h, string m)
    {
      int? hh = ParseToInt(h);
      int? mm = ParseToInt(m);

      if (hh == null || mm == null) return null;

      TimeSpan ret = new TimeSpan(hh.Value, mm.Value, 0);
      return ret;
    }

    private static DateTime? ParseToDateTime(string s)
    {
      DateTime ret;
      if (string.IsNullOrEmpty(s)) return null;
      if (s.Length == 14)
      {
        ret = DateTime.ParseExact(s, "yyyyMMddHHmmss", enUs);
      }
      else
        throw new NotImplementedException();

      return ret;

    }
    #endregion
  }
}
