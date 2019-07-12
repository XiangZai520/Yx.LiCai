using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HPSF;

namespace YxLiCai.Tools.Util
{
    /// <summary>
    /// Excel帮助类
    /// </summary>
    public static class ExcelHelper
    {
        /// <summary>
        /// Excel表格支持的最大行数
        /// </summary>
        const int MAX_ROW_COUNT = 65535;

        /// <summary>
        /// 编码页索引
        /// </summary>
        const int CODEPAGE = 936;

        #region 设置属性名
        /// <summary>
        /// 列宽属性值(一般的=数据长度*256)
        /// </summary>
        const string COLUMN_WIDTH_PROPERTY_NAME = "WIDTH";

        /// <summary>
        /// 列格式属性值(主要用于日期格式显示)
        /// </summary>
        const string COLUMN_FORMAT_PROPERTY_NAME = "FORMAT";

        /// <summary>
        /// 列水平方向对齐方式属性值（参考NPOI.SS.UserModel.HorizontalAlignment枚举定义）
        /// </summary>
        const string COLUMN_ALIGN_PROPERTY_NAME = "ALIGN";

        /// <summary>
        /// 列垂直方向对齐方式属性值
        /// </summary>
        const string COLUMN_VALIGN_PROPERTY_NAME = "VALIGN";

        /// <summary>
        /// 单元格填充模式属性值
        /// </summary>
        const string COLUMN_FILL_PATTERN_PROPERTY_NAME = "FILLPATTERN";

        /// <summary>
        /// 单元格前景色属性值
        /// </summary>
        const string COLUMN_COLOR_PROPERTY_NAME = "COLOR";

        /// <summary>
        /// 单元格背景色属性值
        /// </summary>
        const string COLUMN_BGCOLOR_PROPERTY_NAME = "BGCOLOR";

        /// <summary>
        /// 是否自动收缩
        /// </summary>
        const string COLUMN_SHRINK_TO_FIT_PROPERTY_NAME = "SHRINKTOFIT";

        /// <summary>
        /// 旋转角度
        /// </summary>
        const string COLUMN_ROTATION_PROPERTY_NAME = "ROTATION";

        /// <summary>
        /// 是否自动换行
        /// </summary>
        const string COLUMN_WRAP_TEXT_PROPERTY_NAME = "WRAPTEXT";

        #region 字体属性
        /// <summary>
        /// 粗体
        /// </summary>
        const string FONT_BOLD_WEIGHT_PROPERTY_NAME = "BOLDWEIGHT";
        /// <summary>
        /// 字符集
        /// </summary>
        const string FONT_CHARSET_PROPERTY_NAME = "CHARSET";
        /// <summary>
        /// 字体颜色
        /// </summary>
        const string FONT_COLOR_PROPERTY_NAME = "FCOLOR";
        /// <summary>
        /// 字体高度
        /// </summary>
        const string FONT_HEIGHT_PROPERTY_NAME = "FHEIGHT";
        /// <summary>
        /// 字体高度
        /// </summary>
        const string FONT_HEIGHT_IN_POINTS_PROPERTY_NAME = "FHINPOINTS";
        /// <summary>
        /// 字体名称
        /// </summary>
        const string FONT_NAME_PROPERTY_NAME = "FNAME";
        /// <summary>
        /// 是否斜体
        /// </summary>
        const string FONT_IS_ITALIC_PROPERTY_NAME = "FISITALIC";
        /// <summary>
        /// 是否删除线
        /// </summary>
        const string FONT_IS_STRIKEOUT_PROPERTY_NAME = "FISSTRIKEOUT";
        /// <summary>
        /// 上档
        /// </summary>
        const string FONT_SUPERSCRIPT_PROPERTY_NAME = "FSUPERSCRIPT";
        /// <summary>
        /// 下划线样式
        /// </summary>
        const string FONT_UNDERLINETYPE_PROPERTY_NAME = "FUNDERLINETYPE";
        #endregion

