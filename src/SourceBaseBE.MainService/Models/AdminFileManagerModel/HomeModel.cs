namespace SourceBaseBE.MainService.Models;

public class HomeModel
{
  public string OEEUrlDefault = "";
  public int DisplayTimeDefault = 3000;
  public string LinkGetData = "";
  public int IntervalGetData = 3000;

  public HomeModel(string oEEUrlDefault, int displayTimeDefault, string linkGetData)
  {
    this.OEEUrlDefault = oEEUrlDefault;
    this.DisplayTimeDefault = displayTimeDefault;
    this.LinkGetData = linkGetData;
    this.IntervalGetData = 3000;
  }
}