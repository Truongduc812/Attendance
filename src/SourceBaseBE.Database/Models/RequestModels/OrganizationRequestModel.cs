using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels
{
  public class OrganizationRequestModel : BaseCRUDRequestModel<OrganizationEntity>
  {
    public string Name { get; set; }
    public string? Category { get; set; }
    public ICollection<FactoryEntity> Factories { get; set; }
    public ICollection<DepartmentEntity> Departments { get; set; }

    public override OrganizationEntity GetEntity(OrganizationEntity entity)
    {
      if (this.Id != null) entity.Id = (long)this.Id;
      if (this.Order != null) entity.Order = this.Order;
      if (this.Name != null) entity.Name = this.Name;
      if (this.Category != null) entity.Category = this.Category;
      if (this.Factories != null) entity.Factories = this.Factories;
      if (this.Departments != null) entity.Departments = this.Departments;

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
