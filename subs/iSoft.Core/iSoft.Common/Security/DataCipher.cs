/*
Trong đoạn mã này, chúng ta sử dụng thuật toán mã hóa Rijndael (cũng được gọi là AES) 
để mã hóa và giải mã dữ liệu. Để tạo key và IV từ mật khẩu, chúng ta sử dụng lớp Rfc2898DeriveBytes 
và một giá trị salt ngẫu nhiên. Sau khi mã hóa dữ liệu, chúng ta có thể giải mã dữ liệu bằng cách sử
dụng cùng một key và IV.
 */


using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace iSoft.Common.Security
{
  public class DataCipher
  {
    private static string _salt = "Hdjc4^54cb#";
    private static string _password = "gHcJcnNC#J8*jdn$";

    private RijndaelManaged algorithm;
    private Rfc2898DeriveBytes keyGenerator;

    private readonly string rsaPubKey = @"<RSAKeyValue><Modulus>vxAfHl9g20Skl0n9Cq5mtV+pS0cOuz+rh1Fgsg/MhJHZq4qn6vwABJXC5u+JFeBJJ01BG9rWEwI0CLTcBDawGsQU9lxxxdUIEyqDEUU9AfES7TrXgBgmODaShTdHUSc+QD1iNT8Q1Rd5NsPsrCzAxfmZ1HC8y3vPO+nymG8V6V+FCeCSLcmxFrye/Vda/hARkyQvnGZQhi9aAj5R+nOGzawXgKvP7EupXQaVcUWj+wn5MJ9i9WzzFuXSYslrDfp0KCPUOjnRKmeU2BVzlrtw5H75eBgiAaoZXAakWYei5fKLwapnwh14IJ1kkcTUgtdY7sqkmOzpC1SGhoIfV30oHQ==</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
    private readonly string rsaPriKey = @"<RSAKeyValue><Modulus>vxAfHl9g20Skl0n9Cq5mtV+pS0cOuz+rh1Fgsg/MhJHZq4qn6vwABJXC5u+JFeBJJ01BG9rWEwI0CLTcBDawGsQU9lxxxdUIEyqDEUU9AfES7TrXgBgmODaShTdHUSc+QD1iNT8Q1Rd5NsPsrCzAxfmZ1HC8y3vPO+nymG8V6V+FCeCSLcmxFrye/Vda/hARkyQvnGZQhi9aAj5R+nOGzawXgKvP7EupXQaVcUWj+wn5MJ9i9WzzFuXSYslrDfp0KCPUOjnRKmeU2BVzlrtw5H75eBgiAaoZXAakWYei5fKLwapnwh14IJ1kkcTUgtdY7sqkmOzpC1SGhoIfV30oHQ==</Modulus><Exponent>AQAB</Exponent><P>0jP4i9K75c5zgVBXp6PFvl3/occ+gX6guxrOQokih7PhA/GroUodDnP6WUpCAKHnENcZP4lUd2/mtgr8YmthLXNzl7mRvHpJ/KgFygaUicBjsWBIx+BwK6pK4oC/us0X7xYT5CTWs9eMicR+bVnymQ5D7OW4hYYnEyTUJjRrDLM=</P><Q>6LCef4TblFC3kEBtlSJhRICwEF4M5PIMa8EMI7aNN1koJNQ59sTasiqWEO29JBuQW4t2zYXvXHCs8Gjhhx71EO6zXwKtfNK7rBlF2JH1NuSyWc0unMgzC9xIjQzZyGvz12fb18Tlwu5pIJNGt4N7GcdqaEfSDXTRgILdaOGC/+8=</Q><DP>cIgMHmbB2sRMh8UIOCHwAfr2mJg++TpeN+yg0XPy/W0qIF9nv6AasBscwmKDtSz3s8dDqAUQKCTLAVgeR14vFxMAphBdWeap503YU5B0Qs6xUEs4i3C2/FldX6cHfazAjGloWrHyEzNo9HQyLr6BprjBWnic8Tahgpkrzgwv5ws=</DP><DQ>y/O7CSSHXimdq5d9NWGMBgIR0FbPUIED+BKeNFNW1bOU5ysJn00OL2n6XN40kYiDcGn+eMgzdD/ipVoYi1nMDcpbeCSdsbH4AzGnsdrFJxcvaFzaxVsIuxWald80qGJOuXh0Dlyr0r8rb/0G1+Urqf3LO5nv+BFzbAPIU5yOOQs=</DQ><InverseQ>MRqqGMD2nDRUt8fJNegaQGl6lzyq1pvEaakMsEq95P+SAFWUtbV/aIxkNX9WjTVoBfnhWS5aMR8Vk90Gp936rEKlneaEK8oG4W7MSwEbLhM2Q1FHneQWHmFjuouzxuqo9wqiiY2NxB8xC3KPzcqnLsvfa74WBjhzO4KJGMl0qZk=</InverseQ><D>EfwZgclmm509hQQw7oV2I5KKpAxfRZ77rXQqjvhPvxbfj3cw0TzBX60dOJHJrKZ9HFb4Tv/tcMKfVUZ5A2iGcS6N7pbLGt0vsmYpCkIjOQCRUIVeba9YQahZaHLIosn34SImEW8LTZO1FEOjDp3z027oXQfzrY/M9Q3TiSfDv0DV22ata5I0fF9WGaczcv3cJX5lpQWCjV3BJNb5BI6TSf3MTQaFka7pVKxY6z5IkeoumOL2qgCq8HkRq569hT+61BU2YDpreypRh9+dbaANwBpHe/zfdfbg4S/EA4I5txPN888ph+exse/h5r27se1c9jYk/620/js5hGHLgnBPHQ==</D></RSAKeyValue>";


    public DataCipher(string password = null)
    {
      if (!string.IsNullOrEmpty(password))
        _password = password;

      byte[] salt = Encoding.UTF8.GetBytes(_salt);
      keyGenerator = new Rfc2898DeriveBytes(_password, salt);
      algorithm = new RijndaelManaged();
      algorithm.Key = keyGenerator.GetBytes(algorithm.KeySize / 8);
      algorithm.IV = keyGenerator.GetBytes(algorithm.BlockSize / 8);
    }

    public string EncryptRSA(string plainText)
    {
      byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);

      using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
      {
        rsa.FromXmlString(rsaPubKey);

        byte[] encryptedBytes = rsa.Encrypt(plainBytes, false);
        return Convert.ToBase64String(encryptedBytes);
      }
    }

    public string DecryptRSA(string cipherText)
    {
      byte[] cipherBytes = Convert.FromBase64String(cipherText);

      using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
      {
        rsa.FromXmlString(rsaPriKey);

        byte[] decryptedBytes = rsa.Decrypt(cipherBytes, false);
        return Encoding.UTF8.GetString(decryptedBytes);
      }
    }


    public static string EncryptASE(string data)
    {
      byte[] salt = Encoding.UTF8.GetBytes(_salt);
      using (RijndaelManaged cipher = new RijndaelManaged())
      {
        cipher.KeySize = 128;
        cipher.BlockSize = 128;
        var key = new Rfc2898DeriveBytes(_password, salt, 1000);
        cipher.Key = key.GetBytes(cipher.KeySize / 8);
        cipher.IV = key.GetBytes(cipher.BlockSize / 8);
        using (var ms = new MemoryStream())
        {
          using (var cs = new CryptoStream(ms, cipher.CreateEncryptor(), CryptoStreamMode.Write))
          {
            byte[] dataBytes = Encoding.UTF8.GetBytes(data);
            cs.Write(dataBytes, 0, dataBytes.Length);
          }
          byte[] encrypted = ms.ToArray();
          return Convert.ToBase64String(encrypted);
        }
      }
    }

    public static string DecryptASE(string encryptedData)
    {
      byte[] salt = Encoding.UTF8.GetBytes(_salt);
      using (RijndaelManaged cipher = new RijndaelManaged())
      {
        cipher.KeySize = 128;
        cipher.BlockSize = 128;
        var key = new Rfc2898DeriveBytes(_password, salt, 1000);
        cipher.Key = key.GetBytes(cipher.KeySize / 8);
        cipher.IV = key.GetBytes(cipher.BlockSize / 8);
        using (var ms = new MemoryStream())
        {
          using (var cs = new CryptoStream(ms, cipher.CreateDecryptor(), CryptoStreamMode.Write))
          {
            byte[] encryptedDataBytes = Convert.FromBase64String(encryptedData);
            cs.Write(encryptedDataBytes, 0, encryptedDataBytes.Length);
          }
          byte[] decrypted = ms.ToArray();
          return Encoding.UTF8.GetString(decrypted);
        }
      }
    }


    public byte[] Decrypt(byte[] encryptedData)
    {
      // Create a crypto stream to decrypt the data
      using (ICryptoTransform decryptor = algorithm.CreateDecryptor())
      {
        using (MemoryStream memoryStream = new MemoryStream(encryptedData))
        {
          using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
          {
            byte[] buffer = new byte[encryptedData.Length];
            int bytesRead = cryptoStream.Read(buffer, 0, buffer.Length);
            byte[] result = new byte[bytesRead];
            Buffer.BlockCopy(buffer, 0, result, 0, bytesRead);
            return result;
          }
        }
      }
    }

    public byte[] Encrypt(byte[] data)
    {
      // Create a crypto stream to encrypt the data
      using (ICryptoTransform encryptor = algorithm.CreateEncryptor())
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
          {
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
          }
        }
      }
    }

    static byte[] Encrypt(byte[] data, RijndaelManaged algorithm)
    {
      // Create a crypto stream to encrypt the data
      using (ICryptoTransform encryptor = algorithm.CreateEncryptor())
      {
        using (MemoryStream memoryStream = new MemoryStream())
        {
          using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
          {
            cryptoStream.Write(data, 0, data.Length);
            cryptoStream.FlushFinalBlock();
            return memoryStream.ToArray();
          }
        }
      }
    }

    static byte[] Decrypt(byte[] data, RijndaelManaged algorithm)
    {
      // Create a crypto stream to decrypt the data
      using (ICryptoTransform decryptor = algorithm.CreateDecryptor())
      {
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
          using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
          {
            byte[] buffer = new byte[data.Length];
            int bytesRead = cryptoStream.Read(buffer, 0, buffer.Length);
            byte[] result = new byte[bytesRead];
            Buffer.BlockCopy(buffer, 0, result, 0, bytesRead);
            return result;
          }
        }
      }
    }
  }
}
