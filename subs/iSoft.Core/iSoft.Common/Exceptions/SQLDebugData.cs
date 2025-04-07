using iSoft.Common.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace iSoft.Common.Exceptions
{
    public class SQLDebugData
  {
    public string SQLString { get; set; } = "";
    public Dictionary<string, object> Params { get; set; }
    public override string ToString()
    {
      this.SQLString = this.SQLString.Replace("\r\n", " ").Replace("\n", " ").Replace("\t", " ").Replace("  ", " ");
      return this.ToJson();
    }
  }
}
