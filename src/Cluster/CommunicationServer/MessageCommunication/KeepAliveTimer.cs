using Cluster.Client;
using Cluster.Util;
using Communication;
using Communication.Network.TCP;
using CommunicationServer.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private MessageHandler messageHandler;

        private NetworkClient client;

        private SystemTracker systemTracker;

        private NetworkNode node;

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
        public KeepAliveTimer(MessageHandler messageHandler, NetworkClient networkClient, SystemTracker systemTracker, NetworkNode node)
        {
            this.messageHandler = messageHandler;
            this.client = networkClient;
            this.systemTracker = systemTracker;
            this.node = node;

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

            Communicate(node.ToStatusMessage());
        }

        public void Communicate(Message message)
        {
            lock (this)
            {
                if (!client.Connected)
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
                    // This is wrong Socket
                    MessagePackage package = new MessagePackage(responseMessage[i], client.Socket);
                    messageHandler.HandleMessage(package);
                }
            }
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

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
