using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TCPServerLib.TCPServer;

namespace TryMyServerApp.Server
{
    internal class MyServer : AbstractTCPServer
    {
        public MyServer() { }

        public MyServer(int port = 7) : base(port)
        {
        }

        protected override void TemplateMethod(StreamReader sr, StreamWriter sw)
        {
            string l = sr.ReadLine();
            sw.WriteLine(l);
        }
    }
}
