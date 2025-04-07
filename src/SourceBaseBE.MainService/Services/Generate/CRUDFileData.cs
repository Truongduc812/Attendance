using System.Collections.Generic;

namespace SourceBaseBE.MainService.Services.Generate
{
  public class CRUDFileData
  {
    public string name { get; set; }
    //public string filePath_FormDataConvert { get; set; }
    public string filePath_Service { get; set; }
    public string filePath_Entity { get; set; }
    public string filePath_Controller { get; set; }
    public string filePath_RequestModel { get; set; }
    public string filePath_ResponseModel { get; set; }
    public string filePath_Repository { get; set; }
    public List<string> listFilePath = new List<string>();
    public CRUDFileData(string name, string subFolder)
    {
      this.name = name;
      //this.filePath_FormDataConvert = $"./../SourceBaseBE.MainService/FormDataConvertNS{subFolder}/{name}FormDataConvert.cs";
      filePath_Service = $"./../SourceBaseBE.MainService/Services{subFolder}/{name}Service.cs";
      filePath_Entity = $"./../SourceBaseBE.Database/Entities{subFolder}/{name}Entity.cs";
      filePath_Controller = $"./../SourceBaseBE.MainService/Controllers{subFolder}/{name}Controller.cs";
      filePath_RequestModel = $"./../SourceBaseBE.Database/Models/RequestModels{subFolder}/{name}RequestModel.cs";
      filePath_ResponseModel = $"./../SourceBaseBE.Database/Models/ResponseModels{subFolder}/{name}ResponseModel.cs";
      filePath_Repository = $"./../SourceBaseBE.Database/Repository{subFolder}/{name}Repository.cs";

      //listFilePath.Add(filePath_FormDataConvert);
      listFilePath.Add(filePath_Service);
      listFilePath.Add(filePath_Entity);
      listFilePath.Add(filePath_Controller);
      listFilePath.Add(filePath_RequestModel);
      listFilePath.Add(filePath_ResponseModel);
      listFilePath.Add(filePath_Repository);
    }

    internal string GetBackupFilePathEntity()
    {
      return filePath_Entity.Replace("Entity.cs", "Entity_Backup.txt");
    }
  }
}
