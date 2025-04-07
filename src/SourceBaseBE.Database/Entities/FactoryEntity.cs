using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
  public class FactoryEntity : BaseCRUDEntity, IEntityCategory
  {
    [ForeignKey(nameof(Entities.OrganizationEntity))]
    public long OrganizationId { get; set; }

    public OrganizationEntity Organization { get; set; }

    public ICollection<DepartmentEntity> Departments { get; set; }

    public ICollection<WorkshopEntity> Workshops { get; set; }
    public string? Category { get; set; }

    public FactoryEntity()
    {
      Departments = new HashSet<DepartmentEntity>();
      Workshops = new HashSet<WorkshopEntity>();
    }
  }
}
