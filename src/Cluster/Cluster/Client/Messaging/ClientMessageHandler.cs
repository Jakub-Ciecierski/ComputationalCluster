using Communication;
using Communication.Messages;
using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Client.Messaging
{
    /// <summary>
    ///     Handles the communication with the server
    ///     Uses ClientSystemTracker to fetch system informations.
    /// </summary>
    public abstract class ClientMessageHandler
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        protected NetworkClient client;

        protected ClientSystemTracker systemTracker;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        public ClientMessageHandler(ClientSystemTracker systemTracker, NetworkClient client)
        {
            this.systemTracker = systemTracker;
            this.client = client;
        }

        /*******************************************************************/
        /******************* PRIVATE / PROTECTED METHODS *******************/
        /*******************************************************************/

        /// <summary>
        ///     Handle
        /// </summary>
        /// <param name="message"></param>
        protected abstract void handle(Message message);

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/


        /// <summary>
        ///     Handles the given message.
        /// </summary>
        public void Handle(Message message)
        {
            lock (systemTracker)
            {
                handle(message);
            }
        }
    }
}
