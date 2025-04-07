using iSoft.Database.Entities;
using System.ComponentModel.DataAnnotations;

namespace iSoft.Firebase.Models
{
    public class UserAuthModel:UserEntity
  {
        public string? AccessToken { get; set; }
    }
}