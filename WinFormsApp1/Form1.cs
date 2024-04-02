using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel文件(*.xls)|*.xls";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.FileName = "数据表";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog1.FileName;
                DataTable dt = ExcelHelper.ExcelSheetImportToDataTable(fName, "");
                diagramOrgChartController1.KeyMember = "Tei";
                diagramOrgChartController1.ParentMember = "FatherTei";
                List<Topo> topos = new List<Topo>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    bool flag = true;
                    Topo topo = new Topo()
                    {
                        Tei = Convert.ToString(dt.Rows[i][0]),
                        FatherTei = Convert.ToString(dt.Rows[i][1]),
                        Name = Convert.ToString(dt.Rows[i][2]),
                        NodeAddress = Convert.ToString(dt.Rows[i][3]),
                       // Type = Convert.ToString(dt.[i][5]) == "0" ? "开关" : "用户",
                        Right = Convert.ToString(dt.Rows[i][4]) == "1" ? "识别正确" : "识别错误",
                        TypeName = "",
                    };
                    int index = 1;
                    while (flag)
                    {
                        if (topos.Find(o => o.Tei == topo.Tei) == null)
                        {
                            topos.Add(topo);
                            flag = false;
                        }
                        else
                        {
                            topo.Tei += $"-{index}";
                            flag = true;
                        }
                    }
                }


                //diagramControl1.OptionsView.ZoomFactor = 0.4f;
                //diagramControl1.OptionsTreeLayout.IsCompact = true;
                //diagramControl1.OptionsTreeLayout.HorizontalSpacing = 200;
                //diagramControl1.OptionsTreeLayout.VerticalSpacing = 100;
                //diagramControl1.OptionsSugiyamaLayout.ColumnSpacing = 20;
                //diagramControl1.OptionsSugiyamaLayout.LayerSpacing = 20;
                //diagramControl1.OptionsOrgChartLayout.HierarchySpacing = 2000;
                //diagramControl1.OptionsOrgChartLayout.NodeSpacing = 100;
                diagramOrgChartController1.DataSource = topos;
            }
        }
    }
    public class Topo
    {
        /// <summary>
        /// Gets or sets 节点地址.
        /// </summary>
        public string NodeAddress { get; set; }

        /// <summary>
        /// Gets or sets 层级.
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// Gets or sets 父节点Tei.
        /// </summary>
        public string FatherTei { get; set; }

        /// <summary>
        /// Gets or sets 本节点Tei.
        /// </summary>
        public string Tei { get; set; }

        /// <summary>
        /// Gets or sets 类型.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets 代理变更使用（0：总表；1：下挂）.
        /// </summary>
        public string InfoType { get; set; }

        /// <summary>
        /// Gets or sets 代理变更下挂时间.
        /// </summary>
        public string Psubmissiontime { get; set; }

        public string Name { get; set; }

        public string Right { get; set; }

        /// <summary>
        /// 总表、一级分支、二级分支、……表箱
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// Gets 显示信息.
        /// </summary>
        public string Description
        {
            get
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine($"Tei:{Tei}");
                stringBuilder.AppendLine($"PCO_TEI:{FatherTei}");
                stringBuilder.AppendLine($"节点地址：{NodeAddress}");
                stringBuilder.AppendLine($"名称：{Name}");
                stringBuilder.AppendLine($"角色类型：{Type}");
                stringBuilder.AppendLine($"识别结果：{Right}");
                return stringBuilder.ToString();
            }
        }


    }
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
                                        dr[i] = row.GetCell(i).DateCellValue.ToString("yyyy-MM-dd");
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
