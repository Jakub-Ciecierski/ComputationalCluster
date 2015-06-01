using Cluster.Client;
using Cluster.Client.Messaging;
using Cluster.Util;
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
            SmartConsole.PrintHeader("Starting primary server");
            SmartConsole.PrintLine("Address: " + address.ToString() + ":" + port, SmartConsole.DebugLevel.Advanced);

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
            SmartConsole.PrintHeader("Starting backup server");
            SmartConsole.PrintLine("Address: " + myAddress.ToString() + ":" + myPort, SmartConsole.DebugLevel.Advanced);

            // Create overall system tracker
            SystemTracker systemTracker = new SystemTracker();

            // Create list of all clients
            ClientTracker clientTracker = new ClientTracker();

            // Task Tracker
            TaskTracker taskTracker = new TaskTracker();

            // Start network connection
            NetworkServer server = new NetworkServer(myAddress, myPort);

            // Create messageHandler
            MessageHandler messageHandler = new MessageHandler(systemTracker, clientTracker, taskTracker, server);

            // Start message queue
            MessageQueue messageQueue = new MessageQueue(server);
            
            // Start Message processor
            CommunicationServer.MessageCommunication.MessageProcessor messageProcessor = new CommunicationServer.MessageCommunication.MessageProcessor(messageQueue, messageHandler);

            // blockade to block untill server is switched to primary mode
            Object backupBlockade = new Object();

            server.Open();
            messageQueue.Start();
            messageProcessor.Start();

            /********************* REGISTER AS NORMAL CLIENT *********************/

            RegisterType type = RegisterType.CommunicationServer;
            NetworkNode node = new NetworkNode(type);
            systemTracker.Node = node;

            NetworkClient client = new NetworkClient(masterAddress, masterPort);

            client.Connect();
            SmartConsole.PrintLine("Sending Register message...", SmartConsole.DebugLevel.Advanced);

            CommunicationServer.MessageCommunication.KeepAliveTimer keepAliveTimer = new 
                                                CommunicationServer.MessageCommunication.KeepAliveTimer(messageHandler,
                                                                                                        client, 
                                                                                                        server,
                                                                                                        systemTracker, 
                                                                                                        node,
                                                                                                        clientTracker,
                                                                                                        backupBlockade);
            keepAliveTimer.Communicate(node.ToRegisterMessage());

            /********************* START COMMUNICATING WITH PRIMARY SERVER *********************/
            SmartConsole.PrintLine("Backup Server starting work", SmartConsole.DebugLevel.Advanced);

            keepAliveTimer.Start();

            // This will hold untill server is switched to primary mode
            lock (backupBlockade)
            {
                Monitor.Wait(backupBlockade);
            }

            /********************* SWITCH TO PRIMARY SERVER *********************/

            SmartConsole.PrintHeader("SWITCHING TO PRIMARY");

            Server.primaryMode = true;

            client.Disconnect();

            clientTracker.RefreshTimeout();
            
            // Start measuring timeout
            clientTracker.StartTimeout();

            // Start console manager
            ConsoleManager consoleManager = new ConsoleManager(server);
            consoleManager.Start();
        }

        public void SwitchToPrimary()
        {

        }
    }
}
