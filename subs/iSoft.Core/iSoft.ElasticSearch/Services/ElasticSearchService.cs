using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Common.Utils;
using Microsoft.Extensions.Logging;
using Nest;
using static iSoft.Common.ConstCommon;
using iSoft.ElasticSearch.Models;
using iSoft.Common.Models.ConfigModel.Subs;
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace iSoft.ElasticSearch.Services
{
  public class ElasticSearchService
  {
    int ConstMaxResultWindow = 299999;
    private ILogger<ElasticSearchService> logger;
    private ElasticClient elasticClient = null;
    public ElasticSearchService(ILoggerFactory loggerFactory)
    {
      logger = loggerFactory.CreateLogger<ElasticSearchService>();
    }
    public static string GetConnectionString(ServerConfigModel config)
    {
      string protocol = "";
      string domain = "";
      Regex regex = new Regex(@"^(https?://)");
      Match match = regex.Match(config.Address);
      if (match.Success)
      {
        protocol = match.Groups[1].Value;
        domain = regex.Replace(config.Address, "");
      }
      return $"{protocol}{config.Username}:{config.Password}@{domain}:{config.Port}";
    }

    public void ConnectElasticSearch(ServerConfigModel elasticSearchConfig)
    {
      var settings = new ConnectionSettings(new Uri(GetConnectionString(elasticSearchConfig)))
                //.BasicAuthentication(elasticSearchConfig.Username, elasticSearchConfig.Password)
                .DeadTimeout(TimeSpan.FromSeconds(300))
                .EnableApiVersioningHeader()
                //.DefaultMappingFor<DeviceDataSendVarCountModel>(m => m.PropertyName(p => DateTimeUtil.GetLocalDateTime(p.Timestamp), "@timestamp"))
                //.DefaultMappingFor<DeviceDataSendVarDateTimeModel>(m => m.PropertyName(p => DateTimeUtil.GetLocalDateTime(p.Timestamp), "@timestamp"))
                //.DefaultMappingFor<DeviceDataSendVarSafetyPointModel>(m => m.PropertyName(p => DateTimeUtil.GetLocalDateTime(p.Timestamp), "@timestamp"))
                //.DefaultMappingFor<DeviceDataSendVarStatusModel>(m => m.PropertyName(p => DateTimeUtil.GetLocalDateTime(p.Timestamp), "@timestamp"))
                //.DefaultMappingFor<DeviceDataThirdPartyModel>(m => m.PropertyName(p => DateTimeUtil.GetLocalDateTime(p.Timestamp), "@timestamp"))
                //.DefaultMappingFor<DeviceDataProcessModel>(m => m.PropertyName(p => DateTimeUtil.GetLocalDateTime(p.Timestamp), "@timestamp"))
                ;
      elasticClient = new ElasticClient(settings);
    }

    public async Task<string> CreateDocument<T>(T data, string indexId) where T : class
    {
      string funcName = "CreateDocument";
      IndexResponse indexResponse = await elasticClient.IndexAsync<T>(data, c => c.Index(indexId));
      if (!indexResponse.IsValid)
      {
        return indexResponse.OriginalException.Message;
      }
      //this.logger.LogInformation($"{funcName} Create success, {indexId}, {documentId}");
      return "";
    }

    public async Task<string> CreateDocument2<T>(T data, string indexId, string documentId) where T : class
    {
      string funcName = "CreateDocument";
      CreateResponse indexResponse = await elasticClient.CreateAsync<T>(data, c => c.Index(indexId).Id(documentId));
      if (!indexResponse.IsValid)
      {
        return indexResponse.OriginalException.Message;
      }
      //this.logger.LogInformation($"{funcName} Create success, {indexId}, {documentId}");
      return "";
    }

    public async Task<(List<ChartDataModel>, long)> Search(string searchPattern,
                                                    List<string> searchField,
                                                    EnumShiftId? shiftId,
                                                    DateTime startTime0,
                                                    DateTime endTime0,
                                                    int page,
                                                    int pageSize)
    {
      string funcName = "Search";
      var resultList = new List<ChartDataModel>();
      int skip = (page - 1) * pageSize;
      DateTime startTime = DateTimeUtil.GetUTCDateTime(startTime0);
      DateTime endTime = DateTimeUtil.GetUTCDateTime(endTime0);
      var searchResponse = await elasticClient.SearchAsync<Dictionary<string, object>>(s => s
          .Index(searchPattern)
          .From(skip)
          .Size(pageSize)
          .Source(source => source.Includes(fi =>
          {
            var a = fi.Field("executeat");
            foreach (var searchKey in searchField)
            {
              a.Field(searchKey);
            }
            return a;
          }))
          .Query(q => q
              .Bool(b => b
                  .Must(
                    must => must.Match(m =>
                    {
                      switch (shiftId)
                      {
                        case EnumShiftId.Shift1:
                        case EnumShiftId.Shift2:
                        case EnumShiftId.Shift3:
                          return m.Field("shiftid")
                            .Query(((int)shiftId).ToString());
                      }
                      return m;
                    }),
                    must => must.DateRange(dr => dr
                        .Field("executeat")
                        .GreaterThanOrEquals(startTime)
                        .LessThanOrEquals(endTime)
                    )
                  )
              )
          )
          .Sort(sort => sort
            .Field("executeat", SortOrder.Descending)
          )
          .TrackTotalHits(true)
        );

      if (!searchResponse.IsValid)
      {
        this.logger.LogMsg(Messages.ErrException.SetParameters(funcName, searchResponse.OriginalException));
        return (resultList, 0);
      }

      var total = searchResponse.Total;

      //foreach (var hit in searchResponse.Hits.Reverse())
      foreach (var hit in searchResponse.Hits)
      {
        string executeAtStr = hit.Source["executeat"].ToString();
        var model = new ChartDataModel
        {
          //Id = hit.Id,
          //ExecuteAt = executeAtStr.Substring(0, 10) + "T" + executeAtStr.Substring(11, 12)
          ExecuteAt = executeAtStr.Substring(0, 23)
        };
        foreach (var searchKey in searchField)
        {
          if (!hit.Source.ContainsKey(searchKey.ToLower()))
          {
            continue;
          }
          var fieldVal = hit.Source[searchKey.ToLower()];
          if (fieldVal != null)
          {
            model.DicValue.Add(searchKey.ToLower(), fieldVal.ToString().ToDoubleValue(0));
          }
          else
          {
            this.logger.LogMsg(Messages.ErrInputInvalid_0_1.SetParameters(funcName, $"searchField not found, searchField: {searchField}"));
          }
        }
        resultList.Add(model);
      }

      return (resultList, total);
    }
    public async Task<List<ChartDataModel>> SearchGroup(string searchPattern,
                                                        List<string> listSearchField0,
                                                        EnumShiftId? shiftId,
                                                        DateTime startTime0,
                                                        DateTime endTime0,
                                                        string timeInterval,
                                                        EnumSearchGroupType searchGroupType)
    {
      string funcName = "SearchGroup";
      DateTime startTime = DateTimeUtil.GetUTCDateTime(startTime0);
      DateTime endTime = DateTimeUtil.GetUTCDateTime(endTime0);
      var resultList = new List<ChartDataModel>();
      List<string> listSearchField = listSearchField0.Distinct().ToList();
      var searchResponse = await elasticClient.SearchAsync<Dictionary<string, object>>(s => s
          .Index(searchPattern)
          .Size(0)
          .Query(q => q
              .Bool(b => b
                  .Must(
                    must => must.Match(m =>
                    {
                      switch (shiftId)
                      {
                        case EnumShiftId.Shift1:
                        case EnumShiftId.Shift2:
                        case EnumShiftId.Shift3:
                          return m.Field("shiftid")
                            .Query(((int)shiftId).ToString());
                      }
                      return m;
                    }),
                    must => must.DateRange(dr => dr
                        .Field("executeat")
                        .GreaterThanOrEquals(startTime)
                        .LessThanOrEquals(endTime)
                    )
                  )
              )
          )
          .Aggregations(a => a
            .DateHistogram("values_per_time", dh => dh
              .Field("executeat")
              .FixedInterval(timeInterval)
              .TimeZone("Asia/Ho_Chi_Minh")
              .Format(ConstDateTimeFormat.YYYYMMDDTHHMMSS_SSS)
              .Aggregations(agg =>
              {
                foreach (var searchField in listSearchField)
                {
                  switch (searchGroupType)
                  {
                    case EnumSearchGroupType.Average:
                      agg.Average("gvalue_" + searchField, doc => doc.Field(searchField));
                      break;
                    case EnumSearchGroupType.Sum:
                      agg.Sum("gvalue_" + searchField, doc => doc.Field(searchField));
                      break;
                    case EnumSearchGroupType.Max:
                      agg.Max("gvalue_" + searchField, doc => doc.Field(searchField));
                      break;
                    case EnumSearchGroupType.Min:
                      agg.Min("gvalue_" + searchField, doc => doc.Field(searchField));
                      break;
                  }
                }
                return agg;
              })
            )
          )
          //.TrackTotalHits(true)
        );

      if (!searchResponse.IsValid)
      {
        this.logger.LogMsg(Messages.ErrException.SetParameters(funcName, searchResponse.OriginalException));
        return resultList;
      }

      var total = searchResponse.Total;

      var histogramBuckets = searchResponse.Aggregations.DateHistogram("values_per_time").Buckets;
      ChartDataModel chartDataModel = new ChartDataModel();
      foreach (var bucket in histogramBuckets)
      {
        chartDataModel = new ChartDataModel();
        chartDataModel.ExecuteAt = bucket.KeyAsString;

        switch (searchGroupType)
        {
          case EnumSearchGroupType.Count:
            chartDataModel.DicValue.Add("DocCount", bucket.DocCount);
            break;
          case EnumSearchGroupType.Average:
          case EnumSearchGroupType.Sum:
          case EnumSearchGroupType.Max:
          case EnumSearchGroupType.Min:
            foreach (var searchField in listSearchField)
            {
              chartDataModel.DicValue.Add(searchField, bucket.Average("gvalue_" + searchField).Value);
            }
            break;
        }
        resultList.Add(chartDataModel);
      }

      return resultList;
    }

    //public async Task<List<ChartDataModel>> SearchSafetyPoint(string searchPattern,
    //                                                    List<string> listSearchField0,
    //                                                    EnumShiftId? shiftId,
    //                                                    DateTime startTime0,
    //                                                    DateTime endTime0,
    //                                                    int page,
    //                                                    int pageSize)
    //{
    //  string funcName = "SearchSafetyPoint";
    //  var resultList = new List<ChartDataModel>();
    //  int skip = (page - 1) * pageSize;
    //  DateTime startTime = DateTimeUtil.GetUTCDateTime(startTime0);
    //  DateTime endTime = DateTimeUtil.GetUTCDateTime(endTime0);
    //  List<string> listSearchField = listSearchField0.Distinct().ToList();
    //  var searchResponse = await elasticClient.SearchAsync<Dictionary<string, object>>(s => s
    //      .Index(searchPattern)
    //      .From(skip)
    //      .Size(pageSize)
    //      .Query(q => q
    //          .Bool(b => b
    //              .Must(
    //                must => must.Match(m => {
    //                  switch (shiftId)
    //                  {
    //                    case EnumShiftId.Shift1:
    //                    case EnumShiftId.Shift2:
    //                    case EnumShiftId.Shift3:
    //                      return m.Field("shiftid")
    //                        .Query(((int)shiftId).ToString());
    //                  }
    //                  return m;
    //                }),
    //                must => must.DateRange(dr => dr
    //                    .Field("executeat")
    //                    .GreaterThanOrEquals(startTime)
    //                    .LessThanOrEquals(endTime)
    //                )
    //              )
    //          )
    //      )
    //      .Sort(sort => sort
    //        .Field("executeat", SortOrder.Descending)
    //      )
    //    );

    //  if (!searchResponse.IsValid)
    //  {
    //    this.logger.LogMsg(Messages.ErrException.SetParameters(funcName, searchResponse.OriginalException));
    //    return resultList;
    //  }

    //  var total = searchResponse.Total;

    //  foreach (var hit in searchResponse.Hits.Reverse())
    //  {
    //    var createdAt = DateTimeUtil.GetDateTimeFromString(hit.Source["executeat"].ToString(), ConstDateTimeFormat.YYYYMMDDTHHMMSSFFF_07_00);
    //    string executeAtStr = hit.Source["executeat"].ToString();
    //    var model = new ChartDataModel
    //    {
    //      //Id = hit.Id,
    //      //ExecuteAt = executeAtStr.Substring(0, 10) + " " + executeAtStr.Substring(11, 12)
    //      ExecuteAt = executeAtStr.Substring(0, 23)
    //    };
    //    foreach (var searchField in listSearchField)
    //    {
    //      if (!hit.Source.ContainsKey(searchField.ToLower()))
    //      {
    //        continue;
    //      }
    //      var fieldVal = hit.Source[searchField.ToLower()];
    //      if (fieldVal != null)
    //      {
    //        model.DicValue.Add(searchField.ToLower(), fieldVal.ToString().ToDoubleValue(0));
    //      }
    //      else
    //      {
    //        this.logger.LogMsg(Messages.ErrInputInvalid_0_1.SetParameters(funcName, $"searchField not found, searchField: {searchField}"));
    //      }
    //    }
    //    resultList.Add(model);
    //  }

    //  return resultList;
    //}
    //public async Task<List<ChartDataModel>> SearchAll(string searchPattern,
    //                                                string searchField,
    //                                                EnumShiftId? shiftId,
    //                                                DateTime startTime0,
    //                                                DateTime endTime0
    //                                              )
    //{
    //  string funcName = "Search";
    //  var resultList = new List<ChartDataModel>();
    //  DateTime startTime = DateTimeUtil.GetUTCDateTime(startTime0);
    //  DateTime endTime = DateTimeUtil.GetUTCDateTime(endTime0);
    //  var searchResponse = await elasticClient.SearchAsync<Dictionary<string, object>>(s => s
    //      .Index(searchPattern)
    //      .From(0)
    //      .Size(ConstMaxResultWindow)
    //      .Source(source => source.Includes(fi =>
    //      {
    //        var a = fi.Field("executeat");
    //        a.Field(searchField);
    //        return a;
    //      }
    //          ))
    //      .Query(q => q
    //          .Bool(b => b
    //              .Must(
    //                must => must.Match(m => {
    //                  switch (shiftId)
    //                  {
    //                    case EnumShiftId.Shift1:
    //                    case EnumShiftId.Shift2:
    //                    case EnumShiftId.Shift3:
    //                      return m.Field("shiftid")
    //                        .Query(((int)shiftId).ToString());
    //                  }
    //                  return m;
    //                }),
    //                must => must.DateRange(dr => dr
    //                    .Field("executeat")
    //                    .GreaterThanOrEquals(startTime)
    //                    .LessThanOrEquals(endTime)
    //                )
    //              )
    //          )
    //      )
    //      .Sort(sort => sort
    //        .Field("executeat", SortOrder.Descending)
    //      )
    //    );

    //  if (!searchResponse.IsValid)
    //  {
    //    this.logger.LogMsg(Messages.ErrException.SetParameters(funcName, searchResponse.OriginalException));
    //    return resultList;
    //  }

    //  var total = searchResponse.Total;

    //  foreach (var hit in searchResponse.Hits.Reverse())
    //  {

    //    var createdAt = DateTimeUtil.GetDateTimeFromString(hit.Source["executeat"].ToString(), ConstDateTimeFormat.YYYYMMDDTHHMMSSFFF_07_00);
    //    if (!hit.Source.ContainsKey(searchField.ToLower()))
    //    {
    //      continue;
    //    }
    //    var fieldVal = hit.Source[searchField.ToLower()];
    //    if (fieldVal != null)
    //    {
    //      double value;
    //      if (double.TryParse(fieldVal.ToString(), out value))
    //      {
    //        string executeAtStr = hit.Source["executeat"].ToString();
    //        var model = new ChartDataModel
    //        {
    //          //Id = hit.Id,
    //          ExecuteAt = executeAtStr.Substring(0, 10) + " " + executeAtStr.Substring(11, 12)
    //        };
    //        model.DicValue.Add(searchField.ToLower(), value);
    //        resultList.Add(model);
    //      }
    //      else
    //      {
    //        this.logger.LogMsg(Messages.ErrInputInvalid_0_1.SetParameters(funcName, $"hit.Id: {hit.Id}, hit.Source: {hit.Source}"));
    //      }
    //    }
    //    else
    //    {
    //      this.logger.LogMsg(Messages.ErrInputInvalid_0_1.SetParameters(funcName, $"searchField not found, searchField: {searchField}"));
    //    }
    //  }

    //  return resultList;
    //}
    //public async Task<List<ChartDataModel>> SearchAllInList(string searchPattern,
    //                                                List<string> searchField,
    //                                                EnumShiftId? shiftId,
    //                                                DateTime startTime0,
    //                                                DateTime endTime0
    //                                                )
    //{
    //  string funcName = "Search";
    //  var resultList = new List<ChartDataModel>();
    //  DateTime startTime = DateTimeUtil.GetUTCDateTime(startTime0);
    //  DateTime endTime = DateTimeUtil.GetUTCDateTime(endTime0);
    //  var searchResponse = await elasticClient.SearchAsync<Dictionary<string, object>>(s => s
    //      .Index(searchPattern)
    //      .From(0)
    //      .Size(ConstMaxResultWindow)
    //      .Source(source => source.Includes(fi =>
    //      {
    //        var a = fi.Field("executeat");
    //        foreach (var searchKey in searchField)
    //        {
    //          a.Field(searchKey);
    //        }
    //        return a;
    //      }))
    //      .Query(q => q
    //          .Bool(b => b
    //              .Must(
    //                must => must.Match(m => {
    //                  switch (shiftId)
    //                  {
    //                    case EnumShiftId.Shift1:
    //                    case EnumShiftId.Shift2:
    //                    case EnumShiftId.Shift3:
    //                      return m.Field("shiftid")
    //                        .Query(((int)shiftId).ToString());
    //                  }
    //                  return m;
    //                }),
    //                must => must.DateRange(dr => dr
    //                    .Field("executeat")
    //                    .GreaterThanOrEquals(startTime)
    //                    .LessThanOrEquals(endTime)
    //                )
    //              )
    //          )
    //      )
    //      .Sort(sort => sort
    //        .Field("executeat", SortOrder.Descending)
    //      )
    //    );

    //  if (!searchResponse.IsValid)
    //  {
    //    this.logger.LogMsg(Messages.ErrException.SetParameters(funcName, searchResponse.OriginalException));
    //    return resultList;
    //  }

    //  var total = searchResponse.Total;

    //  foreach (var hit in searchResponse.Hits.Reverse())
    //  {
    //    string executeAtStr = hit.Source["executeat"].ToString();
    //    var model = new ChartDataModel
    //    {
    //      //Id = hit.Id,
    //      ExecuteAt = executeAtStr.Substring(0, 10) + " " + executeAtStr.Substring(11, 12)
    //    };
    //    foreach (var searchKey in searchField)
    //    {
    //      if (!hit.Source.ContainsKey(searchKey.ToLower()))
    //      {
    //        continue;
    //      }
    //      var fieldVal = hit.Source[searchKey.ToLower()];
    //      if (fieldVal != null)
    //      {
    //        double value;
    //        if (double.TryParse(fieldVal.ToString(), out value))
    //        {
    //          model.DicValue.Add(searchKey.ToLower(), value);

    //        }
    //        else
    //        {
    //          this.logger.LogMsg(Messages.ErrInputInvalid_0_1.SetParameters(funcName, $"hit.Id: {hit.Id}, hit.Source: {hit.Source}"));
    //        }
    //      }
    //      else
    //      {
    //        this.logger.LogMsg(Messages.ErrInputInvalid_0_1.SetParameters(funcName, $"searchField not found, searchField: {searchField}"));
    //      }
    //    }
    //    resultList.Add(model);
    //  }

    //  return resultList;
    //}

    //public async Task<List<ChartDataModel>> SearchLastestData(string searchPattern,
    //                                                List<string> searchField)
    //{
    //  string funcName = "SearchLastestData";
    //  var resultList = new List<ChartDataModel>();
    //  var searchResponse = await elasticClient.SearchAsync<Dictionary<string, object>>(s => s
    //      .Index(searchPattern)
    //      .Size(1)
    //      .Source(source => source.Includes(fi =>
    //      {
    //        var a = fi.Field("executeat");
    //        foreach (var searchKey in searchField)
    //        {
    //          a.Field(searchKey);
    //        }
    //        return a;
    //      }))
    //      .Sort(sort => sort
    //        .Field("executeat", SortOrder.Descending)
    //      )
    //    );

    //  if (!searchResponse.IsValid)
    //  {
    //    this.logger.LogMsg(Messages.ErrException.SetParameters(funcName, searchResponse.OriginalException));
    //    return resultList;
    //  }

    //  var total = searchResponse.Total;

    //  foreach (var hit in searchResponse.Hits.Reverse())
    //  {
    //    string executeAtStr = hit.Source["executeat"].ToString();
    //    var model = new ChartDataModel
    //    {
    //      //Id = hit.Id,
    //      ExecuteAt = executeAtStr.Substring(0, 10) + " " + executeAtStr.Substring(11, 12)
    //    };
    //    foreach (var searchKey in searchField)
    //    {
    //      if (!hit.Source.ContainsKey(searchKey.ToLower()))
    //      {
    //        continue;
    //      }
    //      var fieldVal = hit.Source[searchKey.ToLower()];
    //      if (fieldVal != null)
    //      {
    //        double value;
    //        if (double.TryParse(fieldVal.ToString(), out value))
    //        {
    //          model.DicValue.Add(searchKey.ToLower(), value);

    //        }
    //        else
    //        {
    //          this.logger.LogMsg(Messages.ErrInputInvalid_0_1.SetParameters(funcName, $"hit.Id: {hit.Id}, hit.Source: {hit.Source}"));
    //        }
    //      }
    //      else
    //      {
    //        this.logger.LogMsg(Messages.ErrInputInvalid_0_1.SetParameters(funcName, $"searchField not found, searchField: {searchField}"));
    //      }
    //    }
    //    resultList.Add(model);
    //  }

    //  return resultList;
    //}
  }
}