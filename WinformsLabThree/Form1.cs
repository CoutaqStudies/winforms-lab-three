using Cyotek.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WinformsLabThree
{
    public partial class Form1 : Form
    {
        private String currentFolder;
        public Color graphicsColor = Color.BlueViolet;
        public Color officeColor = Color.DarkSalmon;
        public Color archiveColor = Color.SteelBlue;
        public Color executableColor = Color.OrangeRed;
        public long largeFileThreshold = 1000000;
        public long smallFileThreshold = 10000;
        public static string treeInText = String.Empty;
        public Form1()
        {
            InitializeComponent();
            this.menuStrip1.Renderer = new FluentDesignRenderer();
            this.toolStrip1.Renderer = new FluentDesignRenderer();
            this.listView1.Items.Clear();
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            this.treeView1.Nodes.Clear();// DO NOT UNDER ANY CIRCUMSTANCE REMove THIS THIS BREAKS EVERYTHING
            // this.listView1.SizeChanged += new EventHandler(ListView_SizeChanged);
            SizeLastColumn(listView1);
        }

        private void listView1_Resize(object sender, System.EventArgs e)
        {
            SizeLastColumn((ListView)sender);
        }

        private void SizeLastColumn(ListView lv)
        {
            lv.Columns[lv.Columns.Count - 1].Width = -2;
        }

        #region Style
        public class FluentDesignRenderer : ToolStripProfessionalRenderer
        {
            public FluentDesignRenderer()
                : base(new MyColorTable())
            {
            }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(e.ArrowRectangle.Location, e.ArrowRectangle.Size);
                r.Inflate(-2, -6);
                e.Graphics.DrawLines(Pens.Black, new Point[]{
        new Point(r.Left, r.Top),
        new Point(r.Right, r.Top + r.Height /2),
        new Point(r.Left, r.Top+ r.Height)});
            }

            protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var r = new Rectangle(e.ImageRectangle.Location, e.ImageRectangle.Size);
                r.Inflate(-4, -6);
                e.Graphics.DrawLines(Pens.Black, new Point[]{
        new Point(r.Left, r.Bottom - r.Height /2),
        new Point(r.Left + r.Width /3,  r.Bottom),
        new Point(r.Right, r.Top)});
            }
        }
        public class MyColorTable : ProfessionalColorTable
        {
            public new bool UseSystemColors = true;
            /*public override Color MenuItem
            {
                get { return Color.White; }
            }*/
            public override Color ToolStripDropDownBackground
            {
                get { return Color.WhiteSmoke; }
            }
            public override Color ButtonSelectedBorder
            {
                get { return Color.WhiteSmoke; }
            }
            public override Color ButtonSelectedGradientBegin
            {
                get { return Color.White; }
            }
            public override Color ButtonSelectedGradientEnd
            {
                get { return Color.White; }
            }
            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.White; }
            }
            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.White; }
            }
            public override Color MenuItemBorder
            {
                get { return Color.WhiteSmoke; }
            }
            public override Color MenuItemSelected
            {
                get { return Color.WhiteSmoke; }
            }
            public override Color ImageMarginGradientBegin
            {
                get { return Color.White; }
            }
            public override Color ImageMarginGradientMiddle
            {
                get { return Color.White; }
            }
            public override Color ImageMarginGradientEnd
            {
                get { return Color.White; }
            }
            public override Color ToolStripGradientBegin
            {
                get { return Color.White; }
            }
            public override Color ToolStripGradientMiddle
            {
                get { return Color.White; }
            }
            public override Color ToolStripGradientEnd
            {
                get { return Color.White; }
            }
            public override Color ToolStripBorder
            {
                get { return Color.WhiteSmoke; }
            }
        }
        #endregion
        private void OpenClicked(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                currentFolder = folderBrowserDialog1.SelectedPath;
                this.treeView1.Nodes.Clear();
                this.treeView1.Nodes.Add(CreateDirectoryNode(new DirectoryInfo(currentFolder)));
            }

        }
        private void SaveClicked(object sender, EventArgs e)
        {
            Console.WriteLine(treeInText);
            MessageBox.Show(treeInText);
            string fileName;
            if (this.saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                btnCreateTreeData(saveFileDialog1.FileName);

        }

        private void btnCreateTreeData(String filePath)
        {
            System.Text.StringBuilder buffer = new System.Text.StringBuilder();
            foreach (TreeNode rootNode in treeView1.Nodes)
                BuildTreeString(rootNode, buffer);
            System.IO.File.WriteAllText(filePath, buffer.ToString());
        }

        private void BuildTreeString(TreeNode rootNode, System.Text.StringBuilder buffer)
        {
            buffer.Append(rootNode.Text);
            buffer.Append(Environment.NewLine);
            foreach (TreeNode childNode in rootNode.Nodes)
                BuildTreeString(childNode, buffer);
        }
        private static TreeNode CreateDirectoryNode(DirectoryInfo directoryInfo)
        {
            var directoryNode = new TreeNode(directoryInfo.Name);
            foreach (var directory in directoryInfo.GetDirectories())
            {
                directoryNode.Nodes.Add(CreateDirectoryNode(directory));

            }
               
            return directoryNode;
        }
        #region trash
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
        private void loadFilesFromFolder(object sender, TreeViewEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
        #endregion

        private void treeView1_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            int large = 0, medium = 0, small = 0;
            this.listView1.Items.Clear();
            string filepath = currentFolder.Substring(0, currentFolder.LastIndexOf('\\') + 1) + treeView1.SelectedNode.FullPath;
            string[] files = Directory.GetFiles(filepath);
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);
                string name = info.Name;
                string size = info.Length.ToString();
                if (info.Length > largeFileThreshold)
                    large++;
                else if (info.Length < smallFileThreshold)
                    small++;
                else
                    medium++;
                string type = name.Substring(name.LastIndexOf('.'), name.Length - name.LastIndexOf('.'));
                Console.WriteLine(name + size + type);
                ListViewItem item = new ListViewItem(name);
                item.SubItems.Add(size);
                item.SubItems.Add(type);
                item.Checked = true;
                listView1.Items.Add(item);
            }
            List<ChartItem> items = new List<ChartItem>();
            items.Add(new ChartItem("Large", large));
            items.Add(new ChartItem("Medium", medium));
            items.Add(new ChartItem("Small", small));
            changeColors();
            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            SizeLastColumn(listView1);
            drawCharts(items);
        }

        private void chart1_Click(object sender, EventArgs e)
        {
           
        }
        private void drawCharts(List<ChartItem> items)
        {
            chart1.Series[0].Points.Clear();
            chart1.Visible = true;
            List<int> values = new List<int>();
            foreach (var item in items)
            {
                chart1.Series[0].Points.AddXY(item.Name, item.Number);
                values.Add(item.Number);
            }
            chart1.ChartAreas[0].AxisY.Maximum = values.Max()+(int)(values.Max()/20);
        }
        public void drawCheckedItems()
        {
            int large = 0, medium = 0, small = 0;
            List<string> filepaths = new List<string>();
            foreach (ListViewItem item in listView1.CheckedItems)
                filepaths.Add(currentFolder + "\\" + item.Text);
            string[] files = filepaths.ToArray();
            foreach (string file in files)
            {
                FileInfo info = new FileInfo(file);
                string size = info.Length.ToString();
                if (info.Length > largeFileThreshold)
                    large++;
                else if (info.Length < smallFileThreshold)
                    small++;
                else
                    medium++;
            }
            List<ChartItem> items = new List<ChartItem>();
            items.Add(new ChartItem("Large", large));
            items.Add(new ChartItem("Medium", medium));
            items.Add(new ChartItem("Small", small));
            drawCharts(items);
        }
        private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            drawCheckedItems();
        }
        public void changeColors()
        {
            if (Program.lightColors)
                changeColorsButLight();
            else
                foreach (ListViewItem item in listView1.Items)
                {
                    String type = item.SubItems[2].Text;
                    if (type == ".png" || type == ".jpg" || type == ".bmp" || type == ".gif")
                        item.BackColor = graphicsColor;
                    else if (type == ".docx" || type == ".xlsx" || type == ".pdf" || type == ".txt")
                        item.BackColor = officeColor;
                    else if (type == ".zip" || type == ".rar" || type == ".7z")
                        item.BackColor = archiveColor;
                    else if (type == ".exe" || type == ".dll")
                        item.BackColor = executableColor;
               
                }
        }
        private void changeColorsButLight()
        {
            float lowestColor = 0.85F;
            Color newGraphicsColor = graphicsColor;
            Color newOfficeColor = officeColor;
            Color newArchiveColor = archiveColor;
            Color newExecutableColor = executableColor;
            if (newGraphicsColor.GetBrightness() < lowestColor)
                newGraphicsColor = getLightColor(newGraphicsColor, lowestColor);
            if (newOfficeColor.GetBrightness() < lowestColor)
                newOfficeColor = getLightColor(newOfficeColor, lowestColor);
            if (newArchiveColor.GetBrightness() < lowestColor)
                newArchiveColor = getLightColor(newArchiveColor, lowestColor);
            if (newExecutableColor.GetBrightness() < lowestColor)
                newExecutableColor = getLightColor(newExecutableColor, lowestColor);
            foreach (ListViewItem item in listView1.Items)
            {
                String type = item.SubItems[2].Text;
                if (type == ".png" || type == ".jpg" || type == ".bmp" || type == ".gif")
                    item.BackColor = newGraphicsColor;
                else if (type == ".docx" || type == ".xlsx" || type == ".pdf" || type == ".txt")
                    item.BackColor = newOfficeColor;
                else if (type == ".zip" || type == ".rar" || type == ".7z")
                    item.BackColor = newArchiveColor;
                else if (type == ".exe" || type == ".dll")
                    item.BackColor = newExecutableColor;
            }
        }
        private Color getLightColor(Color color, float lowest)
        {
            HslColor h = new HslColor(color.GetHue(), color.GetSaturation(), lowest);
            return h.ToRgbColor();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            new SettingsForm(this).Show();
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButtonFont_Click(object sender, EventArgs e)
        {
            selectFont();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Mikhail Melikov.");
        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Made by Mikhail Melikov.");
        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SettingsForm(this).Show();
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectFont();
        }
        private void selectFont()
        {
            fontDialog1.ShowDialog();
            Font f = fontDialog1.Font;
            listView1.Font = f;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
