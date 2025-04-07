using iSoft.DBLibrary.Entities;
using MathNet.Numerics.Statistics.Mcmc;
using NPOI.SS.Formula.Functions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.Enums;
using SourceBaseBE.Database.Models.RequestModels;

namespace SourceBaseBE.Database.Models.RequestModels.Generate
{
    public class GenerateCRUDEntityRequestModel
    {
        public string EntityName { get; set; }
    }
}
