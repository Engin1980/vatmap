using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using VatMap.Model.Back.Vatsim;

namespace VatMap.Services
{
  public class VatsimProviderService
  {
    private static readonly string[] URLs = new string[] {
      "http://www.pcflyer.net/DataFeed/vatsim-data.txt",
      "http://fsproshop.com/servinfo/vatsim-data.txt",
      "http://info.vroute.net/vatsim-data.txt",
      "http://data.vattastic.com/vatsim-data.txt",
      "http://vatsim.aircharts.org/vatsim-data.txt",
      "http://vatsim-data.hardern.net/vatsim-data.txt" };

    private readonly LogService logService;
    private readonly Downloader downloader = new Downloader();
    private readonly Parser parser = new Parser();

    public VatsimProviderService([FromServices] LogService logService)
    {
      this.logService = logService;

      //TODO
      //shuffle urls
      //Random rnd = new Random();
      //URLs = URLs.OrderBy(q => r.Next()).ToArray();
    }

    public RecordSet Obtain()
    {
      RecordSet ret = null;
      for (int i = 0; i < URLs.Length; i++)
      {
        string url = URLs[i];
        try
        {
          ret = Obtain(url);
          if (ret.State == RecordSet.StateByTime.Current)
            break;
        }
        catch (Exception ex)
        {
          // logged inside already 
          // just skip to take another record
          continue;
        }
      }
      if (ret == null) {
        this.logService.Log(LogService.Level.Critical, "Failed to download some VATSIM data!");
        throw new ApplicationException("Unable to download VATSIM data.");
      }

      return ret;
    }

    public RecordSet Obtain(string url)
    {
      string content;
      RecordSet ret = null;

      try
      {
        content = downloader.Download(url);      
      } catch (Exception ex)
      {
        logService.Log(LogService.Level.Warning, $"Failed to download content from {url}.", ex);
        throw new ApplicationException("Unable to download.", ex);
      }

      try
      {
        ret = parser.Parse(content);
      }
      catch (Exception ex)
      {
        logService.Log(LogService.Level.Warning, $"Failed to parse content from {url}.", ex);
        throw new ApplicationException("Unable to parse content.", ex);
      }

      return ret;
    }
  }
}
