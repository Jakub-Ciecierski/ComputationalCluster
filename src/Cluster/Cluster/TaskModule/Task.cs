using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster
{
    /// <summary>
    ///     Task is either a base or a sub task
    /// </summary>
    public class Task
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     List of sub tasks associated to that task
        /// </summary>
        private List<Task> subTasks = new List<Task>();

        /// <summary>
        ///     ID of the base Task
        /// </summary>
        private int id;

        public int ID
        {
            get { return id; }
            private set { id = value; }
        }

        /// <summary>
        ///     ID of the client that is 
        ///     curretly working on the task.
        ///     
        ///     -1 means no client has been assigned.
        /// </summary>
        private ulong clientID;

        public ulong ClientID
        {
            get { return clientID; }
            set { clientID = value; }
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
            private set { type = value; }
        }

        /// <summary>
        ///     Base data of the task or subtask
        /// </summary>
        private byte[] baseData;

        public byte[] BaseData
        {
            get { return baseData; }
            private set { baseData = value; }
        }

        /// <summary>
        ///     Common data for all subtasks for this base task
        /// </summary>
        private byte[] commonData;

        public byte[] CommonData
        {
            get { return commonData; }
            set { commonData = value; }
        }


        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/
        private Task() { }

        public Task(int id, string type, byte[] data)
        {
            ID = id;
            BaseData = data;
            Type = type;

            Status = TaskStatus.New;
        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        /// <summary>
        ///     Adds a sub task
        /// </summary>
        /// <param name="subTask"></param>
        public void AddSubTask(Task subTask)
        {
            subTasks.Add(subTask);
        }

        public void AddSubTasks(List<Task> subTasks)
        {
            this.subTasks = subTasks;
        }
    }
}
