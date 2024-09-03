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
        
        public MyServer(int port) : base(port, nameof(MyServer))
        {
        }

        public MyServer(): base()
        { }
        

        protected override void TemplateMethod(StreamReader sr, StreamWriter sw)
        {
            string l = sr.ReadLine();
            sw.WriteLine(l);
        }
    }
}
