using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPServerLib.TCPServer
{
    public class ServerConfig
    {
        /*
         * Singleton
         */
        private ServerConfig() { }
        
        private static ServerConfig _instance = new ServerConfig();

        public static ServerConfig Instance => _instance;
        /*
         * End Singleton
         */

        public int ServerPort { get; set; } = 0;
        public int StopServerPort { get; set; } = 0;
        public String ServerName { get; set; } = String.Empty;

        public override string ToString()
        {
            return $"{{{nameof(Instance)}={Instance}, {nameof(ServerPort)}={ServerPort.ToString()}, {nameof(StopServerPort)}={StopServerPort.ToString()}, {nameof(ServerName)}={ServerName}}}";
        }
    }
}
