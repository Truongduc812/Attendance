namespace SourceBaseBE.Database.Utils
{
  public static class QRCode
  {
    public static string Encode(long Id)
    {
      return Id.ToString();
    }

    public static long Decode(string QrCode)
    {
      return long.Parse(QrCode);
    }
  }
}
