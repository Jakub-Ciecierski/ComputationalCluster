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
    public class NetworkListener
    {
        private TcpListener listener = null;

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
        /// Creates a Tcp Server object
        /// </summary>
        /// <param name="address">
        ///     Address of the server
        /// </param>
        /// <param name="port">
        ///     Port to listen to
        /// </param>
        public NetworkListener(IPAddress address, int port)
        {
            this.address = address;
            this.port = port;
        }

        public void StartSocket()
        {
            if(listener == null)
                throw new NullReferenceException("Open Connection before starting socket");
            socket = listener.AcceptSocket();
        }

        public void CloseSocket()
        {
            if(socket != null)
                socket.Close();
        }

        /// <summary>
        ///     Opens a Tcp Listener
        /// </summary>
        public void OpenConnection()
        {
            listener = new TcpListener(address, port);
            listener.Start();
        }

        /// <summary>
        ///     Closes a Tcp Listener
        /// </summary>
        public void Close()
        {
            if (listener != null)
            {
                if (socket != null)
                    socket.Close();
                listener.Stop();
            }
        }

        public bool isPending()
        {
            return listener.Pending();
        }

        public Socket GetAcceptedSocket()
        {
            return listener.AcceptSocket();
        }

        public void Send(Socket socket, string message)
        {
            // set up data to send
            byte[] content = Encoding.UTF8.GetBytes(message);
            byte[] sizeSend = BitConverter.GetBytes(content.Length);

            byte[] dataSend = new byte[content.Length + sizeSend.Length];
            for (int i = 0; i < dataSend.Length; i++)
            {
                if (i < sizeSend.Length)
                    dataSend[i] = sizeSend[i];
                else
                    dataSend[i] = content[i - sizeSend.Length];
            }
            socket.Send(dataSend, dataSend.Length, SocketFlags.None);
        }

        public void Send(Socket socket, Message message)
        {
            // set up data to send
            byte[] content = Encoding.UTF8.GetBytes(message.ToXmlString());
            byte[] sizeSend = BitConverter.GetBytes(content.Length);

            byte[] dataSend = new byte[content.Length + sizeSend.Length];
            for (int i = 0; i < dataSend.Length; i++)
            {
                if (i < sizeSend.Length)
                    dataSend[i] = sizeSend[i];
                else
                    dataSend[i] = content[i - sizeSend.Length];
            }
            socket.Send(dataSend, dataSend.Length, SocketFlags.None);
        }

        /// <summary>
        ///     Receive next message
        /// </summary>
        /// <returns>
        ///     Received message in string
        /// </returns>
        public string Receive(Socket socket)
        {
            if (socket == null)
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
