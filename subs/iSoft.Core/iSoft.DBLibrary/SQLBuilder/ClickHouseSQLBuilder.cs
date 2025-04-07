//using iSoft.Common.Utils;
//using iSoft.DBLibrary.SQLBuilder.Enum;
//using iSoft.DBLibrary.SQLBuilder.Interfaces;
//using Microsoft.EntityFrameworkCore.Metadata.Internal;
//using System.Collections.Generic;

//namespace iSoft.DBLibrary.SQLBuilder
//{
//  public class ClickHouseSQLBuilder : ISQLBuilder
//  {
//    private int subParameterIndex = 0;
//    private static int parameterIndex;
//    private string select = "";
//    private string from = "";
//    private string where = "";
//    private string groupBy = "";
//    private string having = "";
//    private string orderBy = "";
//    private string offset = "";
//    private string limit = "";

//    private string insert = "";
//    private string values = "";
//    private string quoteFieldChar = "`";
//    private string quoteTextChar = "'";

//    private string delete = "";
//    public string GetSQL()
//    {
//      return $"{delete}{insert}{values}{select}{from}{where}{groupBy}{having}{orderBy}{offset}{limit}";
//    }

//    public string GetSQLRaw(ref object[] parameters)
//    {
//      List<Object> listParameters = new List<object>();
//      string sql = this.GetSQL();
//      int count = 0;
//      foreach (var keyval in this.dicParameters)
//      {
//        sql = sql.Replace(keyval.Key, "{" + count + "}");
//        listParameters.Add(keyval.Value);
//        count++;
//      }
//      parameters = listParameters.ToArray();
//      return sql;
//    }

//    private Dictionary<string, object> dicParameters = new Dictionary<string, object>();

//    private string QuoteField(string s)
//    {
//      return $"{quoteFieldChar}{s}{quoteFieldChar}";
//    }
//    private string QuoteText(object s)
//    {
//      return $"{quoteTextChar}{s}{quoteTextChar}";
//    }
//    public ISQLBuilder Insert(object table, object[] fieldNames)
//    {
//      if (insert == "")
//      {
//        insert = "\n INSERT INTO ";
//      }
//      List<string> listFieldStr = new List<string>();
//      foreach (var field in fieldNames)
//      {
//        listFieldStr.Add(this.QuoteField(field.ToString()));
//      }
//      insert += string.Format("{0} ({1}) ", this.QuoteField(table.ToString()), string.Join(", ", listFieldStr));
//      return this;
//    }
//    public ISQLBuilder Values(object[] inputValues)
//    {
//      List<string> listValue = new List<string>();
//      foreach (var value in inputValues)
//      {
//        if (value is Guid || value is string)
//        {
//          listValue.Add(string.Format("{0}", this.QuoteText(value)));
//        }
//        else if (value is DateTime)
//        {
//          listValue.Add(string.Format("{0}", this.QuoteText(DateTimeUtil.GetDateTimeStr((DateTime)value, "yyyy-MM-dd HH:mm:ss.fff"))));
//        }
//        else
//        {
//          listValue.Add(string.Format("{0}", value));
//        }
//      }
//      if (values == "")
//      {
//        values = "\n VALUES ";
//        values += string.Format("({0})", string.Join(", ", listValue));
//      }
//      else
//      {
//        values += ", ";
//        values += string.Format("({0})", string.Join(", ", listValue));
//      }

//      return this;
//    }
//    //public ISQLBuilder Values(object[] inputValues)
//    //{
//    //    List<string> listValue = new List<string>();
//    //    int paramIndex = parameterIndex + 1;
//    //    foreach (var value in inputValues)
//    //    {
//    //        listValue.Add(string.Format("@param{0}", paramIndex));
//    //        this.dicParameters.Add(string.Format("param{0}", paramIndex), value);
//    //        paramIndex++;
//    //        parameterIndex++;
//    //    }
//    //    if (values == "")
//    //    {
//    //        values = "\n VALUES ";
//    //        values += string.Format("({0})", string.Join(", ", listValue));
//    //    }
//    //    else
//    //    {
//    //        values += ", ";
//    //        values += string.Format("({0})", string.Join(", ", listValue));
//    //    }

