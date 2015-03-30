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
        List<Socket> connections = new List<Socket>();
        Thread clientListener;

        private TcpListener server = null;

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

        /// <summary>
        ///     Opens a Tcp Listener
        /// </summary>
        public void OpenConnection()
        {
            server = new TcpListener(Address, Port);
            server.Start();

            // Start client listener thread
            clientListener = new Thread(listenForClients);
            clientListener.Start();
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

        private void listenForClients()
        {
            Console.Write(" >> Starting listening for clients... \n");
            while (true)
            {
                Socket socket = server.AcceptSocket();
                Console.Write(" >> New Client connected: ");
                Console.Write((socket.RemoteEndPoint as IPEndPoint).Address+ ":" + (socket.RemoteEndPoint as IPEndPoint).Port + "\n\n");
                lock (connections)
                {
                    connections.Add(socket);
                }
            }
        }

        public ArrayList SelectForRead()
        {
            ArrayList sockets = new ArrayList();
            lock (connections)
            {
                foreach (Socket socket in connections)
                {
                    if(socket != null)
                        sockets.Add(socket);
                }
            }

            if (sockets.Count == 0)
                return sockets;

            // TODO fix busy waiting
            Socket.Select(sockets, null, null, 1000);
            //Console.Write(" >> Done selecting \n");
            return sockets;
        }
    }
}
