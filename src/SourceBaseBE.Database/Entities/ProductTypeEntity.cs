using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
  public class ProductTypeEntity : BaseCRUDEntity, IEntityCategory
  {
    public ProductTypeEntity()
    {
      Products = new HashSet<ProductEntity>();
    }

    public string? Category { get; set; }

    public IEnumerable<ProductEntity> Products { get; set; }
  }
}
