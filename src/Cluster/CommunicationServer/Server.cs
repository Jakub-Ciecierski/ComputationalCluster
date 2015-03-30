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
namespace CommunicationServer
{
    class Server
    {
        static void Main(string[] args)
        {
            string host = "192.168.1.14";
            IPAddress address = IPAddress.Parse(host);
            int port = 5555;

            // Create overall system tracker
            SystemTracker systemTracker = new SystemTracker();

            // Start network connection
            NetworkServer server = new NetworkServer(address, port);
            server.OpenConnection();

            // Create messageHandler
            MessageHandler messageHandler = new MessageHandler(systemTracker, server);

            // Start message queue
            MessageQueue messageQueue = new MessageQueue(server);
            messageQueue.Start();

            // Start Message processor
            MessageProcessor messageProcessor = new MessageProcessor(messageQueue, messageHandler);
            messageProcessor.Start();
        }
    }
}
