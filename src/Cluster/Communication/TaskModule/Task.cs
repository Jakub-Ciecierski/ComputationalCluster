using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.TaskModule
{
    public class Task
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        private ulong id;

        public ulong ID
        {
            get { return id; }
            private set { id = value; }
        }

        /// <summary>
        ///     Current status
        /// </summary>
        private TaskStatus status;

        public TaskStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        /// <summary>
        ///     Type of the task
        /// </summary>
        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        /// <summary>
        ///     Actual data of the problem
        /// </summary>
        private byte[] data;

        public byte[] Data
        {
            get { return data; }
            private set { data = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        public Task() { }
        public Task(ulong id, string type, byte[] data)
        {
            ID = id;
            Data = data;
            Type = type;

            Status = TaskStatus.New;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/
    }
}
