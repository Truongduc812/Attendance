using iSoft.Database.Entities;
using iSoft.DBLibrary.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SourceBaseBE.Database.Entities
{
  [Table("TestCRUDs")]
  public class TestCRUDEntity : BaseCRUDEntity
  {
    [ForeignKey(nameof(UserEntity))]
    public long? ReviewerId { get; set; }
    public UserEntity? Reviewer { get; set; }
    public DateTime DateTimeValue { get; set; }
    public string StringValue { get; set; } = "";
    public long LongValue { get; set; }
    public int IntValue { get; set; }
    public short ShortValue { get; set; }
    public double DoubleValue { get; set; }
    public bool BoolValue { get; set; }
    public long TimeIntervalInSeconds { get; set; }
    public DateTime? DateTimeValue2 { get; set; }
    public string? StringValue2 { get; set; }
    public long? LongValue2 { get; set; }
    public int? IntValue2 { get; set; }
    public short? ShortValue2 { get; set; }
    public double? DoubleValue2 { get; set; }
    public bool? BoolValue2 { get; set; }
    public long? TimeIntervalInSeconds2 { get; set; }

    //[NotMapped]
    //public List<long> ListEntity2 { get; set; } = new List<long>();
    //[ListEntityAttribute(nameof(TestCRUD2Entity), nameof(ListEntity2), "XXX")]
    //public List<TestCRUD2Entity> TestCRUD2s { get; set; } = new();
  }
}
