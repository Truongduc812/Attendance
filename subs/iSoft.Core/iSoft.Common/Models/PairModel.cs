using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSoft.Common.Models
{
  public class PairObj2<TA, TB>
  {
    public TA obj1 { get; set; }
    public TB obj2 { get; set; }

    public PairObj2(TA obj1, TB obj2)
    {
      this.obj1 = obj1;
      this.obj2 = obj2;
    }
  }
  public class PairObj3<TA, TB, TC>
  {
    public TA obj1 { get; set; }
    public TB obj2 { get; set; }
    public TC obj3 { get; set; }

    public PairObj3(TA obj1, TB obj2, TC obj3)
    {
      this.obj1 = obj1;
      this.obj2 = obj2;
      this.obj3 = obj3;
    }
  }
}
