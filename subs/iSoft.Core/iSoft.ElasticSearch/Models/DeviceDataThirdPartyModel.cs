//using iSoft.Common.Utils;
//using System;

//namespace iSoft.ElasticSearch.Models
//{
//  public class DeviceDataThirdPartyModel : BaseDeviceDataModel
//  {
//    public DateTime Timestamp { get; set; }
//    public override long ConnectionId { get; set; }
//    public override DateTime ExecuteAt { get; set; }
//    public override String MessageId { get; set; }

//    public double? ADS_Local__current_OR_value { get; set; }
//    public double? ADS_Local__current_OLE_value { get; set; }
//    public double? ADS_Local_Setting_UCValue { get; set; }
//    public double? ADS_Local_Setting_UAValue { get; set; }

//    public DeviceDataThirdPartyModel(long id,
//                                  object?[] arrValue,
//                                  DateTime executeAt,
//                                  string messageId)
//    {
//      this.Timestamp = DateTime.Now;
//      this.ConnectionId = id;
//      this.ExecuteAt = executeAt;
//      this.MessageId = messageId;
//      this.ADS_Local__current_OR_value = ConvertUtil.ConvertToNullableDouble(arrValue[0]);
//      this.ADS_Local__current_OLE_value = ConvertUtil.ConvertToNullableDouble(arrValue[1]);
//      this.ADS_Local_Setting_UCValue = ConvertUtil.ConvertToNullableDouble(arrValue[2]);
//      this.ADS_Local_Setting_UAValue = ConvertUtil.ConvertToNullableDouble(arrValue[3]);

//    }

//    public override bool Equals(object? obj)
//    {
//      return obj is DeviceDataThirdPartyModel model &&
//             ADS_Local__current_OR_value == model.ADS_Local__current_OR_value &&
//             ADS_Local__current_OLE_value == model.ADS_Local__current_OLE_value &&
//             ADS_Local_Setting_UCValue == model.ADS_Local_Setting_UCValue &&
//             ADS_Local_Setting_UAValue == model.ADS_Local_Setting_UAValue;
//    }
//  }
//}
