
////using iSoft.Common;
//using iSoft.Common.Exceptions;
//using iSoft.DBLibrary.Entities;
//using Microsoft.Extensions.DependencyInjection;
//using Serilog;
//using NPOI.SS.Formula.Functions;
//using System;
//using System.Collections.Generic;
//using SourceBaseBE.MainService.Models;
//using SourceBaseBE.Database.Entities;
//using SourceBaseBE.MainService.Enums;
//using SourceBaseBE.MainService.Services;
//using static iSoft.Common.Messages;

//namespace SourceBaseBE.MainService.InitFunctionsNS
//{

//  public class InitFunctions
//  {
//    private static ILogger<InitFunctions> _logger;
//    private static IServiceProvider _serviceProvider = null;
//    //public static Dictionary<string, List<FormSelectOptionModel>> dicTable2SelectData = new Dictionary<string, List<FormSelectOptionModel>>();
//    //public static void RepareSelectData(IServiceProvider serviceProvider, ILoggerFactory loggerFactory)
//    //{
//    //  InitFunctions.
//    //  string funcName = nameof(RepareSelectData);
//    //  Message errMessage = null;

//    //  try
//    //  {
//    //    InitFunctions._logger.LogMsg(Messages.IFuncStart_0, funcName);

//    //    //// TODO: reload after..., move to cronjob
//    //    //RepareSelectData(_serviceProvider);

//    //    if (serviceProvider == null)
//    //    {
//    //      return;
//    //    }
//    //    _serviceProvider = serviceProvider;
//    //    using (var scope = serviceProvider.CreateScope())
//    //    {
//    //      // User
//    //      string tableName = nameof(UserEntity);
//    //      var userService = scope.ServiceProvider.GetRequiredService<UserService>();
//    //      var data = userService.GetListOptionData();
//    //      if (!dicTable2SelectData.ContainsKey(tableName))
//    //      {
//    //        dicTable2SelectData.Add(tableName, data);
//    //      }
//    //      else
//    //      {
//    //        dicTable2SelectData[tableName] = data;
//    //      }

//    //      // File
//    //      string tableFile = nameof(FileEntity);
//    //      var fileService = scope.ServiceProvider.GetRequiredService<FileService>();
//    //      var fileData = fileService.GetListOptionData();
//    //      if (!dicTable2SelectData.ContainsKey(tableFile))
//    //      {
//    //        dicTable2SelectData.Add(tableFile, fileData);
//    //      }
//    //      else
//    //      {
//    //        dicTable2SelectData[tableFile] = fileData;
//    //      }

//    //      // ComponentMaster
//    //      tableName = nameof(ComponentMasterEntity);
//    //      data = scope.ServiceProvider.GetRequiredService<ComponentMasterService>().GetListOptionData();
//    //      if (!dicTable2SelectData.ContainsKey(tableName))
//    //      {
//    //        dicTable2SelectData.Add(tableName, data);
//    //      }
//    //      else
//    //      {
//    //        dicTable2SelectData[tableName] = data;
//    //      }

//    //      // ComponentW
//    //      tableName = nameof(ComponentWEntity);
//    //      data = scope.ServiceProvider.GetRequiredService<ComponentWService>().GetListOptionData();
//    //      if (!dicTable2SelectData.ContainsKey(tableName))
//    //      {
//    //        dicTable2SelectData.Add(tableName, data);
//    //      }
//    //      else
//    //      {
//    //        dicTable2SelectData[tableName] = data;
//    //      }

//    //      // PageMaster
//    //      tableName = nameof(PageMasterEntity);
//    //      data = scope.ServiceProvider.GetRequiredService<PageMasterService>().GetListOptionData();
//    //      if (!dicTable2SelectData.ContainsKey(tableName))
//    //      {
//    //        dicTable2SelectData.Add(tableName, data);
//    //      }
//    //      else
//    //      {
//    //        dicTable2SelectData[tableName] = data;
//    //      }

//    //      // PageW
//    //      tableName = nameof(PageWEntity);
//    //      data = scope.ServiceProvider.GetRequiredService<PageWService>().GetListOptionData();
//    //      if (!dicTable2SelectData.ContainsKey(tableName))
//    //      {
//    //        dicTable2SelectData.Add(tableName, data);
//    //      }
//    //      else
//    //      {
//    //        dicTable2SelectData[tableName] = data;
//    //      }

//    //      // ComponentParam
//    //      tableName = nameof(ComponentParamEntity);
//    //      data = scope.ServiceProvider.GetRequiredService<ComponentParamService>().GetListOptionData();
//    //      if (!dicTable2SelectData.ContainsKey(tableName))
//    //      {
//    //        dicTable2SelectData.Add(tableName, data);
//    //      }
//    //      else
//    //      {
//    //        dicTable2SelectData[tableName] = data;
//    //      }

//    //      // ComponentParamMaster
//    //      tableName = nameof(ComponentParamMasterEntity);
//    //      data = scope.ServiceProvider.GetRequiredService<ComponentParamMasterService>().GetListOptionData();
//    //      if (!dicTable2SelectData.ContainsKey(tableName))
//    //      {
//    //        dicTable2SelectData.Add(tableName, data);
//    //      }
//    //      else
//    //      {
//    //        dicTable2SelectData[tableName] = data;
//    //      }
//    //    }

//    //    InitFunctions._logger.LogMsg(Messages.ISuccess_0_1, funcName, dicTable2SelectData.ToJson());
//    //  }
//    //  catch (DBException ex)
//    //  {
//    //    errMessage = Messages.ErrDBException.SetParameters(ex);
//    //  }
//    //  catch (BaseException ex)
//    //  {
//    //    errMessage = Messages.ErrBaseException.SetParameters(ex);
//    //  }
//    //  catch (Exception ex)
//    //  {
//    //    errMessage = Messages.ErrException.SetParameters(ex);
//    //  }
//    //  InitFunctions._logger.LogMsg(errMessage);
//    //}
//    //public static List<FormSelectOptionModel> GetListOptionData(string tableName)
//    //{
//    //  if (dicTable2SelectData.ContainsKey(tableName))
//    //  {
//    //    return dicTable2SelectData[tableName];
//    //  }
//    //  throw new NotImplementedException(tableName);
//    //}
//  }

//}
