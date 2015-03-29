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
        ID id;
        private Dictionary<int, BackupCommunicationServer> backupCommunicationSeverList;
        private Dictionary<int, ComputationalNode> computationalNodeList;     
        private Dictionary<int, TaskManager> taskManagerList;
        void Main(string[] args)
        {
            id = new ID();
            backupCommunicationSeverList = new Dictionary<int, BackupCommunicationServer>();
            computationalNodeList = new Dictionary<int, ComputationalNode>();
            taskManagerList = new Dictionary<int, TaskManager>();
        }
        /// <summary>
        /// Function registering module in the system.
        /// </summary>
        /// <param name="registerMessage"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private async Task RegisterElement(RegisterMessage registerMessage,IPAddress ipAddress)
        {
             await Task.Run(() =>
             {
                 switch (registerMessage.Type)
                 {
                     case RegisterType.CommunicationServer:
                         lock (backupCommunicationSeverList)
                         {
                             var values = backupCommunicationSeverList.Select(x => x.Value).Cast<Module>().ToList();
                             if(!CheckIP(values,ipAddress)){
                                 Console.WriteLine("Module was already added");
                             }
                             else
                             {
                                 BackupCommunicationServer backupCommunicationServer = new BackupCommunicationServer(registerMessage.SolvableProblems,registerMessage.ParallelThreads,ipAddress);
                                 lock (id)
                                 {
                                     id.Id++;
                                     backupCommunicationSeverList.Add(id.Id, backupCommunicationServer);
                                 }

                             }
                         }
                         break;

                     case RegisterType.ComputationalNode:
                         lock (computationalNodeList)
                         {
                             var values = computationalNodeList.Select(x => x.Value).Cast<Module>().ToList();
                             if (!CheckIP(values, ipAddress))
                             {
                                 Console.WriteLine("Module was already added");
                             }
                             else
                             {
                                 ComputationalNode computationalNode = new ComputationalNode(registerMessage.SolvableProblems, registerMessage.ParallelThreads, ipAddress);
                                 lock (id)
                                 {
                                     id.Id++;
                                     computationalNodeList.Add(id.Id, computationalNode);
                                 }
                             }
                         }
                         break;

                     case RegisterType.TaskManager:
                         lock (taskManagerList)
                         {
                             var values = taskManagerList.Select(x => x.Value).Cast<Module>().ToList();
                             if (!CheckIP(values, ipAddress))
                             {
                                 Console.WriteLine("Module was already added");
                             }
                             else
                             {
                                 TaskManager taskManager = new TaskManager(registerMessage.SolvableProblems, registerMessage.ParallelThreads, ipAddress);
                                 lock (id)
                                 {
                                     id.Id++;
                                     taskManagerList.Add(id.Id, taskManager);
                                 }
                             }
                         }
                         break;
                 }
             });
        }
        /// <summary>
        /// Simply removing element from list of nodes.
        /// </summary>
        /// <param name="registerMessage"></param>
        /// <returns></returns>
        private async Task DeregisterElement(RegisterMessage registerMessage){
            await Task.Run(()=>
                {
                    switch (registerMessage.Type)
                    {
                        case RegisterType.CommunicationServer:
                            lock (backupCommunicationSeverList)
                            {
                                backupCommunicationSeverList.Remove(Convert.ToInt32(registerMessage.Id));
                            }
                            break;
                        case RegisterType.ComputationalNode:
                            lock (computationalNodeList)
                            {
                                computationalNodeList.Remove(Convert.ToInt32(registerMessage.Id));
                            }
                            break;
                        case RegisterType.TaskManager:
                            lock (taskManagerList)
                            {
                                taskManagerList.Remove(Convert.ToInt32(registerMessage.Id));
                            }
                            break;
                    }  
                });
        }
        private async Task UpdateElementStatus(StatusMessage statusMessage)
        {
            await Task.Run(() =>
                {
                    bool checkFurther = true;
                    var computationalNode = new ComputationalNode();
                    lock (computationalNodeList)
                    {
                        if (computationalNodeList.TryGetValue(Convert.ToInt32(statusMessage.Id), out computationalNode))
                        {
                            //nie wiem czy mogę edytować wartość computationalNode z tryGetValue
                            computationalNodeList[Convert.ToInt32(statusMessage.Id)].StatusThreads = statusMessage.Threads;
                            computationalNodeList[Convert.ToInt32(statusMessage.Id)].UpdateStatus = true;
                            return;
                        }
                    }
                    var taskManager = new TaskManager();
                    lock (taskManagerList)
                    {
                        if (taskManagerList.TryGetValue(Convert.ToInt32(statusMessage.Id), out taskManager))
                        {
                            taskManagerList[Convert.ToInt32(statusMessage.Id)].StatusThreads = statusMessage.Threads;
                            taskManagerList[Convert.ToInt32(statusMessage.Id)].UpdateStatus = true;
                            return;
                        }
                    }
                    var backupCommunicationServer = new BackupCommunicationServer();
                    lock (backupCommunicationSeverList)
                    {
                        if (backupCommunicationSeverList.TryGetValue(Convert.ToInt32(statusMessage.Id), out backupCommunicationServer))
                        {
                            backupCommunicationSeverList[Convert.ToInt32(statusMessage.Id)].StatusThreads = statusMessage.Threads;
                            backupCommunicationSeverList[Convert.ToInt32(statusMessage.Id)].UpdateStatus = true;
                            return;
                        }
                    }
                });
        }
        /// <summary>
        /// Check if by any chance node/module is already in the system
        /// </summary>
        /// <param name="modulesList"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        private bool CheckIP(List<Module> modulesList,IPAddress ipAddress){
            foreach (var obj in modulesList)
            {
                if (obj.IpAddress == ipAddress)
                {
                    return false;
                }
            }
            return true;
        }


    }
    class ID
    {
        private int id;
        public ID()
        {
            id = 0;
        }
        public int Id
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }
    }
}
