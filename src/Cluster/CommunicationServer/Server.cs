using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication;
using Communication.Messages;
using System.Net;
namespace CommunicationServer
{
    class Server
    {

        static void Main(string[] args)
        {
            ClientListener clientListener = new ClientListener();
            clientListener.ListenForClients();

        }
    }
}
