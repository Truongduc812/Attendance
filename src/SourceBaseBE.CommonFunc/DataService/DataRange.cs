//using iSoft.Common;
//using iSoft.Common.Enums;
//using iSoft.Common.Utils;
//using iSoft.DBLibrary.Entities;
//
//using iSoft.Common.Models;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using SourceBaseBE.Common.MessageQueue;
//using SourceBaseBE.Database.Entities;
//using SourceBaseBE.Database.Entities.TraceData;
//using SourceBaseBE.Database.Repository.TraceDataNS;

//namespace SourceBaseBE.CommonFunc.DataService
//{
//  public class DataRange<TEntity, TESModel> where TEntity: BaseTraceDataEntity where TESModel : BaseDeviceDataModel
//  {
//    public Dictionary<string, EnvironmentEntity> dicEnvEntity = new Dictionary<string, EnvironmentEntity>();
//    Dictionary<string, object> DicEnvKey2Value = new Dictionary<string, object>();
//    Dictionary<string, DeviceConnectionEntity> dicDeviceConnectionEntity = new Dictionary<string, DeviceConnectionEntity>();
//    DevicePayloadMessage? message = null;
//    public List<TEntity> ListTraceDataEntity = new List<TEntity>();
//    public List<TESModel> ListESModel = new List<TESModel>();

//    public EnvExampleModel[] ArrExampleEnv;

//    public DataRange(EnvExampleModel[] arrExampleEnv, 
//                      DevicePayloadMessage message,
//                      Dictionary<string, DeviceConnectionEntity> dicDeviceConnectionEntity)
//    {
//      this.ArrExampleEnv = arrExampleEnv;
//      this.message = message;
//      this.dicDeviceConnectionEntity = dicDeviceConnectionEntity;

//      foreach (var envObj in this.ArrExampleEnv)
//      {
//        if (!this.dicEnvEntity.ContainsKey(envObj.EnviromentVarName))
//        {
//          this.dicEnvEntity.Add(envObj.EnviromentVarName, new EnvironmentEntity()
//          {
//            EnviromentVarName = envObj.EnviromentVarName,
//            TraceColumnDataType = envObj.Requester,
//            TraceColumnName = StringUtil.RemoveSpecialChar(envObj.EnviromentVarName),
//          });
//        }
//      }

//      foreach (var env in message.Data)
//      {
//        if (this.dicEnvEntity.ContainsKey(env.Key))
//        {
//          if (!DicEnvKey2Value.ContainsKey(env.Key))
//          {
//            DicEnvKey2Value.Add(env.Key, env.Value);
//          }
//        }
//      }

//    }

//    public bool SetListEntity(ref TEntity lastEntity, int minTimeIntervalInSeconds = 0)
//    {
//      bool existsFlag = false;
//      object?[] arrValue1 = new object?[dicEnvEntity.Count];
//      int i = 0;
//      foreach (var keyVal in dicEnvEntity)
//      {
//        if (DicEnvKey2Value.ContainsKey(keyVal.Key))
//        {
//          arrValue1[i] = DicEnvKey2Value[keyVal.Key];
//          existsFlag = true;
//        }
//        else
//        {
//          arrValue1[i] = null;
//        }
//        i++;
//      }

//      if (existsFlag)
//      {
//        TEntity entity1 = (TEntity)Activator.CreateInstance(typeof(TEntity), dicDeviceConnectionEntity[message.ConnectionId].Id,
//                                                            arrValue1,
//                                                            message.ExecuteAt,
//                                                            message.MessageId,
//                                                            this.dicEnvEntity);
//        if (lastEntity == null
//          || DateTimeUtil.CompareDateTime(lastEntity.ExecuteAt, entity1.ExecuteAt, 0) >= minTimeIntervalInSeconds
//          || !lastEntity.MatchValues(entity1))
//        {
//          ListTraceDataEntity.Add(entity1);
//          lastEntity = entity1;
//          return true;
//        }
//        else
//        {
//          return false;
//        }
//      }
//      else
//      {
//        return true;
//      }
//    }

//    public bool SetListESModel(ref TESModel lastESModel, int minTimeIntervalInSeconds = 0)
//    {
//      bool existsFlag = false;
//      object?[] arrValue1 = new object?[dicEnvEntity.Count];
//      int i = 0;
//      foreach (var keyVal in dicEnvEntity)
//      {
//        if (DicEnvKey2Value.ContainsKey(keyVal.Key))
//        {
//          arrValue1[i] = DicEnvKey2Value[keyVal.Key];
//          existsFlag = true;
//        }
//        else
//        {
//          arrValue1[i] = null;
//        }
//        i++;
//      }

//      if (existsFlag)
//      {
//        TESModel model1 = (TESModel)Activator.CreateInstance(typeof(TESModel), dicDeviceConnectionEntity[message.ConnectionId].Id,
//                                                  arrValue1,
//                                                  DateTimeUtil.GetLocalDateTime(message.ExecuteAt),
//                                                  message.MessageId);
//        if (lastESModel == null
//          || !lastESModel.MatchValues(model1)
//          || DateTimeUtil.CompareDateTime(lastESModel.ExecuteAt, model1.ExecuteAt, 0) >= minTimeIntervalInSeconds)
//        {
//          ListESModel.Add(model1);
//          lastESModel = model1;
//          return true;
//        }
//        else
//        {
//          return false;
//        }
//      }
//      else
//      {
//        return true;
//      }
//    }
//    public bool HasEnvKey(string key)
//    {
//      return this.dicEnvEntity.ContainsKey(key);
//    }

//    public List<string> getListEnvName()
//    {
//      return this.dicEnvEntity.Values.Select(x => x.EnviromentVarName).ToList();
//    }

//    public List<string> getListColumnName()
//    {
//      return this.dicEnvEntity.Values.Select(x => StringUtil.RemoveSpecialChar(x.EnviromentVarName)).ToList();
//    }

//    public List<EnumDataType> getListDataType()
//    {
//      return this.dicEnvEntity.Values.Select(x => x.TraceColumnDataType).ToList();
//    }
//  }
//}
