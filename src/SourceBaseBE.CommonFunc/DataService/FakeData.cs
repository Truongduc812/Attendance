using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.CommonFunc.DataService
{
	public static class FakeData
	{
		public static string GenerateRandomEmail()
		{
			return string.Format("{0}@{1}.com", GenerateRandomAlphabetString(10), GenerateRandomAlphabetString(10));
		}
		/// <summary>
		/// Gets a string from the English alphabet at random
		/// </summary>
		public static string GenerateRandomAlphabetString(int length)
		{
			string allowedChars = "abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ";
			var rnd = SeedRandom();

			char[] chars = new char[length];
			for (int i = 0; i < length; i++)
			{
				chars[i] = allowedChars[rnd.Next(allowedChars.Length)];
			}

			return new string(chars);
		}
		private static Random SeedRandom()
		{
			return new Random(Guid.NewGuid().GetHashCode());
		}
	}
}
