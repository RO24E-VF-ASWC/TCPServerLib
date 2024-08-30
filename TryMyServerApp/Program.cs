// See https://aka.ms/new-console-template for more information

using TryMyServerApp.div;
using TryMyServerApp.Server;

MyServer server = new MyServer();
server.Start();

//Regn regn = new Regn();

//int res = regn.Div(8,3);
//Console.WriteLine(res);


Console.WriteLine("press any key");
Console.ReadKey();
