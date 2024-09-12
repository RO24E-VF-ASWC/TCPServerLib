using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Xml.Linq;
using System.Xml;
using System.Diagnostics.Tracing;

namespace TCPServerLib.TCPServer
{
    /// <summary>
    /// An Template TCP server to set up the tcp server on a given port and with an given name 
    /// the server can be soft closed down using a stop server stated on the given port plus one
    /// </summary>
    public abstract class AbstractTCPServer
    {
        // instance fields
        private const int SEC = 1000;
        private readonly IPAddress ListenOnIPAddress = IPAddress.Any;
        private bool running = true;
        private readonly List<Task> clients = new List<Task>();



        protected readonly TraceSource _trace;

        ////properties - is defined in ServerConfig

        ///// <summary>
        ///// Get the port number the server is starting on
        ///// </summary>
        //public int PORT { get; private set; }

        ///// <summary>
        ///// Get the the port number of the stopping server
        ///// </summary>
        //public int STOPPORT { get; private set; }

        ///// <summary>
        ///// Get the name given to the server
        ///// </summary>
        //public String NAME { get; private set; }

        /// <summary>
        /// Get an singleton instance of configuration values for the server
        /// </summary>
        protected ServerConfig Conf { get; private set; } = ServerConfig.Instance;


        /*
         * Different constructors
         * 
         * one for port & name
         * 
         */


        /// <summary>
        /// The constructor to template server initilyzing the port and name of the server
        /// </summary>
        /// <param name="port">The port number the server will start / listen on (stopport is port + 1)</param>
        /// <param name="name">The name of the server</param>
        public AbstractTCPServer(int port, string name) : this(port, port + 1, name)
        {
        }

        /// <summary>
        /// The constructor to template server initilyzing the port and name of the server
        /// </summary>
        /// <param name="port">The port number the server will start / listen on (stopport is port + 1)</param>
        /// <param name="stopport">The port number for the stop server</param>
        /// <param name="name">The name of the server</param>
        public AbstractTCPServer(int port, int stopport, string name)
        {
            Conf.ServerPort = port;
            Conf.StopServerPort = stopport;
            Conf.ServerName = name;

            _trace = new TraceSource(name);
            _trace.Switch = new SourceSwitch(name, SourceLevels.Information.ToString());
#if DEBUG
            _trace.Listeners.Add(new ConsoleTraceListener());
#endif
        }

        /// <summary>
        /// The constructor to template server initilyzing the port and name of the server by af configuration file
        /// </summary>
        /// <param name="filename">The name of the configuration file default is 'serverconfig.xml'</param>
        public AbstractTCPServer(String filename = "serverconfig.xml")
        {
            string configpath = @"C:\config\"; // or read env. variable like
                                               // Environment.GetEnvironmentVariable("AbstractServerConf")
            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(configpath + filename);


            XmlNode? portNode = configDoc.DocumentElement?.SelectSingleNode("ServerPort");
            if (portNode != null)
            {
                String portStr = portNode.InnerText.Trim();
                int port = Convert.ToInt32(portStr);
                Conf.ServerPort = port;
            }

            XmlNode? stopportNode = configDoc.DocumentElement?.SelectSingleNode("StopServerPort");
            if (stopportNode != null)
            {
                String stopportStr = stopportNode.InnerText.Trim();
                int stopport = Convert.ToInt32(stopportStr);
                Conf.StopServerPort = stopport;
            }


            XmlNode? nameNode = configDoc.DocumentElement?.SelectSingleNode("ServerName");
            if (nameNode != null)
            {
                String name = nameNode.InnerText.Trim();
                Conf.ServerName = name;
            }

            _trace = new TraceSource(Conf.ServerName);
            _trace.Switch = new SourceSwitch(Conf.ServerName, SourceLevels.Information.ToString());
#if DEBUG
            _trace.Listeners.Add(new ConsoleTraceListener());
#endif
        }


        public void AddTraceLIstener(TraceListener listener)
        {
            _trace.Listeners.Add(listener);
        }
        public void RemoveTraceLIstener(TraceListener listener)
        {
            _trace.Listeners.Remove(listener);
        }

        

        /*
         * Code for the server
         */



        /// <summary>
        /// Starts the server, this include a stopserver  
        /// </summary>
        public void Start()
        {
            // start stop server
            Task.Run(TheStopServer); // kort for Task.Run( ()=>{ TheStopServer(); });


            TcpListener listener = new TcpListener(IPAddress.Any, Conf.ServerPort);
            listener.Start();
            _trace.TraceEvent(TraceEventType.Information, 700, $"Server {Conf.ServerName} started on port {Conf.ServerPort}");

            while (running)
            {
                if (listener.Pending()) // der findes en klient
                {
                    TcpClient client = listener.AcceptTcpClient();
                    _trace.TraceEvent(TraceEventType.Information, 700,$"remote (ip,port) = ({client.Client.RemoteEndPoint})");

                    clients.Add(
                        Task.Run(() =>
                            {
                                TcpClient tmpClient = client;
                                DoOneClient(client);
                            })
                        );
                }
                else  // der er PT ingen klient
                {
                    Thread.Sleep(2 * SEC);
                }

            }
            // vente på alle task bliver færdige
            Task.WaitAll(clients.ToArray());

            _trace.TraceEvent(TraceEventType.Warning, 700, $"Server {Conf.ServerName} stopped");
            _trace.Close();
        }

        private void DoOneClient(TcpClient client)
        {
            using (StreamReader sr = new StreamReader(client.GetStream()))
            using (StreamWriter sw = new StreamWriter(client.GetStream()))
            {
                sw.AutoFlush = true;
                _trace.TraceEvent(TraceEventType.Information, 700, "Handle one client");

                TemplateMethod(sr, sw);
            }
        }

        /// <summary>
        /// This method implement what is specific for this server 
        /// e.g. if this is an echo server read from sr and write to sw
        /// </summary>
        /// <param name="sr">The streamreader from where you can read strings from the socket</param>
        /// <param name="sw">The streamwriter whereto you can write strings to the socket</param>
        protected abstract void TemplateMethod(StreamReader sr, StreamWriter sw);




        /*
        * stop server
        */
        private void StoppingServer()
        {
            running = false;
        }

        private void TheStopServer()
        {
            TcpListener listener = new TcpListener(ListenOnIPAddress, Conf.StopServerPort);
            listener.Start();
            _trace.TraceEvent(TraceEventType.Warning, 700, $"StopServer {Conf.ServerName} started on port {Conf.StopServerPort}");
            TcpClient client = listener.AcceptTcpClient();
            //todo tjek om det er lovligt fx et password

            _trace.TraceEvent(TraceEventType.Warning, 700, $"Server {Conf.ServerName} is closing");
            StoppingServer();
            client?.Close();
            listener?.Stop(); // bare for at være pæn - det hele lukker alligevel
        }

    }
}