        /// <summary>
        /// 列样式属性值
        /// </summary>
        const string COLUMN_STYLE_PROPERTY_NAME = "STYLE";
        #endregion

        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        /// <Author>柳永法 http://www.yongfa365.com/ 2010-5-8 22:21:41</Author>
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
            {
                using (MemoryStream ms = Export(dtSource, strHeaderText))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <Author>柳永法 http://www.yongfa365.com/ 2010-5-8 22:21:41</Author>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            System.Diagnostics.Debug.Assert(dtSource != null);

            HSSFWorkbook workbook = new HSSFWorkbook();

            #region 右击文件 属性信息
            AddDocumentComment(strHeaderText, workbook);
            #endregion

            ISheet sheet = null;
            IRow dataRow = null;

            ReadColumnSettings(workbook, dtSource.Columns);

            sheet = workbook.CreateSheet();

            int rowIndex = NewSheet(dtSource, strHeaderText, workbook, sheet);

            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表
                if (rowIndex == MAX_ROW_COUNT)
                {
                    sheet = workbook.CreateSheet();
                    rowIndex = NewSheet(dtSource, strHeaderText, workbook, sheet);
                }
                #endregion

                #region 填充内容
                dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    FillCell(dataRow.CreateCell(column.Ordinal), row, column);
                }
                #endregion

                rowIndex++;
            }

