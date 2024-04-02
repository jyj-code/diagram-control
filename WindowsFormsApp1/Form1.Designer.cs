namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.diagramControl1 = new DevExpress.XtraDiagram.DiagramControl();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.diagramControl2 = new DevExpress.XtraDiagram.DiagramControl();
            this.diagramOrgChartController1 = new DevExpress.XtraDiagram.DiagramOrgChartController(this.components);
            this.diagramList1 = new DevExpress.XtraDiagram.DiagramList();
            this.diagramShape1 = new DevExpress.XtraDiagram.DiagramShape();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.diagramControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagramControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagramOrgChartController1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagramOrgChartController1.TemplateDiagram)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 42);
            this.panel1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(30, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "导入";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.diagramControl2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 42);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 408);
            this.panel2.TabIndex = 1;
            // 
            // diagramControl1
            // 
            this.diagramControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.diagramControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl1.Location = new System.Drawing.Point(0, 0);
            this.diagramControl1.Name = "diagramControl1";
            this.diagramControl1.OptionsBehavior.SelectedStencils = new DevExpress.Diagram.Core.StencilCollection(new string[] {
            "BasicShapes",
            "BasicFlowchartShapes"});
            this.diagramControl1.OptionsMindMapTreeLayout.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.diagramControl1.OptionsView.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.diagramControl1.OptionsView.ShowPanAndZoomPanel = true;
            this.diagramControl1.Size = new System.Drawing.Size(800, 450);
            this.diagramControl1.TabIndex = 0;
            this.diagramControl1.Text = "diagramControl1";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // diagramControl2
            // 
            this.diagramControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.diagramControl2.Location = new System.Drawing.Point(0, 0);
            this.diagramControl2.Name = "diagramControl2";
            this.diagramControl2.OptionsBehavior.SelectedStencils = new DevExpress.Diagram.Core.StencilCollection(new string[] {
            "BasicShapes",
            "BasicFlowchartShapes"});
            this.diagramControl2.OptionsView.CanvasSizeMode = DevExpress.Diagram.Core.CanvasSizeMode.Fill;
            this.diagramControl2.OptionsView.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.diagramControl2.OptionsView.ShowGrid = false;
            this.diagramControl2.OptionsView.ShowPageBreaks = false;
            this.diagramControl2.OptionsView.ShowRulers = false;
            this.diagramControl2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.diagramControl2.Size = new System.Drawing.Size(800, 408);
            this.diagramControl2.TabIndex = 0;
            this.diagramControl2.Text = "diagramControl2";
            // 
            // diagramOrgChartController1
            // 
            this.diagramOrgChartController1.Diagram = this.diagramControl2;
            // 
            // 
            // 
            this.diagramOrgChartController1.TemplateDiagram.Items.AddRange(new DevExpress.XtraDiagram.DiagramItem[] {
            this.diagramList1});
            this.diagramOrgChartController1.TemplateDiagram.Location = new System.Drawing.Point(0, 0);
            this.diagramOrgChartController1.TemplateDiagram.Name = "";
            this.diagramOrgChartController1.TemplateDiagram.OptionsBehavior.SelectedStencils = new DevExpress.Diagram.Core.StencilCollection(new string[] {
            "TemplateDesigner"});
            this.diagramOrgChartController1.TemplateDiagram.OptionsView.CanvasSizeMode = DevExpress.Diagram.Core.CanvasSizeMode.Fill;
            this.diagramOrgChartController1.TemplateDiagram.OptionsView.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            this.diagramOrgChartController1.TemplateDiagram.OptionsView.ShowPageBreaks = false;
            this.diagramOrgChartController1.TemplateDiagram.TabIndex = 0;
            // 
            // diagramList1
            // 
            this.diagramList1.Anchors = ((DevExpress.Diagram.Core.Sides)((DevExpress.Diagram.Core.Sides.Left | DevExpress.Diagram.Core.Sides.Top)));
            this.diagramList1.CanAddItems = false;
            this.diagramList1.CanCopyWithoutParent = true;
            this.diagramList1.ConnectionPoints = new DevExpress.XtraDiagram.PointCollection(new DevExpress.Utils.PointFloat[] {
            new DevExpress.Utils.PointFloat(0.5F, 0F),
            new DevExpress.Utils.PointFloat(1F, 0.5F),
            new DevExpress.Utils.PointFloat(0.5F, 1F),
            new DevExpress.Utils.PointFloat(0F, 0.5F)});
            this.diagramList1.DragMode = DevExpress.Diagram.Core.ContainerDragMode.ByAnyPoint;
            this.diagramList1.Items.AddRange(new DevExpress.XtraDiagram.DiagramItem[] {
            this.diagramShape1});
            this.diagramList1.ItemsCanAttachConnectorBeginPoint = false;
            this.diagramList1.ItemsCanAttachConnectorEndPoint = false;
            this.diagramList1.ItemsCanChangeParent = false;
            this.diagramList1.ItemsCanCopyWithoutParent = false;
            this.diagramList1.ItemsCanDeleteWithoutParent = false;
            this.diagramList1.ItemsCanEdit = false;
            this.diagramList1.ItemsCanMove = false;
            this.diagramList1.ItemsCanSelect = false;
            this.diagramList1.ItemsCanSnapToOtherItems = false;
            this.diagramList1.ItemsCanSnapToThisItem = false;
            this.diagramList1.MoveWithSubordinates = true;
            this.diagramList1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.diagramList1.Position = new DevExpress.Utils.PointFloat(280F, 140F);
            this.diagramList1.ShowHeader = true;
            this.diagramList1.Size = new System.Drawing.SizeF(100F, 71.53125F);
            this.diagramList1.ThemeStyleId = DevExpress.Diagram.Core.DiagramShapeStyleId.Subtle1;
            // 
            // diagramShape1
            // 
            this.diagramShape1.Anchors = ((DevExpress.Diagram.Core.Sides)((DevExpress.Diagram.Core.Sides.Left | DevExpress.Diagram.Core.Sides.Top)));
            this.diagramShape1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.diagramShape1.Appearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.diagramShape1.Appearance.BorderSize = 0;
            this.diagramShape1.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.diagramShape1.Bindings.Add(new DevExpress.Diagram.Core.DiagramBinding("Content", "Description"));
            this.diagramShape1.CanAttachConnectorBeginPoint = false;
            this.diagramShape1.CanAttachConnectorEndPoint = false;
            this.diagramShape1.CanChangeParent = false;
            this.diagramShape1.CanCopyWithoutParent = false;
            this.diagramShape1.CanDeleteWithoutParent = false;
            this.diagramShape1.CanEdit = false;
            this.diagramShape1.CanMove = false;
            this.diagramShape1.CanResize = false;
            this.diagramShape1.CanRotate = false;
            this.diagramShape1.CanSelect = false;
            this.diagramShape1.MoveWithSubordinates = true;
            this.diagramShape1.Size = new System.Drawing.SizeF(100F, 50F);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.diagramControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.diagramControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagramControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagramOrgChartController1.TemplateDiagram)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.diagramOrgChartController1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private DevExpress.XtraDiagram.DiagramControl diagramControl1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevExpress.XtraDiagram.DiagramControl diagramControl2;
        private DevExpress.XtraDiagram.DiagramOrgChartController diagramOrgChartController1;
        private DevExpress.XtraDiagram.DiagramList diagramList1;
        private DevExpress.XtraDiagram.DiagramShape diagramShape1;
    }
}

