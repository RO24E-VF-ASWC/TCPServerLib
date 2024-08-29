using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TCPServerLib.TCPServer
{
    public abstract class AbstractTCPServer
    {
        private readonly int PORT = 7;

        public AbstractTCPServer(int port = 7)
        {
            PORT = port;
        }
        

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, PORT);
            listener.Start();
            Console.WriteLine("Server started");

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Client incoming");
                Console.WriteLine($"remote (ip,port) = ({client.Client.RemoteEndPoint})");

                Task.Run(() =>
                {
                    TcpClient tmpClient = client;
                    DoOneClient(client);
                });

            }
        }

        private void DoOneClient(TcpClient client)
        {
            using (StreamReader sr = new StreamReader(client.GetStream()))
            using (StreamWriter sw = new StreamWriter(client.GetStream()))
            {
                sw.AutoFlush = true;
                Console.WriteLine("Handle one client");

                TemplateMethod(sr, sw);
            }
        }

        protected abstract void TemplateMethod(StreamReader sr, StreamWriter sw);
        
    }
}
