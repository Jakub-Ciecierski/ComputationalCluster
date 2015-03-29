using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Network
{
    public class Listener
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
        public Listener(IPAddress address, int port)
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

        public void Send(string message)
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
    }
}
