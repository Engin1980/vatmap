using System;
using System.Collections.Generic;

namespace VatMap.Model.Vatsim
{
  public class RecordSet
  {
    public enum StateByTime
    {
      Outdated,
      Current
    }
    public List<Record> Records { get; private set; }
    public DateTime UpdateTime { get; private set; }
    public StateByTime State { get; private set; }
    public RecordSet(DateTime updateTime, List<Record> records)
    {
      this.UpdateTime = updateTime;
      this.Records = records;
      this.State = this.UpdateTime.AddMinutes(3) < DateTime.UtcNow ? StateByTime.Current : StateByTime.Outdated;
    }
  }
}
