using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSoft.Database.Entities;
using SourceBaseBE.Database.Interfaces;

namespace SourceBaseBE.Database.Entities
{
  public class ProductEntity : BaseCRUDEntity, IEntityCategory
  {
    public ProductEntity()
    {
      StockKeepingUnits = new HashSet<StockKeepingUnitEntity>();
    }

    public string? Category { get; set; }
    
    [ForeignKey(nameof(Entities.ProductTypeEntity))]
    public long? ProductTypeId { get; set; }

    public ProductTypeEntity? ProductType { get; set; }

    public ICollection<StockKeepingUnitEntity> StockKeepingUnits { get; set; }
  }
}
