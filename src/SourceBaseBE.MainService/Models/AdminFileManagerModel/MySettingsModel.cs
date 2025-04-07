
namespace SourceBaseBE.MainService.Models;

public class MySettingsModel
{
  public string? Text { get; set; }

  public MySettingsModel()
  {
  }

  public MySettingsModel(string text)
  {
    Text = text;
  }
}