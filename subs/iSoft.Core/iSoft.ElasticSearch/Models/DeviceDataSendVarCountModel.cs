//using iSoft.Common.Utils;
//using System;

//namespace iSoft.ElasticSearch.Models
//{
//  public class DeviceDataSendVarCountModel: BaseDeviceDataModel
//  {
//    public DateTime Timestamp { get; set; }
//    public override long ConnectionId { get; set; }
//    public override DateTime ExecuteAt { get; set; }
//    public override String MessageId { get; set; }

//    public long? ADS_Local_iSendVarCount_0_ { get; set; }
//    public long? ADS_Local_iSendVarCount_1_ { get; set; }
//    public long? ADS_Local_iSendVarCount_2_ { get; set; }
//    public long? ADS_Local_iSendVarCount_3_ { get; set; }
//    public long? ADS_Local_iSendVarCount_4_ { get; set; }
//    public long? ADS_Local_iSendVarCount_5_ { get; set; }
//    public long? ADS_Local_iSendVarCount_6_ { get; set; }
//    public long? ADS_Local_iSendVarCount_7_ { get; set; }
//    public long? ADS_Local_iSendVarCount_8_ { get; set; }
//    public long? ADS_Local_iSendVarCount_9_ { get; set; }
//    public long? ADS_Local_iSendVarCount_10_ { get; set; }
//    public long? ADS_Local_iSendVarCount_11_ { get; set; }
//    public long? ADS_Local_iSendVarCount_12_ { get; set; }
//    public long? ADS_Local_iSendVarCount_13_ { get; set; }
//    public long? ADS_Local_iSendVarCount_14_ { get; set; }
//    public long? ADS_Local_iSendVarCount_15_ { get; set; }
//    public long? ADS_Local_iSendVarCount_16_ { get; set; }
//    public long? ADS_Local_iSendVarCount_17_ { get; set; }
//    public long? ADS_Local_iSendVarCount_18_ { get; set; }
//    public long? ADS_Local_iSendVarCount_19_ { get; set; }
//    public long? ADS_Local_iSendVarCount_20_ { get; set; }
//    public long? ADS_Local_iSendVarCount_21_ { get; set; }
//    public long? ADS_Local_iSendVarCount_22_ { get; set; }
//    public long? ADS_Local_iSendVarCount_23_ { get; set; }
//    public long? ADS_Local_iSendVarCount_24_ { get; set; }
//    public long? ADS_Local_iSendVarCount_25_ { get; set; }
//    public long? ADS_Local_iSendVarCount_26_ { get; set; }
//    public long? ADS_Local_iSendVarCount_27_ { get; set; }
//    public long? ADS_Local_iSendVarCount_28_ { get; set; }
//    public long? ADS_Local_iSendVarCount_29_ { get; set; }
//    public long? ADS_Local_iSendVarCount_30_ { get; set; }
//    public long? ADS_Local_iSendVarCount_31_ { get; set; }
//    public long? ADS_Local_iSendVarCount_32_ { get; set; }
//    public long? ADS_Local_iSendVarCount_33_ { get; set; }
//    public long? ADS_Local_iSendVarCount_34_ { get; set; }
//    public long? ADS_Local_iSendVarCount_35_ { get; set; }
//    public long? ADS_Local_iSendVarCount_36_ { get; set; }
//    public long? ADS_Local_iSendVarCount_37_ { get; set; }
//    public long? ADS_Local_iSendVarCount_38_ { get; set; }
//    public long? ADS_Local_iSendVarCount_39_ { get; set; }

