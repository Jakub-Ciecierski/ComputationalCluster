using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Network.TCP
{
    public abstract class NetworkConnection
    {

        /// <summary>
        ///     End Of Block transmision
        /// </summary>
        const int END_OF_FILE_DECIMEL = 23;



        private int port;
        private IPAddress address;

        public IPAddress Address
        {
            get { return address; }
            private set { address = value; }
        }

        public int Port
        {
            get { return port; }
            private set { port = value; }
        }

        public NetworkConnection(IPAddress address, int port)
        {
            Address = address;
            Port = port;
        }

        protected void Send(Socket socket, string message)
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

        protected void Send(Socket socket, Message message)
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
        /*
        protected void Send(Socket socket, List<Message> messages)
        {
            char endOfFile = char(END_OF_FILE_DECIMEL);

            foreach (Message message in messages) 
            { 

            }

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
        }*/

        /*
        protected void Send(Socket socket, List<Message> messages)
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
        }*/


        /// <summary>
        ///     Receive next message
        /// </summary>
        /// <returns>
        ///     Received message in string
        /// </returns>
        protected Message Receive(Socket socket)
        {
            byte[] sizeReceiveByte = new byte[sizeof(Int32)];
            socket.Receive(sizeReceiveByte, sizeof(Int32), 0);
            int sizeReceive = BitConverter.ToInt32(sizeReceiveByte, 0);

            byte[] contentReceive = new byte[sizeReceive];
            socket.Receive(contentReceive, sizeReceive, SocketFlags.None);

            string messageStr = System.Text.Encoding.UTF8.GetString(contentReceive);

            Message message = Message.Construct(messageStr);

            return message;
        }
        /*
        protected List<Message> ReceiveMany(Socket socket)
        {
            byte[] sizeReceiveByte = new byte[sizeof(Int32)];
            socket.Receive(sizeReceiveByte, sizeof(Int32), 0);
            int sizeReceive = BitConverter.ToInt32(sizeReceiveByte, 0);

            byte[] contentReceive = new byte[sizeReceive];
            socket.Receive(contentReceive, sizeReceive, SocketFlags.None);

            string messageStr = System.Text.Encoding.UTF8.GetString(contentReceive);

            Message message = Message.Construct(messageStr);

            return message;
        }*/
    }
}
