using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database
{
	public class ConstDatabase
	{
		public const string ConstSourceBaseBECacheMainService = "SourceBaseBE|Main";
		public static DateTime StartTimeShift1 = new DateTime(1901, 1, 1, 6, 0, 0);
		public static DateTime StartTimeShift2 = new DateTime(1901, 1, 1, 14, 0, 0);
		public static DateTime StartTimeShift3 = new DateTime(1901, 1, 1, 22, 0, 0);
		public static DateTime NextdayStartTimeShift1 = new DateTime(1901, 1, 2, 6, 0, 0);

        public static string ConstImageGetAvatarPath = "/AvatarImagePath";
        public static string ConstImageSaveAvatarPath = "./wwwroot/AvatarImagePath";

    }
}
