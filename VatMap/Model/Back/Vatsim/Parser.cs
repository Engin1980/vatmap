using System;
using System.Collections.Generic;
using VatMap.Model.Back.Shared;

namespace VatMap.Model.Back.Vatsim
{
  public class Parser
  {
    private const string CLIENTS_LINE = "!CLIENTS:";
    private const string BLOCK_END_LINE = ";";
    private const string UPDATE_LINE = "UPDATE";

    public Parser()
    {

    }

    internal RecordSet Parse(string content)
    {
      RecordSet ret;

      string[] lines = content.Split('\n');
      int index = 0;

      DateTime updateTime = this.ReadUpdateTime(lines, ref index);

      index = this.ScrollToClients(lines, index);
      List<Record> records = ReadClients(lines, ref index);

      ret = new RecordSet(updateTime, records);
      return ret;
    }

    private static List<Record> ReadClients(string[] lines, ref int index)
    {
      List<Record> ret = new List<Record>();

      while (true)
      {
        if (lines[index] == BLOCK_END_LINE) break; // end of data
        index++;
        string line = lines[index];
        string[] pts = line.Split(':');
        if (pts.Length < 39) break; // invalid data? or what?

        Record r = ParseLineBlocks(pts);

        ret.Add(r);
      }

      return ret;
    }

    private static Record ParseLineBlocks(string[] data)
    {
      Record ret = new Record();

      ret.Callsign = data[0];
      ret.CId = data[1];
      ret.RealName = data[2];
      ret.ClientType = ParseToClientType(data[3]);
      ret.Frequency = ParseToDouble(data[4]);
      ret.Position = ParseToPosition(data[5], data[6]);
      ret.Altitude = ParseToInt(data[7]);
      ret.GroundSpeed = ParseToInt(data[8]);
      ret.PlannedAircraft = data[9];
      ret.PlannedTasCruise = ParseToInt(data[10]).Value;
      ret.PlannedDepartureAirport = data[11];
      ret.PlannedAltitude = data[12];
      ret.PlannedDestinationAirport = data[13];
      ret.Server = data[14];
      ret.protrevision = data[15];
      ret.Rating = data[16];
      ret.Transponder = data[17];
      ret.FacilityType = data[18];
      ret.VisualRange = data[19];
      ret.PlannedRevision = data[20];
      ret.PlannedFlightType = data[21];
      ret.PlannedDepTime = ParseToTimeSpan(data[22], ParseFlag.ZeroAsNull | ParseFlag.IllegalAsNull);
      ret.PlannedActDepTime = ParseToTimeSpan(data[23], ParseFlag.ZeroAsNull | ParseFlag.IllegalAsNull);
      ret.PlannedEnrouteTime = ParseToTimeSpan(data[24], data[25]);
      ret.PlannedFuelTime = ParseToTimeSpan(data[26], data[27]);
      ret.PlannedAlternateAirport = data[28];
      ret.PlannedRemark = data[29];
      ret.PlannedRoute = data[30];
      ret.PlannedDepartureAirportLatitude = ParseToDouble(data[31]);
      ret.PlannedDepartureAirportLongitude = ParseToDouble(data[32]);
      ret.PlannedDestinationAirportLatitude = ParseToDouble(data[33]);
      ret.PlannedDestinationAirportLongitude = ParseToDouble(data[34]);
      ret.AtisMessage = data[35];
      ret.TimeLastAtisReceived = ParseToDateTime(data[36]);
      ret.TimeLogon = ParseToDateTime(data[37]).Value;
      ret.Heading = ParseToInt(data[38]);
      ret.QnhHg = ParseToDouble(data[39]);
      ret.QnhMb = ParseToDouble(data[40]);

      return ret;
    }

    private DateTime ReadUpdateTime(string[] lines, ref int index)
    {
      while (lines[index].StartsWith(UPDATE_LINE) == false)
      {
        index++;
        if (index >= lines.Length)
          throw new Exception("Unable to find data in the lines.");
      }
      string tmp = lines[index].Substring(9);
      DateTime ret = Utils.ToDateTime(tmp, false);
      index++;
      return ret;
    }

    private int ScrollToClients(string[] lines, int index)
    {
      while (lines[index] != CLIENTS_LINE) index++;
      index++;
      return index;
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
