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
        const int PACKGE_SIZE = 1024 * 20;

        /// <summary>
        ///     End Of Block transmision
        /// </summary>
        const char END_OF_FILE = (char)23;

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
        
        protected void Send(Socket socket, List<Message> messages)
        {
            int count = messages.Count;
            string strContent = "";

            if(count > 1){
                for(int i=0;i< messages.Count;i++)
                {
                    strContent += messages[i].ToXmlString();
                    strContent += END_OF_FILE;
                }
            }if(count == 1){
                strContent += messages[0].ToXmlString();
            }
            
            // set up data to send
            byte[] content = Encoding.UTF8.GetBytes(strContent);
            socket.Send(content , content .Length, SocketFlags.None); // TODO WHEN SERVER IS SHUTDOWN
        }

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
        
        protected List<Message> ReceiveMessages(Socket socket)
        {
            List<string> messagesStr = new List<string>();
            
            List<Message> messages = new List<Message>();

            byte[] contentReceive = new byte[PACKGE_SIZE];
            socket.Receive(contentReceive, PACKGE_SIZE, SocketFlags.None);

            string contentStr = System.Text.Encoding.UTF8.GetString(contentReceive);
            messagesStr = contentStr.Split(END_OF_FILE).ToList();

            for (int i = 0; i < messagesStr.Count; i++) {
                string messageStr = messagesStr[i];
                // look for '\0'
                string[] actualMessageStr;
                actualMessageStr = messageStr.Split('\0');

                Message message = Message.Construct(actualMessageStr[0]);
                messages.Add(message);
            }

            return messages;
        }
    }
}
