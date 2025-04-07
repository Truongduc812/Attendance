using System;
using System.Security.Claims;
using iSoft.Redis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PRPO.MainService.CustomAttributes
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
  public class TokenAuthorizeAttribute : Attribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      // get accessToken from header request
      string bearerToken = context.HttpContext.Request.Headers["Authorization"];
      string accessToken = bearerToken.Split(" ")[1];
      //* get user from HttpContext
      var user = context.HttpContext.User;

      //* check user authenticated
      if (!user.Identity.IsAuthenticated)
      {
        context.Result = new UnauthorizedResult();
        return;
      }
      //* get userId
      var userId = user.FindFirstValue("EditerId");
      if (!long.TryParse(userId, out long userIdValue))
      {
        context.Result = new UnauthorizedResult();
        return;
      }

    }

  }
}
