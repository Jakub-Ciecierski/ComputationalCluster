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
            Console.Write(" >> Listening for clients Actived \n\n");
            while (Active)
            {
                Socket socket = server.AcceptSocket();
                Console.Write(" >> New Client connected: ");
                Console.Write(SocketRemoteAddressToString(socket) + "\n\n");
                lock (connections)
                {
                    connections.Add(socket);
                }
            }
            Console.Write(" >> Listening for clients Deactived \n");
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Opens a Tcp Listener
        /// </summary>
        public void OpenConnection()
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
                        Console.Write("Client disconnected: " + SocketRemoteAddressToString(socket) + "\n\n");
                        socketsToRemove.Add(socket);
                    }
                    if(socket != null)
                        sockets.Add(socket);
                }

                foreach (Socket socket in socketsToRemove)
                {
                    connections.Remove(socket);
                    //socket.Close();
                }
            }

            if (sockets.Count == 0)
                return sockets;

            // TODO fix busy waiting
            Socket.Select(sockets, null, null, 1000);
            return sockets;
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
        public static string SocketRemoteAddressToString(Socket socket)
        {
            return (socket.RemoteEndPoint as IPEndPoint).Address + ":" + (socket.RemoteEndPoint as IPEndPoint).Port;
        }
    }
}
