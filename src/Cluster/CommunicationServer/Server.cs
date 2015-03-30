using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.Messages;
using System.Net;
using CommunicationServer.Communication;
using Communication.Network.TCP;
using Communication.MessageComponents;
using CommunicationServer.MessageCommunication;
using System.Threading;
namespace CommunicationServer
{
    class Server
    {
        static void Main(string[] args)
        {
            IPAddress address = getIPAddress();
            int port = 5555;

            Console.Write(" >> Starting server... \n\n");
            Console.Write("Address: " + address.ToString() + ":" + port + "\n\n");

            // Create overall system tracker
            SystemTracker systemTracker = new SystemTracker();

            // Start network connection
            NetworkServer server = new NetworkServer(address, port);
            server.Open();

            // Create messageHandler
            MessageHandler messageHandler = new MessageHandler(systemTracker, server);

            // Start message queue
            MessageQueue messageQueue = new MessageQueue(server);
            messageQueue.Start();

            // Start Message processor
            MessageProcessor messageProcessor = new MessageProcessor(messageQueue, messageHandler);
            messageProcessor.Start();

            Thread.Sleep(100);

            // Start console manager
            ConsoleManager consoleManager = new ConsoleManager(server);
            consoleManager.Start();
        }

        private static IPAddress getIPAddress()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return IPAddress.Parse(localIP);
        }
    }
}
