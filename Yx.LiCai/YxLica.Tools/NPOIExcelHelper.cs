using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HPSF;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using NPOI.SS.UserModel;
using NServiceKit.Logging;

namespace YxLica.Tools
{
    public class NPOIExcelHelper
    {
         /// <summary>
         /// DataTable导出到Excel文件
         /// </summary>
         /// <param name="dtSource">源DataTable</param>
         /// <param name="strHeaderText">表头文本</param>
         /// <param name="strFileName">保存位置</param>
         public static byte[] DataTableToExcel(DataTable dtSource, string strFileName)
         {
             using (MemoryStream ms = DataTableToExcel(dtSource))
             {
                 byte[] data = ms.ToArray();

                 return data;
             }
         }
  
         /// <summary>
         /// DataTable导出到Excel的MemoryStream
         /// </summary>
         /// <param name="dtSource">源DataTable</param>
         /// <param name="strHeaderText">表头文本</param>
         public static MemoryStream DataTableToExcel(DataTable dtSource)
         {
             HSSFWorkbook workbook = new HSSFWorkbook();
             HSSFSheet sheet = (HSSFSheet)workbook.CreateSheet();
             HSSFCellStyle dateStyle = (HSSFCellStyle)workbook.CreateCellStyle();
             HSSFDataFormat format = (HSSFDataFormat)workbook.CreateDataFormat();
             dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

             try
             {
                 //取得列宽
                 int[] arrColWidth = new int[dtSource.Columns.Count];
                 foreach (DataColumn item in dtSource.Columns)
                 {
                     arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
                 }
                 for (int i = 0; i < dtSource.Rows.Count; i++)
                 {
                     for (int j = 0; j < dtSource.Columns.Count; j++)
                     {
                         int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                         if (intTemp > arrColWidth[j])
                         {
                             arrColWidth[j] = intTemp;
                         }
                     }
                 }
                 int rowIndex = 0;
                 foreach (DataRow row in dtSource.Rows)
                 {
                     #region 新建表，填充表头，填充列头，样式
                     if (rowIndex == 65535 || rowIndex == 0)
                     {
                         if (rowIndex != 0)
                         {
                             sheet = (HSSFSheet)workbook.CreateSheet();
                         }

                         #region 表头及样式
                         //{
                         //    HSSFRow headerRow = (HSSFRow)sheet.CreateRow(0);
                         //    headerRow.HeightInPoints = 25;
                         //    headerRow.CreateCell(0).SetCellValue(strHeaderText);
                         //    HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                         //    HSSFFont font = (HSSFFont)workbook.CreateFont();
                         //    font.FontHeightInPoints = 20;
                         //    font.Boldweight = 700;
                         //    headStyle.SetFont(font);
                         //    headerRow.GetCell(0).CellStyle = headStyle;
                         //}
                         #endregion

                         #region 列头及样式
                         {
                             HSSFRow headerRow = (HSSFRow)sheet.CreateRow(1);
                             HSSFCellStyle headStyle = (HSSFCellStyle)workbook.CreateCellStyle();
                             //headStyle.Alignment = CellHorizontalAlignment.CENTER;
                             HSSFFont font = (HSSFFont)workbook.CreateFont();
                             font.FontHeightInPoints = 10;
                             font.Boldweight = 700;
                             headStyle.SetFont(font);
                             foreach (DataColumn column in dtSource.Columns)
                             {
                                 headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                                 headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                                 //设置列宽
                                 sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                             }
                             // headerRow.Dispose();
                         }
                         #endregion

                         rowIndex = 2;
                     }
                     #endregion

                     #region 填充内容
                     HSSFRow dataRow = (HSSFRow)sheet.CreateRow(rowIndex);
                     foreach (DataColumn column in dtSource.Columns)
                     {
                         HSSFCell newCell = (HSSFCell)dataRow.CreateCell(column.Ordinal);

                         string drValue = row[column].ToString();

                         switch (column.DataType.ToString())
                         {
                             case "System.String"://字符串类型
                                 newCell.SetCellValue(drValue);
                                 break;
                             case "System.DateTime"://日期类型
                                 System.DateTime dateV;
                                 System.DateTime.TryParse(drValue, out dateV);
                                 newCell.SetCellValue(dateV);

                                 newCell.CellStyle = dateStyle;//格式化显示
                                 break;
                             case "System.Boolean"://布尔型
                                 bool boolV = false;
                                 bool.TryParse(drValue, out boolV);
                                 newCell.SetCellValue(boolV);
                                 break;
                             case "System.Int16"://整型
                             case "System.Int32":
                             case "System.Int64":
                             case "System.Byte":
                                 int intV = 0;
                                 int.TryParse(drValue, out intV);
                                 newCell.SetCellValue(intV);
                                 break;
                             case "System.Decimal"://浮点型
                             case "System.Double":
                                 double doubV = 0;
                                 double.TryParse(drValue, out doubV);
                                 newCell.SetCellValue(doubV);
                                 break;
                             case "System.DBNull"://空值处理
                                 newCell.SetCellValue("");
                                 break;
                             default:
                                 newCell.SetCellValue("");
                                 break;
                         }

                     }
                     #endregion

                     rowIndex++;
                 }
                 using (MemoryStream ms = new MemoryStream())
                 {
                     workbook.Write(ms);
                     ms.Flush();
                     ms.Position = 0;

                     return ms;
                 }
             }
             catch (Exception ex)
             {
                 ILog log = LogManager.GetLogger("Exception Log");
                 log.Error(ex.Message + Environment.NewLine + ex.StackTrace);

                 return null;
             }
             finally
             {
                 workbook = null;
             }
         }

