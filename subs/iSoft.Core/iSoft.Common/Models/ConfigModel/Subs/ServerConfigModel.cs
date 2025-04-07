namespace iSoft.Common.Models.ConfigModel.Subs
{
  public class ServerConfigModel
  {
    public string Address { get; set; }
    public int Port { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string GetHostName()
    {
      return $"{Address}:{Port}";
    }

    public ServerConfigModel(string address, int port, string username, string password)
    {
      Address = address;
      Port = port;
      Username = username;
      Password = password;
    }

    public override bool Equals(object? obj)
    {
      return obj != null &&
             Address == ((ServerConfigModel)obj).Address &&
             Port == ((ServerConfigModel)obj).Port &&
             Username == ((ServerConfigModel)obj).Username &&
             Password == ((ServerConfigModel)obj).Password;
    }

    public virtual object GetLogStr()
    {
      return $"Address: {Address}, Port: {Port}, Username: {Username}, Password: {(Password.Length >= 2 ? Password.Substring(0, 2) : "")}*****";
    }

    public override string ToString()
    {
      return "Call ToString() error !!";
    }
  }
}