//    public DeviceDataSendVarCountModel(long id,
//                                  object?[] arrValue,
//                                  DateTime executeAt,
//                                  string messageId)
//    {
//      this.Timestamp = DateTime.Now;
//      this.ConnectionId = id;
//      this.ExecuteAt = executeAt;
//      this.MessageId = messageId;
//      this.ADS_Local_iSendVarCount_0_ = ConvertUtil.ConvertToNullableLong(arrValue[0]);
//      this.ADS_Local_iSendVarCount_1_ = ConvertUtil.ConvertToNullableLong(arrValue[1]);
//      this.ADS_Local_iSendVarCount_2_ = ConvertUtil.ConvertToNullableLong(arrValue[2]);
//      this.ADS_Local_iSendVarCount_3_ = ConvertUtil.ConvertToNullableLong(arrValue[3]);
//      this.ADS_Local_iSendVarCount_4_ = ConvertUtil.ConvertToNullableLong(arrValue[4]);
//      this.ADS_Local_iSendVarCount_5_ = ConvertUtil.ConvertToNullableLong(arrValue[5]);
//      this.ADS_Local_iSendVarCount_6_ = ConvertUtil.ConvertToNullableLong(arrValue[6]);
//      this.ADS_Local_iSendVarCount_7_ = ConvertUtil.ConvertToNullableLong(arrValue[7]);
//      this.ADS_Local_iSendVarCount_8_ = ConvertUtil.ConvertToNullableLong(arrValue[8]);
//      this.ADS_Local_iSendVarCount_9_ = ConvertUtil.ConvertToNullableLong(arrValue[9]);
//      this.ADS_Local_iSendVarCount_10_ = ConvertUtil.ConvertToNullableLong(arrValue[10]);
//      this.ADS_Local_iSendVarCount_11_ = ConvertUtil.ConvertToNullableLong(arrValue[11]);
//      this.ADS_Local_iSendVarCount_12_ = ConvertUtil.ConvertToNullableLong(arrValue[12]);
//      this.ADS_Local_iSendVarCount_13_ = ConvertUtil.ConvertToNullableLong(arrValue[13]);
//      this.ADS_Local_iSendVarCount_14_ = ConvertUtil.ConvertToNullableLong(arrValue[14]);
//      this.ADS_Local_iSendVarCount_15_ = ConvertUtil.ConvertToNullableLong(arrValue[15]);
//      this.ADS_Local_iSendVarCount_16_ = ConvertUtil.ConvertToNullableLong(arrValue[16]);
//      this.ADS_Local_iSendVarCount_17_ = ConvertUtil.ConvertToNullableLong(arrValue[17]);
//      this.ADS_Local_iSendVarCount_18_ = ConvertUtil.ConvertToNullableLong(arrValue[18]);
//      this.ADS_Local_iSendVarCount_19_ = ConvertUtil.ConvertToNullableLong(arrValue[19]);
//      this.ADS_Local_iSendVarCount_20_ = ConvertUtil.ConvertToNullableLong(arrValue[20]);
//      this.ADS_Local_iSendVarCount_21_ = ConvertUtil.ConvertToNullableLong(arrValue[21]);
//      this.ADS_Local_iSendVarCount_22_ = ConvertUtil.ConvertToNullableLong(arrValue[22]);
//      this.ADS_Local_iSendVarCount_23_ = ConvertUtil.ConvertToNullableLong(arrValue[23]);
//      this.ADS_Local_iSendVarCount_24_ = ConvertUtil.ConvertToNullableLong(arrValue[24]);
//      this.ADS_Local_iSendVarCount_25_ = ConvertUtil.ConvertToNullableLong(arrValue[25]);
//      this.ADS_Local_iSendVarCount_26_ = ConvertUtil.ConvertToNullableLong(arrValue[26]);
//      this.ADS_Local_iSendVarCount_27_ = ConvertUtil.ConvertToNullableLong(arrValue[27]);
//      this.ADS_Local_iSendVarCount_28_ = ConvertUtil.ConvertToNullableLong(arrValue[28]);
//      this.ADS_Local_iSendVarCount_29_ = ConvertUtil.ConvertToNullableLong(arrValue[29]);
//      this.ADS_Local_iSendVarCount_30_ = ConvertUtil.ConvertToNullableLong(arrValue[30]);
//      this.ADS_Local_iSendVarCount_31_ = ConvertUtil.ConvertToNullableLong(arrValue[31]);
//      this.ADS_Local_iSendVarCount_32_ = ConvertUtil.ConvertToNullableLong(arrValue[32]);
//      this.ADS_Local_iSendVarCount_33_ = ConvertUtil.ConvertToNullableLong(arrValue[33]);
//      this.ADS_Local_iSendVarCount_34_ = ConvertUtil.ConvertToNullableLong(arrValue[34]);
//      this.ADS_Local_iSendVarCount_35_ = ConvertUtil.ConvertToNullableLong(arrValue[35]);
//      this.ADS_Local_iSendVarCount_36_ = ConvertUtil.ConvertToNullableLong(arrValue[36]);
//      this.ADS_Local_iSendVarCount_37_ = ConvertUtil.ConvertToNullableLong(arrValue[37]);
//      this.ADS_Local_iSendVarCount_38_ = ConvertUtil.ConvertToNullableLong(arrValue[38]);
//      this.ADS_Local_iSendVarCount_39_ = ConvertUtil.ConvertToNullableLong(arrValue[39]);
//    }

