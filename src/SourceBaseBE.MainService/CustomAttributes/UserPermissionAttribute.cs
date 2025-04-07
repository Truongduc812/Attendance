using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using iSoft.Common.Enums;
using iSoft.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Enums;
using iSoft.Common.Enums;
using SourceBaseBE.MainService.ExtensionMethods;
using SourceBaseBE.MainService.Services;

namespace SourceBaseBE.MainService.CustomAttributes
{
    /// <summary>
    /// *** Attribute Example: ***
    /// [UserPermission]
    /// [UserPermission(EnumUserRole.Admin)]
    /// [UserPermission(EnumUserRole.Root)]
    /// [UserPermission(EnumDepartmentAdmin.User)]
    /// [UserPermission(EnumDepartmentAdmin.Admin1)]
    /// [UserPermission(EnumUserRole.Root, EnumDepartmentAdmin.Admin2, EnumDepartmentAdmin.Admin3)]
    /// 
    /// *** Get current user in controller example: ***
    /// long? currentUserId = CommonFunction.GetCurrentUserId(this.HttpContext);
    /// UserEntity? currentUser = (UserEntity)CommonFunction.GetCurrentUser(this.HttpContext);
    /// List<long> listDepartmentId = CommonFunction.GetHasPermissionDepartmentIds(this.HttpContext);
    /// EnumDepartmentAdmin requiredDeptRole = CommonFunction.GetCurrentCheckDepartmentAdminRole(this.HttpContext);
    /// if (CommonFunction.IsHasPermissionDepartmentIds(this.HttpContext, 1)){}
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class UserPermissionAttribute : Attribute, IAuthorizationFilter
    {
        private EnumUserRole _userRole = EnumUserRole.None;
        private List<EnumDepartmentAdmin> _listDeptAdminRole = new List<EnumDepartmentAdmin>();

        public UserPermissionAttribute()
        {
        }
        public UserPermissionAttribute(EnumUserRole userRole)
        {
            _userRole = userRole;
        }
        public UserPermissionAttribute(EnumUserRole departmentUserRoles = EnumUserRole.None, params EnumDepartmentAdmin[] departmentAdminRoles)
        {
            this._userRole = departmentUserRoles;
            this._listDeptAdminRole = departmentAdminRoles.ToList();
        }
        public UserPermissionAttribute(params EnumDepartmentAdmin[] departmentAdminRoles)
        {
            this._listDeptAdminRole = departmentAdminRoles.ToList();
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                if (!context.HttpContext.Items.ContainsKey(EnumIdentityType.CurrentCheckDepartmentAdminRole.ToString().ToLower()))
                {
                    context.HttpContext.Items.Add(EnumIdentityType.CurrentCheckDepartmentAdminRole.ToString().ToLower(), this._listDeptAdminRole);
                }
                else
                {
                    context.HttpContext.Items[EnumIdentityType.CurrentCheckDepartmentAdminRole.ToString().ToLower()] = this._listDeptAdminRole;
                }

                var user = context.HttpContext.User;
                if (!user.Identity.IsAuthenticated)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var userIdStr = user.FindFirstValue(EnumIdentityType.UserId.ToString());
                var authenTypeRole = user.Claims.FirstOrDefault(x => x.Type.Contains("role"))?.Value;
                if (!long.TryParse(userIdStr, out long userId))
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var _userService = context.HttpContext.RequestServices.GetService<UserService>();

                var currUser = _userService.GetById(userId, true, false);
                if (!context.HttpContext.Items.ContainsKey(EnumIdentityType.CurrentUser.ToString().ToLower()))
                {
                    context.HttpContext.Items.Add(EnumIdentityType.CurrentUser.ToString().ToLower(), currUser);
                }
                else
                {
                    context.HttpContext.Items[EnumIdentityType.CurrentUser.ToString().ToLower()] = currUser;
                }

                // Check UserRole
                if (!SourceBaseBE.MainService.ExtensionMethods.ExtensionMethods.IsHasPermissionOn(this._userRole))
                {

                    context.Result = new UnauthorizedResult();
                    return;
                }

                // Check DepartmentAdminRole
                List<long> listDepartmentId = new List<long>();
                if (this._listDeptAdminRole.Count >= 1 || _userRole != EnumUserRole.None)
                {
                    bool groupExistsFlag = false;
                    if (currUser != null && currUser.DepartmentAdmins != null)
                    {
                        foreach (var departmentAdmin in currUser.DepartmentAdmins)
                        {
                            if (departmentAdmin.DeletedFlag == true
                              || departmentAdmin.Role == null)
                            {
                                continue;
                            }

                            if (this._listDeptAdminRole.Contains(departmentAdmin.Role.Value))
                            {
                                if (departmentAdmin.DepartmentId != null)
                                {
                                    listDepartmentId.Add(departmentAdmin.DepartmentId.Value);
                                    groupExistsFlag = true;
                                }
                            }
                        }
                    }
                    var userRoleStr = authenTypeRole;
                    if (Enum.Parse<EnumUserRole>(userRoleStr) == EnumUserRole.Root)
                    {
                        groupExistsFlag = true;
                    }
                    if (!groupExistsFlag)
                    {
                        var a = new UnauthorizedAccessException($"Current: {userRoleStr},{_listDeptAdminRole.Select(x => x.ToString() + ",")}");
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
                if (listDepartmentId.Count >= 1)
                {
                    if (!context.HttpContext.Items.ContainsKey(EnumIdentityType.ListHasPermissionDepartmentId.ToString().ToLower()))
                    {
                        context.HttpContext.Items.Add(EnumIdentityType.ListHasPermissionDepartmentId.ToString().ToLower(), listDepartmentId);
                    }
                    else
                    {
                        context.HttpContext.Items[EnumIdentityType.ListHasPermissionDepartmentId.ToString().ToLower()] = listDepartmentId;
                    }
                }


            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
    }
}
