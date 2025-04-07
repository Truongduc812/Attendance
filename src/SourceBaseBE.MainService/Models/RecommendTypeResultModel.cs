using SourceBaseBE.Database.Entities;
using SourceBaseBE.Database.EnumProject;
using SourceBaseBE.Database.Enums;
using static iSoft.Common.ConstCommon;
using System.Collections.Generic;
using System;

namespace SourceBaseBE.Database.Models.RequestModels
{
    public class RecommendTypeResultModel
    {
        public WorkingTypeEntity WorkingType { get; set; }
        public string RecommendTypeStr { get; set; }
        public long? TimeDeviatioinInSeconds { get; set; }
        public int Over4HHours { get; set; } = 0;
        public int OTHours { get; set; } = 0;
        public EnumOTHourType OTHourType { get; set; } = EnumOTHourType.None;
        public EnumWorkingDayHighlight workingDayHighlight { get; set; }

        public RecommendTypeResultModel(
            string recommendTypeStr,
            long? timeDeviatioinInSeconds,
            EnumWorkingDayHighlight workingDayHighlight = EnumWorkingDayHighlight.None)
        {
            this.RecommendTypeStr = recommendTypeStr;
            this.TimeDeviatioinInSeconds = timeDeviatioinInSeconds;
            this.workingDayHighlight = workingDayHighlight;
        }

        public override string ToString()
        {
            return WorkingType.Code;
        }
    }
}
