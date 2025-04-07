using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace iSoft.Common.Utils
{
  public class EncodeUtil
  {
    private const string ConstPassEndcode = "3e7da5e8-9540-40ba-bd75-21fe19686e16";

    /// <summary>
    /// MD5
    /// </summary>
    /// <param name="inputStr"></param>
    /// <returns></returns>
    public static string MD5(string inputStr)
    {
      var salt = Encoding.UTF8.GetBytes(ConstPassEndcode + inputStr);
      var password = Encoding.UTF8.GetBytes(ConstPassEndcode);

      var hmacMD5 = new HMACMD5(salt);
      var saltedHash = hmacMD5.ComputeHash(password);

      return Convert.ToBase64String(saltedHash);
    }

    public static byte[] CompressString(string text)
    {
      var bytes = Encoding.UTF8.GetBytes(text);
      using (var msi = new MemoryStream(bytes))
      using (var mso = new MemoryStream())
      {
        using (var gs = new GZipStream(mso, CompressionMode.Compress))
        {
          msi.CopyTo(gs);
        }
        return mso.ToArray();
      }
    }

    public static string DecompressString(byte[] compressed)
    {
      using (var msi = new MemoryStream(compressed))
      using (var mso = new MemoryStream())
      {
        using (var gs = new GZipStream(msi, CompressionMode.Decompress))
        {
          gs.CopyTo(mso);
        }
        return Encoding.UTF8.GetString(mso.ToArray());
      }
    }
  }
}
