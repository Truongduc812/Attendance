using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using iSoft.Common.Utils;
using iSoft.Redis.Services;

namespace iSoft.Database.CachedFuncNS.Tests
{
  [TestClass()]
  public class CachedFuncRequireLockAndRetryTests
  {
    [TestMethod()]
    public void RequireLockTest()
    {
      CachedFunc.SetRedisConfig(new Common.Models.ConfigModel.Subs.ServerConfigModel(
        "localhost",
        16379,
        "admin",
        "AdBV1c2SgWtr6TwhJMQoGvxq"
        ));

      Thread thread1 = new Thread(Method1);
      Thread thread2 = new Thread(Method2);
      Thread thread3 = new Thread(Method3);

      thread1.Start();
      thread2.Start();
      thread3.Start();

      // Đợi cho tất cả các luồng kết thúc
      thread1.Join();
      thread2.Join();
      thread3.Join();

      Debug.WriteLine("All threads have finished execution.");

    }

    string KEY_LOCK = "KEY_LOCK";


    private void Method1(object? obj)
    {
      for (int i = 0; i < 10; i++)
      {
        if (CachedFunc.RequireLockAndRetry(KEY_LOCK, 20, 6, 8))
        {
          Debug.WriteLine($"111 Method1: {i}, {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
          Thread.Sleep(10);
          CachedFunc.UnLock(KEY_LOCK);
          Debug.WriteLine($"111 Method1:  {i}, {DateTimeUtil.GetDateTimeStr(DateTime.Now)} UnLock");
        }
        else
        {
          Debug.WriteLine($"111 Method1: {i} ERROR,  {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
        }
      }
    }
    private void Method2(object? obj)
    {
      for (int i = 0; i < 10; i++)
      {
        if (CachedFunc.RequireLockAndRetry(KEY_LOCK, 20, 6, 8))
        {
          Debug.WriteLine($"222 Method2: {i}, {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
          Thread.Sleep(10);
          CachedFunc.UnLock(KEY_LOCK);
          Debug.WriteLine($"222 Method2:  {i}, {DateTimeUtil.GetDateTimeStr(DateTime.Now)} UnLock");
        }
        else
        {
          Debug.WriteLine($"222 Method2: {i} ERROR, {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
        }
      }
    }
    private void Method3(object? obj)
    {
      for (int i = 0; i < 10; i++)
      {
        if (CachedFunc.RequireLockAndRetry(KEY_LOCK, 20, 6, 8))
        {
          Debug.WriteLine($"333 Method3: {i}, {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
          Thread.Sleep(10);
          CachedFunc.UnLock(KEY_LOCK);
          Debug.WriteLine($"333 Method3:  {i}, {DateTimeUtil.GetDateTimeStr(DateTime.Now)} UnLock");
        }
        else
        {
          Debug.WriteLine($"333 Method3: {i} ERROR, {DateTimeUtil.GetDateTimeStr(DateTime.Now)}");
        }
      }
    }
  }
}