//using iSoft.Common.Utils;
//using System;

//namespace iSoft.ElasticSearch.Models
//{
//  public class DeviceDataProcessModel : BaseDeviceDataModel
//  {
//    public DateTime Timestamp { get; set; }
//    public override long ConnectionId { get; set; }
//    public override DateTime ExecuteAt { get; set; }
//    public override String MessageId { get; set; }

//    public double? ADS_Local_plc1_LT01_PV_TK020 { get; set; }
//    public long? ADS_Local_plc1_ID_TANK_TK020 { get; set; }
//    public bool? ADS_Local_plc1_LSH01_TK020 { get; set; }
//    public double? ADS_Local_plc1_LT01_PV_TK019_T { get; set; }
//    public long? ADS_Local_plc1_ID_TANK_TK019 { get; set; }
//    public bool? ADS_Local_plc1_LSH01_TK019 { get; set; }
//    public double? ADS_Local_plc1_LT01_PV_TK018 { get; set; }
//    public long? ADS_Local_plc1_ID_TANK_TK018 { get; set; }
//    public bool? ADS_Local_plc1_LSH01_TK018 { get; set; }
//    public double? ADS_Local_plc1_LT01_PV_TK017 { get; set; }
//    public long? ADS_Local_plc1_ID_TANK_TK017 { get; set; }
//    public bool? ADS_Local_plc1_LSH01_TK017 { get; set; }
//    public bool? ADS_Local_plc1_PK009_AV13_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK009_AV09_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK009_AV08_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK008_AV09_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK008_AV08_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK008_LS01 { get; set; }
//    public bool? ADS_Local_plc1_PK008_PRV01_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK008_P01_RUN { get; set; }
//    public double? ADS_Local_plc1_PK008_P01_SPEED { get; set; }
//    public bool? ADS_Local_plc1_PK008_AV19_OPENED { get; set; }
//    public double? ADS_Local_plc1_PK008_PT01_VALUE { get; set; }
//    public bool? ADS_Local_plc1_PIG008_AV07_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PIG008_AV04_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PIG008_AV03_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PIG008_SV01_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PIG008_PX01 { get; set; }
//    public bool? ADS_Local_plc1_PIG008_AV06_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PIG008_AV05_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK008_PX08 { get; set; }
//    public bool? ADS_Local_plc1_PIGRE008_SV02_OPENED { get; set; }
//    public long? ADS_Local_plc1_ID_TANK_TK_POSIMAT2 { get; set; }
//    public bool? ADS_Local_plc1_PK008_AV26_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK008_AV25_OPENED { get; set; }
//    public long? ADS_Local_plc1_ID_TANK_POSIMAT2 { get; set; }
//    public bool? ADS_Local_plc1_PK008_AV27_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK008_LS01_T { get; set; }
//    public bool? ADS_Local_plc1_PK008_AV28_OPENED { get; set; }
//    public bool? ADS_Local_plc1_PK004A_LSL03 { get; set; }
//    public bool? ADS_Local_plc1_PK004A_P03_RUN { get; set; }


