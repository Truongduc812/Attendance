using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.ElasticSearch.ESScriptPattern
{
  public class ESScript
  {
    public const string ESScriptIsExistsMessageIdTemplate = @"
{
  ""_source"": [""executeat""], 
  ""from"": 0,
  ""size"": 1,
  ""query"": {
    ""bool"": {
      ""must"": [
        {
          ""match"": {
            ""messageid"": ""@messageId""
          }
        }
      ]
    }
  }
}
";
    public const string ESScriptCreateIndexTempate = @"
{
  ""version"": 1,
    ""priority"": 50,
    ""template"": {
    ""mappings"": {
      ""properties"": {
        ""@timestamp"": {
            ""type"": ""date""
        },
        ""connectionid"": {
            ""type"": ""keyword"",
            ""index"": true,
            ""index_options"": ""docs"",
            ""eager_global_ordinals"": true,
            ""norms"": false,
            ""split_queries_on_whitespace"": false,
            ""doc_values"": true,
            ""store"": true
        },
        ""messageid"": {
            ""type"": ""keyword"",
            ""index"": true,
            ""index_options"": ""docs"",
            ""eager_global_ordinals"": false,
            ""norms"": false,
            ""split_queries_on_whitespace"": false,
            ""doc_values"": true,
            ""store"": true
        },
        ""shiftid"": {
            ""type"": ""keyword"",
            ""index"": true,
            ""index_options"": ""docs"",
            ""eager_global_ordinals"": true,
            ""norms"": false,
            ""split_queries_on_whitespace"": false,
            ""doc_values"": true,
            ""store"": true
        },
        ""executeat"": {
            ""type"": ""date""
        },
        @fields
      }
    }
  },
  ""index_patterns"": [
    ""@esPatternSearch""
  ],
  ""data_stream"": {
    ""hidden"": false
  },
  ""composed_of"": []
}
";
  }
}
