using Cluster.Util;
using Communication;
using Communication.Network.TCP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServer.Communication
{
    /// <summary>
    ///     MessageQueue is used to keep track of received messages from the network
    ///     which later can be handled accordingly
    /// </summary>
    public class MessageQueue
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private NetworkServer server;

        /// <summary>
        ///     The message queueing thread
        /// </summary>
        private Thread queueThread;

        private Queue<MessagePackage> messageQueue = new Queue<MessagePackage>();
        /// <summary>
        ///     The message Queue
        /// </summary>
        public Queue<MessagePackage> Queue
        {
            get { return messageQueue; }
            set { messageQueue = value; } 
        }

        private bool isActive;
        /// <summary>
        ///     Determines if the Message queue will continue 
        ///     to receive messages from the network
        ///     
        ///     TODO thread safetly
        /// </summary>
        public bool Active
        {
            get { return isActive; }
            set { isActive = value; }
        }

        /********************************************************************/
        /*************************** CONSTRUCTORS ***************************/
        /********************************************************************/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server">
        ///     The server which will listen to the network
        /// </param>
        public MessageQueue(NetworkServer server)
        {
            this.server = server;
            Active = false;
        }

        /********************************************************************/
        /************************** PRIVATE METHODS *************************/
        /********************************************************************/

        private void startMessageQueue()
        {
            SmartConsole.PrintLine("Message Queing Actived", SmartConsole.DebugLevel.Advanced);

            while (Active)
            {
                // should hang
                ArrayList socketsToRead = server.SelectForRead();

                foreach (Socket socket in socketsToRead)
                {
                    // NEW SENDING MESSAGE
                    List<Message> messages = server.ReceiveMessages(socket);

                    for (int i = 0; i < messages.Count; i++) 
                    {
                        Message message = messages[i];
                        // Message was null if client disconnected
                        if (message == null)
                            continue;

                        MessagePackage messageHandler = new MessagePackage(message, socket);
                        lock (Queue)
                        {
                            Queue.Enqueue(messageHandler);
                        }
                    }
                    
                    // END NEW SENDING MESSAGE

                }
            }
            SmartConsole.PrintLine("Message Queing Deactived", SmartConsole.DebugLevel.Advanced);
        }

        /********************************************************************/
        /************************** PUBLIC METHODS **************************/
        /********************************************************************/

        /// <summary>
        ///     Starts Receiving messages from the network
        /// </summary>
        public void Start()
        {
            Active = true;
            queueThread = new Thread(startMessageQueue);
            queueThread.Start();
        }

    }
}
