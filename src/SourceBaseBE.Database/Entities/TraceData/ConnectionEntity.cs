using System.Text.Json.Serialization;
using System.Linq;
using System.IO;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.DBLibrary.Entities;
using SourceBaseBE.Database.DTOs;
using System.Data;

namespace SourceBaseBE.Database.Entities.TraceData
{
  public class DeviceConnectionEntity : BaseIdTimeEnity
  {
    [StringLength(255)]
    public long ConnectionKey { get; set; }

    public DeviceConnectionEntity()
    {
    }
    public DeviceConnectionEntity(long connectionKey)
    {
      ConnectionKey = connectionKey;
    }

    public void FillData(IDataReader reader)
    {
      Id = reader.GetInt64(reader.GetOrdinal("Id"));
      ConnectionKey = reader.GetInt64(reader.GetOrdinal("ConnectionKey"));
      CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetInt64(reader.GetOrdinal("CreatedBy"));
      CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
      UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? null : reader.GetInt64(reader.GetOrdinal("UpdatedBy"));
      UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));
    }
  }
}