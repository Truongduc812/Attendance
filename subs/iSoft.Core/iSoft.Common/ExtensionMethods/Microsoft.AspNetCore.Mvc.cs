using iSoft.Common.ExtensionMethods;
using iSoft.Common.ResponseObjectNS;
using static iSoft.Common.Messages;
using static iSoft.Common.ResponseObjectNS.ResponseObject;

namespace Microsoft.AspNetCore.Mvc
{
    public static class ExtensionMethods
	{
		public static IActionResult ResponseOk(this ControllerBase controllerBase, object data)
		{
			try
			{
				ResponseObject rs = new ResponseObject(ResponseStatus.Success.ToString(), data);

				return controllerBase.ResponseJSonStr(rs.ToJson());
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public static IActionResult ResponseJSonObj(this ControllerBase controllerBase, object data)
		{
			try
			{
				ResponseObject rs = new ResponseObject(ResponseStatus.Success.ToString(), data);

				string json = rs.ToJson();
				var contentResult = new ContentResult
				{
					Content = json,
					ContentType = "application/json",
					StatusCode = 200
				};

				return contentResult;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}
		public static IActionResult ResponseJSonStr(this ControllerBase controllerBase, string jsonStr)
		{
			var contentResult = new ContentResult
			{
				Content = jsonStr,
				ContentType = "application/json",
				StatusCode = 200
			};

			return contentResult;
		}
		public static IActionResult ResponseError(this ControllerBase controllerBase, Message message)
		{
			if (message == null)
			{
				return controllerBase.ResponseJSonStr(new ResponseObject(ResponseStatus.Error.ToString(), "").ToJson());
			}

			ResponseObject rs = new ResponseObject(ResponseStatus.Error.ToString(), message.GetCode(), message.GetMessage());

			return controllerBase.BadRequest(rs);
			return controllerBase.ResponseJSonStr(rs.ToJson());
    }
    public static IActionResult ResponseErrorCode(this ControllerBase controllerBase, Message message)
    {
      if (message == null)
      {
        return controllerBase.ResponseJSonStr(new ResponseObject(ResponseStatus.Error.ToString(), "").ToJson());
      }

      ResponseObject rs = new ResponseObject(ResponseStatus.Error.ToString(), message.GetCode(), "");

      return controllerBase.BadRequest(rs);
      return controllerBase.ResponseJSonStr(rs.ToJson());
    }
    public static IActionResult ResponseError(this ControllerBase controllerBase, Message message, params object[] parameters)
		{
			if (message == null)
			{
				return controllerBase.ResponseJSonStr(new ResponseObject(ResponseStatus.Error.ToString(), "").ToJson());
			}

			message.SetParameters(parameters);
			ResponseObject rs = new ResponseObject(ResponseStatus.Error.ToString(), message.GetCode(), message.GetMessage());

			return controllerBase.BadRequest(rs);
			return controllerBase.ResponseJSonStr(rs.ToJson());
		}
	}
}