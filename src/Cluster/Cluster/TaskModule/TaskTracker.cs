using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cluster.Client
{
    public class TaskTracker
    {
        /******************************************************************/
        /******************* PROPERTIES, PRIVATE FIELDS *******************/
        /******************************************************************/

        /// <summary>
        ///     List of tasks
        /// </summary>
        private List<Task> tasks = new List<Task>();

        /******************************************************************/
        /************************** CONSTRUCTORS **************************/
        /******************************************************************/

        public TaskTracker()
        {

        }

        /*******************************************************************/
        /************************ PRIVATE METHODS **************************/
        /*******************************************************************/

        /*******************************************************************/
        /************************* PUBLIC METHODS **************************/
        /*******************************************************************/

        public void AddTask(Task task)
        {
            tasks.Add(task);
        }

        public Task GetTask(int id)
        {
            foreach(Task task in tasks)
            {
                if (task.ID == id)
                {
                    return task;
                }
            }
            return null;
        }
    }
}
