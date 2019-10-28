using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System.Threading;
namespace MACNET
{
    public class Clients
    {
        NetworkStream netst;
        Socket soket;
        public short id;
        public string ip, count;
        public StreamWriter send_Cmd;
        public StreamReader get_Data;
        Server server;
        Thread THR_CONTROL;
        bool yasam;
        public Clients(Socket soket,NetworkStream netst,Server server)
        {
            
            id = (short)server.clients.Count;
            ip = ((IPEndPoint)soket.RemoteEndPoint).Address.ToString();
            count = "NOT ADDED";
            this.netst = netst;
            this.soket = soket;
            this.server = server;
            send_Cmd = new StreamWriter(netst);
            send_Cmd.AutoFlush = true;
            get_Data = new StreamReader(netst);         
            THR_CONTROL = new Thread(new ThreadStart(control));
            THR_CONTROL.Start();
            
        }
        public void send_COMMAND(string text)
        {
            send_Cmd.WriteLine(Convert.ToBase64String(UTF8Encoding.UTF8.GetBytes(text)));
            send_Cmd.Flush();
        }
      
        public void control()
        {
            yasam = true;
            while(yasam)
            {

                try
                {
                    if (((soket.Poll(1000, SelectMode.SelectRead) && (soket.Available == 0)) || !soket.Connected))
                    {
                        try
                        {
                            send_Cmd.Close();
                            get_Data.Close();
                            netst.Close();
                            soket.Close();
                        }
                        catch (Exception ee)
                        {

                        }
                        finally
                        {
                            server.DISCONNECTED_CLIENTS.Add(id);
                            server.form.setLastActivity("Client " + ip + " Has Disconnected");
                            if(server.form.panel.clientID == id)
                            {
                                server.form.panel.Hide();
                            }
                            yasam = false;
                            server.delete_disconnecteds();
                            server.form.refresh_table();
                        }
                    }
                }catch(ObjectDisposedException rrr)
                {
                    server.DISCONNECTED_CLIENTS.Add(id);
                    server.form.setLastActivity("Client " + ip + " Has Disconnected");
                    if (server.form.panel.clientID == id)
                    {
                        server.form.panel.Hide();
                    }
                    yasam = false;
                    server.delete_disconnecteds();
                    server.form.refresh_table();
                }
            }
        }
    }
}
