// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using TryMyServerApp.div;
using TryMyServerApp.Server;

MyServer server = new MyServer(); // use configfile
server.AddTraceLIstener(new TextWriterTraceListener($"Peters.txt"));

server.Start();


//Regn regn = new Regn();

//int res = regn.Div(8,3);
//Console.WriteLine(res);


//ReadConfig conf = new ReadConfig();
//conf.Read("../../../div/Config.xml");

//TryLog log = new TryLog();
//log.Start();

Console.WriteLine("press any key");
Console.ReadKey();
