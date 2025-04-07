using Azure;
using elFinder.NetCore;
using iSoft.Common;
using iSoft.Common.Utils;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using OfficeOpenXml;
using Serilog;
using SourceBaseBE.Database.Entities;
using System;
using System.Collections.Generic;
using System.IO;

namespace SourceBaseBE.MainService.CommonFuncNS
{
    public class CommonFuncMainService
    {

        public static void ReplaceTemplateFile(string newFilePath)
        {
            bool deletedFlag = false;
            string fileBKPath = string.Format(ConstMain.ConstTemplateBKFilePath, DateTimeUtil.GetDateTimeStr(DateTime.Now, "yyyyMMdd_HHmmss"));
            try
            {
                FileUtil.CopyFile(ConstMain.ConstTemplateFilePath, fileBKPath);
                File.Delete(ConstMain.ConstTemplateFilePath);
                deletedFlag = true;
                FileUtil.CopyFile(newFilePath, ConstMain.ConstTemplateFilePath);
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.LogMsg(Messages.ErrException, ex);
                if (deletedFlag)
                {
                    try
                    {
                        FileUtil.CopyFile(fileBKPath, ConstMain.ConstTemplateFilePath);
                    }
                    catch (Exception ex2)
                    {
                        Serilog.Log.Logger.Information("Recover template file error");
                        Serilog.Log.Logger.LogMsg(Messages.ErrException, ex2);
                    }
                }
                throw ex;
            }
        }


        public static int GetCountRow(ExcelWorksheet sheet1, string cellName)
        {
            var maxCellName = GetMaxSearchCell(sheet1, cellName);
            var colIndex = ConvertUtil.GetColumnIndex(cellName);
            var beginRowIndex = ConvertUtil.GetRowIndex(cellName);
            var endRowIndex = ConvertUtil.GetRowIndex(maxCellName);
            var rowIndex = GetValueRowIndex(sheet1, colIndex, beginRowIndex, endRowIndex);
            int count = rowIndex - beginRowIndex + 1;
            return count;
        }
        public static int GetValueRowIndex(ExcelWorksheet sheet1, int colIndex, int beginRowIndex, int endRowIndex)
        {
            int checkRowIndex = (beginRowIndex + endRowIndex) / 2;

            if (beginRowIndex == endRowIndex)
            {
                return beginRowIndex;
            }
            if (beginRowIndex > endRowIndex)
            {
                return beginRowIndex;
            }

            object value1 = sheet1.Cells[checkRowIndex, colIndex].Value;
            object value2 = sheet1.Cells[checkRowIndex + 1, colIndex].Value;
            if (!(value1 == null || value1.ToString().IsNullOrEmpty())
                && (value2 == null || value2.ToString().IsNullOrEmpty()))
            {
                return checkRowIndex;
            }
            else
            {
                if (value1 == null || value1.ToString().IsNullOrEmpty())
                {
                    return GetValueRowIndex(sheet1, colIndex, beginRowIndex, checkRowIndex);
                }
                else
                {
                    return GetValueRowIndex(sheet1, colIndex, checkRowIndex, endRowIndex);
                }
            }
        }
        public static string GetMaxSearchCell(ExcelWorksheet sheet1, string cellName, int step = 1)
        {
            string checkCellName = ConvertUtil.GetCellNext(cellName, Direct.Down, step);
            object value = sheet1.Cells[checkCellName].Value;
            if (value == null || value.ToString().IsNullOrEmpty())
            {
                return checkCellName;
            }
            else
            {
                return GetMaxSearchCell(sheet1, checkCellName, step * 2);
            }
        }

        public static List<DepartmentEntity> RemoveDuplicatesUsingLinq(List<DepartmentEntity> departmentRoles)
        {
            Dictionary<string, bool> dicExists = new Dictionary<string, bool>();
            for (int i = departmentRoles.Count - 1; i >= 0; i--)
            {
                if (dicExists.ContainsKey(departmentRoles[i].Name))
                {
                    departmentRoles.RemoveAt(i);
                }
                else
                {
                    dicExists.Add(departmentRoles[i].Name, true);
                }
            }
            return departmentRoles;
        }
    }
}
