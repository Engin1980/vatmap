using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VatMap.Model.Back.Vatsim;
using VatMap.Model.Back.Vatsim.JsonModel;
using VatMap.Model.Front;

namespace VatMap.Services
{
  public class VatsimProviderService
  {
    //https://status.vatsim.net/status.json
    private static readonly string VATSIM_DATA_URL = "file://r:/vatsim.txt";
    //"https://data.vatsim.net/v3/vatsim-data.json";

    private readonly LogService logService;
    private readonly Downloader downloader = new Downloader();
    private readonly Parser parser = new Parser();

    public VatsimProviderService([FromServices] LogService logService)
    {
      this.logService = logService;
    }

    public Snapshot Obtain()
    {
      Root content;
      Snapshot ret = null;

      try
      {
        content = this.downloader.Download(VATSIM_DATA_URL);
      }
      catch (Exception ex)
      {
        this.logService.Log(LogService.Level.Warning, $"Failed to download content from {VATSIM_DATA_URL}.", ex);
        throw new ApplicationException("Unable to download.", ex);
      }

      try
      {
        ret = this.parser.Parse(content);
      }
      catch (Exception ex)
      {
        this.logService.Log(LogService.Level.Warning, $"Failed to parse content from {VATSIM_DATA_URL}.", ex);
        throw new ApplicationException("Unable to parse content.", ex);
      }

      return ret;
    }
  }
}
