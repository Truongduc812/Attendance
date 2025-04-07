using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iSoft.Firebase.Models
{
  public class RegisterTokenModel
  {
    public long UserId { get; set; }
    public string Token { get; set; }
  }
}