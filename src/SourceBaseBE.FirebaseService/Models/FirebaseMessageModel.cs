using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace iSoft.Firebase.Models
{
  public class FirebaseMessageModel
  {
    public List<long> ListUserId { get; set; }
    public string Tag { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public string ClickAction { get; set; }
    public string Icon { get; set; }
    public List<int> Vibrate { get; set; }
  }
}