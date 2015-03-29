using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Network.TCP
{
    public class NetworkClient : NetworkConnection
    {
        private TcpClient client = null;

        public Socket socket = null;

        public Socket Socket
        {
            get { return socket; }
        }

        /// <summary>
        /// Creates a tcp client
        /// </summary>
        /// <param name="address">
        ///     Address of the server
        /// </param>
        /// <param name="port">
        ///     Port to listen to
        /// </param>
        public NetworkClient(IPAddress address, int port) : base(address, port)
        {
            
        }

        /// <summary>
        ///     Gets the socket
        /// </summary>
        public void OpenSocket()
        {
            if(client == null)
                throw new NullReferenceException("Connect to server before starting socket");
            socket = client.Client;
        }

        /// <summary>
        ///     Closes the socket
        /// </summary>
        public void CloseSocket()
        {
            if (socket != null)
                socket.Close();
        }

        /// <summary>
        ///     TODO Exception handler
        ///     Connects to server
        /// </summary>
        public void Connect()
        {
            client = new TcpClient();
            client.Connect(Address, Port);
        }

        /// <summary>
        ///     Disconnects, closes socket if one was connected
        /// </summary>
        public void Disconnect()
        {
            if (client != null)
            {
                if (socket != null)
                    socket.Close();
                client.Close();
            }
        }

        public void Send(string message)
        {
            base.Send(socket, message);
        }

        public void Send(Message message)
        {
            base.Send(socket, message);
        }

        public Message Receive()
        {
            return base.Receive(socket);
        }
    }
}
