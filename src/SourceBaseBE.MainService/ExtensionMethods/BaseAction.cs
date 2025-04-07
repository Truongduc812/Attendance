using iSoft.Common.Exceptions;
using iSoft.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Serilog;

namespace SourceBaseBE.MainService.ExtensionMethods
{
  public class BaseAction : ControllerBase, IDisposable
  {
    private string funcName;
    private ILogger logger;

    public BaseAction(ILogger logger, string funcName)
    {
      this.logger = logger;
      this.funcName = funcName;
    }
    public async Task<IActionResult> ExecuteWithExceptionHandled(Func<IActionResult> action, Func<Exception, Exception> exception = null)
    {
      iSoft.Common.Messages.Message errMessage = null;
      try
      {
        this.logger.LogMsg(Messages.IFuncStart_0, funcName);
        var _ret = action.Invoke(); // Main Action
        this.logger.LogMsg(Messages.IFuncEnd_0, funcName);
        return _ret;
      }
      catch (DBException ex)
      {
        exception?.Invoke(ex);
        errMessage = Messages.ErrDBException.SetParameters(ex);
      }
      catch (BaseException ex)
      {
        exception?.Invoke(ex);
        errMessage = Messages.ErrBaseException.SetParameters(ex);
      }
      catch (Exception ex)
      {
      exception?.Invoke(ex);
        errMessage = Messages.ErrException.SetParameters(ex);
      }
      this.logger.LogMsg(errMessage);
      return this.ResponseError(errMessage);

    }
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (funcName != null)
        {
          funcName = null;
          logger = null;
        }
      }
    }
  }
}
