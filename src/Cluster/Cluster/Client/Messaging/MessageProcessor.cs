using Cluster.Util;
using Communication;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private NetworkClient client;

        /// <summary>
        ///     Handles all the messages in the queue
        /// </summary>
        private ClientMessageHandler messageHandler;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public MessageProcessor(ClientMessageHandler messageHandler, NetworkClient client)
        {
            this.client = client;
            this.messageHandler = messageHandler;
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
            /* TODO
            lock (this)
            {
                if (!client.Connected) {
                    SmartConsole.PrintLine("Lost connection, reconnecting...");
                    client.Connect();
                }

                // Send to server
                client.Send(message);

                // Wait for response
                Message responseMessage = client.Receive();

                // handle response
                messageHandler.Handle(responseMessage);

                //client.Disconnect();
            }
            */
        }

        public void Communicate(List<Message> messages)
        {
            lock (this)
            {
                if (!client.Connected)
                {
                    SmartConsole.PrintLine("Lost connection, reconnecting...", SmartConsole.DebugLevel.Advanced);
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
