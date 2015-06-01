using Cluster.Util;
using Communication;
using CommunicationServer.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationServer.MessageCommunication
{
    /// <summary>
    ///     MessageProcessor processes all the messages that are added
    ///     to the queue
    /// </summary>
    public class MessageProcessor
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     The message queue to be processed
        /// </summary>
        private MessageQueue messageQueue;

        /// <summary>
        ///     Handles all the messages in the queue
        /// </summary>
        private MessageHandler messageHandler;

        /// <summary>
        ///     The thread for precessing messages
        /// </summary>
        private Thread processorThread;

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

        public MessageProcessor(MessageQueue messageQueue, MessageHandler messageHandler)
        {
            this.messageQueue = messageQueue;
            this.messageHandler = messageHandler;

            Active = false;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     The logic of message processors
        /// </summary>
        private void startMessageProcessor()
        {
            SmartConsole.PrintLine("Message Processor Actived", SmartConsole.DebugLevel.Advanced);
            while (Active)
            {
                // TODO busy waiting
                if (messageQueue.Queue.Count != 0)
                {
                    MessagePackage package = messageQueue.Queue.Dequeue();
                    messageHandler.HandleMessage(package);
                }

            }
            SmartConsole.PrintLine("Message Processor Deactived", SmartConsole.DebugLevel.Advanced);
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Starts the message processor
        /// </summary>
        public void Start()
        {
            Active = true;
            processorThread = new Thread(startMessageProcessor);
            processorThread.Start();
        }
    }
}
