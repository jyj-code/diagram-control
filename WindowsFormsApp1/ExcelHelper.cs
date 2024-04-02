using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WindowsFormsApp1
{
    public class ExcelHelper
    {
        // <summary>
        /// Excel某sheet中内容导入到DataTable中
        /// 区分xsl和xslx分别处理
        /// </summary>
        /// <param name="filePath">Excel文件路径,含文件全名</param>
        /// <param name="sheetName">此Excel中sheet名</param>
        /// <returns></returns>
        public static DataTable ExcelSheetImportToDataTable(string filePath, string sheetName)
        {
            IWorkbook hssfworkbook;

            DataTable dt = new DataTable();

            if (Path.GetExtension(filePath).ToLower() == ".xls".ToLower())
            {//.xls
                #region .xls文件处理:HSSFWorkbook
                try
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {

                        hssfworkbook = new HSSFWorkbook(file);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                ISheet sheet = hssfworkbook.GetSheetAt(0);
                if (!string.IsNullOrEmpty(sheetName))
                {
                    sheet = hssfworkbook.GetSheetAt(0);
                }
                if (sheet == null)
                {
                    // XtraMessageBox.Show($"在excel中找不到名称为{sheetName}的sheet页！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                //ISheet sheet = hssfworkbook.GetSheet(sheetName);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                HSSFRow headerRow = (HSSFRow)sheet.GetRow(0);

                //一行最后一个方格的编号 即总的列数
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    //SET EVERY COLUMN NAME
                    HSSFCell cell = (HSSFCell)headerRow.GetCell(j);

                    dt.Columns.Add(cell?.ToString());
                }

                while (rows.MoveNext())
                {
                    IRow row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();

                    if (row.RowNum == 0) continue;//The firt row is title,no need import

                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        if (i >= dt.Columns.Count)//cell count>column count,then break //每条记录的单元格数量不能大于表格栏位数量 20140213
                        {
                            break;
                        }

                        ICell cell = row.GetCell(i);

                        switch (row.GetCell(i).CellType)
                        {
                            case CellType.Numeric:
                                if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(i)))//日期类型
                                {
                                    dr[i] = row.GetCell(i).DateCellValue?.ToString("yyyy-MM-dd");
                                }
                                else//其他数字类型
                                {
                                    dr[i] = row.GetCell(i).NumericCellValue;
                                }
                                break;
                            case CellType.Blank:
                                dr[i] = string.Empty;
                                break;
                            case CellType.Formula:   //此处是处理公式数据，获取公式执行后的值
                                if (Path.GetExtension(filePath).ToLower().Trim() == ".xlsx")
                                {
                                    XSSFFormulaEvaluator eva = new XSSFFormulaEvaluator(hssfworkbook);
                                    if (eva.Evaluate(row.GetCell(i)).CellType == CellType.Numeric)
                                    {
                                        dr[i] = eva.Evaluate(row.GetCell(i)).NumberValue;
                                    }
                                    else
                                    {
                                        dr[i] = eva.Evaluate(row.GetCell(i)).StringValue;
                                    }
                                }
                                else
                                {
                                    HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(hssfworkbook);
                                    if (eva.Evaluate(row.GetCell(i)).CellType == CellType.Numeric)
                                    {
                                        dr[i] = eva.Evaluate(row.GetCell(i)).NumberValue;
                                    }
                                    else
                                    {
                                        dr[i] = eva.Evaluate(row.GetCell(i)).StringValue;
                                    }
                                }
                                break;
                            default:
                                dr[i] = row.GetCell(i).StringCellValue;
                                break;

                        }
                    }

                    dt.Rows.Add(dr);
                }
                #endregion
            }
            else
            {//.xlsx
                #region .xlsx文件处理:XSSFWorkbook
                try
                {
                    using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                    {

                        hssfworkbook = new XSSFWorkbook(file);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

                ISheet sheet = hssfworkbook.GetSheet(sheetName);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                XSSFRow headerRow = (XSSFRow)sheet.GetRow(0);

                //一行最后一个方格的编号 即总的列数
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    //SET EVERY COLUMN NAME
                    XSSFCell cell = (XSSFCell)headerRow.GetCell(j);
                    if (cell != null)
                    {
                        dt.Columns.Add(cell.ToString());
                    }
                }

                while (rows.MoveNext())
                {
                    IRow row = (XSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();

                    if (row.RowNum == 0) continue;//The firt row is title,no need import

                    for (int i = 0; i < row.LastCellNum; i++)
                    {
                        if (i >= dt.Columns.Count)//cell count>column count,then break //每条记录的单元格数量不能大于表格栏位数量 20140213
                        {
                            break;
                        }

                        if (row.GetCell(i) != null)
                        {

                            switch (row.GetCell(i).CellType)
                            {
                                case CellType.Numeric:
                                    if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(i)))//日期类型
                                    {
                                        dr[i] = row.GetCell(i).DateCellValue?.ToString("yyyy-MM-dd");
                                    }
                                    else//其他数字类型
                                    {
                                        dr[i] = row.GetCell(i).NumericCellValue;
                                    }
                                    break;
                                case CellType.Blank:
                                    dr[i] = string.Empty;
                                    break;
                                case CellType.Formula:   //此处是处理公式数据，获取公式执行后的值
                                    if (Path.GetExtension(filePath).ToLower().Trim() == ".xlsx")
                                    {
                                        XSSFFormulaEvaluator eva = new XSSFFormulaEvaluator(hssfworkbook);
                                        if (eva.Evaluate(row.GetCell(i)).CellType == CellType.Numeric)
                                        {
                                            dr[i] = eva.Evaluate(row.GetCell(i)).NumberValue;
                                        }
                                        else
                                        {
                                            dr[i] = eva.Evaluate(row.GetCell(i)).StringValue;
                                        }
                                    }
                                    else
                                    {
                                        HSSFFormulaEvaluator eva = new HSSFFormulaEvaluator(hssfworkbook);
                                        if (eva.Evaluate(row.GetCell(i)).CellType == CellType.Numeric)
                                        {
                                            dr[i] = eva.Evaluate(row.GetCell(i)).NumberValue;
                                        }
                                        else
                                        {
                                            dr[i] = eva.Evaluate(row.GetCell(i)).StringValue;
                                        }
                                    }
                                    break;
                                default:
                                    dr[i] = row.GetCell(i).StringCellValue;
                                    break;

                            }
                        }
                    }
                    dt.Rows.Add(dr);
                }
                #endregion
            }
            return dt;
        }
    }
}
