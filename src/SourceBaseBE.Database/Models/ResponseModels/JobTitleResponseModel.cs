using iSoft.Common.Utils;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.ResponseModels
{
  public class JobTitleResponseModel : BaseCRUDResponseModel<JobTitleEntity>
  {
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
    public List<EmployeeEntity>? Employees { get; set; }

    public override object SetData(JobTitleEntity entity)
    {
      base.SetData(entity);
      this.Name = entity.Name;
      this.Description = entity.Description;
      this.Notes = entity.Notes;
      this.Employees = entity.Employees;

      return this;
    }
    public override List<object> SetData(List<JobTitleEntity> listEntity)
    {
      List<Object> listRS = new List<object>();
      foreach (JobTitleEntity entity in listEntity)
      {
        listRS.Add(new JobTitleResponseModel().SetData(entity));
      }
      return listRS;
    }
  }
}
