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
        }

    }
}
