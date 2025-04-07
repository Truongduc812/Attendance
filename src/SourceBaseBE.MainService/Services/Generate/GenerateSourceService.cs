using SourceBaseBE.Database.DBContexts;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using iSoft.Common.Utils;
using iSoft.Database.Extensions;
using iSoft.Common.Enums;
using Microsoft.Extensions.Logging;

namespace SourceBaseBE.MainService.Services.Generate
{
  public enum EnumFileType
  {
    RequestFile,
    ResponseFile,
    ServiceFile,
  }
  public class GenerateSourceService
  {
    private ILogger<GenerateSourceService> _logger;
    CRUDFileData templateCRUD = new CRUDFileData("GenTemplate", "/Generate");
    CRUDFileData newCRUD = null;
    AttrParser attrParser = new AttrParser();

    public GenerateSourceService(ILogger<GenerateSourceService> logger)
    {
      _logger = logger;
    }

    internal void BackupEntity(string entityName)
    {
      newCRUD = new CRUDFileData(entityName, "");
      if (File.Exists(newCRUD.filePath_Entity))
      {
        File.Copy(newCRUD.filePath_Entity, newCRUD.GetBackupFilePathEntity(), true);
      }
      else
      {
        throw new Exception($"{entityName}Entity.cs not found");
      }
    }

    internal void CloneFile(string entityName)
    {
      for (int i = 0; i < templateCRUD.listFilePath.Count; i++)
      {
        string filePath = templateCRUD.listFilePath[i];
        if (File.Exists(filePath))
        {
          File.Copy(filePath, newCRUD.listFilePath[i], true);
        }
      }
    }

    internal void ReplaceFileData(string entityName)
    {
      foreach (var filePath in newCRUD.listFilePath)
      {
        string fileContent = File.ReadAllText(filePath);
        fileContent = fileContent.Replace(templateCRUD.name, newCRUD.name)
          .Replace(templateCRUD.name.ToUpper(), newCRUD.name.ToUpper())
          .Replace(templateCRUD.name.ToLower(), newCRUD.name.ToLower());
        File.WriteAllText(filePath, fileContent);
      }
    }

    internal void UpdateByEntityBackup(string entityName)
    {
      if (File.Exists(newCRUD.GetBackupFilePathEntity()))
      {
        File.Copy(newCRUD.GetBackupFilePathEntity(), newCRUD.filePath_Entity, true);
      }
    }

    internal void UpdateReferenceFile(string entityName)
    {

      string entityContent = File.ReadAllText(newCRUD.filePath_Entity);
      var listAttr = attrParser.ParseAttributes(entityContent);

      UpdateDBContextFile(entityName, listAttr);

      UpdateStartupFile(entityName);

      UpdateRequestModel(entityName, listAttr);

      UpdateResponseModel(entityName, listAttr);

      UpdateService(entityName, listAttr);

      UpdateRepository(entityName, listAttr);
    }

    private void UpdateRequestModel(string entityName, List<CustomAttr> listAttr)
    {
      string filePath = newCRUD.filePath_RequestModel;
      //string filePath = $"./../SourceBaseBE.Database/Models/RequestModels/{entityName}RequestModel.txt";
      string fileContent = File.ReadAllText(filePath);

      //string newStr = GetAttrData(listAttr, EnumFileType.RequestFile);
      //fileContent = Regex.Replace(
      //  fileContent,
      //  @"(class " + entityName + @"RequestModel [^\{]+\{).*(public override \w+ GetEntity\(.+)",
      //  "$1\r\n" + newStr + "\r\n    $2",
      //  RegexOptions.Singleline);

      //newStr = GetAttrSetData(listAttr, EnumFileType.RequestFile);
      //fileContent = Regex.Replace(
      //  fileContent,
      //  @"(if \(this\.Order \!\= null\) entity\.Order \= this\.Order;).*(return entity;.+)",
      //  "$1\r\n" + newStr + "\r\n      $2",
      //  RegexOptions.Singleline);

      string newStr = "";
      List<string> listKeyGen = new List<string>()
      {
        "/*[GEN-17]*/",
        "/*[GEN-18]*/",
        "/*[GEN-19]*/",
      };
      foreach (string keyGen in listKeyGen)
      {
        newStr = GetGenData(listAttr, keyGen, fileContent);
        fileContent = fileContent.Replace(keyGen, newStr);
      }
      File.WriteAllText(filePath, fileContent);
    }

