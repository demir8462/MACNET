using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// CLIENT KAPATMA SIRASI AYARLANACAK !
namespace MACNET
{
    public partial class Form1 : Form
    {
        private int move, x, y;
        string[] listviewbuf;
        short SELECTED_CLIENT = 0;
        Server server;
        public ClientPanel panel;
        int PORT;
        public Form1(int port)
        {
            InitializeComponent();
            PORT = port;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(-1);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.AllowColumnReorder = false;
            listView1.Columns.Add("ID", 69);
            listView1.Columns.Add("IP:PORT", 350);
            listView1.Columns.Add("ADD TIME",350);
            server = new Server(this,PORT);
            server.START();
            panel = new ClientPanel(server);
        }
        public void additem(int id,string ip,string count)
        {
            ListViewItem lvitem;
            listviewbuf = new string[]{id.ToString(),ip,count };
            lvitem = new ListViewItem(listviewbuf);
            listView1.Items.Add(lvitem);
          
        }
        public void refresh_table()
        {
            // önce ölü var mı kontrol et
            server.delete_disconnecteds();
            listView1.Items.Clear();
            for(int i =0;i<server.clients.Count;i++)
            {
                additem(server.clients[i].id,server.clients[i].ip, server.clients[i].count);
            }
            setOnline((short)server.clients.Count);
        }
        public void setLastActivity(string text)
        {
            LBL_last_activiy.Text = text;
        }
        public void setOnline(short howmany)
        {
            LBL_HowManyClient.Text = server.clients.Count.ToString() + "Clients Online";
        }
        private void Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if(move == 1)
            {
                this.Location = new Point(Cursor.Position.X - x, Cursor.Position.Y - y);
                this.Update();
            }
        }
        public void Messagebx(string text)
        {
            MessageBox.Show(text);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            refresh_table();
            setOnline((short)server.clients.Count);
        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count > 0)
            {
                SELECTED_CLIENT = (short)listView1.Items.IndexOf(listView1.SelectedItems[0]);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            panel.clientID = SELECTED_CLIENT;
            panel.Show();
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
