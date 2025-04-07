using iSoft.Common.Enums;
using iSoft.Common.Utils;

namespace iSoft.Common.Models
{
  public class SearchFieldOperation
  {
    public EnumDicSearchKey SearchType { get; set; }
    public string EnvKeyStr { get; set; }
    public List<EnumOperation> ListOperation { get; set; } = new List<EnumOperation>();
    public List<string> ListItem { get; set; } = new List<string>();
    //public List<SearchFieldOperation> ChidrenOperation { get; set; }

    public SearchFieldOperation() { }

    public SearchFieldOperation(string envKeyStr)
    {
      SearchType = EnumDicSearchKey.Operation;
      EnvKeyStr = envKeyStr.ConvertToESField(0);
      //ChidrenOperation = new List<SearchFieldOperation>();

      // 1. ChidrenOperation
      // 2. SearchType (Item1, Item2, ListItem)

      // Multiply, Subtract
      var arrStr = envKeyStr.Split(new string[] { "*", "/" }, System.StringSplitOptions.None);
      int strIndex = 0;
      if (arrStr.Length >= 1)
      {
        for (int i = 0; i < envKeyStr.Length; i++)
        {
          if (envKeyStr[i] == '*')
          {
            ListOperation.Add(EnumOperation.Multiply);
            ListItem.Add(StringUtil.ConvertToESField(arrStr[strIndex++], 0));
          }
          else if (envKeyStr[i] == '/')
          {
            ListOperation.Add(EnumOperation.Divide);
            ListItem.Add(StringUtil.ConvertToESField(arrStr[strIndex++], 0));
          }
        }
        if (ListOperation.Count >= 1)
        {
          ListItem.Add(StringUtil.ConvertToESField(arrStr[strIndex++], 0));
        }
      }
    }
  }
}
