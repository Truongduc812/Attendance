//using iSoft.Common.Utils;
//using System;

//namespace iSoft.ElasticSearch.Models
//{
//  public class DeviceDataSendVarStatusModel : BaseDeviceDataModel
//  {
//    public DateTime Timestamp { get; set; }
//    public override long ConnectionId { get; set; }
//    public override DateTime ExecuteAt { get; set; }
//    public override String MessageId { get; set; }


//    public short? ADS_Local_iSendVarStatus_0_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_1_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_2_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_3_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_4_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_5_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_6_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_7_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_8_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_9_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_10_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_11_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_12_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_13_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_14_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_15_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_16_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_17_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_18_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_19_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_20_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_21_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_22_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_23_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_24_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_25_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_26_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_27_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_28_ { get; set; }
//    public short? ADS_Local_iSendVarStatus_29_ { get; set; }

//    public DeviceDataSendVarStatusModel(long id,
//                                  object?[] arrValue,
//                                  DateTime executeAt,
//                                  string messageId)
//    {
//      this.Timestamp = DateTime.Now;
//      this.ConnectionId = id;
//      this.ExecuteAt = executeAt;
//      this.MessageId = messageId;
//      this.ADS_Local_iSendVarStatus_0_ = ConvertUtil.ConvertToNullableShort(arrValue[0]);
//      this.ADS_Local_iSendVarStatus_1_ = ConvertUtil.ConvertToNullableShort(arrValue[1]);
//      this.ADS_Local_iSendVarStatus_2_ = ConvertUtil.ConvertToNullableShort(arrValue[2]);
//      this.ADS_Local_iSendVarStatus_3_ = ConvertUtil.ConvertToNullableShort(arrValue[3]);
//      this.ADS_Local_iSendVarStatus_4_ = ConvertUtil.ConvertToNullableShort(arrValue[4]);
//      this.ADS_Local_iSendVarStatus_5_ = ConvertUtil.ConvertToNullableShort(arrValue[5]);
//      this.ADS_Local_iSendVarStatus_6_ = ConvertUtil.ConvertToNullableShort(arrValue[6]);
//      this.ADS_Local_iSendVarStatus_7_ = ConvertUtil.ConvertToNullableShort(arrValue[7]);
//      this.ADS_Local_iSendVarStatus_8_ = ConvertUtil.ConvertToNullableShort(arrValue[8]);
//      this.ADS_Local_iSendVarStatus_9_ = ConvertUtil.ConvertToNullableShort(arrValue[9]);
//      this.ADS_Local_iSendVarStatus_10_ = ConvertUtil.ConvertToNullableShort(arrValue[10]);
//      this.ADS_Local_iSendVarStatus_11_ = ConvertUtil.ConvertToNullableShort(arrValue[11]);
//      this.ADS_Local_iSendVarStatus_12_ = ConvertUtil.ConvertToNullableShort(arrValue[12]);
//      this.ADS_Local_iSendVarStatus_13_ = ConvertUtil.ConvertToNullableShort(arrValue[13]);
//      this.ADS_Local_iSendVarStatus_14_ = ConvertUtil.ConvertToNullableShort(arrValue[14]);
//      this.ADS_Local_iSendVarStatus_15_ = ConvertUtil.ConvertToNullableShort(arrValue[15]);
//      this.ADS_Local_iSendVarStatus_16_ = ConvertUtil.ConvertToNullableShort(arrValue[16]);
//      this.ADS_Local_iSendVarStatus_17_ = ConvertUtil.ConvertToNullableShort(arrValue[17]);
//      this.ADS_Local_iSendVarStatus_18_ = ConvertUtil.ConvertToNullableShort(arrValue[18]);
//      this.ADS_Local_iSendVarStatus_19_ = ConvertUtil.ConvertToNullableShort(arrValue[19]);
//      this.ADS_Local_iSendVarStatus_20_ = ConvertUtil.ConvertToNullableShort(arrValue[20]);
//      this.ADS_Local_iSendVarStatus_21_ = ConvertUtil.ConvertToNullableShort(arrValue[21]);
//      this.ADS_Local_iSendVarStatus_22_ = ConvertUtil.ConvertToNullableShort(arrValue[22]);
//      this.ADS_Local_iSendVarStatus_23_ = ConvertUtil.ConvertToNullableShort(arrValue[23]);
//      this.ADS_Local_iSendVarStatus_24_ = ConvertUtil.ConvertToNullableShort(arrValue[24]);
//      this.ADS_Local_iSendVarStatus_25_ = ConvertUtil.ConvertToNullableShort(arrValue[25]);
//      this.ADS_Local_iSendVarStatus_26_ = ConvertUtil.ConvertToNullableShort(arrValue[26]);
//      this.ADS_Local_iSendVarStatus_27_ = ConvertUtil.ConvertToNullableShort(arrValue[27]);
//      this.ADS_Local_iSendVarStatus_28_ = ConvertUtil.ConvertToNullableShort(arrValue[28]);
//      this.ADS_Local_iSendVarStatus_29_ = ConvertUtil.ConvertToNullableShort(arrValue[29]);
//    }

