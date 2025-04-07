using iSoft.Common.Enums;
using iSoft.Common.Utils;
using iSoft.Database.Entities;
using Microsoft.AspNetCore.Http;
using static iSoft.Common.ConstCommon;

namespace iSoft.Database.Models.RequestModels
{
    public class UserRequestModel : BaseCRUDRequestModel<UserEntity>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? License { get; set; }
        public EnumActiveStatus? Status { get; set; }
        public List<long>? ListISoftProject { get; set; }

        public UserEntity GetEntity(UserRequestModel request)
        {
            var entity = new UserEntity();
            if (this.Id != null) request.Id = (long)Id;
            if (this.Order != null) request.Order = Order;
            if (this.Username != null) request.Username = this.Username;
            if (this.Password != null) request.Password = EncodeUtil.MD5(this.Password);
            if (this.Role != null) request.Role = this.Role;
            if (this.License != null) request.License = this.License;
            if (this.Status != null) request.Status = this.Status;
            if (this.ListISoftProject != null)
            {
                entity.ISoftProjectIds = this.ListISoftProject.Select(x => x).ToList();
            }
            return entity;
        }

    }
}
