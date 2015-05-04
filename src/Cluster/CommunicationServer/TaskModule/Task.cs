using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationServer.TaskModule
{
    public class Task
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private int id;

        public int ID
        {
            get { return id; }
            private set { id = value; }
        }

        private TaskStatus status;

        public TaskStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public Task(int id)
        {
            ID = id;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
