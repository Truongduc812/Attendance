//using iSoft.Common.Utils;
//using System;

//namespace iSoft.ElasticSearch.Models
//{
//  public class DeviceDataSendVarSafetyPointModel : BaseDeviceDataModel
//  {
//    public DateTime Timestamp { get; set; }
//    public override long ConnectionId { get; set; }
//    public override DateTime ExecuteAt { get; set; }
//    public override String MessageId { get; set; }


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

//    public DeviceDataSendVarSafetyPointModel(long id,
//                                  object?[] arrValue,
//                                  DateTime executeAt,
//                                  string messageId)
//    {
//      this.Timestamp = DateTime.Now;
//      this.ConnectionId = id;
//      this.ExecuteAt = executeAt;
//      this.MessageId = messageId;
//      this.ADS_Local_iSendVarPoint_SEM_0_ = ConvertUtil.ConvertToNullableShort(arrValue[0]);
//      this.ADS_Local_iSendVarPoint_SEM_1_ = ConvertUtil.ConvertToNullableShort(arrValue[1]);
//      this.ADS_Local_iSendVarPoint_SEM_2_ = ConvertUtil.ConvertToNullableShort(arrValue[2]);
//      this.ADS_Local_iSendVarPoint_SEM_3_ = ConvertUtil.ConvertToNullableShort(arrValue[3]);
//      this.ADS_Local_iSendVarPoint_SEM_4_ = ConvertUtil.ConvertToNullableShort(arrValue[4]);
//      this.ADS_Local_iSendVarPoint_SEM_5_ = ConvertUtil.ConvertToNullableShort(arrValue[5]);
//      this.ADS_Local_iSendVarPoint_SEM_6_ = ConvertUtil.ConvertToNullableShort(arrValue[6]);
//      this.ADS_Local_iSendVarPoint_SEM_7_ = ConvertUtil.ConvertToNullableShort(arrValue[7]);
//      this.ADS_Local_iSendVarPoint_SEM_8_ = ConvertUtil.ConvertToNullableShort(arrValue[8]);
//      this.ADS_Local_iSendVarPoint_SEM_9_ = ConvertUtil.ConvertToNullableShort(arrValue[9]);
//      this.ADS_Local_iSendVarPoint_SEM_10_ = ConvertUtil.ConvertToNullableShort(arrValue[10]);
//      this.ADS_Local_iSendVarPoint_SEM_11_ = ConvertUtil.ConvertToNullableShort(arrValue[11]);
//      this.ADS_Local_iSendVarPoint_SEM_12_ = ConvertUtil.ConvertToNullableShort(arrValue[12]);
//      this.ADS_Local_iSendVarPoint_SEM_13_ = ConvertUtil.ConvertToNullableShort(arrValue[13]);
//      this.ADS_Local_iSendVarPoint_SEM_14_ = ConvertUtil.ConvertToNullableShort(arrValue[14]);
//      this.ADS_Local_iSendVarPoint_SEM_15_ = ConvertUtil.ConvertToNullableShort(arrValue[15]);
//      this.ADS_Local_iSendVarPoint_SEM_16_ = ConvertUtil.ConvertToNullableShort(arrValue[16]);
//      this.ADS_Local_iSendVarPoint_SEM_17_ = ConvertUtil.ConvertToNullableShort(arrValue[17]);
//      this.ADS_Local_iSendVarPoint_SEM_18_ = ConvertUtil.ConvertToNullableShort(arrValue[18]);
//      this.ADS_Local_iSendVarPoint_SEM_19_ = ConvertUtil.ConvertToNullableShort(arrValue[19]);
//      this.ADS_Local_iSendVarPoint_SEM_20_ = ConvertUtil.ConvertToNullableShort(arrValue[20]);
//      this.ADS_Local_iSendVarPoint_SEM_21_ = ConvertUtil.ConvertToNullableShort(arrValue[21]);
//      this.ADS_Local_iSendVarPoint_SEM_22_ = ConvertUtil.ConvertToNullableShort(arrValue[22]);
//      this.ADS_Local_iSendVarPoint_SEM_23_ = ConvertUtil.ConvertToNullableShort(arrValue[23]);
//      this.ADS_Local_iSendVarPoint_SEM_24_ = ConvertUtil.ConvertToNullableShort(arrValue[24]);
//      this.ADS_Local_iSendVarPoint_SEM_25_ = ConvertUtil.ConvertToNullableShort(arrValue[25]);
//      this.ADS_Local_iSendVarPoint_SEM_26_ = ConvertUtil.ConvertToNullableShort(arrValue[26]);
//      this.ADS_Local_iSendVarPoint_SEM_27_ = ConvertUtil.ConvertToNullableShort(arrValue[27]);
//      this.ADS_Local_iSendVarPoint_SEM_28_ = ConvertUtil.ConvertToNullableShort(arrValue[28]);
//      this.ADS_Local_iSendVarPoint_SEM_29_ = ConvertUtil.ConvertToNullableShort(arrValue[29]);
//      this.ADS_Local_iSendVarPoint_SEM_30_ = ConvertUtil.ConvertToNullableShort(arrValue[30]);
//      this.ADS_Local_iSendVarPoint_SEM_31_ = ConvertUtil.ConvertToNullableShort(arrValue[31]);
//      this.ADS_Local_iSendVarPoint_SEM_32_ = ConvertUtil.ConvertToNullableShort(arrValue[32]);
//      this.ADS_Local_iSendVarPoint_SEM_33_ = ConvertUtil.ConvertToNullableShort(arrValue[33]);
//      this.ADS_Local_iSendVarPoint_SEM_34_ = ConvertUtil.ConvertToNullableShort(arrValue[34]);
//      this.ADS_Local_iSendVarPoint_SEM_35_ = ConvertUtil.ConvertToNullableShort(arrValue[35]);
//      this.ADS_Local_iSendVarPoint_SEM_36_ = ConvertUtil.ConvertToNullableShort(arrValue[36]);
//      this.ADS_Local_iSendVarPoint_SEM_37_ = ConvertUtil.ConvertToNullableShort(arrValue[37]);
//      this.ADS_Local_iSendVarPoint_SEM_38_ = ConvertUtil.ConvertToNullableShort(arrValue[38]);
//      this.ADS_Local_iSendVarPoint_SEM_39_ = ConvertUtil.ConvertToNullableShort(arrValue[39]);
//      this.ADS_Local_iSendVarPoint_SEM_40_ = ConvertUtil.ConvertToNullableShort(arrValue[40]);
//      this.ADS_Local_iSendVarPoint_SEM_41_ = ConvertUtil.ConvertToNullableShort(arrValue[41]);
//      this.ADS_Local_iSendVarPoint_SEM_42_ = ConvertUtil.ConvertToNullableShort(arrValue[42]);
//      this.ADS_Local_iSendVarPoint_SEM_43_ = ConvertUtil.ConvertToNullableShort(arrValue[43]);
//      this.ADS_Local_iSendVarPoint_SEM_44_ = ConvertUtil.ConvertToNullableShort(arrValue[44]);
//      this.ADS_Local_iSendVarPoint_SEM_45_ = ConvertUtil.ConvertToNullableShort(arrValue[45]);
//      this.ADS_Local_iSendVarPoint_SEM_46_ = ConvertUtil.ConvertToNullableShort(arrValue[46]);
//      this.ADS_Local_iSendVarPoint_SEM_47_ = ConvertUtil.ConvertToNullableShort(arrValue[47]);
//      this.ADS_Local_iSendVarPoint_SEM_48_ = ConvertUtil.ConvertToNullableShort(arrValue[48]);
//      this.ADS_Local_iSendVarPoint_SEM_49_ = ConvertUtil.ConvertToNullableShort(arrValue[49]);
//      this.ADS_Local_iSendVarPoint_SIL_0_ = ConvertUtil.ConvertToNullableShort(arrValue[50]);
//      this.ADS_Local_iSendVarPoint_SIL_1_ = ConvertUtil.ConvertToNullableShort(arrValue[51]);
//      this.ADS_Local_iSendVarPoint_SIL_2_ = ConvertUtil.ConvertToNullableShort(arrValue[52]);
//      this.ADS_Local_iSendVarPoint_SIL_3_ = ConvertUtil.ConvertToNullableShort(arrValue[53]);
//      this.ADS_Local_iSendVarPoint_SIL_4_ = ConvertUtil.ConvertToNullableShort(arrValue[54]);
//      this.ADS_Local_iSendVarPoint_SIL_5_ = ConvertUtil.ConvertToNullableShort(arrValue[55]);
//      this.ADS_Local_iSendVarPoint_SIL_6_ = ConvertUtil.ConvertToNullableShort(arrValue[56]);
//      this.ADS_Local_iSendVarPoint_SIL_7_ = ConvertUtil.ConvertToNullableShort(arrValue[57]);
//      this.ADS_Local_iSendVarPoint_SIL_8_ = ConvertUtil.ConvertToNullableShort(arrValue[58]);
//      this.ADS_Local_iSendVarPoint_SIL_9_ = ConvertUtil.ConvertToNullableShort(arrValue[59]);
//      this.ADS_Local_iSendVarPoint_SIL_10_ = ConvertUtil.ConvertToNullableShort(arrValue[60]);
//      this.ADS_Local_iSendVarPoint_SIL_11_ = ConvertUtil.ConvertToNullableShort(arrValue[61]);
//      this.ADS_Local_iSendVarPoint_SIL_12_ = ConvertUtil.ConvertToNullableShort(arrValue[62]);
//      this.ADS_Local_iSendVarPoint_SIL_13_ = ConvertUtil.ConvertToNullableShort(arrValue[63]);
//      this.ADS_Local_iSendVarPoint_SIL_14_ = ConvertUtil.ConvertToNullableShort(arrValue[64]);
//      this.ADS_Local_iSendVarPoint_SIL_15_ = ConvertUtil.ConvertToNullableShort(arrValue[65]);
//      this.ADS_Local_iSendVarPoint_SIL_16_ = ConvertUtil.ConvertToNullableShort(arrValue[66]);
//      this.ADS_Local_iSendVarPoint_SIL_17_ = ConvertUtil.ConvertToNullableShort(arrValue[67]);
//      this.ADS_Local_iSendVarPoint_SIL_18_ = ConvertUtil.ConvertToNullableShort(arrValue[68]);
//      this.ADS_Local_iSendVarPoint_SIL_19_ = ConvertUtil.ConvertToNullableShort(arrValue[69]);
//      this.ADS_Local_iSendVarPoint_SIL_20_ = ConvertUtil.ConvertToNullableShort(arrValue[70]);
//      this.ADS_Local_iSendVarPoint_SIL_21_ = ConvertUtil.ConvertToNullableShort(arrValue[71]);
//      this.ADS_Local_iSendVarPoint_SIL_22_ = ConvertUtil.ConvertToNullableShort(arrValue[72]);
//      this.ADS_Local_iSendVarPoint_SIL_23_ = ConvertUtil.ConvertToNullableShort(arrValue[73]);
//      this.ADS_Local_iSendVarPoint_SIL_24_ = ConvertUtil.ConvertToNullableShort(arrValue[74]);
//      this.ADS_Local_iSendVarPoint_SIL_25_ = ConvertUtil.ConvertToNullableShort(arrValue[75]);
//      this.ADS_Local_iSendVarPoint_SIL_26_ = ConvertUtil.ConvertToNullableShort(arrValue[76]);
//      this.ADS_Local_iSendVarPoint_SIL_27_ = ConvertUtil.ConvertToNullableShort(arrValue[77]);
//      this.ADS_Local_iSendVarPoint_SIL_28_ = ConvertUtil.ConvertToNullableShort(arrValue[78]);
//      this.ADS_Local_iSendVarPoint_SIL_29_ = ConvertUtil.ConvertToNullableShort(arrValue[79]);
//      this.ADS_Local_iSendVarPoint_SIL_30_ = ConvertUtil.ConvertToNullableShort(arrValue[80]);
//      this.ADS_Local_iSendVarPoint_SIL_31_ = ConvertUtil.ConvertToNullableShort(arrValue[81]);
//      this.ADS_Local_iSendVarPoint_SIL_32_ = ConvertUtil.ConvertToNullableShort(arrValue[82]);
//      this.ADS_Local_iSendVarPoint_SIL_33_ = ConvertUtil.ConvertToNullableShort(arrValue[83]);
//      this.ADS_Local_iSendVarPoint_SIL_34_ = ConvertUtil.ConvertToNullableShort(arrValue[84]);
//      this.ADS_Local_iSendVarPoint_SIL_35_ = ConvertUtil.ConvertToNullableShort(arrValue[85]);
//      this.ADS_Local_iSendVarPoint_SIL_36_ = ConvertUtil.ConvertToNullableShort(arrValue[86]);
//      this.ADS_Local_iSendVarPoint_SIL_37_ = ConvertUtil.ConvertToNullableShort(arrValue[87]);
//      this.ADS_Local_iSendVarPoint_SIL_38_ = ConvertUtil.ConvertToNullableShort(arrValue[88]);
//      this.ADS_Local_iSendVarPoint_SIL_39_ = ConvertUtil.ConvertToNullableShort(arrValue[89]);
//      this.ADS_Local_iSendVarPoint_SIL_40_ = ConvertUtil.ConvertToNullableShort(arrValue[90]);
//      this.ADS_Local_iSendVarPoint_SIL_41_ = ConvertUtil.ConvertToNullableShort(arrValue[91]);
//      this.ADS_Local_iSendVarPoint_SIL_42_ = ConvertUtil.ConvertToNullableShort(arrValue[92]);
//      this.ADS_Local_iSendVarPoint_SIL_43_ = ConvertUtil.ConvertToNullableShort(arrValue[93]);
//      this.ADS_Local_iSendVarPoint_SIL_44_ = ConvertUtil.ConvertToNullableShort(arrValue[94]);
//      this.ADS_Local_iSendVarPoint_SIL_45_ = ConvertUtil.ConvertToNullableShort(arrValue[95]);
//      this.ADS_Local_iSendVarPoint_SIL_46_ = ConvertUtil.ConvertToNullableShort(arrValue[96]);
//      this.ADS_Local_iSendVarPoint_SIL_47_ = ConvertUtil.ConvertToNullableShort(arrValue[97]);
//      this.ADS_Local_iSendVarPoint_SIL_48_ = ConvertUtil.ConvertToNullableShort(arrValue[98]);
//      this.ADS_Local_iSendVarPoint_SIL_49_ = ConvertUtil.ConvertToNullableShort(arrValue[99]);
//      this.ADS_Local_iSendVarPoint_SIL_50_ = ConvertUtil.ConvertToNullableShort(arrValue[100]);
//      this.ADS_Local_iSendVarPoint_SIL_51_ = ConvertUtil.ConvertToNullableShort(arrValue[101]);
//      this.ADS_Local_iSendVarPoint_SIL_52_ = ConvertUtil.ConvertToNullableShort(arrValue[102]);
//      this.ADS_Local_iSendVarPoint_SIL_53_ = ConvertUtil.ConvertToNullableShort(arrValue[103]);
//      this.ADS_Local_iSendVarPoint_SIL_54_ = ConvertUtil.ConvertToNullableShort(arrValue[104]);
//      this.ADS_Local_iSendVarPoint_SIL_55_ = ConvertUtil.ConvertToNullableShort(arrValue[105]);
//      this.ADS_Local_iSendVarPoint_SIL_56_ = ConvertUtil.ConvertToNullableShort(arrValue[106]);
//      this.ADS_Local_iSendVarPoint_SIL_57_ = ConvertUtil.ConvertToNullableShort(arrValue[107]);
//      this.ADS_Local_iSendVarPoint_SIL_58_ = ConvertUtil.ConvertToNullableShort(arrValue[108]);
//      this.ADS_Local_iSendVarPoint_SIL_59_ = ConvertUtil.ConvertToNullableShort(arrValue[109]);
//    }

