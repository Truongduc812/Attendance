using iSoft.Common.Models.ResponseModels;
using Newtonsoft.Json;
using SourceBaseBE.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.ResponseModels
{
	public class UserAndDepartmentAdminResponseModel
  {
		[JsonProperty("user", NullValueHandling = NullValueHandling.Ignore)]
		public new UserModel? UserEntities { get; set; }

    [JsonProperty("departmentAdmin", NullValueHandling = NullValueHandling.Ignore)]
    public new DepartmentAdminModel? DepartmentAdminEntities { get; set; }
  }
	 
}
