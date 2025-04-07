using iSoft.Common.Models.ConfigModel.Subs;
using System.ComponentModel.DataAnnotations;

namespace iSoft.Common.Models.ConfigModel
{
    public class ApiKeyConfigModel : BaseConfigModel
    {
        public ApiKeyModel[] ApiKeys { get; set; }
    }
}