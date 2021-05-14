using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using VatMap.Model.Back.Vatsim.JsonModel;

namespace VatMap.Services
{
  public class VatsimProviderService
  {
    //https://status.vatsim.net/status.json
    private static readonly string VATSIM_DATA_URL = "file://r:/vatsim.txt";
    //"https://data.vatsim.net/v3/vatsim-data.json";

    private readonly LogService logService;

    public VatsimProviderService([FromServices] LogService logService)
    {
      this.logService = logService;
    }

    private Root LoadJson(string txt)
    {
      Root ret = JsonConvert.DeserializeObject<Root>(txt);
      return ret;
    }

    private string Fetch(string url)
    {
      WebRequest request = WebRequest.Create(url);
      request.Method = "GET";
      WebResponse response = request.GetResponse();
      Stream stream = response.GetResponseStream();
      StreamReader reader = new StreamReader(stream);
      string ret = reader.ReadToEnd();
      reader.Close();
      response.Close();

      return ret;
    }

    public Root Obtain()
    {
      String content;
      Root ret = null;

      try
      {
        content = this.Fetch(VATSIM_DATA_URL);
      }
      catch (Exception ex)
      {
        this.logService.Log(LogService.Level.Warning, $"Failed to download content from {VATSIM_DATA_URL}.", ex);
        throw new ApplicationException("Unable to download.", ex);
      }

      try
      {
        ret = this.LoadJson(content);
      }
      catch (Exception ex)
      {
        this.logService.Log(LogService.Level.Warning, $"Failed to parse JSON content from {VATSIM_DATA_URL}.", ex);
        throw new ApplicationException("Unable to parse content.", ex);
      }

      return ret;
    }
  }
}
