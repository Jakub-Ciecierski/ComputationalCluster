using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class Client
    {
        private TcpClient client = null;

        public Socket socket = null;

        private int port;
        private IPAddress address;

        public Socket Socket
        {
            get { return socket; }
        }

        public IPAddress Address 
        { 
            get{return address;}
        }

        public int Port
        { 
            get { return port; }
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
        public Client(IPAddress address, int port)
        {
            this.address = address;
            this.port = port;
        }

        /// <summary>
        ///     Gets the socket
        /// </summary>
        public void StartSocket()
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
            client.Connect(address, port);
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

        /// <summary>
        ///     Receive next message
        /// </summary>
        /// <returns>
        ///     Received message in string
        /// </returns>
        public string Receive()
        {
            if (client == null || socket == null)
                throw new NullReferenceException("Connect and Open socket before calling Receive()");

            byte[] sizeReceiveByte = new byte[sizeof(Int32)];
            socket.Receive(sizeReceiveByte, sizeof(Int32), 0);
            int sizeReceive = BitConverter.ToInt32(sizeReceiveByte, 0);

            byte[] contentReceive = new byte[sizeReceive];
            socket.Receive(contentReceive, sizeReceive, SocketFlags.None);

            string message = System.Text.Encoding.UTF8.GetString(contentReceive);

            return message;
        }
    }
}
