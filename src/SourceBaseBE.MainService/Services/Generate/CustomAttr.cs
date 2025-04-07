using System.Collections.Generic;

namespace SourceBaseBE.MainService.Services.Generate
{
  public class CustomAttr
  {
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsNullable { get; set; }
    //public string DefaultValue { get; set; }
    public List<CustomAnno> ListAnnotation { get; set; }
    public override string ToString()
    {
      return $"{Type}: {Name}";
    }
  }
}
