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
using Cluster.Client;
namespace CommunicationServer
{
    class Server
    {
        static void Main(string[] args)
        {
            // TODO read from config file manager
            IPAddress address = getIPAddress();
            int port = 5555;

            Console.Write(" >> IP: "+ address.ToString() + " Port: "+ port +"\n");

            Console.Write(" >> Starting server... \n\n");
            Console.Write("Address: " + address.ToString() + ":" + port + "\n\n");
            
            // Create overall system tracker
            SystemTracker systemTracker = new SystemTracker();

            // Create list of all clients
            ClientTracker clientTracker = new ClientTracker();

            Thread timeOutCheckThread = new Thread(new ThreadStart(clientTracker.CheckNodesTimeOut));
            timeOutCheckThread.Start();

            // Task Tracker
            TaskTracker taskTracker = new TaskTracker();

            // Start network connection
            NetworkServer server = new NetworkServer(address, port);
            server.Open();

            // Create messageHandler
            MessageHandler messageHandler = new MessageHandler(systemTracker, clientTracker, taskTracker, server);

            // Start message queue
            MessageQueue messageQueue = new MessageQueue(server);
            messageQueue.Start();

            // Start Message processor
            MessageProcessor messageProcessor = new MessageProcessor(messageQueue, messageHandler);
            messageProcessor.Start();

            Thread.Sleep(100);

            clientTracker.CheckNodesTimeOut();

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
                if (ip.AddressFamily.ToString() == "InterNetwork" )
                {
                    localIP = ip.ToString();
                    return IPAddress.Parse(localIP);
                }
            }
            return IPAddress.Parse(localIP);
        }

        private static void consoleInput()
        {

        }
    }
}
