using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using iSoft.Common.Utils;
using iSoft.Redis.Services;

namespace iSoft.Database.CachedFuncNS.Tests
{
  [TestClass()]
  public class CachedFuncIsCanRetryTests
  {
    [TestMethod()]
    public void RequireLockTest()
    {
      CachedFunc.SetRedisConfig(new Common.Models.ConfigModel.Subs.ServerConfigModel(
        "localhost",
        2704,
        "admin",
        "AdBV1c2SgWtr6TwhJMQoGvxq"
        ));

      Thread thread1 = new Thread(Method1);
      Thread thread2 = new Thread(Method2);

      thread1.Start();
      thread2.Start();

      // Đợi cho tất cả các luồng kết thúc
      thread1.Join();
      thread2.Join();

      Debug.WriteLine("All threads have finished execution.");

    }

    string KEY_LOCK = "KEY_LOCK";


    private void Method1(object? obj)
    {
      for (int i = 0; i < 1000; i++)
      {
        if (CachedFunc.IsCanRetry("1234567890d", 10, 5) >= 0)
        {
          Debug.WriteLine($"111 Method1: {i}, {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
        }
        else
        {
          Debug.WriteLine($"111 Method1: {i} ERROR,  {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
        }
        Thread.Sleep(1000);
      }
    }
    private void Method2(object? obj)
    {
      for (int i = 0; i < 1000; i++)
      {
        if (CachedFunc.IsCanRetry("1234567890d", 10, 5) >= 0)
        {
          Debug.WriteLine($"111 Method1: {i}, {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
        }
        else
        {
          Debug.WriteLine($"111 Method1: {i} ERROR,  {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
        }
        Thread.Sleep(1000);
      }
    }
  }
}