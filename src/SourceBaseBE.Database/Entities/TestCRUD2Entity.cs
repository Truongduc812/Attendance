using iSoft.Database.Entities;
using SixLabors.Fonts.Tables.AdvancedTypographic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Entities
{
  [Table("TestCRUD2s")]
  public class TestCRUD2Entity : BaseCRUDEntity
  {
    public string Name { get; set; }
    public List<TestCRUDEntity> TestCRUDs { get; set; } = new();
  }
}