        /// <summary>
        /// Excel文件导成Datatable
        /// </summary>
        /// <param name="strFilePath">Excel文件目录地址</param>
        /// <param name="strTableName">Datatable表名</param>
        /// <param name="iSheetIndex">Excel sheet index</param>
        /// <returns></returns>
        public static DataTable XlSToDataTable(string strFilePath, string strTableName, int iSheetIndex)
        {

            string strExtName = Path.GetExtension(strFilePath);

            DataTable dt = new DataTable();
            if (!string.IsNullOrEmpty(strTableName))
            {
                dt.TableName = strTableName;
            }

            if (strExtName.Equals(".xls") || strExtName.Equals(".xlsx"))
            {
                using (FileStream file = new FileStream(strFilePath, FileMode.Open, FileAccess.Read))
                {
                    HSSFWorkbook workbook = new HSSFWorkbook(file);
                    ISheet sheet = workbook.GetSheetAt(iSheetIndex);

                    //列头
                    foreach (ICell item in sheet.GetRow(sheet.FirstRowNum).Cells)
                    {
                        dt.Columns.Add(item.ToString(), typeof(string));
                    }

                    //写入内容
                    System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                    while (rows.MoveNext())
                    {
                        IRow row = (HSSFRow)rows.Current;
                        if (row.RowNum == sheet.FirstRowNum)
                        {
                            continue;
                        }

                        DataRow dr = dt.NewRow();
                        foreach (ICell item in row.Cells)
                        {
                            switch (item.CellType)
                            {
                                case CellType.Boolean:
                                    dr[item.ColumnIndex] = item.BooleanCellValue;
                                    break;
                                case CellType.Error:
                                    dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                    break;
                                case CellType.Formula:
                                    switch (item.CachedFormulaResultType)
                                    {
                                        case CellType.Boolean:
                                            dr[item.ColumnIndex] = item.BooleanCellValue;
                                            break;
                                        case CellType.Error:
                                            dr[item.ColumnIndex] = ErrorEval.GetText(item.ErrorCellValue);
                                            break;
                                        case CellType.Numeric:
                                            if (DateUtil.IsCellDateFormatted(item))
                                            {
                                                dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = item.NumericCellValue;
                                            }
                                            break;
                                        case CellType.String:
                                            string str = item.StringCellValue;
                                            if (!string.IsNullOrEmpty(str))
                                            {
                                                dr[item.ColumnIndex] = str.ToString();
                                            }
                                            else
                                            {
                                                dr[item.ColumnIndex] = null;
                                            }
                                            break;
                                        case CellType.Unknown:
                                        case CellType.Blank:
                                        default:
                                            dr[item.ColumnIndex] = string.Empty;
                                            break;
                                    }
                                    break;
                                case CellType.Numeric:
                                    if (DateUtil.IsCellDateFormatted(item))
                                    {
                                        dr[item.ColumnIndex] = item.DateCellValue.ToString("yyyy-MM-dd hh:MM:ss");
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = item.NumericCellValue;
                                    }
                                    break;
                                case CellType.String:
                                    string strValue = item.StringCellValue;
                                    if (string.IsNullOrEmpty(strValue))
                                    {
                                        dr[item.ColumnIndex] = strValue.ToString();
                                    }
                                    else
                                    {
                                        dr[item.ColumnIndex] = null;
                                    }
                                    break;
                                case CellType.Unknown:
                                case CellType.Blank:
                                default:
                                    dr[item.ColumnIndex] = string.Empty;
                                    break;
                            }
                        }
                        dt.Rows.Add(dr);
                    }
                }
            }

            return dt;
        }
    }
}
