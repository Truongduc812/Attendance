using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.Database.Enums
{
	public enum EnumApproveStatus
	{
		[Description("PENDING")]
		PENDING = 0,
		[Description("ACCEPT")]
		ACCEPT = 1,
		[Description("REJECT")]
		REJECT = 2,
	}
}
