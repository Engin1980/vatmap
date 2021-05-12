using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VatMap.Services
{
  public class LogService
  {
    private static string logFile = @"~\Data\log.txt";
    public enum Level
    {
      Verbose,
      Info,
      Warning,
      Critical,
      Exception
    }

    public void Log(Level level, string text)
    {
      string msg = $"{DateTime.Now.ToString()} : {level} : {text}\n";
      Console.Write(msg);
      System.IO.File.AppendAllText(logFile, msg);
    }

    public void Log(Level level, string text, Exception ex)
    {
      string msg = $"{DateTime.Now.ToString()} : {level} : {text} (Exception: {ex.Message})\n";
      Console.Write(msg);
      System.IO.File.AppendAllText(logFile, msg);
    }
  }
}
