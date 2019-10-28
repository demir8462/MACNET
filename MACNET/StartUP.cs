using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MACNET
{
    public partial class StartUP : Form
    {
        public StartUP()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Form panel = new Form1(int.Parse(textBox1.Text));
            panel.Show();
            this.Hide();
        }

      

        private void StartUP_Load(object sender, EventArgs e)
        {

        }
    }
}