//    //    return this;
//    //}
//    public ISQLBuilder From(object table, string alt = "")
//    {
//      if (from == "")
//      {
//        from = "\n FROM ";
//      }
//      else
//      {
//        from += ", ";
//      }
//      from += string.Format("{0} {1}", this.QuoteField(table.ToString()), alt);
//      return this;
//    }

//    public ISQLBuilder From(ISQLBuilder subQuery, string alt = "")
//    {
//      if (from == "")
//      {
//        from = "\n FROM ";
//      }
//      else
//      {
//        from += ", ";
//      }
//      from += string.Format("({0}) {1} ", subQuery.GetSQL(), alt);

//      foreach (var keyval in subQuery.GetParameters())
//      {
//        if (!this.dicParameters.ContainsKey(keyval.Key))
//        {
//          this.dicParameters.Add(keyval.Key, keyval.Value);
//        }
//      }
//      return this;
//    }

//    public Dictionary<string, object> GetParameters()
//    {
//      return this.dicParameters;
//    }

//    public ISQLBuilder GroupBy(object table, object fieldName)
//    {
//      if (groupBy == "")
//      {
//        groupBy = "\n GROUP BY ";
//      }
//      else
//      {
//        groupBy += ", ";
//      }
//      groupBy += string.Format("{0}.{1}", this.QuoteField(table.ToString()), this.QuoteField(fieldName.ToString()));
//      return this;
//    }

//    public string GetCondition()
//    {
//      return this.where.Replace("\n WHERE ", "");
//    }

//    public ISQLBuilder Having(ISQLBuilder whereStatement)
//    {
//      string condition = whereStatement.GetCondition();
//      if (condition.Trim() == "")
//      {
//        return this;
//      }

//      if (having == "")
//      {
//        having = "\n HAVING ";
//      }
//      else
//      {
//        having += " AND ";
//      }
//      having += string.Format("{0}", whereStatement.GetCondition());

//      foreach (var keyval in whereStatement.GetParameters())
//      {
//        if (!this.dicParameters.ContainsKey(keyval.Key))
//        {
//          this.dicParameters.Add(keyval.Key, keyval.Value);
//        }
//      }
//      return this;
//    }

//    public ISQLBuilder InnerJoin(object baseTable, object targetTable, object fieldNameBase, object fieldNameTarget)
//    {
//      from += string.Format("\n INNER JOIN {0} ON {0}.{1} = {2}.{3} ",
//          this.QuoteField(targetTable.ToString()),
//          this.QuoteField(fieldNameTarget.ToString()),
//          this.QuoteField(baseTable.ToString()),
//          this.QuoteField(fieldNameBase.ToString()));
//      return this;
//    }

//    public ISQLBuilder Limit(int value)
//    {
//      if (value < 0)
//      {
//        return this;
//      }

//      int paramIndex = parameterIndex + 1;
//      limit = string.Format("\n LIMIT @param{0} ", paramIndex);
//      this.dicParameters.Add(string.Format("param{0}", paramIndex++), value);
//      parameterIndex += 1;
//      return this;
//    }

//    public ISQLBuilder Offset(int value)
//    {
//      if (value < 0)
//      {
//        return this;
//      }

//      int paramIndex = parameterIndex + 1;
//      offset = string.Format("\n OFFSET @param{0} ", paramIndex);
//      this.dicParameters.Add(string.Format("param{0}", paramIndex), value);
//      parameterIndex += 1;
//      return this;
//    }

//    public ISQLBuilder OrderBy(object table, object fieldName, SQLSortOrder order)
//    {
//      if (orderBy == "")
//      {
//        orderBy = "\n ORDER BY ";
//      }
//      else
//      {
//        orderBy += ", ";
//      }
//      orderBy += string.Format("{0}.{1} {2}",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()),
//          order.ToString());
//      return this;
//    }

