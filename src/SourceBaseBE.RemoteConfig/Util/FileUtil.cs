
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace iSoft.RemoteConfig.Util
{
  public class FileUtil
  {
    public static List<string> getFilePath(string[] arrFileExtension, string folderRoot)
    {
      List<string> listPath = new List<string>();
      string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderRoot);

      if (Directory.Exists(folderPath))
      {
        List<string> filePaths = Directory
          .GetFiles(folderPath)
          .Where(file => arrFileExtension.Any(file.ToLower().EndsWith))
          .ToList();

        foreach (string filePath in filePaths)
        {
          string fileName = Path.GetFileName(filePath);
          string fileUrl = $"/{folderRoot}/" + fileName;
          listPath.Add(fileUrl);
        }
      }

      return listPath;
    }
  }
}
