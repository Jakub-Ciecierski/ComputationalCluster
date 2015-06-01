using Cluster.Client;
using Cluster.Util;
using Communication;
using Communication.MessageComponents;
using Communication.Network.TCP;
using CommunicationServer.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace CommunicationServer.MessageCommunication
{
    /// <summary>
    ///     Makes sure that keep alive messages are sent within the timout
    ///     and uses the MessageHandler to handle the requests.
    /// </summary>
    public class KeepAliveTimer
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     How many timeouts to wait untill switching to next server
        /// </summary>
        private const int SERVER_SWITCH_WAIT_SCALLAR = 1;

        /// <summary>
        ///     How many times will the client try to reconnect before switching
        /// </summary>
        private const int MAX_RECONNECTS = 2;

        private MessageHandler messageHandler;

        private NetworkClient client;

        private NetworkServer server;

        private SystemTracker systemTracker;

        private NetworkNode node;

        private ClientTracker clientTracker;

        private object backupBlockade;

        /// <summary>
        ///     Timer for sending messages
        /// </summary>
        private System.Timers.Timer timer;

        private bool isActive;

        /// <summary>
        ///     If turned to false, the Message processor is stoped
        /// </summary>
        public bool Active
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        /// <summary>
        ///     Creates KeepAliveTimer
        /// </summary>
        /// <param name="messageProcessor"></param>
        /// /// <param name="systemTracker"></param>
        public KeepAliveTimer(MessageHandler messageHandler, NetworkClient networkClient, NetworkServer server,
                                SystemTracker systemTracker, NetworkNode node,
                                ClientTracker clientTracker, Object backupBlockade)
        {
            this.messageHandler = messageHandler;
            this.client = networkClient;
            this.server = server;
            this.systemTracker = systemTracker;
            this.node = node;
            this.clientTracker = clientTracker;
            this.backupBlockade = backupBlockade;

            // TODO Magic numbers
            this.timer = new System.Timers.Timer((systemTracker.Timeout * 1000) / 2);
            this.timer.Elapsed += keepAlive;

            Active = false;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void keepAlive(Object source, ElapsedEventArgs e)
        {
            SmartConsole.PrintLine("Sending Status message", SmartConsole.DebugLevel.Basic);

            try
            {
                Communicate(node.ToStatusMessage());
            }
            catch (SocketException excep) 
            {
                if (clientTracker.BackupServers.Length == 0)
                {
                    SmartConsole.PrintLine("No other backup server avaiable", SmartConsole.DebugLevel.Advanced);
                    return;
                }
                BackupCommunicationServer bserver = clientTracker.BackupServers[0];

                // If it is next backup server, switch to primary and remove it from the list
                if (server.Address.ToString().Equals(bserver.address) && server.Port == bserver.port)
                {
                    // TODO Switch only if it is next back up in the list, and decreament the list it self
                    SmartConsole.PrintLine("Switching to primary...", SmartConsole.DebugLevel.Advanced);
                    // stop the timer
                    this.Stop();

                    clientTracker.RemoveBackupServer(0);

                    lock (backupBlockade)
                    {
                        Monitor.Pulse(backupBlockade);
                    }
                    return;
                }
                // connect to next backup server.
                else 
                {
                    client.Address = IPAddress.Parse(bserver.address);
                    client.Port = bserver.port;
                }
            }
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void Communicate(Message message)
        {
            lock (this)
            {
                while (!client.Connected)
                {
                    SmartConsole.PrintLine("Lost connection, reconnecting...", SmartConsole.DebugLevel.Advanced);
                    client.Connect();
                }

                // Send to server
                client.Send(message);

                // Wait for response
                List<Message> responseMessage = client.ReceiveMessages();

                for (int i = 0; i < responseMessage.Count; i++)
                {
                    // handle response  
                    // This is wrong Socket, but we dont use it anyway.
                    MessagePackage package = new MessagePackage(responseMessage[i], client.Socket);
                    messageHandler.HandleMessage(package);
                }
            }
        }

        /// <summary>
        ///     Starts the message processor
        /// </summary>
        public void Start()
        {
            timer.Enabled = true;
        }

        /// <summary>
        ///     Stops the timer
        /// </summary>
        public void Stop()
        {
            timer.Enabled = false;
        }
    }
}
