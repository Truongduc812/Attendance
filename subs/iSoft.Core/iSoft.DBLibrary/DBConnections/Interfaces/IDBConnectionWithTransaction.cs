using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace iSoft.DBLibrary.DBConnections.Interfaces
{
    public interface IDBConnectionWithTransaction:IDBConnectionCustom
    {
        public DbTransaction BeginTransaction();
    }
}
