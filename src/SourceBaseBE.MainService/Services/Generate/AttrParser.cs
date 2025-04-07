using iSoft.DBLibrary.DBConnections.Factory;
using iSoft.Common.Enums.DBProvider;
using Serilog;
using iSoft.Common.ConfigsNS;
using SourceBaseBE.Database.Repository;
using SourceBaseBE.Database.DBContexts;
using SourceBaseBE.Database.Entities;
using iSoft.Common.Models.RequestModels;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text.RegularExpressions;
using ConnectionCommon.Connection;
using Nest;
using System.Text;
using iSoft.Common.Utils;

namespace SourceBaseBE.MainService.Services.Generate
{
  public class AttrParser
  {
    public List<CustomAttr> ParseAttributes(string input)
    {

      List<CustomAttr> attributes = new List<CustomAttr>();

      string patternAttr = @"^\s*public( virtual)? (?<type>[\<\>\w]+\??) (?<name>\w+)";
      string patternAnno = @"^\s*\[(?<name>\w+)\(nameof\((?<param1>[^\(\)\,\[\]]+)\)\, nameof\((?<param2>[^\(\)\,\[\]]+)\)\,[^\[\]]+\)\]";
      string patternAnnoForeignkey = @"^\s*\[(?<name>\w+)\(nameof\((?<param1>[^\(\)\,\[\]]+)\)\)\]";
      string patternAnnoFormData = @"^\s*\[(?<name>\w+)\(EnumFormDataType\.(?<param1>[\w\d_]+)[\,\s]+[^\[\]\(\)]*\)\]";
      string patternAnnoOther = @"^\s*\[(?<name>\w+)[^\[\]]*\]";

      string[] lines = input.Split("\n", StringSplitOptions.RemoveEmptyEntries);

      List<CustomAnno> listAnnotation = new List<CustomAnno>();

      bool matchedFlag = false;
      foreach (string line in lines)
      {
        matchedFlag = false;
        Match matchAttribute = Regex.Match(line, patternAttr);
        if (!matchedFlag && matchAttribute.Success)
        {
          string name = matchAttribute.Groups["name"].Value;
          string type = matchAttribute.Groups["type"].Value;
          bool isNullable = type.EndsWith("?");
          type = type.TrimEnd('?');

          if (type != "class")
          {
            attributes.Add(new CustomAttr
            {
              Name = name,
              Type = type,
              IsNullable = isNullable,
              ListAnnotation = listAnnotation
            });
            listAnnotation = new List<CustomAnno>();
            matchedFlag = true;
          }
        }


        Match matchAnnotation = Regex.Match(line, patternAnno);
        if (!matchedFlag && matchAnnotation.Success)
        {
          string name = matchAnnotation.Groups["name"].Value;
          string param1 = matchAnnotation.Groups["param1"].Value;
          string param2 = matchAnnotation.Groups["param2"].Value;

          listAnnotation.Add(new CustomAnno
          {
            Name = name,
            Param1 = param1,
            Param2 = param2,
          });
          matchedFlag = true;
        }

        Match matchAnnoForeignkey = Regex.Match(line, patternAnnoForeignkey);
        if (!matchedFlag && matchAnnoForeignkey.Success)
        {
          string name = matchAnnoForeignkey.Groups["name"].Value;
          string param1 = matchAnnoForeignkey.Groups["param1"].Value;

          listAnnotation.Add(new CustomAnno
          {
            Name = name,
            Param1 = param1,
          });
          matchedFlag = true;
        }

        Match matchAnnoFormData = Regex.Match(line, patternAnnoFormData);
        if (!matchedFlag && matchAnnoFormData.Success)
        {
          string name = matchAnnoFormData.Groups["name"].Value;
          string param1 = matchAnnoFormData.Groups["param1"].Value;

          listAnnotation.Add(new CustomAnno
          {
            Name = name,
            Param1 = param1,
          });
          matchedFlag = true;
        }

        Match matchAnnoOther = Regex.Match(line, patternAnnoOther);
        if (!matchedFlag && matchAnnoOther.Success)
        {
          string name = matchAnnoOther.Groups["name"].Value;

          listAnnotation.Add(new CustomAnno
          {
            Name = name,
          });
          matchedFlag = true;
        }
      }

      return attributes;
    }
  }
}