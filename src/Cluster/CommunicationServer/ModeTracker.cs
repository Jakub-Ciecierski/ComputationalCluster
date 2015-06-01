using Cluster.Client;
using Cluster.Client.Messaging;
using Communication.Messages;
using Communication.Network.TCP;
using CommunicationServer.Communication;
using CommunicationServer.MessageCommunication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServer
{
    public class ModeTracker
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public ModeTracker()
        {

        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
        public void InitiatePrimary(IPAddress address, int port)
        {
            Console.Write(" >> Starting server... \n\n");
            Console.Write(" >> Address: " + address.ToString() + ":" + port + "\n\n");

            // Create overall system tracker
            SystemTracker systemTracker = new SystemTracker();

            // Create list of all clients
            ClientTracker clientTracker = new ClientTracker();

            // Start measuring timeout
            clientTracker.StartTimeout();

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
            CommunicationServer.MessageCommunication.MessageProcessor messageProcessor = new CommunicationServer.MessageCommunication.MessageProcessor(messageQueue, messageHandler);
            messageProcessor.Start();

            Thread.Sleep(100);

            // Start console manager
            ConsoleManager consoleManager = new ConsoleManager(server);
            consoleManager.Start();
        }

        public void InitiateBackup(IPAddress myAddress, int myPort, IPAddress masterAddress, int masterPort)
        {
            Console.Write(" >> Starting backup server... \n\n");
            Console.Write(" >> Address: " + myAddress.ToString() + ":" + myPort + "\n\n");

            // Create overall system tracker
            SystemTracker systemTracker = new SystemTracker();

            // Create list of all clients
            ClientTracker clientTracker = new ClientTracker();

            // Start measuring timeout
            clientTracker.StartTimeout();

            // Task Tracker
            TaskTracker taskTracker = new TaskTracker();

            // Start network connection
            NetworkServer server = new NetworkServer(myAddress, myPort);
            server.Open();

            // Create messageHandler
            MessageHandler messageHandler = new MessageHandler(systemTracker, clientTracker, taskTracker, server);

            // Start message queue
            MessageQueue messageQueue = new MessageQueue(server);
            messageQueue.Start();

            // Start Message processor
            CommunicationServer.MessageCommunication.MessageProcessor messageProcessor = new CommunicationServer.MessageCommunication.MessageProcessor(messageQueue, messageHandler);
            messageProcessor.Start();

            //Thread.Sleep(100);

            // Start console manager
            ConsoleManager consoleManager = new ConsoleManager(server);
            //consoleManager.Start();

            /********************* REGISTER AS NORMAL CLIENT *********************/

            RegisterType type = RegisterType.CommunicationServer;
            NetworkNode node = new NetworkNode(type);
            systemTracker.Node = node;

            NetworkClient client = new NetworkClient(masterAddress, masterPort);


            /************ Register ************/
            client.Connect();
            Console.Write(" >> Sending Register message... \n\n");

            CommunicationServer.MessageCommunication.KeepAliveTimer keepAliveTimer = new 
                                                CommunicationServer.MessageCommunication.KeepAliveTimer(messageHandler,
                                                                                                        client, 
                                                                                                        systemTracker, 
                                                                                                        node);
            keepAliveTimer.Communicate(node.ToRegisterMessage());
             
            /************ Start Logic modules ************/
            keepAliveTimer.Start();

            Object mutex = new Object();
            //for (; ; ) ;
            // TODO Thread pool waiting

            lock (mutex)
            {
                Monitor.Wait(mutex);
            }

            // switch to Primary here
        }

        public void SwitchToPrimary()
        {

        }
    }
}
