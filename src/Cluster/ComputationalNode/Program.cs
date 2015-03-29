using Communication.Network;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Communication;

namespace ComputationalNode
{
    class Program
    {
        static void Main(string[] args)
        {
            /************ Create node object ************/
            RegisterType type = RegisterType.ComputationalNode;
            byte parallelThreads = 5;
            string[] problems = { "DVRP", "Graph coloring" };

            NetworkNode node = new NetworkNode(type, parallelThreads, problems);

            /************ Setup connection ************/
            string host = "192.168.1.14";
            IPAddress address = IPAddress.Parse(host);
            int port = 5555;

            Console.Write(">> Creating client object \n");
            NetworkClient client = new NetworkClient(address, port);

            registerToServer(client, node);
        }

        private static void registerToServer(NetworkClient client, NetworkNode node)
        {
            Console.Write(">> Press enter to connect... \n");
            Console.ReadLine();

            Console.Write(">> Client connecting... \n");
            client.Connect();
            Console.Write(">> Starting socket... \n");
            client.OpenSocket();

            RegisterMessage registerMessage = node.ToRegisterMessage();

            Console.Write(">> Sending message... \n");
            client.Send(registerMessage.ToXmlString());

            Console.Write(">> Waiting for response... \n");
            Message response = client.Receive();

            Console.Write(">> The response is: \n" + response);

            Thread.Sleep(5000);
        }

    }
}
