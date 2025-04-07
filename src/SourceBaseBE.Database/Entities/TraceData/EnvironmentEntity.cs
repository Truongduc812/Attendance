using System.Text.Json.Serialization;
using System.Linq;
using System.IO;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.DBLibrary.Entities;
using SourceBaseBE.Database.DTOs;
using System.Data;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Entities.TraceData
{
  public class EnvironmentEntity : BaseIdTimeEnity
  {

    [StringLength(255)]
    public string? ConnectionId { get; set; }

    [StringLength(255)]
    public string? EnvKey { get; set; }

    [StringLength(50)]
    public string TraceColumnName { get; set; }

    public EnumDataType TraceColumnDataType { get; set; }

    public EnvironmentEntity()
    {
    }
    public EnvironmentEntity(string? connectionId, 
                              string? key, 
                              string traceColumnName, 
                              EnumDataType traceColumnDataType)
    {
      this.ConnectionId = connectionId;
      this.EnvKey = key;
      this.TraceColumnName = traceColumnName;
      this.TraceColumnDataType = traceColumnDataType;
    }

    public void FillData(IDataReader reader)
    {
      Id = reader.GetInt64(reader.GetOrdinal("Id"));
      ConnectionId = reader.IsDBNull(reader.GetOrdinal("ConnectionId")) ? null : reader.GetString(reader.GetOrdinal("ConnectionId"));
      EnvKey = reader.IsDBNull(reader.GetOrdinal("EnvKey")) ? null : reader.GetString(reader.GetOrdinal("EnvKey"));
      TraceColumnName = reader.GetString(reader.GetOrdinal("TraceColumnName"));
      TraceColumnDataType = (EnumDataType)reader.GetInt32(reader.GetOrdinal("TraceColumnDataType"));
      CreatedBy = reader.IsDBNull(reader.GetOrdinal("CreatedBy")) ? null : reader.GetInt64(reader.GetOrdinal("CreatedBy"));
      CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("CreatedAt"));
      UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? null : reader.GetInt64(reader.GetOrdinal("UpdatedBy"));
      UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt"));
    }
  }
}