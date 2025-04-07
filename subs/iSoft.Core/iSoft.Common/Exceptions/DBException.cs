using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Exceptions
{
    public class DBException: BaseException
    {
        public DBException(string? message)
            : base(message)
        {
        }
        public DBException(Exception ex)
            : base(ex)
        {
        }
        public DBException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
