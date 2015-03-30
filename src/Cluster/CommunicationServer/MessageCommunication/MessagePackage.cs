using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.Communication
{
    /// <summary>
    ///     Keeps the message and the network socket associated with that message
    /// </summary>
    public class MessagePackage
    {
        private Message message;
        /// <summary>
        ///     The message from the client
        /// </summary>
        public Message Message
        {
            get{return message;}
            private set { message = value; } 
        }

        private Socket socket;
        /// <summary>
        ///     The client which sent this message
        /// </summary>
        public Socket Socket
        {
            get { return socket; }
            private set { socket = value; }
        }

        public MessagePackage(Message message, Socket socket)
        { 
            Message = message;
            Socket = socket;
        }
    }
}
