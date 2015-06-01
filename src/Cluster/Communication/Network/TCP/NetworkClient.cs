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

        private IPAddress clientAddress;

        private int clientPort;

        public Socket Socket
        {
            get { return socket; }
        }

        public IPAddress ClientAddress
        {
            get { return clientAddress; }
            set { clientAddress = value; }
        }

        public int ClientPort
        {
            get { return clientPort; }
            set { clientPort = value; }
        }

        public bool Connected { 
            get 
            {
                if (client == null)
                    return false;
                if (client.Client == null)
                    return false;
                else
                    return client.Connected;
            } 
        }
        /// <summary>
        /// Creates a tcp client
        /// </summary>
        /// <param name="address">
        ///     Address of the server
        /// </param>
        /// <param name="port">
        ///     Port of the server
        /// </param>
        public NetworkClient(IPAddress address, int port) 
            : base(address, port)
        {
            
        }

        /// <summary>
        ///     Creates a tcp client
        /// </summary>
        /// <param name="client">
        ///     Connected tcp client
        /// </param>
        public NetworkClient(TcpClient client)
            : base((client.Client.LocalEndPoint as IPEndPoint).Address, (client.Client.LocalEndPoint as IPEndPoint).Port)
        {
            this.client = client;
            socket = client.Client;
            ClientAddress = (client.Client.RemoteEndPoint as IPEndPoint).Address;
            ClientPort = (client.Client.RemoteEndPoint as IPEndPoint).Port;
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
            socket = client.Client;
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
            List<Message> messages = new List<Message>();
            messages.Add(message);
            //base.Send(socket, messages);
            base.Send(socket, messages);
        }

        public void Send(List<Message> messages)
        {
            try
            {
                //base.Send(socket, message);
                base.Send(socket, messages);
            }
            catch (SocketException e)
            {
                Console.Write(" >> [Send] Socket unavaible, removing connection... \n");
            }
        }

        public Message Receive()
        {
            Message message = null;
            try
            {
                message = base.Receive(socket);
            }
            catch (SocketException e)
            {
                Console.Write(" >> [Receive] Socket unavaible, removing connection... \n");;
            }
            return message;
        }

        public List<Message> ReceiveMessages()
        {
            lock (this)
            {
                List<Message> messages = new List<Message>();

                Message message = null;
                try
                {
                    messages = base.ReceiveMessages(socket);
                }
                catch (SocketException e)
                {
                    Console.Write(" >> [Receive] Socket unavaible, removing connection... \n");
                }

                return messages;
            }
        }
    }
}
