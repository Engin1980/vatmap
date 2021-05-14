using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VatMap.Model.Front
{
  public class Snapshot
  {
    public List<Plane> Planes { get; set; }
    public List<Atc> Atcs { get; set; }
    public DateTime Date { get; set; }

    public Snapshot Clone()
    {
      MemoryStream ms = new MemoryStream();
      BinaryFormatter bf = new BinaryFormatter();

      bf.Serialize(ms, this);

      ms.Position = 0;
      object obj = bf.Deserialize(ms);
      ms.Close();

      return obj as Snapshot;
    }
  }
}
