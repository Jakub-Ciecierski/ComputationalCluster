using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Network.TCP
{
    public class NetworkListener : NetworkConnection
    {
        private TcpListener listener = null;

        /// <summary>
        /// Creates a Tcp Server object
        /// </summary>
        /// <param name="address">
        ///     Address of the server
        /// </param>
        /// <param name="port">
        ///     Port to listen to
        /// </param>
        public NetworkListener(IPAddress address, int port) : base(address, port)
        {
            
        }

        /// <summary>
        ///     Opens a Tcp Listener
        /// </summary>
        public void OpenConnection()
        {
            listener = new TcpListener(Address, Port);
            listener.Start();
        }

        /// <summary>
        ///     Closes a Tcp Listener
        /// </summary>
        public void Close()
        {
            if (listener != null)
            {
                listener.Stop();
            }
        }

        public bool IsPending()
        {
            return listener.Pending();
        }

        public Socket GetAcceptedSocket()
        {
            return listener.AcceptSocket();
        }

        public void Send(Socket socket, string message)
        {
            base.Send(socket, message);
        }

        public void Send(Socket socket, Message message)
        {
            base.Send(socket, message);
        }

        public Message Receive(Socket socket)
        {
            return base.Receive(socket);
        }
    }
}
