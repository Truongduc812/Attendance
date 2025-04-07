using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SourceBaseBE.MainService.Services;

namespace PRPO.MainService.CustomAttributes
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
  public class GetPermissionAttribute : Attribute, IAuthorizationFilter
  {
    public void OnAuthorization(AuthorizationFilterContext context)
    {
      //* get user from HttpContext
      var user = context.HttpContext.User;
      // you can also use registered services
      var _userService = context.HttpContext.RequestServices.GetService<UserService>();

      //* check user authenticated
      if (!user.Identity.IsAuthenticated)
      {
        context.Result = new UnauthorizedResult();
        return;
      }

      //* ROOT
      var isRoot = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value == "ROOT";
      if (isRoot)
      {
        context.HttpContext.Items["Permissions"] = new List<string> { "ROOT" };
        return;
      }

      //* get userId
      var userId = user.FindFirstValue("EditerId");

      //* get user permission
      if (!long.TryParse(userId, out long userIdValue))
      {
        context.Result = new UnauthorizedResult();
        return;
      }
      //var permissions = _userService.GetPermissionOfUser(userIdValue);

    }
  }
}