    private void UpdateResponseModel(string entityName, List<CustomAttr> listAttr)
    {
      string filePath = newCRUD.filePath_ResponseModel;
      string fileContent = File.ReadAllText(filePath);

      //string newStr = GetAttrData(listAttr, EnumFileType.ResponseFile);
      //fileContent = Regex.Replace(
      //  fileContent,
      //  @"(class " + entityName + @"ResponseModel [^\{]+\{).*(public override object SetData\(.+)",
      //  "$1\r\n" + newStr + "\r\n    $2",
      //  RegexOptions.Singleline);

      //newStr = GetAttrSetData(listAttr, EnumFileType.ResponseFile);
      //fileContent = Regex.Replace(
      //  fileContent,
      //  @"(base\.SetData\(entity\);).*(return this;.+)",
      //  "$1\r\n" + newStr + "\r\n      $2",
      //  RegexOptions.Singleline);

      string newStr = "";
      List<string> listKeyGen = new List<string>()
      {
        "/*[GEN-20]*/",
        "/*[GEN-21]*/",
      };
      foreach (string keyGen in listKeyGen)
      {
        newStr = GetGenData(listAttr, keyGen, fileContent);
        fileContent = fileContent.Replace(keyGen, newStr);
      }
      File.WriteAllText(filePath, fileContent);
    }

    private void UpdateService(string entityName, List<CustomAttr> listAttr)
    {
      string filePath = newCRUD.filePath_Service;
      //string filePath = $"./../SourceBaseBE.MainService/Services/{entityName}Service.txt";
      string fileContent = File.ReadAllText(filePath);

      List<string> listKeyGen = new List<string>()
      {
        "/*[GEN-1]*/",
        "/*[GEN-2]*/",
        "/*[GEN-3]*/",
        "/*[GEN-4]*/",
        "/*[GEN-5]*/",
        "/*[GEN-6]*/",
      };
      foreach (string keyGen in listKeyGen)
      {
        string newStr = GetGenData(listAttr, keyGen);
        fileContent = fileContent.Replace(keyGen, newStr);
      }
      File.WriteAllText(filePath, fileContent);
    }

    private void UpdateRepository(string entityName, List<CustomAttr> listAttr)
    {
      string filePath = newCRUD.filePath_Repository;
      //string filePath = $"./../SourceBaseBE.Database/Repository/{entityName}Repository.txt";
      string fileContent = File.ReadAllText(filePath);

      List<string> listKeyGen = new List<string>()
      {
        "/*[GEN-4]*/",
        "/*[GEN-7]*/",
        "/*[GEN-8]*/",
        "/*[GEN-9]*/",
        "/*[GEN-10]*/",
        "/*[GEN-11]*/",
        "/*[GEN-12]*/",
        "/*[GEN-13]*/",
        "/*[GEN-14]*/",
      };
      foreach (string keyGen in listKeyGen)
      {
        string newStr = GetGenData(listAttr, keyGen);
        fileContent = fileContent.Replace(keyGen, newStr);
      }
      File.WriteAllText(filePath, fileContent);
    }