//    public override bool Equals(object? obj)
//    {
//      return obj is DeviceDataSendVarSafetyPointModel model &&
//             ADS_Local_iSendVarPoint_SEM_0_ == model.ADS_Local_iSendVarPoint_SEM_0_ &&
//             ADS_Local_iSendVarPoint_SEM_1_ == model.ADS_Local_iSendVarPoint_SEM_1_ &&
//             ADS_Local_iSendVarPoint_SEM_2_ == model.ADS_Local_iSendVarPoint_SEM_2_ &&
//             ADS_Local_iSendVarPoint_SEM_3_ == model.ADS_Local_iSendVarPoint_SEM_3_ &&
//             ADS_Local_iSendVarPoint_SEM_4_ == model.ADS_Local_iSendVarPoint_SEM_4_ &&
//             ADS_Local_iSendVarPoint_SEM_5_ == model.ADS_Local_iSendVarPoint_SEM_5_ &&
//             ADS_Local_iSendVarPoint_SEM_6_ == model.ADS_Local_iSendVarPoint_SEM_6_ &&
//             ADS_Local_iSendVarPoint_SEM_7_ == model.ADS_Local_iSendVarPoint_SEM_7_ &&
//             ADS_Local_iSendVarPoint_SEM_8_ == model.ADS_Local_iSendVarPoint_SEM_8_ &&
//             ADS_Local_iSendVarPoint_SEM_9_ == model.ADS_Local_iSendVarPoint_SEM_9_ &&
//             ADS_Local_iSendVarPoint_SEM_10_ == model.ADS_Local_iSendVarPoint_SEM_10_ &&
//             ADS_Local_iSendVarPoint_SEM_11_ == model.ADS_Local_iSendVarPoint_SEM_11_ &&
//             ADS_Local_iSendVarPoint_SEM_12_ == model.ADS_Local_iSendVarPoint_SEM_12_ &&
//             ADS_Local_iSendVarPoint_SEM_13_ == model.ADS_Local_iSendVarPoint_SEM_13_ &&
//             ADS_Local_iSendVarPoint_SEM_14_ == model.ADS_Local_iSendVarPoint_SEM_14_ &&
//             ADS_Local_iSendVarPoint_SEM_15_ == model.ADS_Local_iSendVarPoint_SEM_15_ &&
//             ADS_Local_iSendVarPoint_SEM_16_ == model.ADS_Local_iSendVarPoint_SEM_16_ &&
//             ADS_Local_iSendVarPoint_SEM_17_ == model.ADS_Local_iSendVarPoint_SEM_17_ &&
//             ADS_Local_iSendVarPoint_SEM_18_ == model.ADS_Local_iSendVarPoint_SEM_18_ &&
//             ADS_Local_iSendVarPoint_SEM_19_ == model.ADS_Local_iSendVarPoint_SEM_19_ &&
//             ADS_Local_iSendVarPoint_SEM_20_ == model.ADS_Local_iSendVarPoint_SEM_20_ &&
//             ADS_Local_iSendVarPoint_SEM_21_ == model.ADS_Local_iSendVarPoint_SEM_21_ &&
//             ADS_Local_iSendVarPoint_SEM_22_ == model.ADS_Local_iSendVarPoint_SEM_22_ &&
//             ADS_Local_iSendVarPoint_SEM_23_ == model.ADS_Local_iSendVarPoint_SEM_23_ &&
//             ADS_Local_iSendVarPoint_SEM_24_ == model.ADS_Local_iSendVarPoint_SEM_24_ &&
//             ADS_Local_iSendVarPoint_SEM_25_ == model.ADS_Local_iSendVarPoint_SEM_25_ &&
//             ADS_Local_iSendVarPoint_SEM_26_ == model.ADS_Local_iSendVarPoint_SEM_26_ &&
//             ADS_Local_iSendVarPoint_SEM_27_ == model.ADS_Local_iSendVarPoint_SEM_27_ &&
//             ADS_Local_iSendVarPoint_SEM_28_ == model.ADS_Local_iSendVarPoint_SEM_28_ &&
//             ADS_Local_iSendVarPoint_SEM_29_ == model.ADS_Local_iSendVarPoint_SEM_29_ &&
//             ADS_Local_iSendVarPoint_SEM_30_ == model.ADS_Local_iSendVarPoint_SEM_30_ &&
//             ADS_Local_iSendVarPoint_SEM_31_ == model.ADS_Local_iSendVarPoint_SEM_31_ &&
//             ADS_Local_iSendVarPoint_SEM_32_ == model.ADS_Local_iSendVarPoint_SEM_32_ &&
//             ADS_Local_iSendVarPoint_SEM_33_ == model.ADS_Local_iSendVarPoint_SEM_33_ &&
//             ADS_Local_iSendVarPoint_SEM_34_ == model.ADS_Local_iSendVarPoint_SEM_34_ &&
//             ADS_Local_iSendVarPoint_SEM_35_ == model.ADS_Local_iSendVarPoint_SEM_35_ &&
//             ADS_Local_iSendVarPoint_SEM_36_ == model.ADS_Local_iSendVarPoint_SEM_36_ &&
//             ADS_Local_iSendVarPoint_SEM_37_ == model.ADS_Local_iSendVarPoint_SEM_37_ &&
//             ADS_Local_iSendVarPoint_SEM_38_ == model.ADS_Local_iSendVarPoint_SEM_38_ &&
//             ADS_Local_iSendVarPoint_SEM_39_ == model.ADS_Local_iSendVarPoint_SEM_39_ &&
//             ADS_Local_iSendVarPoint_SEM_40_ == model.ADS_Local_iSendVarPoint_SEM_40_ &&
//             ADS_Local_iSendVarPoint_SEM_41_ == model.ADS_Local_iSendVarPoint_SEM_41_ &&
//             ADS_Local_iSendVarPoint_SEM_42_ == model.ADS_Local_iSendVarPoint_SEM_42_ &&
//             ADS_Local_iSendVarPoint_SEM_43_ == model.ADS_Local_iSendVarPoint_SEM_43_ &&
//             ADS_Local_iSendVarPoint_SEM_44_ == model.ADS_Local_iSendVarPoint_SEM_44_ &&
//             ADS_Local_iSendVarPoint_SEM_45_ == model.ADS_Local_iSendVarPoint_SEM_45_ &&
//             ADS_Local_iSendVarPoint_SEM_46_ == model.ADS_Local_iSendVarPoint_SEM_46_ &&
//             ADS_Local_iSendVarPoint_SEM_47_ == model.ADS_Local_iSendVarPoint_SEM_47_ &&
//             ADS_Local_iSendVarPoint_SEM_48_ == model.ADS_Local_iSendVarPoint_SEM_48_ &&
//             ADS_Local_iSendVarPoint_SEM_49_ == model.ADS_Local_iSendVarPoint_SEM_49_ &&
//             ADS_Local_iSendVarPoint_SIL_0_ == model.ADS_Local_iSendVarPoint_SIL_0_ &&
//             ADS_Local_iSendVarPoint_SIL_1_ == model.ADS_Local_iSendVarPoint_SIL_1_ &&
//             ADS_Local_iSendVarPoint_SIL_2_ == model.ADS_Local_iSendVarPoint_SIL_2_ &&
//             ADS_Local_iSendVarPoint_SIL_3_ == model.ADS_Local_iSendVarPoint_SIL_3_ &&
//             ADS_Local_iSendVarPoint_SIL_4_ == model.ADS_Local_iSendVarPoint_SIL_4_ &&
//             ADS_Local_iSendVarPoint_SIL_5_ == model.ADS_Local_iSendVarPoint_SIL_5_ &&
//             ADS_Local_iSendVarPoint_SIL_6_ == model.ADS_Local_iSendVarPoint_SIL_6_ &&
//             ADS_Local_iSendVarPoint_SIL_7_ == model.ADS_Local_iSendVarPoint_SIL_7_ &&
//             ADS_Local_iSendVarPoint_SIL_8_ == model.ADS_Local_iSendVarPoint_SIL_8_ &&
//             ADS_Local_iSendVarPoint_SIL_9_ == model.ADS_Local_iSendVarPoint_SIL_9_ &&
//             ADS_Local_iSendVarPoint_SIL_10_ == model.ADS_Local_iSendVarPoint_SIL_10_ &&
//             ADS_Local_iSendVarPoint_SIL_11_ == model.ADS_Local_iSendVarPoint_SIL_11_ &&
//             ADS_Local_iSendVarPoint_SIL_12_ == model.ADS_Local_iSendVarPoint_SIL_12_ &&
//             ADS_Local_iSendVarPoint_SIL_13_ == model.ADS_Local_iSendVarPoint_SIL_13_ &&
//             ADS_Local_iSendVarPoint_SIL_14_ == model.ADS_Local_iSendVarPoint_SIL_14_ &&
//             ADS_Local_iSendVarPoint_SIL_15_ == model.ADS_Local_iSendVarPoint_SIL_15_ &&
//             ADS_Local_iSendVarPoint_SIL_16_ == model.ADS_Local_iSendVarPoint_SIL_16_ &&
//             ADS_Local_iSendVarPoint_SIL_17_ == model.ADS_Local_iSendVarPoint_SIL_17_ &&
//             ADS_Local_iSendVarPoint_SIL_18_ == model.ADS_Local_iSendVarPoint_SIL_18_ &&
//             ADS_Local_iSendVarPoint_SIL_19_ == model.ADS_Local_iSendVarPoint_SIL_19_ &&
//             ADS_Local_iSendVarPoint_SIL_20_ == model.ADS_Local_iSendVarPoint_SIL_20_ &&
//             ADS_Local_iSendVarPoint_SIL_21_ == model.ADS_Local_iSendVarPoint_SIL_21_ &&
//             ADS_Local_iSendVarPoint_SIL_22_ == model.ADS_Local_iSendVarPoint_SIL_22_ &&
//             ADS_Local_iSendVarPoint_SIL_23_ == model.ADS_Local_iSendVarPoint_SIL_23_ &&
//             ADS_Local_iSendVarPoint_SIL_24_ == model.ADS_Local_iSendVarPoint_SIL_24_ &&
//             ADS_Local_iSendVarPoint_SIL_25_ == model.ADS_Local_iSendVarPoint_SIL_25_ &&
//             ADS_Local_iSendVarPoint_SIL_26_ == model.ADS_Local_iSendVarPoint_SIL_26_ &&
//             ADS_Local_iSendVarPoint_SIL_27_ == model.ADS_Local_iSendVarPoint_SIL_27_ &&
//             ADS_Local_iSendVarPoint_SIL_28_ == model.ADS_Local_iSendVarPoint_SIL_28_ &&
//             ADS_Local_iSendVarPoint_SIL_29_ == model.ADS_Local_iSendVarPoint_SIL_29_ &&
//             ADS_Local_iSendVarPoint_SIL_30_ == model.ADS_Local_iSendVarPoint_SIL_30_ &&
//             ADS_Local_iSendVarPoint_SIL_31_ == model.ADS_Local_iSendVarPoint_SIL_31_ &&
//             ADS_Local_iSendVarPoint_SIL_32_ == model.ADS_Local_iSendVarPoint_SIL_32_ &&
//             ADS_Local_iSendVarPoint_SIL_33_ == model.ADS_Local_iSendVarPoint_SIL_33_ &&
//             ADS_Local_iSendVarPoint_SIL_34_ == model.ADS_Local_iSendVarPoint_SIL_34_ &&
//             ADS_Local_iSendVarPoint_SIL_35_ == model.ADS_Local_iSendVarPoint_SIL_35_ &&
//             ADS_Local_iSendVarPoint_SIL_36_ == model.ADS_Local_iSendVarPoint_SIL_36_ &&
//             ADS_Local_iSendVarPoint_SIL_37_ == model.ADS_Local_iSendVarPoint_SIL_37_ &&
//             ADS_Local_iSendVarPoint_SIL_38_ == model.ADS_Local_iSendVarPoint_SIL_38_ &&
//             ADS_Local_iSendVarPoint_SIL_39_ == model.ADS_Local_iSendVarPoint_SIL_39_ &&
//             ADS_Local_iSendVarPoint_SIL_40_ == model.ADS_Local_iSendVarPoint_SIL_40_ &&
//             ADS_Local_iSendVarPoint_SIL_41_ == model.ADS_Local_iSendVarPoint_SIL_41_ &&
//             ADS_Local_iSendVarPoint_SIL_42_ == model.ADS_Local_iSendVarPoint_SIL_42_ &&
//             ADS_Local_iSendVarPoint_SIL_43_ == model.ADS_Local_iSendVarPoint_SIL_43_ &&
//             ADS_Local_iSendVarPoint_SIL_44_ == model.ADS_Local_iSendVarPoint_SIL_44_ &&
//             ADS_Local_iSendVarPoint_SIL_45_ == model.ADS_Local_iSendVarPoint_SIL_45_ &&
//             ADS_Local_iSendVarPoint_SIL_46_ == model.ADS_Local_iSendVarPoint_SIL_46_ &&
//             ADS_Local_iSendVarPoint_SIL_47_ == model.ADS_Local_iSendVarPoint_SIL_47_ &&
//             ADS_Local_iSendVarPoint_SIL_48_ == model.ADS_Local_iSendVarPoint_SIL_48_ &&
//             ADS_Local_iSendVarPoint_SIL_49_ == model.ADS_Local_iSendVarPoint_SIL_49_ &&
//             ADS_Local_iSendVarPoint_SIL_50_ == model.ADS_Local_iSendVarPoint_SIL_50_ &&
//             ADS_Local_iSendVarPoint_SIL_51_ == model.ADS_Local_iSendVarPoint_SIL_51_ &&
//             ADS_Local_iSendVarPoint_SIL_52_ == model.ADS_Local_iSendVarPoint_SIL_52_ &&
//             ADS_Local_iSendVarPoint_SIL_53_ == model.ADS_Local_iSendVarPoint_SIL_53_ &&
//             ADS_Local_iSendVarPoint_SIL_54_ == model.ADS_Local_iSendVarPoint_SIL_54_ &&
//             ADS_Local_iSendVarPoint_SIL_55_ == model.ADS_Local_iSendVarPoint_SIL_55_ &&
//             ADS_Local_iSendVarPoint_SIL_56_ == model.ADS_Local_iSendVarPoint_SIL_56_ &&
//             ADS_Local_iSendVarPoint_SIL_57_ == model.ADS_Local_iSendVarPoint_SIL_57_ &&
//             ADS_Local_iSendVarPoint_SIL_58_ == model.ADS_Local_iSendVarPoint_SIL_58_ &&
//             ADS_Local_iSendVarPoint_SIL_59_ == model.ADS_Local_iSendVarPoint_SIL_59_;
//    }
//  }
//}
