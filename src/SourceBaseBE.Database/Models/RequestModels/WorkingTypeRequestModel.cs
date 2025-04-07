using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SourceBaseBE.Database.Models.RequestModels
{
	public class WorkingTypeRequestModel
	{
		public IFormFile? FileImport { get; set; }
		public bool? IsReplace { get; set; }


		public Dictionary<string, (string, IFormFile)> GetFiles()
		{
			Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
			if (this.FileImport != null)
			{
				dicRS.Add(nameof(FileImport), (Path.Combine(ConstFolderPath.Image, ConstFolderPath.Upload), this.FileImport));
			}
			/*[GEN-17]*/
			return dicRS;
		}
	}
}
