using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities
{
  public class StockKeepingUnitEntity : BaseCRUDEntity
  {
    [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? ManufactoryDate { get; set; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? ExpirationDate { get; set; }

    [ForeignKey(nameof(Entities.ProductEntity))]
    public long? ProductId { get; set; }
    public ProductEntity? Product { get; set; }

  }
}
