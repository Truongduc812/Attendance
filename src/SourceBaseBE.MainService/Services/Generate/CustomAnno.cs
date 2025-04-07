namespace SourceBaseBE.MainService.Services.Generate
{
  public class CustomAnno
  {
    public string Name { get; set; }
    public string Param1 { get; set; }
    public string Param2 { get; set; }
    public string GetTableName()
    {
      return Param1.Replace("Entity", "");
    }
    public override string ToString()
    {
      return $"{Name} | {Param1} | {Param2}";
    }
  }
}
