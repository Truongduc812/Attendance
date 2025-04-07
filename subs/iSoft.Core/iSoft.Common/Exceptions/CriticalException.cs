using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Exceptions
{
    public class CriticalException : BaseException
    {
        public CriticalException(string? message)
            : base(message)
        {
        }
        public CriticalException(Exception ex)
            : base(ex)
        {
        }
        public CriticalException(string message, Exception ex)
            : base(message, ex)
        {
        }
    }
}
