namespace SourceBaseBE.MainService.Models;

public class LoginRequestModel
{
  public string username { get; set; }
  public string password { get; set; }
  public string customCheck { get; set; }

  public LoginRequestModel() { }
  public LoginRequestModel(string username, string password, string customCheck)
  {
    this.username = username;
    this.password = password;
    this.customCheck = customCheck;
  }
}
