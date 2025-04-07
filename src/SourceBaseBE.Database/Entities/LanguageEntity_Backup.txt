//using iSoft.Common.Models.RemoteConfigModels;
using iSoft.Database.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Entities
{
  [Table("Languages")]
  public class LanguageEntity : BaseCRUDEntity
  {
    [Required]
    [Column("Key")]
    public string? Key { get; set; }

    [Required]
    [Column("DisplayName")]
    public string? DisplayName { get; set; }

    [Required]
    [Column("Category")]
    public string? Category { get; set; }

    [Required]
    [Column("Language")]
    public string? Language { get; set; }
  }
}
