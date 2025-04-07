namespace SourceBaseBE.MainService.Models;

public class TestDataModel
{
  public string Data1 { get; set; }
  public int Data2 { get; set; }

  public TestDataModel(string data1, int data2)
  {
    Data1 = data1;
    Data2 = data2;
  }
}
