using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraDiagram;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel文件(*.xls)|*.xls";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.FileName = "数据表";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fName = openFileDialog1.FileName;
                DataTable dt = ExcelHelper.ExcelSheetImportToDataTable(fName, "");
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
                        Color = Color.Orange,
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
                diagramOrgChartController1.KeyMember = "Tei";
                diagramOrgChartController1.ParentMember = "FatherTei";
                diagramOrgChartController1.DataSource = topos;
                foreach (DiagramItem item in diagramControl1.Items)
                {
                    SetNodeColor(item);
                }
            }
        }
        int x = 100;
        float y = 0;
        private void SetNodeColor(DiagramItem item)
        {
            if (item is DiagramList)
            {
                foreach (DiagramItem _item in (item as DiagramList).Items)
                {
                    if (((DevExpress.XtraDiagram.DiagramShape)_item).Content.StartsWith("Tei:1"))
                    {
                        y = item.Y + 20;
                        x += 200; item.Y = y;
                    }
                    else if (((DevExpress.XtraDiagram.DiagramShape)_item).Content.StartsWith("Tei:2"))
                    {
                        x += 500; item.Y = y;
                    }
                    else if (((DevExpress.XtraDiagram.DiagramShape)_item).Content.Replace("\r\n", "").StartsWith("Tei:PCO_TEI"))
                    {
                        x += 500;
                        item.Y = y;
                    }
                    else if (_item.ParentItem != null)
                    {
                        item.Y =180;
                    }
                    item.X = x + item.X;
                    SetNodeColor(_item);
                }
            }
            if (item is DiagramShape)
            {
                DiagramShape container = item as DiagramShape;
                Topo topo = item.DataContext as Topo;
                if (topo.Tei == "0" || string.IsNullOrWhiteSpace(topo.Tei))
                {
                    //item.Y = 100;
                    //item.X = 100;
                    container.Appearance.BackColor = Color.Blue;
                    container.Appearance.ForeColor = Color.White;
                }
                else if (topo.Tei == "1")
                {
                    container.Appearance.BackColor = Color.Green;
                    container.Appearance.ForeColor = Color.White;

                }
                else if (topo.Tei == "2")
                {
                    container.Appearance.BackColor = Color.Yellow;
                    container.Appearance.ForeColor = Color.Black;
                }
                else
                {
                    container.Appearance.ForeColor = Color.Black;
                }
            }

        }
    }
}
