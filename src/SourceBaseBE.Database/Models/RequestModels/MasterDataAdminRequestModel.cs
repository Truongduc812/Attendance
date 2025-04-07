using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class MasterDataAdminRequestModel : MasterDataRequestModel
  {
    public bool? Role { get; set; }
  }
}
