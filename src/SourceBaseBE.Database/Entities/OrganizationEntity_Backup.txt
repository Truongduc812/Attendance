using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Entities
{
	public class OrganizationEntity : BaseCRUDEntity, IEntityCategory
	{
		public string Name { get; set; }
		public string? Category { get; set; }
		public ICollection<FactoryEntity> Factories { get; set; }
		public ICollection<DepartmentEntity> Departments { get; set; }
	}
}
