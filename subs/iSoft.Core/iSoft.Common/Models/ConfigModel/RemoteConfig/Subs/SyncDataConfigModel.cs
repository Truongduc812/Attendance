namespace iSoft.Common.Models.ConfigModel.RemoteConfig.Subs
{
    public class SyncDataConfigModel
    {
        public string Version { get; set; }
        public bool ActiveFlag { get; set; }
        public int IntervalInMiliseconds { get; set; }
        public int StepTimeInSeconds { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public SyncDataConfigModel(string version, bool activeFlag, int intervalInMiliseconds, int stepTimeInSeconds, string startTime, string endTime)
        {
            Version = version;
            ActiveFlag = activeFlag;
            IntervalInMiliseconds = intervalInMiliseconds;
            StepTimeInSeconds = stepTimeInSeconds;
            StartTime = startTime;
            EndTime = endTime;
        }

        public override bool Equals(object? obj)
        {
            return obj != null &&
                   Version == ((SyncDataConfigModel)obj).Version &&
                   ActiveFlag == ((SyncDataConfigModel)obj).ActiveFlag &&
                   IntervalInMiliseconds == ((SyncDataConfigModel)obj).IntervalInMiliseconds &&
                   StepTimeInSeconds == ((SyncDataConfigModel)obj).StepTimeInSeconds &&
                   StartTime == ((SyncDataConfigModel)obj).StartTime &&
                   EndTime == ((SyncDataConfigModel)obj).EndTime;
        }

        public object GetLogStr()
        {
            return $"Version: {Version}, ActiveFlag: {ActiveFlag}, IntervalInMiliseconds: {IntervalInMiliseconds}, StepTimeInSeconds: {StepTimeInSeconds}, StartTime: {StartTime}, EndTime: {EndTime}";
        }
    }
}