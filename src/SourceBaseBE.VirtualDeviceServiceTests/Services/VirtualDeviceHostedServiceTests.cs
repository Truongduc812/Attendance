using Microsoft.VisualStudio.TestTools.UnitTesting;
using SourceBaseBE.VirtualDeviceService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SourceBaseBE.VirtualDeviceService.Services.Tests
{
  [TestClass()]
  public class VirtualDeviceHostedServiceTests
  {
    [TestMethod()]
    public void PushMessageToRabbitAndSocketIOTest()
    {
      VirtualDeviceHostedService service = new VirtualDeviceHostedService();
      PushMessageToRabbitAndSocketIO();
    }
  }
}