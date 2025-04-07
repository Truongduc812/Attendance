using iSoft.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Utils
{
  public class FileUtil
  {
    public static void CopyFile(string srcFilePath, string desFilePath)
    {
      if (File.Exists(srcFilePath))
      {
        File.Copy(srcFilePath, desFilePath, true);
      }
      else
      {
        throw new Exception($"[CopyFile] File source not exists, srcFilePath: {srcFilePath}");
      }
    }

    public static string GetCurrentPath()
    {
      return Directory.GetCurrentDirectory();
    }

    public static string? ReadFile(string filePath)
    {
      if (File.Exists(filePath))
      {
        return File.ReadAllText(filePath);
      }
      return null;
    }

    public static void WriteFile(string filePath, string text)
    {
      string directoryPath = Path.GetDirectoryName(filePath);
      if (!Directory.Exists(directoryPath))
      {
        Directory.CreateDirectory(directoryPath);
      }
      using (StreamWriter sw = new StreamWriter(filePath, false))
      {
        sw.Write(text);
      }
    }
  }
}
