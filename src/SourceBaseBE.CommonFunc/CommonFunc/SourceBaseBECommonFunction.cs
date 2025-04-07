using iSoft.Common.Utils;
using iSoft.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iSoft.ElasticSearch.Models;
using System.Runtime.Serialization.Formatters.Binary;
using SourceBaseBE.Database.Entities;
using iSoft.DBLibrary.Entities;
using iSoft.Database.Entities;
using iSoft.Redis.Services;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using iSoft.Common.ExtensionMethods;
using Microsoft.Extensions.Logging;
using iSoft.Common;
using StackExchange.Redis;
using SourceBaseBE.Database.Interfaces;
using iSoft.Common.Enums;
using iSoft.Common.Exceptions;

namespace SourceBaseBE.CommonFunc.CommonFuncNS
{
  public class SourceBaseBECommonFunction
  {
    public static List<ChartDataModel> FixSafetyPointData(List<ChartDataModel> rs, DateTime startTime, DateTime endTime)
    {
      if (rs.Count <= 0)
      {
        return rs;
      }
      var now = DateTime.Now;

      // now nằm ngoài khoảng start-end
      if (DateTimeUtil.CompareDateTime(startTime, now) > 0 && DateTimeUtil.CompareDateTime(now, endTime) > 0)
      {
        var modelStart = new ChartDataModel()
        {
          Id = null,
          ExecuteAt = DateTimeUtil.GetDateTimeStr(startTime),
          DicValue = new Dictionary<string, object>()
        };
        foreach (var keyVal in rs[0].DicValue)
        {
          modelStart.DicValue.Add(keyVal.Key, keyVal.Value);
        }

        // insert thêm giá vào vị trí đầu với value của giá trị startTime
        rs.Insert(0, modelStart);

        var modelEnd = new ChartDataModel()
        {
          Id = null,
          ExecuteAt = DateTimeUtil.GetDateTimeStr(now),
          DicValue = new Dictionary<string, object>()
        };
        foreach (var keyVal in rs[rs.Count - 1].DicValue)
        {
          modelEnd.DicValue.Add(keyVal.Key, keyVal.Value);
        }

        // add thêm giá vào vị trí cuối với value của giá trị now
        rs.Add(modelEnd);
      }

      // now nằm trong khoảng start-end
      else
      {
        var modelStart = new ChartDataModel()
        {
          Id = null,
          ExecuteAt = DateTimeUtil.GetDateTimeStr(startTime),
          DicValue = new Dictionary<string, object>()
        };
        foreach (var keyVal in rs[0].DicValue)
        {
          modelStart.DicValue.Add(keyVal.Key, keyVal.Value);
        }

        // insert thêm giá vào vị trí đầu với value của giá trị startTime
        rs.Insert(0, modelStart);

        var modelEnd = new ChartDataModel()
        {
          Id = null,
          ExecuteAt = DateTimeUtil.GetDateTimeStr(endTime),
          DicValue = new Dictionary<string, object>()
        };
        foreach (var keyVal in rs[rs.Count - 1].DicValue)
        {
          modelEnd.DicValue.Add(keyVal.Key, keyVal.Value);
        }

        // add thêm giá vào vị trí cuối với value của giá trị cuối trong range
        rs.Add(modelEnd);
      }

      //rs.Sort((x, y) =>
      //{
      //  return string.Compare(x.ExecuteAt, y.ExecuteAt);
      //});

      return rs;
    }

    public static string Calculator(DateTime? timeIn, DateTime? timeOut)
    {
      if (timeIn == null || timeOut == null)
      {
        return null;
      }
      return DateTimeUtil.GetTimeSpanStr(timeOut.Value - timeIn.Value);
    }
  }
}
