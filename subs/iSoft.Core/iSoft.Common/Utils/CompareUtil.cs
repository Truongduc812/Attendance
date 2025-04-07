using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Utils
{
  public class CompareUtil
  {
    public static bool MatchData(List<object> list1, List<object> list2, string tableName)
    {
      if (list1 == null || list2 == null)
      {
        return false;
      }
      if (list1.Count != list2.Count)
      {
        return false;
      }
      if (tableName.Trim().ToLower() == "TraceDataSafetyPoints".ToLower())
      {
        for (int i = 0; i < list1.Count; i++)
        {
          if (list1[i] == null || list2[i] == null)
          {
            if (list1[i] != null || list2[i] != null)
            {
              return false;
            }
          }
          else if (list1[i].ToString() != list2[i].ToString()
            && (!((list1[i].ToString().Trim() != "1") && (list2[i].ToString().Trim() != "1"))))
          {
            return false;
          }
        }
      }
      else
      {
        for (int i = 0; i < list1.Count; i++)
        {
          if (list1[i] == null || list2[i] == null)
          {
            if (list1[i] != null || list2[i] != null)
            {
              return false;
            }
          }
          else if (list1[i].ToString() != list2[i].ToString())
          {
            return false;
          }
        }
      }
      return true;
    }
  }
}
