using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace VatMap.Model.Shared
{
  public static class Utils
  {
    internal const int MAX_HISTORY_LENGTH = 3;


    private static readonly System.Globalization.CultureInfo cultureInfo =
      System.Globalization.CultureInfo.GetCultureInfo("en-US");


    public static double? ToDouble(string txt, bool canBeEmpty = false)
    {
      double ret;
      try
      {
        if (string.IsNullOrEmpty(txt) && canBeEmpty)
          ret = 0;
        else
          ret = double.Parse(txt, cultureInfo);

      }
      catch (Exception ex)
      {
        throw new ApplicationException("Failed to parse value " + txt + " to double.", ex);
      }
      return ret;
    }
    public static int ToInt(string txt, bool canBeEmpty = false)
    {
      int ret;

      try
      {
        if (string.IsNullOrEmpty(txt) && canBeEmpty)
          ret = 0;
        else
          ret = int.Parse(txt, cultureInfo);
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Failed to parse value " + txt + " to int.", ex);
      }
      return ret;
    }
    public static DateTime ToDateTime(string txt, bool canBeEmpty = false)
    {
      DateTime ret;

      try
      {
        if (string.IsNullOrEmpty(txt) && canBeEmpty)
          ret = new DateTime();
        else
        {
          string p = @"(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})";
          var m = Regex.Match(txt, p);
          int year = ToInt(m.Groups[1].Value);
          int month = ToInt(m.Groups[2].Value);
          int day = ToInt(m.Groups[3].Value);
          int hour = ToInt(m.Groups[4].Value);
          int minute = ToInt(m.Groups[5].Value);
          int second = ToInt(m.Groups[6].Value);

          ret = new DateTime(year, month, day, hour, minute, second);
        }
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Failed to parse value " + txt + " to DateTime.", ex);
      }
      return ret;
    }
    [Obsolete]
    public static int ToAltitude(string txt, bool canBeEmpty)
    {
      int ret = 0;

      try
      {
        if (string.IsNullOrEmpty(txt) && canBeEmpty)
          ret = 0;
        else
        {
          string pattern = @"(F|FP)?(\d+)";
          var m = Regex.Match(txt, pattern);
          int val = ToInt(m.Groups[2].Value, false);
          if (m.Groups[1].Success)
            ret = int.Parse(txt, cultureInfo);
        }
      }
      catch (Exception ex)
      {
        throw new ApplicationException("Failed to parse value " + txt + " to int.", ex);
      }
      return ret;
    }

    internal static TimeSpan ToTimeSpanAsHour(double v)
    {
      int hrs = (int)Math.Floor(v);
      int mins = (int)Math.Floor((v - hrs) * 60.0);
      TimeSpan ret = new TimeSpan(hrs, mins, 0);
      return ret;
    }

    internal static TimeSpan ToTime(string v)
    {
      TimeSpan ret;
      int h = 0;
      int m = 0;
      switch (v.Length)
      {
        case 0:
        case 1:
        case 2:
          h = 0;
          m = 0;
          break;
        case 3:
          h = int.Parse(v.Substring(0, 1));
          m = int.Parse(v.Substring(1));
          break;
        case 4:
          h = int.Parse(v.Substring(0, 2));
          m = int.Parse(v.Substring(2));
          break;
        default:
          //throw new NotImplementedException();
          break;
      } // switch
      ret = new TimeSpan(h, m, 0);
      return ret;
    }

    internal static TimeSpan ToTimeSpan(string v1, string v2)
    {
      int hours = int.Parse(v1);
      int mins = int.Parse(v2);
      TimeSpan ret = new TimeSpan(hours, mins, 0);
      return ret;
    }

    //internal static void CopyEntintyData<T>(T source, T target)
    //{
    //  var props = source.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Public);
    //  object o;
    //  foreach (var prop in props)
    //  {
    //    o = prop.GetValue(source);
    //    prop.SetValue(target, o);
    //  }
    //}

    internal static void Adjust<T>(ref T target, T data)
    {
      if (target == null)
        if (data == null)
          return;
        else
          target = data;
      else
        if (!target.Equals(data))
        target = data;
    }
    internal static void Adjust<T>(ref T target, Nullable<T> data, T defaultValue) where T : struct
    {
      if (data == null)
        Adjust(ref target, defaultValue);
      else
        Adjust(ref target, data.Value);
    }


  }
}
