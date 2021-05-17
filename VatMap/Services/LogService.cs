using System;

namespace VatMap.Services
{
  public class LogService
  {
    private static string logFile = @"Data\log.txt";
    public enum Level
    {
      Verbose,
      Info,
      Warning,
      Critical,
      Exception
    }

    private void EnsureFileExist()
    {
      //if (!System.IO.File.Exists(logFile))
      //{
      //  System.IO.File.Create(logFile);
      //}
    }

    public void Log(Level level, string text)
    {

      string msg = $"{DateTime.Now.ToString()} : {level} : {text}\n";
      Console.Write(msg);
      lock (this)
      {
        this.EnsureFileExist();
        System.IO.File.AppendAllText(logFile, msg);
      }
    }

    public void Log(Level level, string text, Exception ex)
    {
      string msg = $"{DateTime.Now.ToString()} : {level} : {text} (Exception: {ex.Message})\n";
      Console.Write(msg);
      lock (this)
      {
        this.EnsureFileExist();
        System.IO.File.AppendAllText(logFile, msg);
      }
    }
  }
}
