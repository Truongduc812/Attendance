
using iSoft.Common.Enums;
using iSoft.Common.Models;

namespace iSoft.Common.Utils
{
  public static class ParamUtil
  {
    public static Dictionary<string, SearchFieldOperation> SplitExpressionSearchField(string expression)
    {
      var dicRS = new Dictionary<string, SearchFieldOperation>();

      var arrTemp = expression.Split(',');
      foreach (var item in arrTemp)
      {
        var arrStr = item.Split(new string[] { "*", "/" }, System.StringSplitOptions.None);
        if (arrStr.Length >= 2)
        {
          dicRS.Add(item, new SearchFieldOperation(item));
        }
        else
        {
          dicRS.Add(item, new SearchFieldOperation()
          {
            SearchType = EnumDicSearchKey.Single,
            ListItem = { item },
            EnvKeyStr = item,
          });
        }
      }
      return dicRS;
    }

    public static List<string> GetListEnvKey(Dictionary<string, SearchFieldOperation> dicExpression)
    {
      List<string> listRS = new List<string>();
      foreach(var keyVal in dicExpression)
      {
        if (keyVal.Value.SearchType == EnumDicSearchKey.Operation)
        {
          foreach(var item in keyVal.Value.ListItem)
          {
            listRS.Add(item);
          }
        }
        else
        {
          listRS.Add(keyVal.Value.EnvKeyStr);
        }
      }
      return listRS;
    }
  }
}
