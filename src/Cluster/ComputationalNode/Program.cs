using Communication.Messages;
using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ComputationalNode
{
    class Program
    {
        static void Main(string[] args)
        {
            registerToServer();
        }

        private static void registerToServer()
        {
            // Create server
            string host = "192.168.1.14";
            IPAddress address = IPAddress.Parse(host);
            int port = 5555;

            Console.Write(">> Creating client object \n");
            NetworkClient client = new NetworkClient(address, port);

            Console.Write(">> Press enter to connect... \n");

            Console.ReadLine();

            Console.Write(">> Client connecting... \n");
            client.Connect();
            Console.Write(">> Starting socket... \n");
            client.StartSocket();

            string[] problems = {"TCP"};
            RegisterMessage registerMessage = new RegisterMessage(RegisterType.ComputationalNode, 5, problems);

            Console.Write(">> Sending message... \n");
            client.Send(registerMessage.ToXmlString());

            Console.Write(">> Waiting for response... \n");
            string response = client.Receive();

            Console.Write(">> The response is: \n" + response);
        }

    }
}
