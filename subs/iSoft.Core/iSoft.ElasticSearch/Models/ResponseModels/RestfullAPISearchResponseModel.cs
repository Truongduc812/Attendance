using iSoft.Common.Utils;
using Newtonsoft.Json;
using System;

namespace iSoft.ElasticSearch.Models
{
  public class SearchResult
  {
    [JsonProperty("took")]
    public int? Took { get; set; }

    //[JsonProperty("timed_out")]
    //public bool? TimedOut { get; set; }

    //[JsonProperty("_shards")]
    //public Shards? Shards { get; set; }

    [JsonProperty("hits")]
    public Hits? Hits { get; set; }
  }

  public class Shards
  {
    [JsonProperty("total")]
    public int? Total { get; set; }

    [JsonProperty("successful")]
    public int? Successful { get; set; }

    [JsonProperty("skipped")]
    public int? Skipped { get; set; }

    [JsonProperty("failed")]
    public int? Failed { get; set; }
  }

  public class Hits
  {
    [JsonProperty("total")]
    public Total? Total { get; set; }

    //[JsonProperty("max_score")]
    //public double? MaxScore { get; set; }

    //[JsonProperty("hits")]
    //public List<Hit>? HitList { get; set; }
  }

  public class Total
  {
    [JsonProperty("value")]
    public int? Value { get; set; }

    //[JsonProperty("relation")]
    //public string? Relation { get; set; }
  }

  public class Hit
  {
    [JsonProperty("_index")]
    public string? Index { get; set; }

    [JsonProperty("_type")]
    public string? Type { get; set; }

    [JsonProperty("_id")]
    public string? Id { get; set; }

    [JsonProperty("_score")]
    public double? Score { get; set; }

    [JsonProperty("_source")]
    public Source? Source { get; set; }
  }

  public class Source
  {
    [JsonProperty("executeat")]
    public DateTime? ExecuteAt { get; set; }
  }
}
