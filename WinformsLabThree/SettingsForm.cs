using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinformsLabThree
{
    public partial class SettingsForm : Form
    {
        Form1 form;
        public SettingsForm(Form1 f1)
        {
            form = f1;
            InitializeComponent();
            this.checkBox1.Checked = Program.lightColors;
            this.comboBox1.SelectedIndex = 0;
        }

        public void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Program.lightColors = checkBox1.Checked;
        }

        private void colorEditor1_ColorChanged(object sender, EventArgs e)
        {
            Color color = colorEditor1.Color;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    form.graphicsColor = color;
                    break;
                case 1:
                    form.officeColor = color;
                    break;
                case 2:
                    form.archiveColor = color;
                    break;
                case 3:
                    form.executableColor = color;
                    break;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void SettingsForm_FormClosing(Object sender, FormClosingEventArgs e)
        {
            //form.changeColors();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form.changeColors();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {

        }
    }
}
