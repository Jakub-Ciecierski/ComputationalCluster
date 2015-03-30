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
        /************************** PRIVATEC METHODS ************************/
        /********************************************************************/

        private void startMessageQueue()
        {
            Console.Write(" >> Message Queing Actived \n\n");
            while (Active)
            {
                // should hang
                ArrayList socketsToRead = server.SelectForRead();

                foreach (Socket socket in socketsToRead)
                {
                    Message message = server.Receive(socket);

                    MessagePackage messageHandler = new MessagePackage(message, socket);
                    lock (Queue)
                    {
                        Queue.Enqueue(messageHandler);
                        Console.Write(" >> Added message to queue \n\n");
                    }
                }
            }
            Console.Write(" >> Message Queing Deactived \n\n");
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
