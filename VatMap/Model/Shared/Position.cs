using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VatMap.Model.Shared
{
  public struct Position
  {
    public readonly double Latitude;
    public readonly double Longitude;

    public bool IsZero
    {
      get
      {
        return Latitude == 0 || Longitude == 0;
      }
    }

    public Position(double lat, double lon)
    {
      this.Latitude = lat;
      this.Longitude = lon;
    }

    public override bool Equals(object obj)
    {
      if (obj is Position)
      {
        Position other = (Position)obj;
        return this.Latitude == other.Latitude && this.Longitude == other.Longitude;
      }
      else
        return false;
    }
    public override int GetHashCode()
    {
      return Latitude.GetHashCode() + Longitude.GetHashCode();
    }

    public Position Clone()
    {
      return new Position(this.Latitude, this.Longitude);
    }

    public override string ToString()
    {
      return string.Format("({0},{1})", this.Latitude, this.Longitude);
    }

    public static bool IsInRectangle(double nLat, double sLat, double wLng, double eLng, double lat, double lng)
    {
      bool retLat;
      bool retLng;
      bool ret;

      retLat = lat.IsBetween(sLat, nLat);
      if (eLng < wLng)
      {
        // pres 0 polednik
        retLng = lng.IsBetween(eLng, 180) || lng.IsBetween(-180, wLng);
      }
      else
      {
        retLng = lng.IsBetween(wLng, eLng);
      }
      ret = retLat && retLng;
      return ret;
    }

    public static double GetDistanceInNM(Position positionA, Position positionB)
    {
      double R = 6371;
      double o1 = ToRad(positionA.Latitude);
      double o2 = ToRad(positionB.Latitude);
      double o = ToRad(positionB.Latitude - positionA.Latitude);
      double l = ToRad(positionB.Longitude - positionA.Longitude);

      double a =
        Math.Sin(o / 2) * Math.Sin(o / 2) +
        Math.Cos(o1) * Math.Cos(o2) *
        Math.Sin(l / 2) * Math.Sin(l / 2);
      double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
      double ret = R * c;
      ret *= 0.5399568; // to NM
      return ret;
    }

    public static double GetBearing(Position from, Position to)
    {
      /*
      var y = Math.sin(λ2-λ1) * Math.cos(φ2);
var x = Math.cos(φ1)*Math.sin(φ2) -
        Math.sin(φ1)*Math.cos(φ2)*Math.cos(λ2-λ1);
var brng = Math.atan2(y, x).toDegrees();
*/
      double l1 = ToRad(from.Latitude);
      double l2 = ToRad(to.Latitude);
      double o1 = ToRad(from.Longitude);
      double o2 = ToRad(to.Longitude);

      double x = Math.Sin(l2 - l1) * Math.Cos(o2);
      double y = Math.Cos(o1) * Math.Sin(o2) -
        Math.Sin(o1) * Math.Cos(o2) * Math.Cos(l2 - l1);
      double ret = Math.Atan2(y, x);
      ret = ToDeg(ret);
      return ret;
    }

    private static double ToRad(double value)
    {
      return Math.PI * value / 180;
    }
    private static double ToDeg(double value)
    {
      return value * 180 / Math.PI;
    }

    public static TimeSpan GetTimeTo(Position pa, Position pb, double speedInMPH)
    {
      double dist = Position.GetDistanceInNM(pa, pb);
      double tmp = dist / speedInMPH;
      int h;
      int m;
      int s;
      h = (int)tmp;
      tmp = (tmp - h) * 60;
      m = (int)tmp;
      tmp = (tmp - m) * 60;
      s = (int)tmp;
      TimeSpan ret = new TimeSpan(h, m, s);
      return ret;
    }

    public static bool operator ==(Position a, Position b)
    {
      return a.Latitude == b.Latitude && a.Longitude == b.Longitude;
    }
    public static bool operator !=(Position a, Position b)
    {
      return !(a == b);
    }
  }
}