//    public ISQLBuilder OrderBy(string alt, SQLSortOrder order)
//    {
//      if (orderBy == "")
//      {
//        orderBy = "\n ORDER BY ";
//      }
//      else
//      {
//        orderBy += ", ";
//      }
//      orderBy += string.Format("{0} {1}", alt, order.ToString());
//      return this;
//    }

//    public ISQLBuilder Select(FieldName fieldName, string alt)
//    {
//      if (select == "")
//      {
//        select = "\n SELECT ";
//      }
//      else
//      {
//        select += ", ";
//      }
//      select += string.Format("{0} {1}",
//          fieldName.GetSQL(this.QuoteField),
//          alt);
//      return this;
//    }

//    private ISQLBuilder selectFunc(string func, FieldName fieldName, string alt)
//    {
//      if (select == "")
//      {
//        select = "\n SELECT ";
//      }
//      else
//      {
//        select += ", ";
//      }
//      select += string.Format("{0}({1}) {2}",
//          func,
//          fieldName.GetSQL(this.QuoteField),
//          alt);
//      return this;
//    }

//    public ISQLBuilder SelectAvg(FieldName fieldName, string alt)
//    {
//      return this.selectFunc("AVG", fieldName, alt);
//    }

//    public ISQLBuilder SelectCount(FieldName fieldName, string alt)
//    {
//      return this.selectFunc("COUNT", fieldName, alt);
//    }

//    public ISQLBuilder SelectMax(FieldName fieldName, string alt)
//    {
//      return this.selectFunc("MAX", fieldName, alt);
//    }

//    public ISQLBuilder SelectMin(FieldName fieldName, string alt)
//    {
//      return this.selectFunc("MIN", fieldName, alt);
//    }

//    public ISQLBuilder SelectSum(FieldName fieldName, string alt)
//    {
//      return this.selectFunc("SUM", fieldName, alt);
//    }

//    public ISQLBuilder Selects(params string[] fields)
//    {
//      if (select == "")
//      {
//        select = "\n SELECT ";
//      }
//      else
//      {
//        select += ", ";
//      }
//      select += string.Format("{0}", string.Join(", ", fields));
//      return this;
//    }

//    public ISQLBuilder SelectIncNumber(FieldOrder[] fieldOrders, string alt)
//    {
//      if (select == "")
//      {
//        select = "\n SELECT ";
//      }
//      else
//      {
//        select += ", ";
//      }
//      List<string> listFieldStr = new List<string>();
//      foreach (var fieldOrder in fieldOrders)
//      {
//        listFieldStr.Add(fieldOrder.GetSQL(this.QuoteField));
//      }
//      select += string.Format("ROW_NUMBER() OVER(ORDER BY {0}) {1}",
//          string.Join(", ", listFieldStr),
//          alt);
//      return this;
//    }

//    public ISQLBuilder Where(FieldName fieldName, object value)
//    {
//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      int paramIndex = parameterIndex + 1;
//      where += string.Format("{0} = @param{1}",
//          fieldName.GetSQL(this.QuoteField),
//          paramIndex);

//      this.dicParameters.Add(string.Format("param{0}", paramIndex), value);
//      parameterIndex += 1;

//      return this;
//    }

//    public ISQLBuilder WhereAnd(ISQLBuilder statement1, ISQLBuilder statement2)
//    {
//      string condition1 = statement1.GetCondition();
//      string condition2 = statement2.GetCondition();
//      if (condition1.Contains(" AND ") || condition1.Contains(" OR "))
//      {
//        condition1 = $"({condition1})";
//      }
//      if (condition2.Contains(" AND ") || condition2.Contains(" OR "))
//      {
//        condition2 = $"({condition2})";
//      }
//      if (condition1.Trim() == "" && condition2.Trim() == "")
//      {
//        return this;
//      }

