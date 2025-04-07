using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class JobTitleRequestModel : BaseCRUDRequestModel<JobTitleEntity>
  {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public List<EmployeeEntity>? Employees { get; set; }

    public override JobTitleEntity GetEntity(JobTitleEntity entity)
    {
      if (this.Id != null) entity.Id = (long)this.Id;
      if (this.Order != null) entity.Order = this.Order;
      if (this.Name != null) entity.Name = this.Name;
      if (this.Description != null) entity.Description = this.Description;
      if (this.Notes != null) entity.Notes = this.Notes;
      if (this.Employees != null) entity.Employees = this.Employees;

      return entity;
    }

    public override Dictionary<string, (string, IFormFile)> GetFiles()
    {
      Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
      
/*[GEN-17]*/
      return dicRS;
    }
  }
}
