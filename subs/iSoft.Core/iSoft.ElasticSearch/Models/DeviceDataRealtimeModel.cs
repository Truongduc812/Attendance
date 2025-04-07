//using iSoft.Common.Utils;
//using System;

//namespace iSoft.ElasticSearch.Models
//{
//  public class DeviceDataRealtimeModel
//  {
//    public DateTime Timestamp { get; set; }
//    public long ConnectionId { get; set; }
//    public DateTime ExecuteAt { get; set; }
//    public String MessageId { get; set; }

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
//    public short? ADS_Local_iSendVarPoint_SEM_0_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_1_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_2_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_3_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_4_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_5_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_6_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_7_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_8_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_9_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_10_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_11_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_12_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_13_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_14_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_15_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_16_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_17_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_18_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_19_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_20_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_21_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_22_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_23_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_24_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_25_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_26_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_27_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_28_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_29_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_30_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_31_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_32_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_33_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_34_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_35_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_36_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_37_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_38_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_39_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_40_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_41_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_42_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_43_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_44_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_45_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_46_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_47_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_48_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SEM_49_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_0_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_1_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_2_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_3_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_4_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_5_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_6_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_7_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_8_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_9_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_10_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_11_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_12_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_13_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_14_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_15_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_16_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_17_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_18_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_19_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_20_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_21_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_22_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_23_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_24_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_25_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_26_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_27_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_28_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_29_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_30_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_31_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_32_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_33_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_34_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_35_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_36_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_37_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_38_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_39_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_40_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_41_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_42_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_43_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_44_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_45_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_46_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_47_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_48_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_49_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_50_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_51_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_52_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_53_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_54_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_55_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_56_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_57_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_58_ { get; set; }
//    public short? ADS_Local_iSendVarPoint_SIL_59_ { get; set; }