//    public override bool Equals(object? obj)
//    {
//      return obj is DeviceDataSendVarStatusModel model &&
//             ADS_Local_iSendVarStatus_0_ == model.ADS_Local_iSendVarStatus_0_ &&
//             ADS_Local_iSendVarStatus_1_ == model.ADS_Local_iSendVarStatus_1_ &&
//             ADS_Local_iSendVarStatus_2_ == model.ADS_Local_iSendVarStatus_2_ &&
//             ADS_Local_iSendVarStatus_3_ == model.ADS_Local_iSendVarStatus_3_ &&
//             ADS_Local_iSendVarStatus_4_ == model.ADS_Local_iSendVarStatus_4_ &&
//             ADS_Local_iSendVarStatus_5_ == model.ADS_Local_iSendVarStatus_5_ &&
//             ADS_Local_iSendVarStatus_6_ == model.ADS_Local_iSendVarStatus_6_ &&
//             ADS_Local_iSendVarStatus_7_ == model.ADS_Local_iSendVarStatus_7_ &&
//             ADS_Local_iSendVarStatus_8_ == model.ADS_Local_iSendVarStatus_8_ &&
//             ADS_Local_iSendVarStatus_9_ == model.ADS_Local_iSendVarStatus_9_ &&
//             ADS_Local_iSendVarStatus_10_ == model.ADS_Local_iSendVarStatus_10_ &&
//             ADS_Local_iSendVarStatus_11_ == model.ADS_Local_iSendVarStatus_11_ &&
//             ADS_Local_iSendVarStatus_12_ == model.ADS_Local_iSendVarStatus_12_ &&
//             ADS_Local_iSendVarStatus_13_ == model.ADS_Local_iSendVarStatus_13_ &&
//             ADS_Local_iSendVarStatus_14_ == model.ADS_Local_iSendVarStatus_14_ &&
//             ADS_Local_iSendVarStatus_15_ == model.ADS_Local_iSendVarStatus_15_ &&
//             ADS_Local_iSendVarStatus_16_ == model.ADS_Local_iSendVarStatus_16_ &&
//             ADS_Local_iSendVarStatus_17_ == model.ADS_Local_iSendVarStatus_17_ &&
//             ADS_Local_iSendVarStatus_18_ == model.ADS_Local_iSendVarStatus_18_ &&
//             ADS_Local_iSendVarStatus_19_ == model.ADS_Local_iSendVarStatus_19_ &&
//             ADS_Local_iSendVarStatus_20_ == model.ADS_Local_iSendVarStatus_20_ &&
//             ADS_Local_iSendVarStatus_21_ == model.ADS_Local_iSendVarStatus_21_ &&
//             ADS_Local_iSendVarStatus_22_ == model.ADS_Local_iSendVarStatus_22_ &&
//             ADS_Local_iSendVarStatus_23_ == model.ADS_Local_iSendVarStatus_23_ &&
//             ADS_Local_iSendVarStatus_24_ == model.ADS_Local_iSendVarStatus_24_ &&
//             ADS_Local_iSendVarStatus_25_ == model.ADS_Local_iSendVarStatus_25_ &&
//             ADS_Local_iSendVarStatus_26_ == model.ADS_Local_iSendVarStatus_26_ &&
//             ADS_Local_iSendVarStatus_27_ == model.ADS_Local_iSendVarStatus_27_ &&
//             ADS_Local_iSendVarStatus_28_ == model.ADS_Local_iSendVarStatus_28_ &&
//             ADS_Local_iSendVarStatus_29_ == model.ADS_Local_iSendVarStatus_29_;
//    }
//  }
//}
