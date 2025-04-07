using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using iSoft.Firebase.Entities;
using iSoft.DBLibrary.SQLBuilder;
using Microsoft.Extensions.Logging;
using iSoft.DBLibrary.DBConnections.Factory;
using System.Threading.Tasks;
using iSoft.Common.Enums.DBProvider;
using iSoft.Database.Repository;
using iSoft.Database.Entities;
using iSoft.Database.DBContexts;
using iSoft.Firebase.ExtensionMethods;

using iSoft.Common.ConfigsNS;
using iSoft.Firebase.Models;
using iSoft.Common.Utils;

namespace iSoft.Firebase.Services
{
  public class FirebaseService: BaseCRUDService<FCMEntity>
  {
    private ILogger<FirebaseService> _logger;
    private CommonDBContext _dbContext;
    public FCMRepository _repositoryImp;

    public FirebaseService(ILoggerFactory loggerFactory)
    {
      this._logger = loggerFactory.CreateLogger<FirebaseService>();

      this._dbContext = new CommonDBContext(DBConnectionFactory.CreateDBConnection(
        CommonConfig.GetConfig().MasterDatabaseConfig));

      _repository = new FCMRepository(this._dbContext, CommonConfig.GetConfig().RedisConfig, loggerFactory);
      _repositoryImp = (FCMRepository)_repository;
      //fcmRepo = (FCMRepository)this._repository;
    }

    public FCMEntity GetByUserId(long userId)
    {
      return _repositoryImp.GetByUserId(userId);
    }
    public List<FCMEntity> GetByListUserId(List<long> listUserId)
    {
      return _repositoryImp.GetByListUserIds(listUserId);
    }
    public FCMEntity Upsert(FCMEntity entity, long? userId = null)
    {
      return _repository.Upsert(entity, userId);
    }
    public int Delete(FCMEntity entity, long? userId = null)
    {
      return _repository.Delete(entity, userId);
    }
  }
}