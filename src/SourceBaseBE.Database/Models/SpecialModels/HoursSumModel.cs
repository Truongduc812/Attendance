//using iSoft.Common.Enums;
//using SourceBaseBE.Database.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SourceBaseBE.Database.Models.SpecialModels
//{
//  public class HoursCalculatorModel
//  {
//    public EnumShiftId shiftId {  get; set; }
//    public long shift1Total { get; set; }
//    public long shift2Total { get; set; }
//    public long shift3Total { get; set; }
//    public long shift3TotalSubAM { get; set; }
//    public long shift3TotalSubPM { get; set; }
//    public void CalculatorShiftId()
//    {
//      if (shift1Total >= shift2Total && shift1Total >= shift3Total)
//      {
//        this.shiftId = EnumShiftId.Shift1;
//      }
//      else if (shift2Total >= shift1Total && shift2Total >= shift3Total)
//      {
//        this.shiftId = EnumShiftId.Shift2;
//      }
//      else
//      {
//        this.shiftId = EnumShiftId.Shift3;
//      }
//    }

//    public HoursCalculatorModel(long shift1Total, long shift2Total, long shift3TotalSubAM, long shift3TotalSubPM)
//    {
//      this.shift1Total = shift1Total;
//      this.shift2Total = shift2Total;
//      this.shift3TotalSubAM = shift3TotalSubAM;
//      this.shift3TotalSubPM = shift3TotalSubPM;
//      this.shift3Total = shift3TotalSubAM + shift3TotalSubPM;

//      CalculatorShiftId();
//    }

//    public override string ToString()
//    {
//      return $"{this.shiftId.ToString()}|{this.shift3TotalSubAM}|{this.shift1Total}|{this.shift2Total}|{this.shift3TotalSubPM}";
//    }
//  }
//}
