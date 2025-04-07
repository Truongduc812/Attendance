using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PRPO.MainService.CustomAttributes
{
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
  public class PermissionAttribute : Attribute, IAuthorizationFilter
  {
    private string[] _permits { get; set; }

    public PermissionAttribute(params string[] permits)
    {
      this._permits = permits;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
      if (context.HttpContext.Items.TryGetValue("Permissions", out object permissionsObj) && permissionsObj is List<string> userPermits)
      {
        //* root permission
        if (userPermits.Contains("ROOT"))
        {
          return;
        }

        //* check user has permission
        if (!userPermits.Intersect(this._permits).Any())
        {
          context.Result = new UnauthorizedResult();
          return;
        }
      }
      else
      {
        context.Result = new UnauthorizedResult();
        return;
      }
    }

  }
}
