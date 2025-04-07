using System.Collections.Generic;
using System.Linq;
using iSoft.Common.Enums;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Enums;

namespace SourceBaseBE.MainService.ExtensionMethods
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

		public static bool IsHasPermissionOn(EnumUserRole userRole)
		{
			switch (userRole)
			{
				case EnumUserRole.None:
					return true;
				case EnumUserRole.User:
					if (userRole.ToString().ToLower().Trim() == EnumUserRole.User.ToString().ToLower()
					  || userRole.ToString().ToLower().Trim() == EnumUserRole.Admin.ToString().ToLower()
					  || userRole.ToString().ToLower().Trim() == EnumUserRole.Root.ToString().ToLower())
					{
						return true;
					}
					else
					{
						return false;
					}
				case EnumUserRole.Admin:
					if ((userRole.ToString().ToLower().Trim() == EnumUserRole.Admin.ToString().ToLower()
					  || userRole.ToString().ToLower().Trim() == EnumUserRole.Root.ToString().ToLower()))
					{
						return true;
					}
					else
					{
						return false;
					}
				case EnumUserRole.Root:
					return true;
				default:
					return true;
			}
		}
	}
}