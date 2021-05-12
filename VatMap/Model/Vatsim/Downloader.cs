using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace VatMap.Model.Vatsim
{
  public class Downloader
  {

    public Downloader()
    {
     
    }

    public string Download(string url)
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
  }
}
