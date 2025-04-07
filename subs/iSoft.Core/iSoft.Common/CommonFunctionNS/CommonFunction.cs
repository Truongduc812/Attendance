using iSoft.Common.Enums;
using iSoft.Common.Exceptions;
using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.CommonFunctionNS
{
    public class CommonFunction
    {

        public static long? GetCurrentUserId(HttpContext httpContext)
        {
            if (httpContext == null || httpContext.User == null)
            {
                return null;
            }

            if (httpContext.User.Claims == null)
            {
                return null;
            }

            try
            {
                return long.Parse(httpContext.User.Claims.Where(x => x.Type.ToLower() == EnumIdentityType.UserId.ToString().ToLower()).FirstOrDefault().Value);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public static Object? GetCurrentUser(HttpContext httpContext)
        {
            if (httpContext == null || httpContext.Items == null)
            {
                return null;
            }
            if (httpContext.Items.ContainsKey(EnumIdentityType.CurrentUser.ToString().ToLower()))
            {
                return httpContext.Items[EnumIdentityType.CurrentUser.ToString().ToLower()];
            }
            return null;
        }
        public static List<long> GetHasPermissionDepartmentIds(HttpContext httpContext)
        {
            if (httpContext == null || httpContext.Items == null)
            {
                return new List<long>();
            }
            if (httpContext.Items.ContainsKey(EnumIdentityType.ListHasPermissionDepartmentId.ToString().ToLower()))
            {
                var listRS = (List<long>)httpContext.Items[EnumIdentityType.ListHasPermissionDepartmentId.ToString().ToLower()];
                listRS = ConvertUtil.RemoveDuplicate(listRS);
                return listRS;
            }
            return new List<long>();
        }
    }
}
