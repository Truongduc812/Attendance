using iSoft.Common.Enums;
using iSoft.Common.Utils;
using iSoft.Common.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSoft.ElasticSearch.Models;
using System.Data;
using SourceBaseBE.Database.Enums;
using iSoft.Common.Enums.DBProvider;

namespace SourceBaseBE.CommonFunc.EnvConfigData
{
  public class EnvConfigModel : IESFieldName
  {
    public string? TableSerial { get; set; }
    public long ConnectionId { get; set; }
    public string? EnviromentVarName { get; set; }
    public EnumDataType Type { get; set; }
    [Column("DataType")]
    public string? DataType { get; set; }
    [Column("MinTimeSavingDataIntervals")]
    public int MinTimeIntervalInSeconds { get; set; } = 28;
    public EnvConfigModel()
    {
    }
    public EnvConfigModel(string tableName, long connectionId, string envKey, EnumDataType dataType, int minTimeIntervalInSeconds = 0)
    {
      TableSerial = tableName;
      ConnectionId = connectionId;
      EnviromentVarName = envKey;
      Type = dataType;
      MinTimeIntervalInSeconds = minTimeIntervalInSeconds;
    }
    public string GetEnvKeyDB()
    {
      return $"{this.EnviromentVarName}".RemoveSpecialChar();
    }
    public string GetSQLDataType(EnumDBProvider databaseType)
    {
      switch (databaseType)
      {
        case EnumDBProvider.SqlServer:
          switch (Type)
          {
            case EnumDataType.Bool:
              return "BIT";
            case EnumDataType.Double:
              return "FLOAT";
            case EnumDataType.Long:
              return "BIGINT";
            case EnumDataType.Int:
              return "INT";
            case EnumDataType.Short:
              return "TINYINT";
            case EnumDataType.String:
              return "NVARCHAR(50)";
            //return "NVARCHAR(MAX)";
            case EnumDataType.String50:
              return "NVARCHAR(50)";
            case EnumDataType.String255:
              return "NVARCHAR(255)";
            case EnumDataType.Byte:
              return "BYTE";
            case EnumDataType.DateTime:
              return "DATETIME2(7)";
          }
          break;
        case EnumDBProvider.Postgres:
          switch (Type)
          {
            case EnumDataType.Bool:
              return "BOOLEAN";
            case EnumDataType.Double:
              return "DOUBLE PRECISION";
            case EnumDataType.Long:
              return "BIGINT";
            case EnumDataType.Int:
              return "INTEGER";
            case EnumDataType.Short:
            case EnumDataType.Byte:
              return "SMALLINT";
            case EnumDataType.String:
              return "VARCHAR(50)";
            //return "TEXT";
            case EnumDataType.String50:
              return "VARCHAR(50)";
            case EnumDataType.String255:
              return "VARCHAR(255)";
            case EnumDataType.DateTime:
              return "TIMESTAMP";
          }
          break;
      }
      return "";
    }

    public string GetESIndexName(string subfix = "")
    {
      return this.TableSerial.ToLower() + subfix;
    }
    public string GetESPatternSearch(string subfix = "")
    {
      return this.TableSerial.ToLower() + subfix + "*";
    }

    //public string GetESFieldName()
    //{
    //  return this.GetEnvKeyDB().ToLower();
    //}

    public string GetESFieldName2()
    {
      return this.GetEnvKeyDB().ToLower();
    }

    public string GetKey()
    {
      return $"{this.ConnectionId}_{this.EnviromentVarName}".RemoveSpecialChar().ToLower();
    }

    public EnumDataType GetESDataType()
    {
      return this.Type;
    }

