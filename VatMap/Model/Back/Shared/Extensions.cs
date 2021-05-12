using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VatMap.Model.Back.Shared
{
  public static class Extensions
  {
    public static string ToHHMM(this TimeSpan ts)
    {
      if (ts == null)
        return null;
      else
        return string.Format("{0:D2}:{1:D2}",
            ts.Hours,
            ts.Minutes);
    }

    public static string ToHHMM(this DateTime d)
    {
      if (d == null) return null;
      else
        return string.Format("{0:D2}:{1:D2}",
          d.Hour,
          d.Minute);
    }

    public static string ToWebString(this DateTime value)
    {
      return value.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public static bool IsBetween<T>(this T val, T min, T max) where T : IComparable<T>
    {
      return min.CompareTo(val) < 1 && val.CompareTo(max) < 1;
    }
  }
}