    private string GetGenData(List<CustomAttr> listAttr, string genKey, string inputString = "")
    {
      string strNew = "";
      StringBuilder sb = new StringBuilder();
      GetDicIdsAttr(listAttr);

      switch (genKey)
      {
        case "/*[GEN-1]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                if (newCRUD.name == anno.GetTableName())
                {
                  continue;
                }
                if ("User" == anno.GetTableName())
                {
                  continue;
                }
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                sb.Append($"    private {tbName2}Repository _{lowTbName2}Repository;\r\n");
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;
        case "/*[GEN-2]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                if (newCRUD.name == anno.GetTableName())
                {
                  continue;
                }
                if ("User" == anno.GetTableName())
                {
                  continue;
                }
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                sb.Append($"      this._{lowTbName2}Repository = new {tbName2}Repository(this._dbContext);\r\n");
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;
        case "/*[GEN-3]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                string idsName2 = anno.Param2;
                if (newCRUD.name == anno.GetTableName())
                {
                  sb.Append($@"
      List<{tbName2}Entity> {lowTbName2}Children = null;
      if (entity.{idsName2} != null && entity.{idsName2}.Count >= 1)
      {{
        {lowTbName2}Children = _repository.GetListByListIds(entity.{idsName2}, true);
      }}
");
                }
                else
                {
                  sb.Append($@"
      List<{tbName2}Entity> {lowTbName2}Children = null;
      if (entity.{idsName2} != null && entity.{idsName2}.Count >= 1)
      {{
        {lowTbName2}Children = _{lowTbName2}Repository.GetListByListIds(entity.{idsName2}, true);
      }}
");
                }
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;
        case "/*[GEN-4]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                string idsName2 = anno.Param2;
                sb.Append($", {lowTbName2}Children");
              }
            }
          }
          return sb.ToString().Trim() + genKey;
        case "/*[GEN-5]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                string idsName2 = anno.Param2;
                sb.Append($@"
        case nameof({tbName2}Entity):
          if (entity.{attr.Name} == null)
          {{
            return new List<long>();
          }}
          return entity.{attr.Name}.Select(x => x.Id).ToList();
");
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;
        case "/*[GEN-6]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                string idsName2 = anno.Param2;
                if (newCRUD.name == anno.GetTableName())
                {
                  sb.Append($@"
          case nameof({tbName2}Entity):
            listRS = this._repository.GetSelectData(entityName, category);
            break;
");
                }
                else
                {
                  sb.Append($@"
          case nameof({tbName2}Entity):
            listRS = this._{lowTbName2}Repository.GetSelectData(entityName, category);
            break;
");
                }
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;
        case "/*[GEN-7]*/":
        case "/*[GEN-11]*/":
        case "/*[GEN-13]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                string idsName2 = anno.Param2;
                sb.Append($"                .Include(entity => entity.{attr.Name})\r\n");
              }
            }
          }
          return sb.ToString().Trim() + genKey;
        case "/*[GEN-8]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                string idsName2 = anno.Param2;
                sb.Append($", List<{tbName2}Entity> {lowTbName2}Children");
              }
            }
          }
          return sb.ToString().Trim() + genKey;
        case "/*[GEN-9]*/":
        case "/*[GEN-10]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                string idsName2 = anno.Param2;
                sb.Append($@"
          entity.{attr.Name} = MergeChildrenEntity(entity.{attr.Name}, {lowTbName2}Children);
");
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;
        case "/*[GEN-12]*/":
        case "/*[GEN-14]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                string idsName2 = anno.Param2;
                sb.Append($"            result[i].{attr.Name} = result[i].{attr.Name}?.Select(x => new {tbName2}Entity() {{Id = x.Id}}).ToList();\r\n");
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;
        case "/*[GEN-15]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                string tbName2 = anno.GetTableName();
                string lowTbName2 = StringUtil.LowerFirstLetter(anno.GetTableName());
                string idsName2 = anno.Param2;
                if (!IsExitsFileContent(inputString, GetJoinEntityST(newCRUD.name, tbName2, attr.Name)))
                {
                  if (!IsExitsFileContent(inputString, GetJoinEntityST(tbName2, newCRUD.name, attr.Name)))
                  {
                    sb.Append(GetJoinEntityST(newCRUD.name, tbName2, attr.Name));
                  }
                }
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;
        case "/*[GEN-16]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null && anno.Name == "ForeignKey")
              {
                string tbName2 = anno.GetTableName();
                if (!IsExitsFileContent(inputString, GetJoinEntityForeignKey(newCRUD.name, tbName2, attr.Name)))
                {
                  if (!IsExitsFileContent(inputString, GetJoinEntityForeignKey(tbName2, newCRUD.name, attr.Name)))
                  {
                    sb.Append(GetJoinEntityForeignKey(newCRUD.name, tbName2, attr.Name));
                  }
                }
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;
        case "/*[GEN-17]*/":
          foreach (CustomAttr attr in listAttr)
          {
            foreach (var anno in attr.ListAnnotation)
            {
              if (anno != null
                && anno.Name == nameof(FormDataTypeAttribute)
                && anno.Param1 == nameof(EnumFormDataType.image))
              {
                sb.Append($@"
      if (this.{attr.Name} != null)
      {{
        dicRS.Add(nameof({attr.Name}), (Path.Combine(ConstFolderPath.Image, ConstFolderPath.Upload), this.{attr.Name}));
      }}");
              }
            }
          }
          return sb.ToString().Trim() + "\r\n" + genKey;

        case "/*[GEN-18]*/":
          strNew = GetAttrData(listAttr, EnumFileType.RequestFile);
          return strNew.Trim() + "\r\n" + genKey;

        case "/*[GEN-19]*/":
          strNew = GetAttrSetData(listAttr, EnumFileType.RequestFile);
          return strNew.Trim() + "\r\n" + genKey;

        case "/*[GEN-20]*/":
          strNew = GetAttrData(listAttr, EnumFileType.ResponseFile);
          return strNew.Trim() + "\r\n" + genKey;

        case "/*[GEN-21]*/":
          strNew = GetAttrSetData(listAttr, EnumFileType.ResponseFile);
          return strNew.Trim() + "\r\n" + genKey;

        default:
          return "";
      }
    }

    string GetJoinEntityForeignKey(string table1, string table2, string fieldName)
    {
      return $@"
      modelBuilder.Entity<{table1}Entity>()
            .HasOne(e => e.Item{table2})
            .WithMany()
            .HasForeignKey(e => e.{fieldName})
            .OnDelete(DeleteBehavior.SetNull);
";
    }
    string GetJoinEntityST(string table1, string table2, string fieldName)
    {
      return $@"
      modelBuilder.Entity<{table1}Entity>()
          .HasMany(e => e.{fieldName})
          .WithMany(e => e.List{table1})
          .UsingEntity<Dictionary<string, object>>(
                ""ref{table1}{table2}"",
                j => j
                    .HasOne<{table2}Entity>()
                    .WithMany()
                    .HasForeignKey(""{table2}Id"")
                    .OnDelete(DeleteBehavior.SetNull),
                j => j
                    .HasOne<{table1}Entity>()
                    .WithMany()
                    .HasForeignKey(""{table1}Id"")
                    .OnDelete(DeleteBehavior.SetNull));
";
    }

    Dictionary<string, bool> dicIdsAttr = null;
    private void GetDicIdsAttr(List<CustomAttr> listAttr)
    {
      if (dicIdsAttr == null)
      {
        dicIdsAttr = new Dictionary<string, bool>();
        foreach (CustomAttr attr in listAttr)
        {
          foreach (var anno in attr.ListAnnotation)
          {
            if (anno != null && anno.Name == nameof(ListEntityAttribute))
            {
              if (!dicIdsAttr.ContainsKey(anno.Name))
              {
                dicIdsAttr.Add(anno.Param2, true);
              }
            }
          }
        }
      }
      return;
    }
    private string GetAttrData(List<CustomAttr> listAttr, EnumFileType fileType)
    {
      GetDicIdsAttr(listAttr);

      StringBuilder sb = new StringBuilder();
      bool setFlag = false;
      switch (fileType)
      {
        case EnumFileType.RequestFile:
          foreach (CustomAttr attr in listAttr)
          {
            setFlag = false;
            if (dicIdsAttr.ContainsKey(attr.Name))
            {
              continue;
            }
            if (attr.Name == "void")
            {
              continue;
            }

            foreach (var anno in attr.ListAnnotation)
            {
              if (setFlag)
              {
                break;
              }
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                sb.Append($"    public List<long>? {attr.Name} {{ get; set; }}\r\n");
                setFlag = true;
              }
              else if (anno != null
                && anno.Name == nameof(FormDataTypeAttribute)
                && anno.Param1 == nameof(EnumFormDataType.dateOnly))
              {
                string lowAttrName = StringUtil.LowerFirstLetter(attr.Name);
                sb.Append($@"
    DateTime? _{lowAttrName} {{ get; set; }}
    public string? {attr.Name}
    {{
      get
      {{
        if (_{lowAttrName}.HasValue)
        {{
          return _{lowAttrName}.Value.ToString(ConstDateTimeFormat.YYYYMMDD);
        }}
        return null;
      }}
      set
      {{
        if (!string.IsNullOrEmpty(value))
        {{
          try
          {{
            _{lowAttrName} = DateTimeUtil.GetDateTimeFromString(value, ConstDateTimeFormat.YYYYMMDD);
          }}
          catch
          {{
            _{lowAttrName} = null;
          }}
        }}
      }}
    }}
");
                setFlag = true;
              }
              else if (anno != null
                && anno.Name == nameof(FormDataTypeAttribute)
                && anno.Param1 == nameof(EnumFormDataType.image))
              {
                sb.Append($"    public IFormFile? {attr.Name} {{ get; set; }}\r\n");
                setFlag = true;
              }
            }

            if (!setFlag)
            {
              string isNullStr = attr.IsNullable ? "?" : "";
              sb.Append($"    public {attr.Type}{isNullStr} {attr.Name} {{ get; set; }}\r\n");
            }
          }
          return sb.ToString();
        case EnumFileType.ResponseFile:
          foreach (CustomAttr attr in listAttr)
          {
            setFlag = false;
            if (dicIdsAttr.ContainsKey(attr.Name))
            {
              continue;
            }
            if (attr.Name == "void")
            {
              continue;
            }
            foreach (var anno in attr.ListAnnotation)
            {
              if (setFlag)
              {
                break;
              }
              if (anno != null
                && anno.Name == nameof(FormDataTypeAttribute)
                && anno.Param1 == nameof(EnumFormDataType.password))
              {
                setFlag = true;
              }
              else if (anno != null
              && anno.Name == nameof(FormDataTypeAttribute)
              && anno.Param1 == nameof(EnumFormDataType.dateOnly))
              {
                string lowAttrName = StringUtil.LowerFirstLetter(attr.Name);
                sb.Append($@"
    DateTime? _{lowAttrName} {{ get; set; }}
    public string? {attr.Name}
    {{
      get
      {{
        if (_{lowAttrName}.HasValue)
        {{
          return _{lowAttrName}.Value.ToString(ConstDateTimeFormat.YYYYMMDD);
        }}
        return null;
      }}
      set
      {{
        if (!string.IsNullOrEmpty(value))
        {{
          try
          {{
            _{lowAttrName} = DateTimeUtil.GetDateTimeFromString(value, ConstDateTimeFormat.YYYYMMDD);
          }}
          catch
          {{
            _{lowAttrName} = null;
          }}
        }}
      }}
    }}
");
                setFlag = true;
              }
            }

            if (!setFlag)
            {
              string isNullStr = attr.IsNullable ? "?" : "";
              sb.Append($"    public {attr.Type}{isNullStr} {attr.Name} {{ get; set; }}\r\n");
            }
          }
          return sb.ToString();
      }
      return "";
    }

    private string GetAttrSetData(List<CustomAttr> listAttr, EnumFileType fileType)
    {
      StringBuilder sb = new StringBuilder();
      bool setFlag = false;
      switch (fileType)
      {
        case EnumFileType.RequestFile:
          foreach (CustomAttr attr in listAttr)
          {
            setFlag = false;
            if (dicIdsAttr.ContainsKey(attr.Name))
            {
              continue;
            }
            if (attr.Name == "void")
            {
              continue;
            }

            foreach (var anno in attr.ListAnnotation)
            {
              if (setFlag)
              {
                break;
              }
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                sb.Append($@"
      if (this.{attr.Name} != null)
      {{
        entity.{anno.Param2} = this.{attr.Name}.Select(x => x).ToList();
      }}
");
                setFlag = true;
              }
              else if (anno != null
                && anno.Name == nameof(FormDataTypeAttribute)
                && anno.Param1 == nameof(EnumFormDataType.dateOnly))
              {
                sb.Append($"      if (this._{attr.Name.LowerFirstLetter()} != null) entity.{attr.Name} = this._{attr.Name.LowerFirstLetter()};\r\n");
                setFlag = true;
              }
              else if (anno != null
                && anno.Name == nameof(FormDataTypeAttribute)
                && anno.Param1 == nameof(EnumFormDataType.image))
              {
                setFlag = true;
              }
            }

            if (!setFlag)
            {
              sb.Append($"      if (this.{attr.Name} != null) entity.{attr.Name} = this.{attr.Name};\r\n");
            }
          }
          return sb.ToString();
        case EnumFileType.ResponseFile:
          foreach (CustomAttr attr in listAttr)
          {
            setFlag = false;
            if (dicIdsAttr.ContainsKey(attr.Name))
            {
              continue;
            }
            if (attr.Name == "void")
            {
              continue;
            }

            foreach (var anno in attr.ListAnnotation)
            {
              if (setFlag)
              {
                break;
              }
              if (anno != null && anno.Name == nameof(ListEntityAttribute))
              {
                sb.Append($@"
      if (entity.{attr.Name} != null)
      {{
        this.{attr.Name} = entity.{attr.Name}.Select(x => x).ToList();
      }}
");
                setFlag = true;
              }
              else if (anno != null
                && anno.Name == nameof(FormDataTypeAttribute)
                && anno.Param1 == nameof(EnumFormDataType.password))
              {
                setFlag = true;
              }
              else if (anno != null
                && anno.Name == nameof(FormDataTypeAttribute)
                && anno.Param1 == nameof(EnumFormDataType.dateOnly))
              {
                sb.Append($"      this._{attr.Name.LowerFirstLetter()} = entity.{attr.Name};\r\n");
                setFlag = true;
              }
            }

            if (!setFlag)
            {
              sb.Append($"      this.{attr.Name} = entity.{attr.Name};\r\n");
            }
          }
          return sb.ToString();
      }
      return "";
    }

    private void UpdateDBContextFile(string entityName, List<CustomAttr> listAttr)
    {
      string filePath = "./../SourceBaseBE.Database/DBContexts/CommonDbContext.cs";
      string fileContent = File.ReadAllText(filePath);
      string newStr = $"    public virtual DbSet<{entityName}Entity> {entityName}s {{ get; set; }}";
      if (!IsExitsFileContent(fileContent, newStr))
      {
        fileContent = Regex.Replace(
          fileContent,
          @"(protected IConfiguration Configuration \{ get; set; \})",
          "$1\r\n" + newStr);
      }

      List<string> listKeyGen = new List<string>()
      {
        "/*[GEN-15]*/",
        "/*[GEN-16]*/",
      };
      foreach (string keyGen in listKeyGen)
      {
        newStr = GetGenData(listAttr, keyGen, fileContent);
        if (!IsExitsFileContent(fileContent, newStr))
        {
          fileContent = fileContent.Replace(keyGen, newStr);
        }
      }
      File.WriteAllText(filePath, fileContent);
    }

    private bool IsExitsFileContent(string fileContent, string newStr)
    {
      string fileContentTemp = Regex.Replace(fileContent + "", @"\s", "");
      string searchStr = Regex.Replace(newStr, @"\s", "");
      if (fileContentTemp.IndexOf(searchStr) == -1)
      {
        return false;
      }
      return true;
    }

    private void UpdateStartupFile(string entityName)
    {
      string filePath = "./../SourceBaseBE.MainService/Startup.cs";
      string fileContent = File.ReadAllText(filePath);
      string newStr = $"      services.AddScoped<{entityName}Service>();";
      if (!IsExitsFileContent(fileContent, newStr))
      {
        fileContent = Regex.Replace(
        fileContent,
        @"(services\.AddScoped\<GenTemplateService\>\(\);)",
        "$1\r\n" + newStr);
        File.WriteAllText(filePath, fileContent);
      }
    }
  }
}