//    public DeviceDataProcessModel(long id,
//                                  object?[] arrValue,
//                                  DateTime executeAt,
//                                  string messageId)
//    {
//      this.Timestamp = DateTime.Now;
//      this.ConnectionId = id;
//      this.ExecuteAt = executeAt;
//      this.MessageId = messageId;
//      this.ADS_Local_plc1_LT01_PV_TK020 = ConvertUtil.ConvertToNullableDouble(arrValue[0]);
//      this.ADS_Local_plc1_ID_TANK_TK020 = ConvertUtil.ConvertToNullableLong(arrValue[1]);
//      this.ADS_Local_plc1_LSH01_TK020 = ConvertUtil.ConvertToNullableBool(arrValue[2]);
//      this.ADS_Local_plc1_LT01_PV_TK019_T = ConvertUtil.ConvertToNullableDouble(arrValue[3]);
//      this.ADS_Local_plc1_ID_TANK_TK019 = ConvertUtil.ConvertToNullableLong(arrValue[4]);
//      this.ADS_Local_plc1_LSH01_TK019 = ConvertUtil.ConvertToNullableBool(arrValue[5]);
//      this.ADS_Local_plc1_LT01_PV_TK018 = ConvertUtil.ConvertToNullableDouble(arrValue[6]);
//      this.ADS_Local_plc1_ID_TANK_TK018 = ConvertUtil.ConvertToNullableLong(arrValue[7]);
//      this.ADS_Local_plc1_LSH01_TK018 = ConvertUtil.ConvertToNullableBool(arrValue[8]);
//      this.ADS_Local_plc1_LT01_PV_TK017 = ConvertUtil.ConvertToNullableDouble(arrValue[9]);
//      this.ADS_Local_plc1_ID_TANK_TK017 = ConvertUtil.ConvertToNullableLong(arrValue[10]);
//      this.ADS_Local_plc1_LSH01_TK017 = ConvertUtil.ConvertToNullableBool(arrValue[11]);
//      this.ADS_Local_plc1_PK009_AV13_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[12]);
//      this.ADS_Local_plc1_PK009_AV09_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[13]);
//      this.ADS_Local_plc1_PK009_AV08_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[14]);
//      this.ADS_Local_plc1_PK008_AV09_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[15]);
//      this.ADS_Local_plc1_PK008_AV08_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[16]);
//      this.ADS_Local_plc1_PK008_LS01 = ConvertUtil.ConvertToNullableBool(arrValue[17]);
//      this.ADS_Local_plc1_PK008_PRV01_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[18]);
//      this.ADS_Local_plc1_PK008_P01_RUN = ConvertUtil.ConvertToNullableBool(arrValue[19]);
//      this.ADS_Local_plc1_PK008_P01_SPEED = ConvertUtil.ConvertToNullableDouble(arrValue[20]);
//      this.ADS_Local_plc1_PK008_AV19_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[21]);
//      this.ADS_Local_plc1_PK008_PT01_VALUE = ConvertUtil.ConvertToNullableDouble(arrValue[22]);
//      this.ADS_Local_plc1_PIG008_AV07_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[23]);
//      this.ADS_Local_plc1_PIG008_AV04_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[24]);
//      this.ADS_Local_plc1_PIG008_AV03_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[25]);
//      this.ADS_Local_plc1_PIG008_SV01_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[26]);
//      this.ADS_Local_plc1_PIG008_PX01 = ConvertUtil.ConvertToNullableBool(arrValue[27]);
//      this.ADS_Local_plc1_PIG008_AV06_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[28]);
//      this.ADS_Local_plc1_PIG008_AV05_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[29]);
//      this.ADS_Local_plc1_PK008_PX08 = ConvertUtil.ConvertToNullableBool(arrValue[30]);
//      this.ADS_Local_plc1_PIGRE008_SV02_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[31]);
//      this.ADS_Local_plc1_ID_TANK_TK_POSIMAT2 = ConvertUtil.ConvertToNullableLong(arrValue[32]);
//      this.ADS_Local_plc1_PK008_AV26_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[33]);
//      this.ADS_Local_plc1_PK008_AV25_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[34]);
//      this.ADS_Local_plc1_ID_TANK_POSIMAT2 = ConvertUtil.ConvertToNullableLong(arrValue[35]);
//      this.ADS_Local_plc1_PK008_AV27_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[36]);
//      this.ADS_Local_plc1_PK008_LS01_T = ConvertUtil.ConvertToNullableBool(arrValue[37]);
//      this.ADS_Local_plc1_PK008_AV28_OPENED = ConvertUtil.ConvertToNullableBool(arrValue[38]);
//      this.ADS_Local_plc1_PK004A_LSL03 = ConvertUtil.ConvertToNullableBool(arrValue[39]);
//      this.ADS_Local_plc1_PK004A_P03_RUN = ConvertUtil.ConvertToNullableBool(arrValue[40]);

//    }

//  }
//}