//    public DeviceDataRealtimeModel(long id,
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
//      this.ADS_Local_iSendVarStatus_0_ = ConvertUtil.ConvertToNullableShort(arrValue[10]);
//      this.ADS_Local_iSendVarStatus_1_ = ConvertUtil.ConvertToNullableShort(arrValue[11]);
//      this.ADS_Local_iSendVarStatus_2_ = ConvertUtil.ConvertToNullableShort(arrValue[12]);
//      this.ADS_Local_iSendVarStatus_3_ = ConvertUtil.ConvertToNullableShort(arrValue[13]);
//      this.ADS_Local_iSendVarStatus_4_ = ConvertUtil.ConvertToNullableShort(arrValue[14]);
//      this.ADS_Local_iSendVarStatus_5_ = ConvertUtil.ConvertToNullableShort(arrValue[15]);
//      this.ADS_Local_iSendVarStatus_6_ = ConvertUtil.ConvertToNullableShort(arrValue[16]);
//      this.ADS_Local_iSendVarStatus_7_ = ConvertUtil.ConvertToNullableShort(arrValue[17]);
//      this.ADS_Local_iSendVarStatus_8_ = ConvertUtil.ConvertToNullableShort(arrValue[18]);
//      this.ADS_Local_iSendVarStatus_9_ = ConvertUtil.ConvertToNullableShort(arrValue[19]);
//      this.ADS_Local_iSendVarStatus_10_ = ConvertUtil.ConvertToNullableShort(arrValue[20]);
//      this.ADS_Local_iSendVarStatus_11_ = ConvertUtil.ConvertToNullableShort(arrValue[21]);
//      this.ADS_Local_iSendVarStatus_12_ = ConvertUtil.ConvertToNullableShort(arrValue[22]);
//      this.ADS_Local_iSendVarStatus_13_ = ConvertUtil.ConvertToNullableShort(arrValue[23]);
//      this.ADS_Local_iSendVarStatus_14_ = ConvertUtil.ConvertToNullableShort(arrValue[24]);
//      this.ADS_Local_iSendVarStatus_15_ = ConvertUtil.ConvertToNullableShort(arrValue[25]);
//      this.ADS_Local_iSendVarStatus_16_ = ConvertUtil.ConvertToNullableShort(arrValue[26]);
//      this.ADS_Local_iSendVarStatus_17_ = ConvertUtil.ConvertToNullableShort(arrValue[27]);
//      this.ADS_Local_iSendVarStatus_18_ = ConvertUtil.ConvertToNullableShort(arrValue[28]);
//      this.ADS_Local_iSendVarStatus_19_ = ConvertUtil.ConvertToNullableShort(arrValue[29]);
//      this.ADS_Local_iSendVarStatus_20_ = ConvertUtil.ConvertToNullableShort(arrValue[30]);
//      this.ADS_Local_iSendVarStatus_21_ = ConvertUtil.ConvertToNullableShort(arrValue[31]);
//      this.ADS_Local_iSendVarStatus_22_ = ConvertUtil.ConvertToNullableShort(arrValue[32]);
//      this.ADS_Local_iSendVarStatus_23_ = ConvertUtil.ConvertToNullableShort(arrValue[33]);
//      this.ADS_Local_iSendVarStatus_24_ = ConvertUtil.ConvertToNullableShort(arrValue[34]);
//      this.ADS_Local_iSendVarStatus_25_ = ConvertUtil.ConvertToNullableShort(arrValue[35]);
//      this.ADS_Local_iSendVarStatus_26_ = ConvertUtil.ConvertToNullableShort(arrValue[36]);
//      this.ADS_Local_iSendVarStatus_27_ = ConvertUtil.ConvertToNullableShort(arrValue[37]);
//      this.ADS_Local_iSendVarStatus_28_ = ConvertUtil.ConvertToNullableShort(arrValue[38]);
//      this.ADS_Local_iSendVarStatus_29_ = ConvertUtil.ConvertToNullableShort(arrValue[39]);
//      this.ADS_Local_iSendVarCount_0_ = ConvertUtil.ConvertToNullableLong(arrValue[40]);
//      this.ADS_Local_iSendVarCount_1_ = ConvertUtil.ConvertToNullableLong(arrValue[41]);
//      this.ADS_Local_iSendVarCount_2_ = ConvertUtil.ConvertToNullableLong(arrValue[42]);
//      this.ADS_Local_iSendVarCount_3_ = ConvertUtil.ConvertToNullableLong(arrValue[43]);
//      this.ADS_Local_iSendVarCount_4_ = ConvertUtil.ConvertToNullableLong(arrValue[44]);
//      this.ADS_Local_iSendVarCount_5_ = ConvertUtil.ConvertToNullableLong(arrValue[45]);
//      this.ADS_Local_iSendVarCount_6_ = ConvertUtil.ConvertToNullableLong(arrValue[46]);
//      this.ADS_Local_iSendVarCount_7_ = ConvertUtil.ConvertToNullableLong(arrValue[47]);
//      this.ADS_Local_iSendVarCount_8_ = ConvertUtil.ConvertToNullableLong(arrValue[48]);
//      this.ADS_Local_iSendVarCount_9_ = ConvertUtil.ConvertToNullableLong(arrValue[49]);
//      this.ADS_Local_iSendVarCount_10_ = ConvertUtil.ConvertToNullableLong(arrValue[50]);
//      this.ADS_Local_iSendVarCount_11_ = ConvertUtil.ConvertToNullableLong(arrValue[51]);
//      this.ADS_Local_iSendVarCount_12_ = ConvertUtil.ConvertToNullableLong(arrValue[52]);
//      this.ADS_Local_iSendVarCount_13_ = ConvertUtil.ConvertToNullableLong(arrValue[53]);
//      this.ADS_Local_iSendVarCount_14_ = ConvertUtil.ConvertToNullableLong(arrValue[54]);
//      this.ADS_Local_iSendVarCount_15_ = ConvertUtil.ConvertToNullableLong(arrValue[55]);
//      this.ADS_Local_iSendVarCount_16_ = ConvertUtil.ConvertToNullableLong(arrValue[56]);
//      this.ADS_Local_iSendVarCount_17_ = ConvertUtil.ConvertToNullableLong(arrValue[57]);
//      this.ADS_Local_iSendVarCount_18_ = ConvertUtil.ConvertToNullableLong(arrValue[58]);
//      this.ADS_Local_iSendVarCount_19_ = ConvertUtil.ConvertToNullableLong(arrValue[59]);
//      this.ADS_Local_iSendVarCount_20_ = ConvertUtil.ConvertToNullableLong(arrValue[60]);
//      this.ADS_Local_iSendVarCount_21_ = ConvertUtil.ConvertToNullableLong(arrValue[61]);
//      this.ADS_Local_iSendVarCount_22_ = ConvertUtil.ConvertToNullableLong(arrValue[62]);
//      this.ADS_Local_iSendVarCount_23_ = ConvertUtil.ConvertToNullableLong(arrValue[63]);
//      this.ADS_Local_iSendVarCount_24_ = ConvertUtil.ConvertToNullableLong(arrValue[64]);
//      this.ADS_Local_iSendVarCount_25_ = ConvertUtil.ConvertToNullableLong(arrValue[65]);
//      this.ADS_Local_iSendVarCount_26_ = ConvertUtil.ConvertToNullableLong(arrValue[66]);
//      this.ADS_Local_iSendVarCount_27_ = ConvertUtil.ConvertToNullableLong(arrValue[67]);
//      this.ADS_Local_iSendVarCount_28_ = ConvertUtil.ConvertToNullableLong(arrValue[68]);
//      this.ADS_Local_iSendVarCount_29_ = ConvertUtil.ConvertToNullableLong(arrValue[69]);
//      this.ADS_Local_iSendVarCount_30_ = ConvertUtil.ConvertToNullableLong(arrValue[70]);
//      this.ADS_Local_iSendVarCount_31_ = ConvertUtil.ConvertToNullableLong(arrValue[71]);
//      this.ADS_Local_iSendVarCount_32_ = ConvertUtil.ConvertToNullableLong(arrValue[72]);
//      this.ADS_Local_iSendVarCount_33_ = ConvertUtil.ConvertToNullableLong(arrValue[73]);
//      this.ADS_Local_iSendVarCount_34_ = ConvertUtil.ConvertToNullableLong(arrValue[74]);
//      this.ADS_Local_iSendVarCount_35_ = ConvertUtil.ConvertToNullableLong(arrValue[75]);
//      this.ADS_Local_iSendVarCount_36_ = ConvertUtil.ConvertToNullableLong(arrValue[76]);
//      this.ADS_Local_iSendVarCount_37_ = ConvertUtil.ConvertToNullableLong(arrValue[77]);
//      this.ADS_Local_iSendVarCount_38_ = ConvertUtil.ConvertToNullableLong(arrValue[78]);
//      this.ADS_Local_iSendVarCount_39_ = ConvertUtil.ConvertToNullableLong(arrValue[79]);
//      this.ADS_Local_iSendVarPoint_SEM_0_ = ConvertUtil.ConvertToNullableShort(arrValue[80]);
//      this.ADS_Local_iSendVarPoint_SEM_1_ = ConvertUtil.ConvertToNullableShort(arrValue[81]);
//      this.ADS_Local_iSendVarPoint_SEM_2_ = ConvertUtil.ConvertToNullableShort(arrValue[82]);
//      this.ADS_Local_iSendVarPoint_SEM_3_ = ConvertUtil.ConvertToNullableShort(arrValue[83]);
//      this.ADS_Local_iSendVarPoint_SEM_4_ = ConvertUtil.ConvertToNullableShort(arrValue[84]);
//      this.ADS_Local_iSendVarPoint_SEM_5_ = ConvertUtil.ConvertToNullableShort(arrValue[85]);
//      this.ADS_Local_iSendVarPoint_SEM_6_ = ConvertUtil.ConvertToNullableShort(arrValue[86]);
//      this.ADS_Local_iSendVarPoint_SEM_7_ = ConvertUtil.ConvertToNullableShort(arrValue[87]);
//      this.ADS_Local_iSendVarPoint_SEM_8_ = ConvertUtil.ConvertToNullableShort(arrValue[88]);
//      this.ADS_Local_iSendVarPoint_SEM_9_ = ConvertUtil.ConvertToNullableShort(arrValue[89]);
//      this.ADS_Local_iSendVarPoint_SEM_10_ = ConvertUtil.ConvertToNullableShort(arrValue[90]);
//      this.ADS_Local_iSendVarPoint_SEM_11_ = ConvertUtil.ConvertToNullableShort(arrValue[91]);
//      this.ADS_Local_iSendVarPoint_SEM_12_ = ConvertUtil.ConvertToNullableShort(arrValue[92]);
//      this.ADS_Local_iSendVarPoint_SEM_13_ = ConvertUtil.ConvertToNullableShort(arrValue[93]);
//      this.ADS_Local_iSendVarPoint_SEM_14_ = ConvertUtil.ConvertToNullableShort(arrValue[94]);
//      this.ADS_Local_iSendVarPoint_SEM_15_ = ConvertUtil.ConvertToNullableShort(arrValue[95]);
//      this.ADS_Local_iSendVarPoint_SEM_16_ = ConvertUtil.ConvertToNullableShort(arrValue[96]);
//      this.ADS_Local_iSendVarPoint_SEM_17_ = ConvertUtil.ConvertToNullableShort(arrValue[97]);
//      this.ADS_Local_iSendVarPoint_SEM_18_ = ConvertUtil.ConvertToNullableShort(arrValue[98]);
//      this.ADS_Local_iSendVarPoint_SEM_19_ = ConvertUtil.ConvertToNullableShort(arrValue[99]);
//      this.ADS_Local_iSendVarPoint_SEM_20_ = ConvertUtil.ConvertToNullableShort(arrValue[100]);
//      this.ADS_Local_iSendVarPoint_SEM_21_ = ConvertUtil.ConvertToNullableShort(arrValue[101]);
//      this.ADS_Local_iSendVarPoint_SEM_22_ = ConvertUtil.ConvertToNullableShort(arrValue[102]);
//      this.ADS_Local_iSendVarPoint_SEM_23_ = ConvertUtil.ConvertToNullableShort(arrValue[103]);
//      this.ADS_Local_iSendVarPoint_SEM_24_ = ConvertUtil.ConvertToNullableShort(arrValue[104]);
//      this.ADS_Local_iSendVarPoint_SEM_25_ = ConvertUtil.ConvertToNullableShort(arrValue[105]);
//      this.ADS_Local_iSendVarPoint_SEM_26_ = ConvertUtil.ConvertToNullableShort(arrValue[106]);
//      this.ADS_Local_iSendVarPoint_SEM_27_ = ConvertUtil.ConvertToNullableShort(arrValue[107]);
//      this.ADS_Local_iSendVarPoint_SEM_28_ = ConvertUtil.ConvertToNullableShort(arrValue[108]);
//      this.ADS_Local_iSendVarPoint_SEM_29_ = ConvertUtil.ConvertToNullableShort(arrValue[109]);
//      this.ADS_Local_iSendVarPoint_SEM_30_ = ConvertUtil.ConvertToNullableShort(arrValue[110]);
//      this.ADS_Local_iSendVarPoint_SEM_31_ = ConvertUtil.ConvertToNullableShort(arrValue[111]);
//      this.ADS_Local_iSendVarPoint_SEM_32_ = ConvertUtil.ConvertToNullableShort(arrValue[112]);
//      this.ADS_Local_iSendVarPoint_SEM_33_ = ConvertUtil.ConvertToNullableShort(arrValue[113]);
//      this.ADS_Local_iSendVarPoint_SEM_34_ = ConvertUtil.ConvertToNullableShort(arrValue[114]);
//      this.ADS_Local_iSendVarPoint_SEM_35_ = ConvertUtil.ConvertToNullableShort(arrValue[115]);
//      this.ADS_Local_iSendVarPoint_SEM_36_ = ConvertUtil.ConvertToNullableShort(arrValue[116]);
//      this.ADS_Local_iSendVarPoint_SEM_37_ = ConvertUtil.ConvertToNullableShort(arrValue[117]);
//      this.ADS_Local_iSendVarPoint_SEM_38_ = ConvertUtil.ConvertToNullableShort(arrValue[118]);
//      this.ADS_Local_iSendVarPoint_SEM_39_ = ConvertUtil.ConvertToNullableShort(arrValue[119]);
//      this.ADS_Local_iSendVarPoint_SEM_40_ = ConvertUtil.ConvertToNullableShort(arrValue[120]);
//      this.ADS_Local_iSendVarPoint_SEM_41_ = ConvertUtil.ConvertToNullableShort(arrValue[121]);
//      this.ADS_Local_iSendVarPoint_SEM_42_ = ConvertUtil.ConvertToNullableShort(arrValue[122]);
//      this.ADS_Local_iSendVarPoint_SEM_43_ = ConvertUtil.ConvertToNullableShort(arrValue[123]);
//      this.ADS_Local_iSendVarPoint_SEM_44_ = ConvertUtil.ConvertToNullableShort(arrValue[124]);
//      this.ADS_Local_iSendVarPoint_SEM_45_ = ConvertUtil.ConvertToNullableShort(arrValue[125]);
//      this.ADS_Local_iSendVarPoint_SEM_46_ = ConvertUtil.ConvertToNullableShort(arrValue[126]);
//      this.ADS_Local_iSendVarPoint_SEM_47_ = ConvertUtil.ConvertToNullableShort(arrValue[127]);
//      this.ADS_Local_iSendVarPoint_SEM_48_ = ConvertUtil.ConvertToNullableShort(arrValue[128]);
//      this.ADS_Local_iSendVarPoint_SEM_49_ = ConvertUtil.ConvertToNullableShort(arrValue[129]);
//      this.ADS_Local_iSendVarPoint_SIL_0_ = ConvertUtil.ConvertToNullableShort(arrValue[130]);
//      this.ADS_Local_iSendVarPoint_SIL_1_ = ConvertUtil.ConvertToNullableShort(arrValue[131]);
//      this.ADS_Local_iSendVarPoint_SIL_2_ = ConvertUtil.ConvertToNullableShort(arrValue[132]);
//      this.ADS_Local_iSendVarPoint_SIL_3_ = ConvertUtil.ConvertToNullableShort(arrValue[133]);
//      this.ADS_Local_iSendVarPoint_SIL_4_ = ConvertUtil.ConvertToNullableShort(arrValue[134]);
//      this.ADS_Local_iSendVarPoint_SIL_5_ = ConvertUtil.ConvertToNullableShort(arrValue[135]);
//      this.ADS_Local_iSendVarPoint_SIL_6_ = ConvertUtil.ConvertToNullableShort(arrValue[136]);
//      this.ADS_Local_iSendVarPoint_SIL_7_ = ConvertUtil.ConvertToNullableShort(arrValue[137]);
//      this.ADS_Local_iSendVarPoint_SIL_8_ = ConvertUtil.ConvertToNullableShort(arrValue[138]);
//      this.ADS_Local_iSendVarPoint_SIL_9_ = ConvertUtil.ConvertToNullableShort(arrValue[139]);
//      this.ADS_Local_iSendVarPoint_SIL_10_ = ConvertUtil.ConvertToNullableShort(arrValue[140]);
//      this.ADS_Local_iSendVarPoint_SIL_11_ = ConvertUtil.ConvertToNullableShort(arrValue[141]);
//      this.ADS_Local_iSendVarPoint_SIL_12_ = ConvertUtil.ConvertToNullableShort(arrValue[142]);
//      this.ADS_Local_iSendVarPoint_SIL_13_ = ConvertUtil.ConvertToNullableShort(arrValue[143]);
//      this.ADS_Local_iSendVarPoint_SIL_14_ = ConvertUtil.ConvertToNullableShort(arrValue[144]);
//      this.ADS_Local_iSendVarPoint_SIL_15_ = ConvertUtil.ConvertToNullableShort(arrValue[145]);
//      this.ADS_Local_iSendVarPoint_SIL_16_ = ConvertUtil.ConvertToNullableShort(arrValue[146]);
//      this.ADS_Local_iSendVarPoint_SIL_17_ = ConvertUtil.ConvertToNullableShort(arrValue[147]);
//      this.ADS_Local_iSendVarPoint_SIL_18_ = ConvertUtil.ConvertToNullableShort(arrValue[148]);
//      this.ADS_Local_iSendVarPoint_SIL_19_ = ConvertUtil.ConvertToNullableShort(arrValue[149]);
//      this.ADS_Local_iSendVarPoint_SIL_20_ = ConvertUtil.ConvertToNullableShort(arrValue[150]);
//      this.ADS_Local_iSendVarPoint_SIL_21_ = ConvertUtil.ConvertToNullableShort(arrValue[151]);
//      this.ADS_Local_iSendVarPoint_SIL_22_ = ConvertUtil.ConvertToNullableShort(arrValue[152]);
//      this.ADS_Local_iSendVarPoint_SIL_23_ = ConvertUtil.ConvertToNullableShort(arrValue[153]);
//      this.ADS_Local_iSendVarPoint_SIL_24_ = ConvertUtil.ConvertToNullableShort(arrValue[154]);
//      this.ADS_Local_iSendVarPoint_SIL_25_ = ConvertUtil.ConvertToNullableShort(arrValue[155]);
//      this.ADS_Local_iSendVarPoint_SIL_26_ = ConvertUtil.ConvertToNullableShort(arrValue[156]);
//      this.ADS_Local_iSendVarPoint_SIL_27_ = ConvertUtil.ConvertToNullableShort(arrValue[157]);
//      this.ADS_Local_iSendVarPoint_SIL_28_ = ConvertUtil.ConvertToNullableShort(arrValue[158]);
//      this.ADS_Local_iSendVarPoint_SIL_29_ = ConvertUtil.ConvertToNullableShort(arrValue[159]);
//      this.ADS_Local_iSendVarPoint_SIL_30_ = ConvertUtil.ConvertToNullableShort(arrValue[160]);
//      this.ADS_Local_iSendVarPoint_SIL_31_ = ConvertUtil.ConvertToNullableShort(arrValue[161]);
//      this.ADS_Local_iSendVarPoint_SIL_32_ = ConvertUtil.ConvertToNullableShort(arrValue[162]);
//      this.ADS_Local_iSendVarPoint_SIL_33_ = ConvertUtil.ConvertToNullableShort(arrValue[163]);
//      this.ADS_Local_iSendVarPoint_SIL_34_ = ConvertUtil.ConvertToNullableShort(arrValue[164]);
//      this.ADS_Local_iSendVarPoint_SIL_35_ = ConvertUtil.ConvertToNullableShort(arrValue[165]);
//      this.ADS_Local_iSendVarPoint_SIL_36_ = ConvertUtil.ConvertToNullableShort(arrValue[166]);
//      this.ADS_Local_iSendVarPoint_SIL_37_ = ConvertUtil.ConvertToNullableShort(arrValue[167]);
//      this.ADS_Local_iSendVarPoint_SIL_38_ = ConvertUtil.ConvertToNullableShort(arrValue[168]);
//      this.ADS_Local_iSendVarPoint_SIL_39_ = ConvertUtil.ConvertToNullableShort(arrValue[169]);
//      this.ADS_Local_iSendVarPoint_SIL_40_ = ConvertUtil.ConvertToNullableShort(arrValue[170]);
//      this.ADS_Local_iSendVarPoint_SIL_41_ = ConvertUtil.ConvertToNullableShort(arrValue[171]);
//      this.ADS_Local_iSendVarPoint_SIL_42_ = ConvertUtil.ConvertToNullableShort(arrValue[172]);
//      this.ADS_Local_iSendVarPoint_SIL_43_ = ConvertUtil.ConvertToNullableShort(arrValue[173]);
//      this.ADS_Local_iSendVarPoint_SIL_44_ = ConvertUtil.ConvertToNullableShort(arrValue[174]);
//      this.ADS_Local_iSendVarPoint_SIL_45_ = ConvertUtil.ConvertToNullableShort(arrValue[175]);
//      this.ADS_Local_iSendVarPoint_SIL_46_ = ConvertUtil.ConvertToNullableShort(arrValue[176]);
//      this.ADS_Local_iSendVarPoint_SIL_47_ = ConvertUtil.ConvertToNullableShort(arrValue[177]);
//      this.ADS_Local_iSendVarPoint_SIL_48_ = ConvertUtil.ConvertToNullableShort(arrValue[178]);
//      this.ADS_Local_iSendVarPoint_SIL_49_ = ConvertUtil.ConvertToNullableShort(arrValue[179]);
//      this.ADS_Local_iSendVarPoint_SIL_50_ = ConvertUtil.ConvertToNullableShort(arrValue[180]);
//      this.ADS_Local_iSendVarPoint_SIL_51_ = ConvertUtil.ConvertToNullableShort(arrValue[181]);
//      this.ADS_Local_iSendVarPoint_SIL_52_ = ConvertUtil.ConvertToNullableShort(arrValue[182]);
//      this.ADS_Local_iSendVarPoint_SIL_53_ = ConvertUtil.ConvertToNullableShort(arrValue[183]);
//      this.ADS_Local_iSendVarPoint_SIL_54_ = ConvertUtil.ConvertToNullableShort(arrValue[184]);
//      this.ADS_Local_iSendVarPoint_SIL_55_ = ConvertUtil.ConvertToNullableShort(arrValue[185]);
//      this.ADS_Local_iSendVarPoint_SIL_56_ = ConvertUtil.ConvertToNullableShort(arrValue[186]);
//      this.ADS_Local_iSendVarPoint_SIL_57_ = ConvertUtil.ConvertToNullableShort(arrValue[187]);
//      this.ADS_Local_iSendVarPoint_SIL_58_ = ConvertUtil.ConvertToNullableShort(arrValue[188]);
//      this.ADS_Local_iSendVarPoint_SIL_59_ = ConvertUtil.ConvertToNullableShort(arrValue[189]);



//    }
//  }
//}
