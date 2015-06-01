using Cluster.Util;
using Communication;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Cluster.Client.Messaging
{
    /// <summary>
    ///     Used to send a given message and wait response.
    ///     Uses MessageHandler
    /// </summary>
    public class MessageProcessor
    {
        /// <summary>
        ///     How many timeouts to wait untill switching to next server
        /// </summary>
        private const int SERVER_SWITCH_WAIT_SCALLAR = 2;

        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        public NetworkClient client;

        /// <summary>
        ///     Handles all the messages in the queue
        /// </summary>
        private ClientMessageHandler messageHandler;

        private NetworkNode node;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public MessageProcessor(ClientMessageHandler messageHandler, NetworkClient client, NetworkNode node)
        {
            this.client = client;
            this.messageHandler = messageHandler;

            this.node = node;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/



        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Sends status to the server and waits for response
        /// </summary>
        public void Communicate(Message message)
        {
            List<Message> messages = new List<Message>();
            messages.Add(message);
            this.Communicate(messages);
        }

        public void Communicate(List<Message> messages)
        {
            lock (this)
            {
                while (!client.Connected)
                {
                    client.Connect();
                }
                // Send to server
                client.Send(messages);

                // Wait for response
                List<Message> responseMessage = client.ReceiveMessages();

                for (int i = 0; i < responseMessage.Count; i++)
                {
                    // handle response  
                    Message message = responseMessage[i];
                    messageHandler.Handle(message);
                }
            }
        }
    }
}
