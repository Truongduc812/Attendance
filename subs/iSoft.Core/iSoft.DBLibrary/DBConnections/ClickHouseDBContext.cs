//using System;
//using System.Data;
//using System.Data.Common;
//using ClickHouse.Client.ADO;
//using ClickHouse.Client.Utility;
//using iSoft.DBLibrary.DBConnections.Interfaces;
//using iSoft.Common.Enums.DBProvider;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Npgsql;

//#nullable disable

//namespace iSoft.DBLibrary.DBConnections
//{
//  public partial class ClickHouseDBConnection : IDBConnectionCustom
//  {
//    public EnumDBProvider dbProvider { get; set; }
//    ClickHouseConnection conn;

//    public ClickHouseDBConnection(EnumDBProvider dbProvider)
//    {
//      this.dbProvider = dbProvider;
//    }

//    public string GetConnectionString()
//    {
//      return "Compress=True;CheckCompressedHash=False;Compressor=lz4;Host=dev.i-soft.com.vn;Port=9123;User=default;Password=;SocketTimeout=600000;Database=imag_syngenta;";
//    }
//    public IDbConnection GetConnection()
//    {
//      this.conn = new ClickHouseConnection(this.GetConnectionString());
//      return this.conn;
//    }
//    public IDbCommand AddParam(IDbCommand command, string key, object value)
//    {
//      ((ClickHouseCommand)command).AddParameter(key, "UUID", value);
//      return command;
//    }
//    public IDbCommand AddParam(IDbCommand command, string key, object value, string type = "")
//    {
//      ((ClickHouseCommand)command).AddParameter(key, type, value);
//      return command;
//    }

//    public void SetOptionBuilder(ref DbContextOptionsBuilder optionsBuilder)
//    {
//      throw new NotImplementedException();
//    }

//    public EnumDBProvider GetDBProvider()
//    {
//      return this.dbProvider;
//    }
//  }
//}

