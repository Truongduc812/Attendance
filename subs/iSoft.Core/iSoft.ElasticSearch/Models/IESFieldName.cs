using iSoft.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.ElasticSearch.Models
{
  public interface IESFieldName
  {
    public string GetESIndexName(string esSubfix = "");
    //public string GetESFieldName();
    public string GetESFieldName2();
    public string GetKey();
    public EnumDataType GetESDataType();
    object GetValueFromObject(object obj);
  }
}
