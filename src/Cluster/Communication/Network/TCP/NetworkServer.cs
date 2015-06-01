using Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication.Network.TCP
{
    /// <summary>
    ///     Network server is used to start TCP connections.
    ///     The server keeps all the connected clients.
    ///     
    ///     It has a saparete thread, listening for client wanting to connect to server
    /// </summary>
    public class NetworkServer : NetworkConnection
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     Thread that listens to the network for new connections
        ///     TODO move to saparete class
        /// </summary>
        private Thread clientListener;

        /// <summary>
        ///     The tcp Listener
        /// </summary>
        private TcpListener server = null;

        private List<Socket> connections = new List<Socket>();
        /// <summary>
        ///     List of current connections in the server
        /// </summary>
        public List<Socket> Connections
        {
            get
            {
                List<Socket> nc = new List<Socket>();
                lock(connections)
                { 
                    nc.AddRange(connections);
                }
                return nc;
            }
        }

        /// <summary>
        ///     Turning this flag to false will stop network listening thread
        ///     TODO thread safetly
        /// </summary>
        private bool isActive;

        public bool Active
        {
            get { return isActive; }
            set { isActive = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        /// <summary>
        /// Creates a Tcp Server object
        /// </summary>
        /// <param name="address">
        ///     Address of the server
        /// </param>
        /// <param name="port">
        ///     Port to listen to
        /// </param>
        public NetworkServer(IPAddress address, int port) : base(address, port)
        {
            
        }

        /******************************************************************/
        /************************ PRIVATE METHODS *************************/
        /******************************************************************/

        /// <summary>
        ///     The logic of StartListeningForClients() method
        /// </summary>
        private void listenForClients()
        {
            Console.Write(" >> Listening for clients Activated \n\n");
            while (Active)
            {
                Socket socket = server.AcceptSocket();
                Console.Write(" >> New Client connected: ");
                
                Console.Write(RemoteAddressToString(socket) + "\n\n");
                lock (connections)
                {
                    connections.Add(socket);
                }
            }
            Console.Write(" >> Listening for clients Deactivated \n");
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Opens a Tcp connection
        /// </summary>
        public void Open()
        {
            server = new TcpListener(Address, Port);
            server.Start();

            // Start client listener thread
            StartListeningForClients();
        }

        /// <summary>
        ///     Closes a Tcp Listener
        /// </summary>
        public void Close()
        {
            if (server != null)
            {
                server.Stop();
            }
        }

        /// <summary>
        ///     Checks if a clients want to connect to the server
        /// </summary>
        /// <returns></returns>
        public bool IsPending()
        {
            return server.Pending();
        }

        /// <summary>
        ///     Sends a string message to the socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        public void Send(Socket socket, string message)
        {
            try
            {
                base.Send(socket, message);
            }
            catch (SocketException e)
            {
                Console.Write(" >> [Send] Socket unavaible, removing connection... \n");
                RemoveConnection(socket);
            }
        }

        /// <summary>
        ///     Sends a message to the socket
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="message"></param>
        public void Send(Socket socket, Message message)
        {
            List<Message> messages = new List<Message>();
            messages.Add(message);
            try
            {
                //base.Send(socket, message);
                base.Send(socket, messages);
            }
            catch (SocketException e)
            {
                Console.Write(" >> [Send] Socket unavaible, removing connection... \n");
                RemoveConnection(socket);
            }
        }

        public void Send(Socket socket, List<Message> messages)
        {
            try
            {
                //base.Send(socket, message);
                base.Send(socket, messages);
            }
            catch (SocketException e)
            {
                Console.Write(" >> [Send] Socket unavaible, removing connection... \n");
                RemoveConnection(socket);
            }
        }

        /// <summary>
        ///     Receives a message
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public Message Receive(Socket socket)
        {
            Message message = null;
            try
            {
                message = base.Receive(socket);
            }
            catch (SocketException e) {
                Console.Write(" >> [Receive] Socket unavaible, removing connection... \n");
                RemoveConnection(socket);
            }
            return message;
        }

        /// <summary>
        ///     Receives many messages
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public List<Message> ReceiveMessages(Socket socket)
        {
            List<Message> messages = new List<Message>();
            try
            {
                messages = base.ReceiveMessages(socket);
            }
            catch (SocketException e) {
                Console.Write(" >> [Receive] Socket unavaible, removing connection... \n");
                RemoveConnection(socket);
            }
            return messages;
        }
        

        /// <summary>
        ///     Starts listening for clients in the network
        /// </summary>
        public void StartListeningForClients()
        {
            Active = true;

            clientListener = new Thread(listenForClients);
            clientListener.Start();
        }

        /// <summary>
        ///     Selects for sockets ready to read, and removed disconnected sockets
        ///     TODO loose cuple the socket Removal part
        /// </summary>
        /// 
        /// <returns>
        ///     List of sockets ready to be read
        /// </returns>
        public ArrayList SelectForRead()
        {
            ArrayList sockets = new ArrayList();
            lock (connections)
            {
                ArrayList socketsToRemove = new ArrayList();

                foreach (Socket socket in connections)
                {
                    if (!IsSocketConnected(socket))
                    {
                        Console.Write("Client disconnected: " + RemoteAddressToString(socket) + "\n\n");
                        socketsToRemove.Add(socket);
                    }
                    if(socket != null)
                        sockets.Add(socket);
                }

                foreach (Socket socket in socketsToRemove)
                {
                    RemoveConnection(socket);
                }
            }

            if (sockets.Count == 0)
                return sockets;

            // TODO fix busy waiting
            Socket.Select(sockets, null, null, 1000);
            return sockets;
        }

        public bool RemoveConnection(Socket socket)
        {
            return connections.Remove(socket);
        }

        /// <summary>
        ///     Returns all connections as a string
        /// </summary>
        /// <returns></returns>
        public string ListAllConnections()
        {
            string con = "";

            foreach (Socket socket in connections)
            {
                con += RemoteAddressToString(socket) + " \n";
            }

            return con;
        }

        /*******************************************************************/
        /************************* STATIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Checks if socket is connected to the network
        /// </summary>
        /// <param name="socket">
        ///     The socket that connections should be checked
        /// </param>
        /// <returns>
        ///     True if socket is connected, false otherwise
        /// </returns>
        public static bool IsSocketConnected(Socket socket)
        {
            return !((socket.Poll(1000, SelectMode.SelectRead) && (socket.Available == 0)) || !socket.Connected);
        }

        /// <summary>
        ///     Returns a convenient socket remote location string
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static string RemoteAddressToString(Socket socket)
        {
            return (socket.RemoteEndPoint as IPEndPoint).Address + ":" + (socket.RemoteEndPoint as IPEndPoint).Port;
        }
    }
}