            MemoryStream ms = new MemoryStream();
            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            return ms;
        }

        /// <summary>
        /// 绘制新的表格，填充表头，填充列头，样式
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="strHeaderText">表头</param>
        /// <param name="workbook">工作簿</param>
        /// <param name="sheet">表格</param>
        /// <returns>当前行索引(从0开始)</returns>
        private static int NewSheet(DataTable dtSource, string strHeaderText, HSSFWorkbook workbook, ISheet sheet)
        {
            int rowIndex = 0;

            #region 表头及样式
            if (SetDocumentHeadStyle(dtSource.Columns.Count, strHeaderText, workbook, sheet, rowIndex))
            {
                rowIndex++;
            }
            #endregion

            #region 列头及样式
            SetDocumentColumnStyle(dtSource, workbook, sheet, rowIndex);
            #endregion

            //从第3行开始填充数据
            rowIndex++;

            return rowIndex;
        }

        #region private

        /// <summary>
        /// 读取列设置
        /// </summary>
        /// <param name="dc">列</param>
        private static void ReadColumnSettings(HSSFWorkbook workbook,DataColumnCollection dcs)
        {
            foreach (DataColumn dc in dcs)
            {
                if (string.IsNullOrEmpty(dc.Caption))
                {
                    dc.Caption = dc.ColumnName;
                }

                ResolveSettings(workbook, dc);
            }
        }

        /// <summary>
        /// 解析列配置
        /// </summary>
        /// <param name="workbook">工作簿</param>
        /// <param name="dc">数据列</param>
        private static void ResolveSettings(HSSFWorkbook workbook, DataColumn dc)
        {
            ICellStyle cs = null;
            object obj = null;
            //格式
            if (dc.ExtendedProperties.ContainsKey(COLUMN_FORMAT_PROPERTY_NAME)
                && dc.ExtendedProperties[COLUMN_FORMAT_PROPERTY_NAME] != null)
            {
                cs = workbook.CreateCellStyle();
                obj = dc.ExtendedProperties[COLUMN_FORMAT_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(COLUMN_FORMAT_PROPERTY_NAME);
                if (obj is short)
                {
                    cs.DataFormat = short.Parse(obj.ToString());
                }
                else
                {
                    cs.DataFormat = workbook.CreateDataFormat().GetFormat(obj.ToString());
                }
            }

            //水平方向对齐方式
            if (dc.ExtendedProperties.ContainsKey(COLUMN_ALIGN_PROPERTY_NAME)
                && dc.ExtendedProperties[COLUMN_ALIGN_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                obj = dc.ExtendedProperties[COLUMN_ALIGN_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(COLUMN_ALIGN_PROPERTY_NAME);
                if (obj is int)
                {
                    cs.Alignment = (HorizontalAlignment)int.Parse(obj.ToString());
                }
                else
                {
                    cs.Alignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), obj.ToString(), true);
                }
            }

            //垂直方式对齐方式
            if (dc.ExtendedProperties.ContainsKey(COLUMN_VALIGN_PROPERTY_NAME)
                && dc.ExtendedProperties[COLUMN_VALIGN_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                obj = dc.ExtendedProperties[COLUMN_VALIGN_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(COLUMN_VALIGN_PROPERTY_NAME);
                if (obj is int)
                {
                    cs.VerticalAlignment = (VerticalAlignment)int.Parse(obj.ToString());
                }
                else
                {
                    cs.VerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), obj.ToString(), true);
                }
            }

            //单元格填充方式
            if (dc.ExtendedProperties.ContainsKey(COLUMN_FILL_PATTERN_PROPERTY_NAME)
                && dc.ExtendedProperties[COLUMN_FILL_PATTERN_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                obj = dc.ExtendedProperties[COLUMN_FILL_PATTERN_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(COLUMN_FILL_PATTERN_PROPERTY_NAME);
                if (obj is int)
                {
                    cs.FillPattern = (FillPattern)int.Parse(obj.ToString());
                }
                else
                {
                    cs.FillPattern = (FillPattern)Enum.Parse(typeof(FillPattern), obj.ToString(), true);
                }
            }

            //前景色
            if (dc.ExtendedProperties.ContainsKey(COLUMN_COLOR_PROPERTY_NAME)
                && dc.ExtendedProperties[COLUMN_COLOR_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                obj = dc.ExtendedProperties[COLUMN_COLOR_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(COLUMN_COLOR_PROPERTY_NAME);
                cs.FillForegroundColor = short.Parse(obj.ToString());
            }

            //背景色
            if (dc.ExtendedProperties.ContainsKey(COLUMN_BGCOLOR_PROPERTY_NAME)
                && dc.ExtendedProperties[COLUMN_BGCOLOR_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                obj = dc.ExtendedProperties[COLUMN_BGCOLOR_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(COLUMN_BGCOLOR_PROPERTY_NAME);
                cs.FillBackgroundColor = short.Parse(obj.ToString());
            }

            //是否自动收缩
            if (dc.ExtendedProperties.ContainsKey(COLUMN_SHRINK_TO_FIT_PROPERTY_NAME)
                && dc.ExtendedProperties[COLUMN_SHRINK_TO_FIT_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                obj = dc.ExtendedProperties[COLUMN_SHRINK_TO_FIT_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(COLUMN_SHRINK_TO_FIT_PROPERTY_NAME);
                cs.ShrinkToFit = obj.ToString().Equals(true.ToString());
            }

            //旋转角度
            if (dc.ExtendedProperties.ContainsKey(COLUMN_ROTATION_PROPERTY_NAME)
                && dc.ExtendedProperties[COLUMN_ROTATION_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                obj = dc.ExtendedProperties[COLUMN_ROTATION_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(COLUMN_ROTATION_PROPERTY_NAME);
                cs.Rotation = short.Parse(obj.ToString());
            }

            //是否自动换行
            if (dc.ExtendedProperties.ContainsKey(COLUMN_WRAP_TEXT_PROPERTY_NAME)
                && dc.ExtendedProperties[COLUMN_WRAP_TEXT_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                obj = dc.ExtendedProperties[COLUMN_WRAP_TEXT_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(COLUMN_WRAP_TEXT_PROPERTY_NAME);
                cs.WrapText = obj.ToString().Equals(true.ToString());
            }

            ResolveFontSettings(workbook, dc, ref cs);

            if (cs != null)
            {
                dc.ExtendedProperties.Add(COLUMN_STYLE_PROPERTY_NAME, cs);
            }
        }

        /// <summary>
        /// 解析字体配置
        /// </summary>
        /// <param name="workbook">工作簿</param>
        /// <param name="dc">数据列</param>
        private static void ResolveFontSettings(HSSFWorkbook workbook, DataColumn dc,ref ICellStyle cs)
        {
            object obj = null;
            IFont f = null;
            //粗体
            if (dc.ExtendedProperties.ContainsKey(FONT_BOLD_WEIGHT_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_BOLD_WEIGHT_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                f = workbook.CreateFont();
                obj = dc.ExtendedProperties[FONT_BOLD_WEIGHT_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_BOLD_WEIGHT_PROPERTY_NAME);
                f.Boldweight = short.Parse(obj.ToString());
            }
            //字符集
            if (dc.ExtendedProperties.ContainsKey(FONT_CHARSET_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_CHARSET_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                if (f == null)
                {
                    f = workbook.CreateFont();
                }
                obj = dc.ExtendedProperties[FONT_CHARSET_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_CHARSET_PROPERTY_NAME);
                f.Charset = short.Parse(obj.ToString());
            }
            //字体颜色
            if (dc.ExtendedProperties.ContainsKey(FONT_COLOR_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_COLOR_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                if (f == null)
                {
                    f = workbook.CreateFont();
                }
                obj = dc.ExtendedProperties[FONT_COLOR_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_COLOR_PROPERTY_NAME);
                f.Color = short.Parse(obj.ToString());
            }
            //字体高度
            if (dc.ExtendedProperties.ContainsKey(FONT_HEIGHT_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_HEIGHT_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                if (f == null)
                {
                    f = workbook.CreateFont();
                }
                obj = dc.ExtendedProperties[FONT_HEIGHT_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_HEIGHT_PROPERTY_NAME);
                f.FontHeight = double.Parse(obj.ToString());
            }
            //字体高度
            if (dc.ExtendedProperties.ContainsKey(FONT_HEIGHT_IN_POINTS_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_HEIGHT_IN_POINTS_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                if (f == null)
                {
                    f = workbook.CreateFont();
                }
                obj = dc.ExtendedProperties[FONT_HEIGHT_IN_POINTS_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_HEIGHT_IN_POINTS_PROPERTY_NAME);
                f.FontHeightInPoints = short.Parse(obj.ToString());
            }
            //字体名称
            if (dc.ExtendedProperties.ContainsKey(FONT_NAME_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_NAME_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                if (f == null)
                {
                    f = workbook.CreateFont();
                }
                obj = dc.ExtendedProperties[FONT_NAME_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_NAME_PROPERTY_NAME);
                f.FontName = obj.ToString();
            }
            //是否斜体
            if (dc.ExtendedProperties.ContainsKey(FONT_IS_ITALIC_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_IS_ITALIC_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                if (f == null)
                {
                    f = workbook.CreateFont();
                }
                obj = dc.ExtendedProperties[FONT_IS_ITALIC_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_IS_ITALIC_PROPERTY_NAME);
                f.IsItalic = obj.ToString().Equals(true.ToString());
            }
            //是否删除线
            if (dc.ExtendedProperties.ContainsKey(FONT_IS_STRIKEOUT_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_IS_STRIKEOUT_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                if (f == null)
                {
                    f = workbook.CreateFont();
                }
                obj = dc.ExtendedProperties[FONT_IS_STRIKEOUT_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_IS_STRIKEOUT_PROPERTY_NAME);
                f.IsStrikeout = obj.ToString().Equals(true.ToString());
            }
            //上档
            if (dc.ExtendedProperties.ContainsKey(FONT_SUPERSCRIPT_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_SUPERSCRIPT_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                if (f == null)
                {
                    f = workbook.CreateFont();
                }
                obj = dc.ExtendedProperties[FONT_SUPERSCRIPT_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_SUPERSCRIPT_PROPERTY_NAME);
                if (obj is int)
                {
                    f.TypeOffset = (FontSuperScript)int.Parse(obj.ToString());
                }
                else
                {
                    f.TypeOffset = (FontSuperScript)Enum.Parse(typeof(FontSuperScript), obj.ToString());
                }
            }
            //下划线样式
            if (dc.ExtendedProperties.ContainsKey(FONT_UNDERLINETYPE_PROPERTY_NAME)
                && dc.ExtendedProperties[FONT_UNDERLINETYPE_PROPERTY_NAME] != null)
            {
                if (cs == null)
                {
                    cs = workbook.CreateCellStyle();
                }
                if (f == null)
                {
                    f = workbook.CreateFont();
                }
                obj = dc.ExtendedProperties[FONT_UNDERLINETYPE_PROPERTY_NAME];
                dc.ExtendedProperties.Remove(FONT_UNDERLINETYPE_PROPERTY_NAME);
                if (obj is int)
                {
                    f.Underline = (FontUnderlineType)int.Parse(obj.ToString());
                }
                else
                {
                    f.Underline = (FontUnderlineType)Enum.Parse(typeof(FontUnderlineType), obj.ToString());
                }
            }
        }

        /// <summary>
        /// 设置文档列样式
        /// </summary>
        /// <param name="dtSource">数据源</param>
        /// <param name="workbook">工作簿</param>
        /// <param name="sheet">工作表</param>
        /// <param name="arrColWidth">列宽</param>
        /// <param name="rowIndex">行号(从0开始)</param>
        private static void SetDocumentColumnStyle(DataTable dtSource, HSSFWorkbook workbook, ISheet sheet, int rowIndex)
        {
            IRow headerRow = sheet.CreateRow(rowIndex);

            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;

            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 10;
            font.Boldweight = 700;
            headStyle.SetFont(font);

            ICell cell = null;

            string val = null;
            int width = 0;

            foreach (DataColumn column in dtSource.Columns)
            {
                cell = headerRow.CreateCell(column.Ordinal);
                cell.SetCellValue(column.Caption);
                cell.CellStyle = headStyle;

                //解析列宽度
                if (column.ExtendedProperties.ContainsKey(COLUMN_WIDTH_PROPERTY_NAME)
                    && !string.IsNullOrEmpty((string)column.ExtendedProperties[COLUMN_WIDTH_PROPERTY_NAME]))
                {
                    val = (string)column.ExtendedProperties[COLUMN_WIDTH_PROPERTY_NAME];
                    if (!string.IsNullOrEmpty(val) && int.TryParse(val, out width))
                    {
                        //设置列宽
                        sheet.SetColumnWidth(column.Ordinal, width);
                    }
                    else
                    {
                        column.ExtendedProperties.Remove(COLUMN_WIDTH_PROPERTY_NAME);
                    }
                }
            }
        }

        /// <summary>
        /// 设置文档头部样式
        /// </summary>
        /// <param name="columnCount">数据源列数量</param>
        /// <param name="strHeaderText">文档标题</param>
        /// <param name="workbook">工作簿</param>
        /// <param name="sheet">工作表</param>
        /// <param name="rowIndex">行号(从0开始)</param>
        private static bool SetDocumentHeadStyle(int columnCount, string strHeaderText, HSSFWorkbook workbook, ISheet sheet, int rowIndex)
        {
            if (strHeaderText == null)
            {
                return false;
            }
            IRow headerRow = sheet.CreateRow(rowIndex);
            headerRow.HeightInPoints = 25;
            ICell cell = headerRow.CreateCell(0);
            cell.SetCellValue(strHeaderText);

            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.Alignment = HorizontalAlignment.Center;

            IFont font = workbook.CreateFont();
            font.FontHeightInPoints = 20;
            font.Boldweight = 700;
            headStyle.SetFont(font);

            cell.CellStyle = headStyle;

            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(rowIndex, rowIndex, 0, columnCount - 1));
            return true;
        }

        /// <summary>
        /// 添加文档注释
        /// </summary>
        /// <param name="strHeaderText">标题</param>
        /// <param name="workbook">工作簿</param>
        private static void AddDocumentComment(string strHeaderText, HSSFWorkbook workbook)
        {
            DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
            dsi.Company = "http://www.jiumei.com/";
            workbook.DocumentSummaryInformation = dsi;

            if (!string.IsNullOrEmpty(strHeaderText))
            {
                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "酒美网"; //填加xls文件作者信息
                si.ApplicationName = "酒美网"; //填加xls文件创建程序信息
                si.LastAuthor = "酒美网"; //填加xls文件最后保存者信息
                si.Comments = "酒美网"; //填加xls文件作者信息
                si.Title = strHeaderText; //填加xls文件标题信息
                si.Subject = strHeaderText;//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
        }

        /// <summary>
        /// 填充表格
        /// </summary>
        /// <param name="newCell">表格</param>
        /// <param name="row">行</param>
        /// <param name="column">数据源列</param>
        private static void FillCell(ICell newCell, DataRow row, DataColumn column)
        {
            object drValue = row[column];
            if (drValue == DBNull.Value || drValue == null)
            {
                newCell.SetCellValue(string.Empty);
            }
            else
            {
                string val = drValue.ToString();

                switch (column.DataType.ToString())
                {
                    case "System.Char":
                    case "System.String"://字符串类型
                        newCell.SetCellValue(drValue.ToString());
                        break;
                    case "System.DateTime"://日期类型
                        newCell.SetCellValue(DateTime.Parse(drValue.ToString()));
                        break;
                    case "System.Boolean"://布尔型
                        newCell.SetCellValue(bool.Parse(drValue.ToString()));
                        break;
                    case "System.UInt16":
                    case "System.Int16":
                    case "System.UInt32":
                    case "System.Int32":
                    case "System.UInt64":
                    case "System.Int64":
                    case "System.Byte":
                    case "System.Decimal":
                    case "System.Single":
                    case "System.Double":
                        newCell.SetCellValue(double.Parse(drValue.ToString()));
                        break;
                    default:
                        newCell.SetCellValue(string.Empty);
                        break;
                }

                if (column.ExtendedProperties.ContainsKey(COLUMN_STYLE_PROPERTY_NAME))
                {
                    newCell.CellStyle = column.ExtendedProperties[COLUMN_STYLE_PROPERTY_NAME] as ICellStyle;
                }
            }
        }
        #endregion

    }
}
