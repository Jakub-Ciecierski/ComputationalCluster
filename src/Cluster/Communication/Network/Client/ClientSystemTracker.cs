using Communication.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.Network.Client
{
    public abstract class ClientSystemTracker
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     Networknode describing this system
        /// </summary>
        private NetworkNode node;

        public NetworkNode Node
        {
            get { return node; }
            private set { node = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public ClientSystemTracker(NetworkNode node)
        {
            this.Node = node;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
