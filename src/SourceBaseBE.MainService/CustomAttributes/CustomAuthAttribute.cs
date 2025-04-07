
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Policy;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace SourceBaseBE.CustomAttributes
{
	public class SessionFilter : Attribute, IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext filterContext)
		{
			//if there is no session whitch key is "register", user will not access to specified action and redirect to login page.

			//var result = filterContext.HttpContext.Session.GetString("register");
			//if (result == null)
			//{
			//    filterContext.Result = new RedirectToActionResult("", "Login", null);
			//}
		}

		public void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var loginUser = filterContext.HttpContext.Session.GetString("userinfo");
			if (loginUser == null)
			{
				filterContext.Result = new RedirectToActionResult("Index", "Login", null);

			}
		}

	}
	
}