//    public override bool Equals(object? obj)
//    {
//      return obj is DeviceDataSendVarCountModel model &&
//             ADS_Local_iSendVarCount_0_ == model.ADS_Local_iSendVarCount_0_ &&
//             ADS_Local_iSendVarCount_1_ == model.ADS_Local_iSendVarCount_1_ &&
//             ADS_Local_iSendVarCount_2_ == model.ADS_Local_iSendVarCount_2_ &&
//             ADS_Local_iSendVarCount_3_ == model.ADS_Local_iSendVarCount_3_ &&
//             ADS_Local_iSendVarCount_4_ == model.ADS_Local_iSendVarCount_4_ &&
//             ADS_Local_iSendVarCount_5_ == model.ADS_Local_iSendVarCount_5_ &&
//             ADS_Local_iSendVarCount_6_ == model.ADS_Local_iSendVarCount_6_ &&
//             ADS_Local_iSendVarCount_7_ == model.ADS_Local_iSendVarCount_7_ &&
//             ADS_Local_iSendVarCount_8_ == model.ADS_Local_iSendVarCount_8_ &&
//             ADS_Local_iSendVarCount_9_ == model.ADS_Local_iSendVarCount_9_ &&
//             ADS_Local_iSendVarCount_10_ == model.ADS_Local_iSendVarCount_10_ &&
//             ADS_Local_iSendVarCount_11_ == model.ADS_Local_iSendVarCount_11_ &&
//             ADS_Local_iSendVarCount_12_ == model.ADS_Local_iSendVarCount_12_ &&
//             ADS_Local_iSendVarCount_13_ == model.ADS_Local_iSendVarCount_13_ &&
//             ADS_Local_iSendVarCount_14_ == model.ADS_Local_iSendVarCount_14_ &&
//             ADS_Local_iSendVarCount_15_ == model.ADS_Local_iSendVarCount_15_ &&
//             ADS_Local_iSendVarCount_16_ == model.ADS_Local_iSendVarCount_16_ &&
//             ADS_Local_iSendVarCount_17_ == model.ADS_Local_iSendVarCount_17_ &&
//             ADS_Local_iSendVarCount_18_ == model.ADS_Local_iSendVarCount_18_ &&
//             ADS_Local_iSendVarCount_19_ == model.ADS_Local_iSendVarCount_19_ &&
//             ADS_Local_iSendVarCount_20_ == model.ADS_Local_iSendVarCount_20_ &&
//             ADS_Local_iSendVarCount_21_ == model.ADS_Local_iSendVarCount_21_ &&
//             ADS_Local_iSendVarCount_22_ == model.ADS_Local_iSendVarCount_22_ &&
//             ADS_Local_iSendVarCount_23_ == model.ADS_Local_iSendVarCount_23_ &&
//             ADS_Local_iSendVarCount_24_ == model.ADS_Local_iSendVarCount_24_ &&
//             ADS_Local_iSendVarCount_25_ == model.ADS_Local_iSendVarCount_25_ &&
//             ADS_Local_iSendVarCount_26_ == model.ADS_Local_iSendVarCount_26_ &&
//             ADS_Local_iSendVarCount_27_ == model.ADS_Local_iSendVarCount_27_ &&
//             ADS_Local_iSendVarCount_28_ == model.ADS_Local_iSendVarCount_28_ &&
//             ADS_Local_iSendVarCount_29_ == model.ADS_Local_iSendVarCount_29_ &&
//             ADS_Local_iSendVarCount_30_ == model.ADS_Local_iSendVarCount_30_ &&
//             ADS_Local_iSendVarCount_31_ == model.ADS_Local_iSendVarCount_31_ &&
//             ADS_Local_iSendVarCount_32_ == model.ADS_Local_iSendVarCount_32_ &&
//             ADS_Local_iSendVarCount_33_ == model.ADS_Local_iSendVarCount_33_ &&
//             ADS_Local_iSendVarCount_34_ == model.ADS_Local_iSendVarCount_34_ &&
//             ADS_Local_iSendVarCount_35_ == model.ADS_Local_iSendVarCount_35_ &&
//             ADS_Local_iSendVarCount_36_ == model.ADS_Local_iSendVarCount_36_ &&
//             ADS_Local_iSendVarCount_37_ == model.ADS_Local_iSendVarCount_37_ &&
//             ADS_Local_iSendVarCount_38_ == model.ADS_Local_iSendVarCount_38_ &&
//             ADS_Local_iSendVarCount_39_ == model.ADS_Local_iSendVarCount_39_;
//    }
//  }
//}
