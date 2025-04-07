using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class OrganizationResponseModel : BaseCRUDResponseModel<OrganizationEntity>
  {
    public string Name { get; set; }
    public string? Category { get; set; }
    public ICollection<FactoryEntity> Factories { get; set; }
    public ICollection<DepartmentEntity> Departments { get; set; }

    public override object SetData(OrganizationEntity entity)
    {
      base.SetData(entity);
      this.Name = entity.Name;
      this.Category = entity.Category;
      this.Factories = entity.Factories;
      this.Departments = entity.Departments;

      return this;
    }
    public override List<object> SetData(List<OrganizationEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (OrganizationEntity entity in listEntity)
      {
        listRS.Add(new OrganizationResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
