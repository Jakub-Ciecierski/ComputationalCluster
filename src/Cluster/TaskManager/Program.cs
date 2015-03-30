using Communication;
using Communication.Messages;
using Communication.Network;
using Communication.Network.TCP;
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

            Console.Write(" >> I'm a " + node.Type + "\n");
            NetworkClient client = new NetworkClient(address, port);

            registerToServer(client, node);

            ConsoleManager consoleManager = new ConsoleManager(client, node);
            consoleManager.StartConsole();
        }

        private static void registerToServer(NetworkClient client, NetworkNode node)
        {
            Console.Write(" >> Press enter to connect to server... \n");
            Console.ReadLine();

            Console.Write(" >> Client connecting to server... " + client.Address + ":" + client.Port + "\n");
            Console.Write(" >> \n\n");
            client.Connect();

            RegisterMessage registerMessage = node.ToRegisterMessage();

            Console.Write(" >> Sending Register message... \n\n");
            client.Send(registerMessage);

            Console.Write(" >> Waiting for response... \n\n");
            Message response = client.Receive();

            Console.Write(" >> The response is: \n\n" + response.ToString() + "\n\n");
        }

    }
}