//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      if (condition1.Trim() == "" || condition2.Trim() == "")
//      {
//        where += string.Format("{0}", condition1.Trim() == "" ? condition2 : condition1);
//      }
//      else
//      {
//        where += string.Format("{0} AND {1}", condition1, condition2);
//      }

//      foreach (var keyval in statement1.GetParameters())
//      {
//        if (!this.dicParameters.ContainsKey(keyval.Key))
//        {
//          this.dicParameters.Add(keyval.Key, keyval.Value);
//        }
//      }
//      foreach (var keyval in statement2.GetParameters())
//      {
//        if (!this.dicParameters.ContainsKey(keyval.Key))
//        {
//          this.dicParameters.Add(keyval.Key, keyval.Value);
//        }
//      }
//      return this;
//    }

//    public ISQLBuilder WhereAnd(ISQLBuilder statement1)
//    {
//      string condition1 = statement1.GetCondition();
//      if (condition1.Contains(" AND ") || condition1.Contains(" OR "))
//      {
//        condition1 = $"({condition1})";
//      }
//      if (condition1.Trim() == "")
//      {
//        return this;
//      }

//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      where += string.Format("{0}", condition1);

//      foreach (var keyval in statement1.GetParameters())
//      {
//        if (!this.dicParameters.ContainsKey(keyval.Key))
//        {
//          this.dicParameters.Add(keyval.Key, keyval.Value);
//        }
//      }
//      return this;
//    }

//    public ISQLBuilder WhereOr(ISQLBuilder statement1, ISQLBuilder statement2)
//    {
//      string condition1 = statement1.GetCondition();
//      string condition2 = statement2.GetCondition();
//      if (condition1.Contains(" AND ") || condition1.Contains(" OR "))
//      {
//        condition1 = $"({condition1})";
//      }
//      if (condition2.Contains(" AND ") || condition2.Contains(" OR "))
//      {
//        condition2 = $"({condition2})";
//      }
//      if (condition1.Trim() == "" && condition2.Trim() == "")
//      {
//        return this;
//      }

//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      where += string.Format("{0} OR {1}", condition1, condition2);

//      foreach (var keyval in statement1.GetParameters())
//      {
//        if (!this.dicParameters.ContainsKey(keyval.Key))
//        {
//          this.dicParameters.Add(keyval.Key, keyval.Value);
//        }
//      }
//      foreach (var keyval in statement2.GetParameters())
//      {
//        if (!this.dicParameters.ContainsKey(keyval.Key))
//        {
//          this.dicParameters.Add(keyval.Key, keyval.Value);
//        }
//      }
//      return this;
//    }

//    public ISQLBuilder WhereBetween<T>(object table, object fieldName, object fromValue, object toValue)
//    {
//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      int paramIndex = parameterIndex + 1;
//      where += string.Format("{0}.{1} BETWEEN @param{2} AND @param{3}",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()),
//          paramIndex,
//          paramIndex + 1);

//      this.dicParameters.Add(string.Format("param{0}", paramIndex), (T)fromValue);
//      this.dicParameters.Add(string.Format("param{0}", paramIndex + 1), (T)toValue);
//      parameterIndex += 2;

//      return this;
//    }

//    public ISQLBuilder WhereEquals<T>(object table, object fieldName, object value)
//    {
//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      int paramIndex = parameterIndex + 1;
//      where += string.Format("{0}.{1} = @param{2}",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()),
//          paramIndex);

//      this.dicParameters.Add(string.Format("param{0}", paramIndex), (T)value);
//      parameterIndex += 1;

//      return this;
//    }

//    public ISQLBuilder WhereILike(object table, object fieldName, string value)
//    {
//      if (value.Trim() == "")
//      {
//        return this;
//      }

//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      int paramIndex = parameterIndex + 1;
//      where += string.Format("{0}.{1} ILIKE @param{2}",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()),
//          paramIndex);

//      this.dicParameters.Add(string.Format("param{0}", paramIndex), $"%{value}%");
//      parameterIndex += 1;

//      return this;
//    }

