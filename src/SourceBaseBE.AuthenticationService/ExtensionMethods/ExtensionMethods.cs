using System.Collections.Generic;
using System.Linq;
using iSoft.Auth.Models;
using iSoft.Database.Entities;
using iSoft.Database.Models.ResponseModels;

namespace iSoft.Auth.ExtensionMethods
{
  public static class ExtensionMethods
  {
    public static IEnumerable<UserEntity> WithoutPasswords(this IEnumerable<UserEntity> users)
    {
      if (users == null) return null;

      return users.Select(x => x.WithoutPassword());
    }

    public static UserEntity WithoutPassword(this UserEntity user)
    {
      if (user == null) return null;

      user.Password = null;
      return user;
    }
    public static IEnumerable<UserResponseModel> WithoutPasswords(this IEnumerable<UserResponseModel> users)
    {
      if (users == null) return null;

      return users.Select(x => x.WithoutPassword());
    }

    public static UserResponseModel WithoutPassword(this UserResponseModel user)
    {
      if (user == null) return null;

      user.Password = null;
      return user;
    }
  }
}