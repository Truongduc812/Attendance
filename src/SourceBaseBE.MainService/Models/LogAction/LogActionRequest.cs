using iSoft.Common.Enums;
using iSoft.Common.Models.RequestModels;
using Newtonsoft.Json;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SourceBaseBE.MainService.Models.RequestModels
{
	public class LogActionRequest : PagingFilterRequestModel
	{
        public bool MyProperty { get; set; }
    }
}
