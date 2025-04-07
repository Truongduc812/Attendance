//using iSoft.Common.Utils;
//using System;

//namespace iSoft.ElasticSearch.Models
//{
//  public class DeviceDataSendVarDateTimeModel : BaseDeviceDataModel
//  {
//    public DateTime Timestamp { get; set; }
//    public override long ConnectionId { get; set; }
//    public override DateTime ExecuteAt { get; set; }
//    public override String MessageId { get; set; }


//    public long? ADS_Local_iSendVarDateTime_0_ { get; set; }
//    public long? ADS_Local_iSendVarDateTime_1_ { get; set; }
//    public long? ADS_Local_iSendVarDateTime_2_ { get; set; }
//    public long? ADS_Local_iSendVarDateTime_3_ { get; set; }
//    public long? ADS_Local_iSendVarDateTime_4_ { get; set; }
//    public long? ADS_Local_iSendVarDateTime_5_ { get; set; }
//    public long? ADS_Local_iSendVarDateTime_6_ { get; set; }
//    public long? ADS_Local_iSendVarDateTime_7_ { get; set; }
//    public long? ADS_Local_iSendVarDateTime_8_ { get; set; }
//    public long? ADS_Local_iSendVarDateTime_9_ { get; set; }

//    public DeviceDataSendVarDateTimeModel(long id,
//                                  object?[] arrValue,
//                                  DateTime executeAt,
//                                  string messageId)
//    {
//      this.Timestamp = DateTime.Now;
//      this.ConnectionId = id;
//      this.ExecuteAt = executeAt;
//      this.MessageId = messageId;
//      this.ADS_Local_iSendVarDateTime_0_ = ConvertUtil.ConvertToNullableLong(arrValue[0]);
//      this.ADS_Local_iSendVarDateTime_1_ = ConvertUtil.ConvertToNullableLong(arrValue[1]);
//      this.ADS_Local_iSendVarDateTime_2_ = ConvertUtil.ConvertToNullableLong(arrValue[2]);
//      this.ADS_Local_iSendVarDateTime_3_ = ConvertUtil.ConvertToNullableLong(arrValue[3]);
//      this.ADS_Local_iSendVarDateTime_4_ = ConvertUtil.ConvertToNullableLong(arrValue[4]);
//      this.ADS_Local_iSendVarDateTime_5_ = ConvertUtil.ConvertToNullableLong(arrValue[5]);
//      this.ADS_Local_iSendVarDateTime_6_ = ConvertUtil.ConvertToNullableLong(arrValue[6]);
//      this.ADS_Local_iSendVarDateTime_7_ = ConvertUtil.ConvertToNullableLong(arrValue[7]);
//      this.ADS_Local_iSendVarDateTime_8_ = ConvertUtil.ConvertToNullableLong(arrValue[8]);
//      this.ADS_Local_iSendVarDateTime_9_ = ConvertUtil.ConvertToNullableLong(arrValue[9]);
//    }

//    public override bool Equals(object? obj)
//    {
//      return obj is DeviceDataSendVarDateTimeModel model &&
//             ADS_Local_iSendVarDateTime_0_ == model.ADS_Local_iSendVarDateTime_0_ &&
//             ADS_Local_iSendVarDateTime_1_ == model.ADS_Local_iSendVarDateTime_1_ &&
//             ADS_Local_iSendVarDateTime_2_ == model.ADS_Local_iSendVarDateTime_2_ &&
//             ADS_Local_iSendVarDateTime_3_ == model.ADS_Local_iSendVarDateTime_3_ &&
//             ADS_Local_iSendVarDateTime_4_ == model.ADS_Local_iSendVarDateTime_4_ &&
//             ADS_Local_iSendVarDateTime_5_ == model.ADS_Local_iSendVarDateTime_5_ &&
//             ADS_Local_iSendVarDateTime_6_ == model.ADS_Local_iSendVarDateTime_6_ &&
//             ADS_Local_iSendVarDateTime_7_ == model.ADS_Local_iSendVarDateTime_7_ &&
//             ADS_Local_iSendVarDateTime_8_ == model.ADS_Local_iSendVarDateTime_8_ &&
//             ADS_Local_iSendVarDateTime_9_ == model.ADS_Local_iSendVarDateTime_9_;
//    }
//  }
//}
