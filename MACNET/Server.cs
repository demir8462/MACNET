using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Net;

namespace MACNET
{
    public class Server
    {
        public  List<Clients> clients = new List<Clients> { };
        TcpListener listener;
        Socket soket;
        NetworkStream netst;
        public Form1 form;
        Thread THR_Connection_Get;
        public List<short> DISCONNECTED_CLIENTS;
        public bool get_connections;
        int port;
        public Server(Form1 form,int port)
        {
            DISCONNECTED_CLIENTS = new List<short> { };
            this.port = port;
            listener = new TcpListener(IPAddress.Any,port);
            this.form = form;
        }
        public void delete_disconnecteds()
        {
            short id;
            for(int i =0;i<DISCONNECTED_CLIENTS.Count;i++)
            {
                id = DISCONNECTED_CLIENTS[i];
                for(int i2=i+1;i2<clients.Count;i2++)
                {
                    clients[i2].id -= 1;
                }
                clients.RemoveAt(i);
            }
            DISCONNECTED_CLIENTS.Clear();
        }

        public bool START()
        {
            get_connections = true;
            THR_Connection_Get = new Thread(new ThreadStart(getConnections));
            THR_Connection_Get.Start();
            return true;
        }
        public bool STOP()
        {
            get_connections = false;
            return true;
        }
        public void getConnections()
        {
            try
            {
                listener.Start();
                while(get_connections)
                {
                    soket = listener.AcceptSocket();
                    form.setLastActivity("Client "+soket.RemoteEndPoint.ToString()+" Has Connected");
                    netst = new NetworkStream(soket);
                    delete_disconnecteds();
                    clients.Add(new Clients(soket, netst,this));
                    form.refresh_table();
                    form.setOnline((short)clients.Count);
                }
            }catch(Exception e)
            {
                form.Messagebx(e.StackTrace);
            }
        }
        public void sendCommand(int id,string command)
        {
            clients[id].send_Cmd.WriteLine(command);
        }
    }
}
