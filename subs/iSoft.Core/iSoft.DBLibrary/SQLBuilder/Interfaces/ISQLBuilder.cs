using iSoft.Common.Enums.DBProvider;
using iSoft.DBLibrary.SQLBuilder.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace iSoft.DBLibrary.SQLBuilder.Interfaces
{
  public class FieldOrder
  {
    public object table { get; set; }
    public object fieldName { get; set; }
    public SQLSortOrder order { get; set; }
    public string field { get; set; }
    public bool isAltField { get; set; }

    public FieldOrder(object table, object fieldName, SQLSortOrder order)
    {
      isAltField = false;
      this.table = table;
      this.fieldName = fieldName;
      this.order = order;
    }
    public FieldOrder(string field, SQLSortOrder order)
    {
      isAltField = true;
      this.field = field;
      this.order = order;
    }
    public string GetSQL(Func<string, string> quoteField)
    {
      if (!isAltField)
      {
        return string.Format("{0}.{1} {2}", quoteField(table.ToString()), quoteField(fieldName.ToString()), order);
      }
      else
      {
        return string.Format("{0} {1}", field, order);
      }
    }
  }
  public class FieldName
  {
    public object table { get; set; }
    public object fieldName { get; set; }
    public string field { get; set; }
    public bool isAltField { get; set; }

    public FieldName(object table, object fieldName)
    {
      this.isAltField = false;
      this.table = table;
      this.fieldName = fieldName;
    }
    public FieldName(string field, bool isAlt = false)
    {
      this.isAltField = isAlt;
      this.table = "";
      this.field = field;
    }
    public string GetSQL(Func<string, string> quoteField)
    {
      if (!isAltField)
      {
        if (table.ToString().Trim() != "")
        {
          return string.Format("{0}.{1}", quoteField(table.ToString()), quoteField(fieldName.ToString()));
        }
        if (!string.IsNullOrWhiteSpace(fieldName as string))
        {
          return quoteField(fieldName.ToString());
        }
        return quoteField(field);
      }
      else
      {
        return field;
      }
    }
    public override string ToString()
    {
      if (!isAltField)
      {
        if (table.ToString().Trim() != "")
        {
          return string.Format("{0}.{1}", (table.ToString()), (fieldName.ToString()));
        }
        if (!string.IsNullOrWhiteSpace(fieldName as string))
        {
          return (fieldName.ToString());
        }
        return (field);
      }
      else
      {
        return field;
      }
    }
  }
  public interface ISQLBuilder
  {
    public EnumDBProvider GetDatabaseType ();
    public ISQLBuilder Selects(params string[] fields);
    public ISQLBuilder Select(FieldName fieldName, string alt);
    public ISQLBuilder SelectIncNumber(FieldOrder[] fieldOrders, string alt);
    public ISQLBuilder SelectSum(FieldName fieldName, string alt);
    public ISQLBuilder SelectAvg(FieldName fieldName, string alt);
    public ISQLBuilder SelectMax(FieldName fieldName, string alt);
    public ISQLBuilder SelectMin(FieldName fieldName, string alt);
    public ISQLBuilder SelectCount(FieldName fieldName, string alt);
    public ISQLBuilder From(object table, string alt = "");
    public ISQLBuilder From(ISQLBuilder subQuery, string alt = "");
    public ISQLBuilder InnerJoin(object baseTable, object targetTable, object fieldNameBase, object fieldNameTarget);
    public ISQLBuilder LeftJoin(object baseTable, object targetTable, object fieldNameBase, object fieldNameTarget);
    public ISQLBuilder Where(FieldName fieldName, object value);
    public ISQLBuilder WhereEquals<T>(FieldName fieldName, object value);
    public ISQLBuilder WhereBetween<T>(FieldName fieldName, object fromValue, object toValue);
    public ISQLBuilder WhereLessThen<T>(FieldName fieldName, object value, bool isEqual = false);
    public ISQLBuilder WhereGreaterThen<T>(FieldName fieldName, object value, bool isEqual = false);
    public ISQLBuilder WhereAnd(ISQLBuilder statement1, ISQLBuilder statement2);
    public ISQLBuilder WhereAnd(ISQLBuilder statement1);
    public ISQLBuilder WhereOr(ISQLBuilder statement1, ISQLBuilder statement2);
    public ISQLBuilder WhereLike(FieldName fieldName, string value);
    public ISQLBuilder WhereLikeUnaccent(FieldName fieldName, string value);
    public ISQLBuilder WhereILike(FieldName fieldName, string value);
    public ISQLBuilder WhereILikeUnaccent(FieldName fieldName, string value);
    public ISQLBuilder WhereIn(FieldName fieldName, object[] values);
    public ISQLBuilder WhereIsNull(FieldName fieldName);
    public ISQLBuilder WhereIsNotNull(FieldName fieldName);
    public ISQLBuilder GroupBy(FieldName fieldName);
    public ISQLBuilder OrderBy(FieldName fieldName, SQLSortOrder order);
    public ISQLBuilder OrderBy(string alt, SQLSortOrder order);
    public ISQLBuilder Having(ISQLBuilder whereStatement);
    public ISQLBuilder Offset(int offset);
    public ISQLBuilder Limit(int limit);
    public ISQLBuilder Insert(object table, object[] fieldNames);
    public ISQLBuilder Values(object[] inputValues);
    public Dictionary<string, object> GetParameters();
    public ISQLBuilder New();
    public ISQLBuilder New(ISQLBuilder parent);
    public string GetSQL();
    public string GetSQLRaw(ref object[] parameters);
    public string GetCondition();
    public void ResetParameters(int parameter = 0);
    public void IncSubParameters();
    public int getSubParameterIndex();
    public ISQLBuilder Delete(object table);
    public ISQLBuilder Update(object table);
    public ISQLBuilder Set(FieldName fieldName, object value);
  }
}
