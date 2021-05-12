using System;
using VatMap.Model.Shared;

namespace VatMap.Model.Vatsim
{
  public class Record
  {
    public enum RecordClientType
    {
      Empty,
      Pilot,
      Atc
    }

    public string Callsign { get; set; }
    public string CId { get; set; }
    public string RealName { get; set; }
    public RecordClientType ClientType { get; set; }
    public double? Frequency { get; set; }
    public Position? Position { get; set; }
    public int? Altitude { get; set; }
    public int? GroundSpeed { get; set; }
    public string PlannedAircraft { get; set; }
    public int? PlannedTasCruise { get; set; }
    public string PlannedDepartureAirport { get; set; }
    public string PlannedAltitude { get; set; }
    public string PlannedDestinationAirport { get; set; }
    public string Server { get; set; }
    public string protrevision { get; set; }
    public string Rating { get; set; }
    public string Transponder { get; set; }
    public string FacilityType { get; set; }
    public string VisualRange { get; set; }
    public string PlannedRevision { get; set; }
    public string PlannedFlightType { get; set; }
    public TimeSpan? PlannedDepTime { get; set; }
    public TimeSpan? PlannedActDepTime { get; set; }
    public TimeSpan? PlannedEnrouteTime { get; set; }
    public TimeSpan? PlannedFuelTime { get; set; }
    public string PlannedAlternateAirport { get; set; }
    public string PlannedRemark { get; set; }
    public string PlannedRoute { get; set; }
    public double? PlannedDepartureAirportLatitude { get; set; }
    public double? PlannedDepartureAirportLongitude { get; set; }
    public double? PlannedDestinationAirportLatitude { get; set; }
    public double? PlannedDestinationAirportLongitude { get; set; }
    public string AtisMessage { get; set; }
    public DateTime? TimeLastAtisReceived { get; set; }
    public DateTime TimeLogon { get; set; }
    public int? Heading { get; set; }
    public double? QnhHg { get; set; }
    public double? QnhMb { get; set; }

    public Record() { }

    public override string ToString()
    {
      return string.Format("{0} : {1}-{2}",
        this.Callsign, this.PlannedDepartureAirport, this.PlannedDestinationAirport);
    }
  }
}
