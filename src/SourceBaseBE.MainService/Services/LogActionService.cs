using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.Common.Enums.DBProvider;
using Serilog;
using iSoft.Common.ConfigsNS;
using SourceBaseBE.Database.Repository;
using SourceBaseBE.Database.DBContexts;
using MathNet.Numerics.Statistics.Mcmc;

using System;
using iSoft.Common.Exceptions;
using System.Data;
using Microsoft.EntityFrameworkCore.Storage;
using iSoft.Common.Models.RequestModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using iSoft.Database.Entities;
using System.Linq;
using SourceBaseBE.MainService.Models;
using SourceBaseBE.Database.Enums;

using iSoft.Database.Extensions;
using iSoft.Common.Models;
using SourceBaseBE.Database.Entities;
using static iSoft.Common.ConstCommon;
using iSoft.Common;
using iSoft.Common.Enums;
using iSoft.Database.Models;
using UserEntity = SourceBaseBE.Database.Entities.UserEntity;
using ISoftProjectEntity = SourceBaseBE.Database.Entities.ISoftProjectEntity;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SourceBaseBE.Database.Models.RequestModels;
using SourceBaseBE.Database.Models.ResponseModels;

namespace SourceBaseBE.MainService.Services
{
	public class LogActionService : BaseCRUDService<WorkingDayUpdateEntity>
	{
		private WorkingDayUpdateRepository workingDayUpdateRepository;
		private WorkingDayApprovalRepository WorkingDayApprovalRepository;
		private ILogger<LogActionService> logger;
		/*[GEN-1]*/

		public LogActionService(CommonDBContext dbContext, ILogger<LogActionService> logger)
		  : base(dbContext, logger)
		{
			workingDayUpdateRepository = new WorkingDayUpdateRepository(_dbContext);
			WorkingDayApprovalRepository = new WorkingDayApprovalRepository(_dbContext);
			/*[GEN-2]*/
		}
		//public Task<LogActionReponse> GetLogActionReponse()
		//{

		//}
	}
}