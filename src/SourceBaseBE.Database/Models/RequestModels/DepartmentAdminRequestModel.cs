using iSoft.Common.Utils;
using Microsoft.AspNetCore.Http;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common.Enums;
using iSoft.Common.Enums;

namespace SourceBaseBE.Database.Models.RequestModels.Generate
{
    public class DepartmentAdminRequestModel : BaseCRUDRequestModel<DepartmentAdminEntity>
    {
        public long? UserId { get; set; }
        public UserEntity? User { get; set; }
        public long? DepartmentId { get; set; }
        public DepartmentEntity? Department { get; set; }
        public List<int>? Role { get; set; }
        public string? Note { get; set; }
        public IFormFile? FileImport { get; set; }
        public bool? IsReplace { get; set; }

        public override DepartmentAdminEntity GetEntity(DepartmentAdminEntity entity)
        {
            if (Id != null) entity.Id = (long)Id;
            if (Order != null) entity.Order = Order;
            if (DepartmentId != null) entity.DepartmentId = (long)DepartmentId;
            if (UserId != null) entity.UserId = (long)UserId;

            return entity;
        }

        public override Dictionary<string, (string, IFormFile)> GetFiles()
        {
            Dictionary<string, (string, IFormFile)> dicRS = new Dictionary<string, (string, IFormFile)>();
            if (this.FileImport != null)
            {
                dicRS.Add(nameof(FileImport), (Path.Combine(ConstFolderPath.Image, ConstFolderPath.Upload), this.FileImport));
            }
            /*[GEN-17]*/
            return dicRS;
        }
    }
}
