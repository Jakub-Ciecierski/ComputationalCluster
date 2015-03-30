using Communication.Network.TCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer
{
    public class ConsoleManager
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private NetworkServer server;

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        public ConsoleManager(NetworkServer server)
        {
            this.server = server;
        }
        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        private void printHelp()
        {
            Console.Write("\tclients - Lists all clients connected\n");
        }

        private void listClients()
        {
            Console.Write(server.ListAllConnections());
        }

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
        public void Start()
        {
            Console.Write(" >> Welcome to the Server  \n");
            printHelp();

            bool _continue = true;
            while (_continue)
            {
                string line = Console.ReadLine();
                switch (line)
                {
                    case "clients":
                        listClients();
                        break;

                    default:
                        printHelp();
                        break;

                }
            }
        }
    }
}
