using DevExpress.Utils;
using DevExpress.XtraDiagram;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.Data.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;
using TopologyIdentification.Properties;
using System.Xml.Linq;

namespace TopologyIdentification
{
    public partial class FrmMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public DataTable RawDataTable;
        public DataTable dataTable;
        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            BindCustomDrawRowIndicator(gvMain);
            BindCustomDrawRowIndicator(gvRawData);
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
                        item.Y = 180;
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
        //导入数据
        private void btnImportData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //openFileDialog1.InitialDirectory = "c:\\";//注意这里写路径时要用c:\\而不是c:\
            openFileDialog1.Filter = "Excel文件(*.xls)|*.xls";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.FileName = "数据表";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                splashScreenManager1.ShowWaitForm();
                string fName = openFileDialog1.FileName;
                IntPtr vHandle = _lopen(fName, OF_READWRITE | OF_SHARE_DENY_NONE);
                if (vHandle == HFILE_ERROR)
                {
                    MessageBox.Show("文件被占用！");
                    return;
                }
                CloseHandle(vHandle);

                DataTable dt = ExcelHelper.ExcelSheetImportToDataTable(fName, "");
                gcMain.DataSource = dt;
                gvMain.BestFitColumns();
                dataTable = dt.Copy();
                diagramOrgChartController1.KeyMember = "Tei";
                diagramOrgChartController1.ParentMember = "FatherTei";
                List<Topo> topos = new List<Topo>();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    bool flag = true;
                    Topo topo = new Topo()
                    {
                        Tei = Convert.ToString(dataTable.Rows[i][0]),
                        FatherTei = Convert.ToString(dataTable.Rows[i][1]),
                        Name = Convert.ToString(dataTable.Rows[i][2]),
                        NodeAddress = Convert.ToString(dataTable.Rows[i][3]),
                        Type = Convert.ToString(dataTable.Rows[i][5]) == "0" ? "开关" : "用户",
                        Right = Convert.ToString(dataTable.Rows[i][4]) == "1" ? "识别正确" : "识别错误",
                        TypeName = "",
                    };
                    int index = 1;
                    while(flag)
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
                
                
                diagramControl1.OptionsView.ZoomFactor = 0.4f;
                diagramControl1.OptionsTreeLayout.IsCompact = true;
                //diagramControl1.OptionsTreeLayout.HorizontalSpacing = 200;
                diagramControl1.OptionsTreeLayout.VerticalSpacing = 100;
                //diagramControl1.OptionsSugiyamaLayout.ColumnSpacing = 20;
                //diagramControl1.OptionsSugiyamaLayout.LayerSpacing = 20;
                //diagramControl1.OptionsOrgChartLayout.HierarchySpacing = 2000;
                //diagramControl1.OptionsOrgChartLayout.NodeSpacing = 100;
                diagramOrgChartController1.DataSource = topos;
                foreach (DiagramItem item in diagramControl1.Items)
                {
                    SetNodeColor(item);
                }
                List<DiagramItem> treeItems = new List<DiagramItem>();
                List<DiagramItem> tipoverItems = new List<DiagramItem>();
                foreach (DiagramItem item in diagramControl1.Items)
                {
                    int itemLevel;
                    if (item is DiagramConnector)
                    {
                        itemLevel = GetLevel((DiagramItem)((DiagramConnector)item).BeginItem);
                      
                    }
                    else
                        itemLevel = GetLevel(item);
                    if(itemLevel ==0)
                    {
                        item.Appearance.BackColor = Color.Red;
                        treeItems.Add(item);
                        if (item is DiagramShape)
                        {
                            DiagramContainer container = item as DiagramContainer;

                            Topo topo = item.DataContext as Topo;
                            topo.TypeName = "总表";
                        }
                       
                    }
                    if (itemLevel ==1)
                    {
                        item.Appearance.BackColor = Color.Yellow;
                        treeItems.Add(item);
                        if(item is DiagramShape)
                        {
                            DiagramContainer container = item as DiagramContainer;
                            Topo topo = item.DataContext as Topo;
                            topo.TypeName = "一级分支";
                        }
                    }
                    return;
                    if (itemLevel == 2)
                    {
                        tipoverItems.Add(item);
                        if (item is DiagramShape)
                        {
                            //DiagramContainer container = item as DiagramContainer;
                            Topo topo = item.DataContext as Topo;
                            topo.TypeName = "表箱";
                        }
                    }
                    if (item is DiagramShape)
                    {
                        DiagramShape container = item as DiagramShape;
                        Topo topo = item.DataContext as Topo;
                        if (topo.Right == "识别正确")
                        {
                            container.Appearance.BackColor = Color.Green;
                            container.Appearance.ForeColor = Color.White;
                        }
                        else
                        {
                            container.Appearance.BackColor = Color.Red;
                            container.Appearance.ForeColor = Color.White;
                        }

                    }
                }
                diagramControl1.ApplyTreeLayout(treeItems);
                diagramControl1.ApplyTipOverTreeLayoutForSubordinates(tipoverItems);
                diagramControl1.FitToItems(diagramControl1.Items);
                diagramControl1.Update();
                splashScreenManager1.CloseWaitForm();
            }
        }
        [DllImport("kernel32.dll")]
        public static extern IntPtr _lopen(string lpPathName, int iReadWrite);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);

        public const int OF_READWRITE = 2;
        public const int OF_SHARE_DENY_NONE = 0x40;
        public readonly IntPtr HFILE_ERROR = new IntPtr(-1);

        #region gridview 行号
        public static void BindCustomDrawRowIndicator(DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            view.IndicatorWidth = CalcIndicatorDefaultWidth(view);
            view.CustomDrawRowIndicator += (s, e) =>
            {
                if (e.RowHandle >= 0)
                {
                    e.Info.DisplayText = (e.RowHandle + 1).ToString();
                }
            };
            view.TopRowChanged += (s, e) =>
            {
                int width = CalcIndicatorBestWidth(view);
                if ((view.IndicatorWidth - 4 < width || view.IndicatorWidth + 4 > width) && view.IndicatorWidth != width)
                {
                    view.IndicatorWidth = width;
                }
            };

        }
        /// <summary>
        /// 计算行头宽度
        /// </summary>
        /// <param name="sender"></param>
        /// <returns></returns>
        static int CalcIndicatorBestWidth(DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            Graphics graphics = new System.Windows.Forms.Control().CreateGraphics();
            SizeF sizeF = new SizeF();
            int count = view.TopRowIndex + ((DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo)view.GetViewInfo()).RowsInfo.Count;
            if (count == 0)
            {
                count = 30;
            }
            sizeF = graphics.MeasureString(count.ToString(), view.Appearance.Row.Font);
            return Convert.ToInt32(sizeF.Width) + 20;
        }
        /// <summary>
        /// 计算默认的宽度
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        static int CalcIndicatorDefaultWidth(DevExpress.XtraGrid.Views.Grid.GridView view)
        {
            var grid = view.GridControl;
            Graphics graphics = new System.Windows.Forms.Control().CreateGraphics();
            SizeF sizeF = new SizeF();
            int rowHeight = 22;//22是Row的高度
            if (view.RowHeight > 0)
            {
                rowHeight = view.RowHeight;
            }
            int count = grid != null ? grid.Height / rowHeight : 30;
            sizeF = graphics.MeasureString(count.ToString(), view.Appearance.Row.Font);
            return Convert.ToInt32(sizeF.Width) + 20;
        }
        #endregion

        //数据预处理
        private void btnPreProcessData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            diagramOrgChartController1.KeyMember = "Tei";
            diagramOrgChartController1.ParentMember = "FatherTei";
            List<Topo> topos = new List<Topo>();
            topos.Add(new Topo() { Tei = "0", NodeAddress = "00000", Layer = 0 });
            for (int i = 1; i < 10; i++)
            {
                Topo topo = new Topo()
                { Tei = i.ToString(), FatherTei = "0", NodeAddress = "00000", Layer = 0 };
                topos.Add(topo);
            }
            diagramOrgChartController1.BeginInit();
            diagramOrgChartController1.DataSource = topos;
            diagramOrgChartController1.EndInit();
            diagramControl1.FitToItems(diagramControl1.Items);
            diagramControl1.OptionsView.ZoomFactor = 0.5f;
            for (int i = 0; i < diagramControl1.Items.Count; i++)
            {
                diagramControl1.Items[i].Appearance.BackColor = Color.Transparent;
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
          
        }

        private void toolTipController1_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            ToolTipControlInfo toolTipControlInfo = null;
            DiagramShape diagramShape = this.diagramControl1.CalcHitItem(e.ControlMousePosition) as DiagramShape;
            if(diagramShape!=null)
            {
                var topo = diagramShape.DataContext as Topo;
                string str = topo.Description;
                toolTipControlInfo=new ToolTipControlInfo(diagramShape,str);
                e.Info = toolTipControlInfo;
                e.Info.Title = "节点信息" ;
            }

        }
        private void btnExport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            diagramControl1.ExportDiagram();
            splashScreenManager1.CloseWaitForm();
        }

        //导入原始数据
        private void btnImportRawData_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            openFileDialog1.Filter = "Excel文件(*.xls)|*.xls";
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.FileName = "数据表";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                splashScreenManager1.ShowWaitForm();
                string fName = openFileDialog1.FileName;
                IntPtr vHandle = _lopen(fName, OF_READWRITE | OF_SHARE_DENY_NONE);
                if (vHandle == HFILE_ERROR)
                {
                    MessageBox.Show("文件被占用！");
                    splashScreenManager1.CloseWaitForm();
                    return;
                }
                CloseHandle(vHandle);

                DataTable dt = ExcelHelper.ExcelSheetImportToDataTable(fName, "");
                gcRawData.DataSource = dt;
                RawDataTable = dt;
                gvRawData.BestFitColumns();
                splashScreenManager1.CloseWaitForm();
            }
        }

        public List<RawData>GetAllMeterName(DataTable dataTable)
        {
            List<RawData> list = new List<RawData>();
            foreach(DataRow dataRow in dataTable.Rows)
            {
                RawData rawData = new RawData();
                rawData.DataIdentification = Convert.ToString(dataRow[0]);
                rawData.TerminalName = Convert.ToString(dataRow[1]);
                rawData.MeasurePoint = Convert.ToString(dataRow[2]);
                rawData.MeterName = Convert.ToString(dataRow[3]);
                rawData.MeterAddress = Convert.ToString(dataRow[4]);
                rawData.DataTimescale = Convert.ToString(dataRow[5]);
                rawData.Electricity = Convert.ToDouble(dataRow[7] == "" ? 0 : dataRow[7]);
                list.Add(rawData);
            }
            return list;
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
            List<RawData> list = GetAllMeterName(RawDataTable);
            List<string> listMeterName = new List<string>();
            foreach(var item in list)
            {
                if(!listMeterName.Contains(item.MeterName))
                {
                    listMeterName.Add(item.MeterName);
                }
            }
            List<Temp> listTempName = new List<Temp>()
            {
                new Temp{Name1="3号楼1单元",Name2="3号楼一单元表箱"},
                new Temp{Name1="4号楼1单元",Name2="4号楼一单元表箱"},
                new Temp{Name1="5号楼1单元",Name2="5号楼一单元表箱"},
                new Temp{Name1="6号楼1单元",Name2="6号楼一单元表箱"},
                new Temp{Name1="7号楼1单元",Name2="7号楼一单元表箱"},
                new Temp{Name1="8号楼1单元",Name2="8号楼一单元表箱"}
            };

           foreach(var itemx in listTempName)
            {
                List<DataALL> dataALLs = new List<DataALL>();
                memoEdit1.Text += "======================开始==============================" + "\r\n";
                   memoEdit1.Text += itemx.Name1 + ";"+itemx.Name2+"\r\n";
                List<string> listMeterNameFind = listMeterName.Where(o => o.Contains(itemx.Name1) || o.Contains(itemx.Name2)).ToList();
                foreach (var item in listMeterNameFind)
                {
                    memoEdit1.Text += item + "; ";
                }
                foreach (var item in listMeterNameFind)
                {
                    //获得同一块表的序列
                    List<RawData> rawDatas = list.Where(o => o.MeterName == item).ToList();
                    //计算该表的用电序列
                    DataALL dataALL = new DataALL();
                    dataALL.RawDatas = rawDatas;
                    dataALLs.Add(dataALL);
                }
                double[,] preM = new double[listMeterNameFind.Count, 96];
                double[,] nextM = new double[listMeterNameFind.Count, 96];
                for (int i = 0; i < dataALLs.Count; i++)
                {
                    for (int j = 0; j < dataALLs[i].RawDatas.Count; j++)
                    {
                        if (j < dataALLs[i].RawDatas.Count - 1)
                        {
                            preM[i, j] = dataALLs[i].RawDatas[j].Electricity;
                        }
                        if (j > 0)
                        {
                            nextM[i, j - 1] = dataALLs[i].RawDatas[j].Electricity;
                        }
                    }
                }

                var matrix1 = DenseMatrix.OfArray(preM);
                var matrix2 = DenseMatrix.OfArray(nextM);
                var matrixResult = matrix2 - matrix1;
                for (int i = 0; i < dataALLs.Count; i++)
                {
                    dataALLs[i].Data = matrixResult.Row(i).ToArray();
                }
                // memoEdit1.Text += matrixResult.ToString();
                DelimitedWriter.Write("data.csv", matrixResult, ",");

                List<int> listTemp = new List<int>();
                for (int i = 1; i < listMeterNameFind.Count; i++)
                {
                    listTemp.Add(i);
                }
                List<List<int>> listXX = new List<List<int>>();

                //计算
                // for (int i=1;i<5;i++)
                {
                    listXX = Combination.GetCombinationList<int>(listTemp, listTemp.Count);
                    for (int k = 0; k < listXX.Count; k++)
                    {
                        Vector<double> v = matrixResult.Row(listXX[k][0]);
                        memoEdit1.Text += "子节点：" + listXX[k][0] + ";";
                        for (int j = 1; j < listXX[k].Count; j++)
                        {
                            //求和
                            v = v + matrixResult.Row(listXX[k][j]);
                            memoEdit1.Text += listXX[k][j] + ";";
                        }
                        memoEdit1.Text += "\r\n";
                        //计算相关系数
                        memoEdit1.Text += matrixResult.Row(0).ToString()+"\r\n";
                        memoEdit1.Text += v.ToString()+"\r\n";

                        double xxxx = Correlation1.Pearson(matrixResult.Row(0), v);
                        double xxxx1 = Correlation.Pearson(matrixResult.Row(0), v);
                        memoEdit1.Text += Math.Round(xxxx, 6).ToString() + "\r\n";
                        memoEdit1.Text += Math.Round(xxxx1, 6).ToString() + "\r\n";
                    }
                }
                memoEdit1.Text += "======================结束==============================" + "\r\n";
            }
            MessageBox.Show("结束");
        }

        //计算皮尔逊相关系数
        public double RelatedCoefficient(Vector x, Vector y)
        {
            return Distance.Pearson(x, y);
        }

        private void diagramOrgChartController1_GenerateItem(object sender, DiagramGenerateItemEventArgs e)
        {
            if(((Topo)e.DataObject).Tei =="1")
            {
                e.Item = e.CreateItemFromTemplate("Name1");
            }
            else
            {
                if (((Topo)e.DataObject).Type == "开关")
                {
                    e.Item = e.CreateItemFromTemplate("1");
                }
                else
                {
                    e.Item = e.CreateItemFromTemplate("Template2");
                }
            }
        }

        private void diagramControl1_CustomDrawBackground(object sender, CustomDrawBackgroundEventArgs e)
        {
            //var myImage = Resources.background;
            //e.GraphicsCache.DrawImage(myImage, e.TotalBounds);
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            List<DiagramItem> treeItems = new List<DiagramItem>();
            List<DiagramItem> tipoverItems = new List<DiagramItem>();
            foreach (DiagramItem item in diagramControl1.Items)
            {
                int itemLevel;
                if (item is DiagramConnector)
                {
                    itemLevel = GetLevel((DiagramItem)((DiagramConnector)item).BeginItem);
                }
                else
                    itemLevel = GetLevel(item);
                if (itemLevel < 2)
                    treeItems.Add(item);
                if (itemLevel == 2)
                    tipoverItems.Add(item);
            }
            diagramControl1.ApplyTreeLayout(treeItems);
            diagramControl1.ApplyTipOverTreeLayoutForSubordinates(tipoverItems);
          
        }
        public int GetLevel(DiagramItem item)
        {
            if (item == null || item.IncomingConnectors.Count() == 0)
                return 0;
            return GetLevel((DiagramItem)item.IncomingConnectors.First().BeginItem) + 1;
        }

        private void diagramControl1_ItemsMoving(object sender, DiagramItemsMovingEventArgs e)
        {

        }

        private void diagramOrgChartController1_CustomLayoutItems(object sender, DiagramCustomLayoutItemsEventArgs e)
        {
            List<DiagramItem> treeItems = new List<DiagramItem>();
            List<DiagramItem> tipoverItems = new List<DiagramItem>();
            foreach (DiagramItem item in diagramControl1.Items)
            {
                int itemLevel;
                if (item is DiagramConnector)
                {
                    itemLevel = GetLevel((DiagramItem)((DiagramConnector)item).BeginItem);
                }
                else
                    itemLevel = GetLevel(item);
                if (itemLevel < 2)
                    treeItems.Add(item);
                if (itemLevel == 2)
                    tipoverItems.Add(item);
            }
            diagramControl1.ApplyTreeLayout(treeItems);
            diagramControl1.ApplyTipOverTreeLayoutForSubordinates(tipoverItems);
        }

        private void diagramControl1_SubordinatesShowing(object sender, DiagramSubordinatesShowingEventArgs e)
        {
           
        }

        private void diagramControl1_SubordinatesHidden(object sender, DiagramSubordinatesHiddenEventArgs e)
        {
         
          
        }

        private void diagramControl1_SubordinatesShown(object sender, DiagramSubordinatesShownEventArgs e)
        {

           
        }
    }
    class Temp
    {
        public string Name1 { get; set; }
        public string Name2 { get; set; }
    }
}
