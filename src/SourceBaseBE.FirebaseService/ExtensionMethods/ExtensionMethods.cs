using System.Collections.Generic;
using System.Linq;
using iSoft.Firebase.Models;
using iSoft.Database.Entities;

namespace iSoft.Firebase.ExtensionMethods
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
    public static IEnumerable<UserAuthModel> WithoutPasswords(this IEnumerable<UserAuthModel> users)
    {
      if (users == null) return null;

      return users.Select(x => x.WithoutPassword());
    }

    public static UserAuthModel WithoutPassword(this UserAuthModel user)
    {
      if (user == null) return null;

      user.Password = null;
      return user;
    }
  }
}