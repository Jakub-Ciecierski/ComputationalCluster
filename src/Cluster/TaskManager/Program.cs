using Communication;
using Communication.Messages;
using Communication.Network;
using Communication.Network.TCP;
using Cluster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager
{
    class Program
    {
        static void Main(string[] args)
        {
            /************ Create node object ************/
            RegisterType type = RegisterType.TaskManager;
            byte parallelThreads = 5;
            string[] problems = { "DVRP", "Graph coloring" };

            NetworkNode node = new NetworkNode(type, parallelThreads, problems);

            /************ Setup connection ************/
            string host = "192.168.1.14";
            IPAddress address = IPAddress.Parse(host);
            int port = 5555;

            Console.Write("I'm a " + node.Type + "\n\n");
            NetworkClient client = new NetworkClient(address, port);

            ConsoleManager consoleManager = new ConsoleManager(client, node);
            consoleManager.StartConsole();   
        }
    }
}
