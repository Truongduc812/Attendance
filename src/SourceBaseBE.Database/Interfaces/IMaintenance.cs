using iSoft.Common.Utils;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Interfaces
{
  public interface IMaintenance
  {
    [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? ExpiryDate { set; get; }

    [DataType(System.ComponentModel.DataAnnotations.DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
    public DateTime? OperationStartDate { set; get; }

    [MaxLength(255)] // Độ dài tối đa cho Description là 255 ký tự
    [Column(TypeName = "NCHAR(255)")] // Xác định kiểu dữ liệu cơ sở dữ liệu là NVARCHAR(255)
    public string? Supplier { set; get; }

    [MaxLength(100)] // Độ dài tối đa cho Description là 255 ký tự
    [Column(TypeName = "NCHAR(100)")] // Xác định kiểu dữ liệu cơ sở dữ liệu là NVARCHAR(255)
    public string? Model { set; get; }

    /// <summary>
    /// Time In Seconds
    /// </summary>
    public long? MaxOperationTime { set; get; }

    /// <summary>
    /// Time In Seconds
    /// </summary>
    [NotMapped]
    public long? RunTime
    {
      get
      {
        if (this.OperationStartDate == null)
        {
          return 0;
        }
        return DateTimeUtil.CompareDateTime(this.OperationStartDate.Value, DateTime.Now);
      }
    }
  }
}
