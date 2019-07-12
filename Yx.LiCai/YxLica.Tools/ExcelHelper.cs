using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;
using Row = DocumentFormat.OpenXml.Spreadsheet.Row;


namespace YxLiCai.Tools
{
    public class ExcelHelper
    {
        public DataSet ChooseReadMethod(string fullName)
        {
            var ds = new DataSet();

            if (Path.GetExtension(fullName) == ".xls")
            {
                // XLS - Excel 2003 or older
                ds = ReadExcelOleDB(fullName);
            }
            else if (Path.GetExtension(fullName) == ".xlsx")
            {
                // XLSX - Excel 2007 or later
                ds = ReadExceOpenXml(fullName);
            }

            return ds;
        }
        /// <summary>
        /// 读Excel数据，返回DataSet        
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        private DataSet ReadExcelOleDB(string fullName)
        {
            var ds = new DataSet();
            var props = new Dictionary<string, string>();
            props["Provider"] = "Microsoft.Jet.OLEDB.4.0";
            props["Data Source"] = fullName;
            props["Extended Properties"] = "'Excel 8.0;CharacterSet=UTF8;HDR=No;IMEX=1'";

            var sb = new StringBuilder();
            foreach (KeyValuePair<string, string> prop in props)
            {
                sb.Append(prop.Key);
                sb.Append('=');
                sb.Append(prop.Value);
                sb.Append(';');
            }

            var connectionString = sb.ToString();

            using (var conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                var cmd = new OleDbCommand {Connection = conn};

                // Get all Sheets in Excel File
                var dtSheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                // Loop through all Sheets to get data
                foreach (DataRow dr in dtSheet.Rows)
                {
                    var sheetName = dr["TABLE_NAME"].ToString();

                    if (!sheetName.EndsWith("$"))
                        continue;

                    // Get all rows from the Sheet
                    cmd.CommandText = "SELECT * FROM [" + sheetName + "]";
                    var dt = new DataTable();
                    //dt.TableName = sheetName;
                    var da = new OleDbDataAdapter(cmd);
                    da.Fill(dt);
                    ds.Tables.Add(dt);
                }

                cmd = null;
                conn.Close();
            }

            return ds;
        }
        /// <summary>
        /// 打开的XMl
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private DataSet ReadExceOpenXml(string fileName)
        {
            var ds = new DataSet();
            var dt = new DataTable();

            using (var spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadSheetDocument.WorkbookPart;
                var worksheetPart = workbookPart.WorksheetParts.First();
                var sheet = worksheetPart.Worksheet;

                foreach (var cell in sheet.Descendants<Row>().Last())
                {
                    dt.Columns.Add(null, typeof(string));
                }

                foreach (var row in sheet.Descendants<Row>()) //this will also include the header row...
                {
                    var newRow = dt.NewRow();

                    for (var i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        if (row.RowIndex == 1)
                        {
                            newRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                        }
                        else
                        {
                            var cellValue = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                            if (cellValue.StartsWith("+"))
                            {
                                newRow[i] = cellValue + "=" + row.Descendants<Cell>().ElementAt(i + 2).CellFormula.Text;
                            }
                            else
                            {
                                newRow[i] = cellValue;
                            }
                        }
                    }
                    dt.Rows.Add(newRow);
                }
            }
            ds.Tables.Add(dt);

            return ds;
        }
        /// <summary>
        /// cell 值
        /// </summary>
        /// <param name="document"></param>
        /// <param name="cell"></param>
        /// <returns></returns>
        private string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            var stringTablePart = document.WorkbookPart.SharedStringTablePart;
            var cellValue = "";

            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
            {
                cellValue = stringTablePart.SharedStringTable.ChildElements[Int32.Parse(cell.CellValue.Text)].InnerText;
            }
            else if (cell.CellValue != null)
            {
                cellValue = cell.CellValue.Text;
            }
            else if (cell.DataType == null && cell.CellValue == null)
            {
                cellValue = "";
            }

            return cellValue;
        }
        /// <summary>
        /// 读Excel
        /// </summary>
        /// <param name="fileName"></param>
        private void ReadExcelSAX(string fileName)
        {
            var cellList = new List<string>();
            using (var spreadsheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = spreadsheetDocument.WorkbookPart;
                var worksheetPart = workbookPart.WorksheetParts.First();

                var reader = OpenXmlReader.Create(worksheetPart);
                while (reader.Read())
                {
                    var text = reader.GetText();
                    cellList.Add(text);
                }
            }
            Console.WriteLine("Completed");
        }
        /// <summary>
        /// 导出到Excel 
        /// </summary>
        /// <returns></returns>
        public void ExportExcel(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            var xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                return;
            }
            System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture;
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
            Microsoft.Office.Interop.Excel.Range range;
            long totalCount = dt.Rows.Count;
            long rowRead = 0;
            float percent = 0;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
                range = (Microsoft.Office.Interop.Excel.Range)worksheet.Cells[1, i + 1];
                range.Interior.ColorIndex = 15;
                range.Font.Bold = true;
            }
            for (int r = 0; r < dt.Rows.Count; r++)
            {
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
                }
                rowRead++;
                percent = ((float)(100 * rowRead)) / totalCount;
            }
            xlApp.Visible = true;
        }
    }
}