    public object GetValueFromObject(object obj)
    {
      switch (Type)
      {
        case EnumDataType.Bool:
          return ConvertUtil.ConvertToNullableBool(obj);
        case EnumDataType.Double:
          return ConvertUtil.ConvertToNullableDouble(obj);
        case EnumDataType.Long:
          return ConvertUtil.ConvertToNullableLong(obj);
        case EnumDataType.Int:
          return ConvertUtil.ConvertToNullableInt(obj);
        case EnumDataType.Short:
          return ConvertUtil.ConvertToNullableShort(obj);
        case EnumDataType.String:
        case EnumDataType.String50:
        case EnumDataType.String255:
          return obj.ToString();
        case EnumDataType.Byte:
          return ConvertUtil.ConvertToNullableByte(obj);
        case EnumDataType.DateTime:
          return ConvertUtil.ConvertToNullableDateTime(obj);
      }
      return "";
    }

    public object? GetValueFromReader(IDataReader reader)
    {
      switch (Type)
      {
        case EnumDataType.Bool:
          return reader.IsDBNull(reader.GetOrdinal(this.GetEnvKeyDB())) ? null : reader.GetBoolean(reader.GetOrdinal(this.GetEnvKeyDB()));
        case EnumDataType.Double:
          return reader.IsDBNull(reader.GetOrdinal(this.GetEnvKeyDB())) ? null : reader.GetDouble(reader.GetOrdinal(this.GetEnvKeyDB()));
        case EnumDataType.Long:
          return reader.IsDBNull(reader.GetOrdinal(this.GetEnvKeyDB())) ? null : reader.GetInt64(reader.GetOrdinal(this.GetEnvKeyDB()));
        case EnumDataType.Int:
          return reader.IsDBNull(reader.GetOrdinal(this.GetEnvKeyDB())) ? null : reader.GetInt32(reader.GetOrdinal(this.GetEnvKeyDB()));
        case EnumDataType.Short:
          return reader.IsDBNull(reader.GetOrdinal(this.GetEnvKeyDB())) ? null : reader.GetByte(reader.GetOrdinal(this.GetEnvKeyDB()));
        case EnumDataType.String:
        case EnumDataType.String50:
        case EnumDataType.String255:
          return reader.IsDBNull(reader.GetOrdinal(this.GetEnvKeyDB())) ? null : reader.GetString(reader.GetOrdinal(this.GetEnvKeyDB()));

        case EnumDataType.Byte:
          return reader.IsDBNull(reader.GetOrdinal(this.GetEnvKeyDB())) ? null : reader.GetByte(reader.GetOrdinal(this.GetEnvKeyDB()));
        case EnumDataType.DateTime:
          return reader.IsDBNull(reader.GetOrdinal(this.GetEnvKeyDB())) ? null : reader.GetDateTime(reader.GetOrdinal(this.GetEnvKeyDB()));
      }
      return "";
    }

    public override string ToString()
    {
      return $"{this.TableSerial} | {this.EnviromentVarName} | {this.DataType}";
    }

    /// <summary>
    /// this.DataType: Input from excel: public string DataType { get; set; }
    /// this.Requester: Ouput for used: public EnumDataType Requester { get; set; }

    /// </summary>
    public bool IsValid()
    {
      if (this.TableSerial == null || this.TableSerial.Length == 0
        || this.EnviromentVarName == null || this.EnviromentVarName.Trim().Length <= 1)

      {
        return false;
      }
      this.Type = getDataTypeEnum(this.DataType);
      if (this.Type == EnumDataType.None)
      {
        return false;
      }
      return true;
    }

    private EnumDataType getDataTypeEnum(string dataType)
    {
      if (string.IsNullOrEmpty(dataType)) return EnumDataType.None;
      switch (dataType.Trim().ToUpper())
      {
        case "BOOL":
          return EnumDataType.Bool;
        case "BYTE":
          return EnumDataType.Short;
        case "INT":
        case "UINT":
          return EnumDataType.Int;
        case "DINT":
        case "UDINT":
          return EnumDataType.Long;
        case "REAL":
          return EnumDataType.Double;
        case "STRING":
          return EnumDataType.String;
        case "DATETIME":
          return EnumDataType.DateTime;
        default:
          return EnumDataType.None;
      }
    }
  }
}
