using iSoft.Common.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static iSoft.Common.ConstCommon;
using static System.Net.Mime.MediaTypeNames;

namespace iSoft.Common.Utils
{
	public class UploadUtil
	{
		public static Dictionary<string, string> UploadFile(
		  Dictionary<string, (string, IFormFile)> dicFormFile)
		{
			Dictionary<string, string> dicRS = new Dictionary<string, string>();
			foreach (var keyVal in dicFormFile)
			{
				if (keyVal.Value.Item2 != null)
				{
					string folderPath = keyVal.Value.Item1;
					IFormFile opImage = keyVal.Value.Item2;
					var fileNameEx = StringUtil.SplitFileName(opImage.FileName);
					string uniqueFileName = StringUtil.GenerateRandomKeyWithDateTime() + "_"
					  + fileNameEx.fileName.RemoveSpecialChar("").Truncate(10) + fileNameEx.extension;
					string fullPathUploadFolder = Path.Combine(Directory.GetCurrentDirectory(), ConstFolderPath.Root, folderPath);
					if (!Directory.Exists(fullPathUploadFolder))
					{
						Directory.CreateDirectory(fullPathUploadFolder);
					}
					string filePath = Path.Combine(fullPathUploadFolder, uniqueFileName);
					using (var fileStream = new FileStream(filePath, FileMode.Create))
					{
						opImage.CopyTo(fileStream);
					}
					string imageUrl = Path.DirectorySeparatorChar + Path.Combine(folderPath, uniqueFileName);
					dicRS.Add(keyVal.Key, imageUrl);
				}
			}
			return dicRS;
		}

        public static Dictionary<string, string> UploadImage(
          Dictionary<string, (string, IFormFile)> dicFormFile)
        {
            Dictionary<string, string> dicRS = new Dictionary<string, string>();
            foreach (var keyVal in dicFormFile)
            {
                if (keyVal.Value.Item2 != null)
                {
                    string folderPath = keyVal.Value.Item1;
                    IFormFile opImage = keyVal.Value.Item2;
                    var fileNameEx = StringUtil.SplitFileName(opImage.FileName);
                   
					string uniqueFileName = StringUtil.GenerateRandomKeyWithDateTime() + "_"
                      + fileNameEx.fileName.RemoveSpecialChar("").Truncate(10) + fileNameEx.extension;
                    
					if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }
                    string filePath = Path.Combine(folderPath, uniqueFileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        opImage.CopyTo(fileStream);
                    }
                    string imageUrl = Path.Combine(folderPath, uniqueFileName);
                    dicRS.Add(keyVal.Key, imageUrl);
                }
            }
            return dicRS;
        }

        public static string UploadFileExcel(
		  Dictionary<string, (string, IFormFile)> dicFormFile)
		{
			string filePath = "";
			foreach (var keyVal in dicFormFile)
			{
				if (keyVal.Value.Item2 != null)
				{
					string folderPath = keyVal.Value.Item1;
					IFormFile opImage = keyVal.Value.Item2;
					var fileNameEx = StringUtil.SplitFileName(opImage.FileName);
					string uniqueFileName = StringUtil.GenerateRandomKeyWithDateTime() + "_"
					  + fileNameEx.fileName.RemoveSpecialChar("").Truncate(10) + fileNameEx.extension;
					string fullPathUploadFolder = Path.Combine(Directory.GetCurrentDirectory(), folderPath);
					if (!Directory.Exists(fullPathUploadFolder))
					{
						Directory.CreateDirectory(fullPathUploadFolder);
					}
					filePath = Path.Combine(fullPathUploadFolder, uniqueFileName);
					using (var fileStream = new FileStream(filePath, FileMode.OpenOrCreate))
					{
						opImage.CopyTo(fileStream);
					}

				}
			}
			return filePath;
		}
	}
}
