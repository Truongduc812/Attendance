using System.Data;

namespace SourceBaseBE.Database.DTOs.Interfaces
{
    public interface IDataDTO
    {
        public void FillData(IDataReader reader);
    }
}