//    public ISQLBuilder WhereILikeUnaccent(object table, object fieldName, string value)
//    {
//      // NOTE: before search run this sql: CREATE EXTENSION unaccent;
//      if (value.Trim() == "")
//      {
//        return this;
//      }

//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      int paramIndex = parameterIndex + 1;
//      where += string.Format("UNACCENT({0}.{1}) ILIKE UNACCENT(@param{2})",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()),
//          paramIndex);

//      this.dicParameters.Add(string.Format("param{0}", paramIndex), $"%{value}%");
//      parameterIndex += 1;

//      return this;
//    }

//    public ISQLBuilder WhereLike(object table, object fieldName, string value)
//    {
//      if (value.Trim() == "")
//      {
//        return this;
//      }

//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      int paramIndex = parameterIndex + 1;
//      where += string.Format("{0}.{1} LIKE @param{2}",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()),
//          paramIndex);

//      this.dicParameters.Add(string.Format("param{0}", paramIndex), $"%{value}%");
//      parameterIndex += 1;

//      return this;
//    }

//    public ISQLBuilder WhereLikeUnaccent(object table, object fieldName, string value)
//    {
//      // NOTE: before search run this sql: CREATE EXTENSION unaccent;
//      if (value.Trim() == "")
//      {
//        return this;
//      }

//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      int paramIndex = parameterIndex + 1;
//      where += string.Format("UNACCENT({0}.{1}) LIKE UNACCENT(@param{2})",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()),
//          paramIndex);

//      this.dicParameters.Add(string.Format("param{0}", paramIndex), $"%{value}%");
//      parameterIndex += 1;

//      return this;
//    }

//    public ISQLBuilder WhereIn(object table, object fieldName, object[] values)
//    {
//      if (values.Length == 0)
//      {
//        return this;
//      }

//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      string valuesStr = quoteTextChar + string.Join($"{quoteTextChar},{quoteTextChar}", values) + quoteTextChar;
//      where += string.Format("{0}.{1} IN ({2})",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()),
//          valuesStr);

//      return this;
//    }
//    public ISQLBuilder WhereIn(object fieldName, object[] values)
//    {
//      if (values.Length == 0)
//      {
//        return this;
//      }

//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      string valuesStr = quoteTextChar + string.Join($"{quoteTextChar},{quoteTextChar}", values) + quoteTextChar;
//      where += string.Format("{0} IN ({1})",
//          this.QuoteField(fieldName.ToString()),
//          valuesStr);

//      return this;
//    }

//    public ISQLBuilder WhereIsNotNull(object table, object fieldName)
//    {
//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      where += string.Format("{0}.{1} IS NOT NULL",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()));

//      return this;
//    }

//    public ISQLBuilder WhereIsNull(object table, object fieldName)
//    {
//      if (where == "")
//      {
//        where = "\n WHERE ";
//      }
//      else
//      {
//        where += " AND ";
//      }
//      where += string.Format("{0}.{1} IS NULL",
//          this.QuoteField(table.ToString()),
//          this.QuoteField(fieldName.ToString()));

//      return this;
//    }

//    public ISQLBuilder New()
//    {
//      return new ClickHouseSQLBuilder();
//    }

//    public ISQLBuilder New(ISQLBuilder parent)
//    {
//      parent.IncSubParameters();
//      var newBuilder = new PostgreSQLBuilder();
//      newBuilder.ResetParameters(parent.getSubParameterIndex() * 1000);
//      return newBuilder;
//    }

//    public void ResetParameters(int parameter = 0)
//    {
//      this.dicParameters.Clear();
//      parameterIndex = parameter;
//    }
//    public void IncSubParameters()
//    {
//      this.subParameterIndex++;
//    }

//    public int getSubParameterIndex()
//    {
//      return subParameterIndex;
//    }

//    public ISQLBuilder Delete(object table)
//    {
//      delete = string.Format("\n ALTER TABLE {0} DELETE ", this.QuoteField(table.ToString()));
//      return this;
//    }
//  }
//}
