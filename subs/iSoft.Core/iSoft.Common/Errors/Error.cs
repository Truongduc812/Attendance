using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Errors
{
  public class Error
  {
    public UInt128 Id { get; set; }
    [Column("Code", TypeName = "varchar(20)")]
    [NotNull]
    public string Code { get; set; }
    [Column("Description", TypeName = "varchar(100)")]

    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
  }
}
