using System;
using System.Collections.Generic;

namespace VatMap.Model.Front
{
  public class Snapshot
  {
    public List<Plane> Planes { get; set; }
    public List<Atc> Atcs { get; set; }
    public DateTime Date { get; set; }
  }
}
