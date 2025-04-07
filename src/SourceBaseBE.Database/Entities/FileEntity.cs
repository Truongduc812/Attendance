using iSoft.Database.Entities;

namespace SourceBaseBE.Database.Entities;

public class FileEntity : BaseCRUDEntity
{
  public FileEntity()
  {
    Machines = new HashSet<MachineEntity>();
  }
  public string? Category { get; set; }

  public DateTime? UploadedDate { get; set; }

  public string? Path { get; set; }

  public string Name { get; set; } = null!;

  public string? SerialCode { get; set; }

  public virtual ICollection<MachineEntity> Machines { get; set; }
}
