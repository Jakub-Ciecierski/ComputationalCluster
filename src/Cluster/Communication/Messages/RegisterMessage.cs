using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Communication.Messages
{
    /// <summary>
    /// Register message is sent by TM, CN and Backup CS to the CS after they are activated. In the register
    /// message they send their type TaskManager, ComputationalNode or CommunicationServer; the type of
    /// problems they could solve (if applicable) and the computational power of the component (note that the
    /// protocol would support both the registration of many components from the same computer with one
    /// thread and registration of one component with many threads). When the register message is used to
    /// inform Backup CS, the Id field should be non-empty and if used to delete component from the CS
    /// or Backup CS, Deregister should be set to true.
    /// </summary>
    public partial class RegisterMessage : Message
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME = "Register";

        private RegisterMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">
        ///     Defines the type of node (either TM, CN or CS)
        /// </param>
        /// <param name="parallelThreads">
        ///     The number of threads that could be efficiently run in parallel
        /// </param>
        /// <param name="problems">
        ///     Gives the list of names of the problems which could be solved (probably sth
        ///     like DVRP-[group no.])
        /// </param>
        public RegisterMessage(RegisterType type, byte parallelThreads, string[] problems)
        {
            Type = type;
            ParallelThreads = parallelThreads;
            SolvableProblems = problems;

            Deregister = false;

            DeregisterSpecified = true;
            IdSpecified = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">
        ///     Defines the type of node (either TM, CN or CS)
        /// </param>
        /// <param name="parallelThreads">
        ///     The number of threads that could be efficiently run in parallel
        /// </param>
        /// <param name="problems">
        ///     Gives the list of names of the problems which could be solved (probably sth
        ///     like DVRP-[group no.])
        /// </param>
        /// <param name="deregister">
        ///     When used to inform Backup Server of the need to remove element
        ///     should be set to true
        /// </param>
        /// <param name="id">
        ///     When used to inform Backup Server of the need
        ///     to add/remove element should be set to ID given by main server
        /// </param>
        public RegisterMessage(RegisterType type, byte parallelThreads, string[] problems, bool deregister, ulong id)
        {
            Type = type;
            ParallelThreads = parallelThreads;
            SolvableProblems = problems;
            Deregister = deregister;
            Id = id;

            DeregisterSpecified = true;
            IdSpecified = true;

        }

        /// <summary>
        ///     Construct an object message from input xml
        /// </summary>
        /// <param name="xmlString">
        ///     xml for which the object should be created
        /// </param>
        /// <returns>
        ///     Deserialized object
        /// </returns>
        public static RegisterMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(RegisterMessage)); ;
            StringReader strReader = new StringReader(xmlString);

            return (RegisterMessage)serializer.Deserialize(strReader);
        }

        public override bool Equals(object obj)
        {
            RegisterMessage message = obj as RegisterMessage;

            return (Deregister = message.Deregister && Id == message.Id &&
                Enumerable.SequenceEqual(SolvableProblems, message.SolvableProblems) && Type == message.Type);
        }
    }
}
