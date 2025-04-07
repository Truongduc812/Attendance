using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Exceptions
{
  public class RetryException : BaseException
  {
    public RetryException(string? message)
        : base(message)
    {
    }
    public RetryException(Exception ex)
        : base(ex)
    {
    }
    public RetryException(string message, Exception ex)
        : base(message, ex)
    {
    }
  }
}
