using System.Text.Json.Serialization;
using System.Linq;
using System.IO;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using iSoft.DBLibrary.Entities;
using SourceBaseBE.Database.DTOs;
using System.Data;
using iSoft.ConnectionCommon.Model;
using System.Reflection;
using iSoft.Common.Utils;
using iSoft.Common.Exceptions;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Entities.TraceData
{
  public class BaseTraceDataEntity: BaseEntity
  {
    [NotMapped]
    public override bool? isDelete { get => base.isDelete; set => base.isDelete = value; }
    [NotMapped]
    public override bool? isEnable { get => base.isEnable; set => base.isEnable = value; }
    [Key]
    [Column(Order = 0)]
    public long Id { get; set; }
    [Column(Order = 1)]
    public string? MessageId { get; set; }
    [Column(Order = 2)]
    public long? ConnectionId { get; set; }

    [Column(Order = 3)]
    public Int64? ExecuteAt { get; set; }

    [Column(Order = 4)]
    public Int64? CreatedAt { get; set; }
    [NotMapped]
    public Dictionary<string, object?> DicDynamicKey2Value { get; set; } = new Dictionary<string, object?>();

    public BaseTraceDataEntity()
    {

    }
    public BaseTraceDataEntity(long? connectionId,
                            object?[] arrValue,
                            DateTime executeAt,
                            string messageId,
                            Dictionary<string, EnvironmentEntity> dicEnvEntity)
    {
      int i = 0;
      this.MessageId = messageId;
      this.ConnectionId = connectionId;
      object?[] newArrValue = new object?[dicEnvEntity.Count];
      for (i = 0; i < arrValue.Length && i < dicEnvEntity.Count; i++)
      {
        newArrValue[i] = arrValue[i];
      }

      string traceColumnKey = "";
      i = 0;
      foreach (KeyValuePair<string, EnvironmentEntity> keyVal in dicEnvEntity)
      {
        traceColumnKey = keyVal.Value.TraceColumnName;
        if (!this.DicDynamicKey2Value.ContainsKey(traceColumnKey))
        {
          this.DicDynamicKey2Value.Add(traceColumnKey, newArrValue[i]);
        }
        else
        {
          this.DicDynamicKey2Value[traceColumnKey] = newArrValue[i];
        }
        i++;
      }

      ExecuteAt = executeAt.Ticks;
    }

    public void FillData(IDataReader reader, Dictionary<string, EnumDataType> dicEnv2DataType)
    {
      Id = reader.GetInt64(reader.GetOrdinal("Id"));
      MessageId = reader.IsDBNull(reader.GetOrdinal("MessageId")) ? null : reader.GetString(reader.GetOrdinal("MessageId"));
      ConnectionId = reader.IsDBNull(reader.GetOrdinal("ConnectionId")) ? null : reader.GetInt64(reader.GetOrdinal("ConnectionId"));
      ExecuteAt = reader.IsDBNull(reader.GetOrdinal("ExecuteAt")) ? null : reader.GetInt64(reader.GetOrdinal("ExecuteAt"));
      CreatedAt = reader.IsDBNull(reader.GetOrdinal("CreatedAt")) ? null : reader.GetInt64(reader.GetOrdinal("CreatedAt"));

      foreach (KeyValuePair<string, EnumDataType> keyVal in dicEnv2DataType)
      {
        switch (keyVal.Value)
        {
          case EnumDataType.Bool:
            this.DicDynamicKey2Value[keyVal.Key] = reader.IsDBNull(reader.GetOrdinal(keyVal.Key)) ?
              null : reader.GetBoolean(reader.GetOrdinal(keyVal.Key));
            break;
          case EnumDataType.Int:
            this.DicDynamicKey2Value[keyVal.Key] = reader.IsDBNull(reader.GetOrdinal(keyVal.Key)) ?
              null : reader.GetInt32(reader.GetOrdinal(keyVal.Key));
            break;
          case EnumDataType.Double:
            this.DicDynamicKey2Value[keyVal.Key] = reader.IsDBNull(reader.GetOrdinal(keyVal.Key)) ?
              null : reader.GetDouble(reader.GetOrdinal(keyVal.Key));
            break;
          case EnumDataType.String:
            this.DicDynamicKey2Value[keyVal.Key] = reader.IsDBNull(reader.GetOrdinal(keyVal.Key)) ?
              null : reader.GetString(reader.GetOrdinal(keyVal.Key));
            break;
          default:
            throw new BaseException($"DataType not found, data: {keyVal.Value}");
        }
      }
    }

    public object? GetValueByColumnName(string traceColumnName)
    {
      if (!this.DicDynamicKey2Value.ContainsKey(traceColumnName))
      {
        return null;
      }
      return this.DicDynamicKey2Value[traceColumnName];
    }
    public bool MatchValues(BaseTraceDataEntity? entity2)
    {
      if (entity2 == null)
      {
        return false;
      }
      foreach (KeyValuePair<string, object?> keyVal in this.DicDynamicKey2Value)
      {
        if (entity2.DicDynamicKey2Value.ContainsKey(keyVal.Key))
        {
          if (keyVal.Value == null || entity2.DicDynamicKey2Value[keyVal.Key] == null)
          {
            return false;
          }

          if (!keyVal.Value.Equals(entity2.DicDynamicKey2Value[keyVal.Key]))
          {
            return false;
          }
        }
        else
        {
          return false;
        }
      }
      return true;
    }
  }
}