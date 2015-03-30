using Communication;
using CommunicationServer.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Console.Write(" >> Message Processor Actived \n\n");
            while (Active)
            {
                // TODO busy waiting
                if (messageQueue.Queue.Count != 0)
                {
                    MessagePackage package = messageQueue.Queue.Dequeue();
                    messageHandler.HandleMessage(package);
                }

            }
            Console.Write(" >> Message Processor Deactived \n\n");
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
            startMessageProcessor();
        }
    }
}
