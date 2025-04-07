using iSoft.Database.Entities;
using iSoft.DBLibrary.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Entities
{
  [Table("GenTemplates")]
  public class GenTemplateEntity : BaseCRUDEntity
  {
    public string Name { get; set; }
    public List<Example001Entity>? ListExample001 { get; set; } = new();
  }
}
