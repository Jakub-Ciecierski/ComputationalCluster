using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Communication.MessageComponents;
using System.Xml.Serialization;
using System.IO;

namespace Communication.Messages
{
    /// <summary>
    /// Solutions message is used for sending info about ongoing computations, 
    /// partial and final solutions from
    /// CN, to TM and to CC and to relay information for synchronizing 
    /// info with Backup CS. In addition to
    /// sending task and solution data it also gives information about 
    /// the time it took to compute the solution
    /// and whether computations were stopped due to timeout.
    /// </summary>
    public partial class SolutionsMessage : Message
    {
        // Specifies the name of element as presented in xml file
        public const string ELEMENT_NAME = "Solutions";

        private SolutionsMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="problemType">
        ///     The problem type name as given by TaskSolver and Client
        /// </param>
        /// <param name="id">
        ///     The ID of the problem instance assigned by the server
        /// </param>
        /// <param name="commonData">
        ///     common data which was previously sent to all Computational Nodes (possibly
        ///     could be stored on server as TaskManagers could have changed)
        /// </param>
        /// <param name="solutions">
        ///     Solutions 
        /// </param>
        public SolutionsMessage(string problemType, ulong id, byte[] commonData, Solution[] solutions)
        {
            ProblemType = problemType;
            Id = id;
            CommonData = commonData;
            Solutions = solutions;
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
        public static SolutionsMessage Construct(string xmlString)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SolutionsMessage)); ;
            StringReader strReader = new StringReader(xmlString);

            return (SolutionsMessage)serializer.Deserialize(strReader);
        }

        public override bool Equals(object obj)
        {
            SolutionsMessage message = obj as SolutionsMessage;

            return (Id == message.Id && ProblemType == message.ProblemType &&
                             Enumerable.SequenceEqual(CommonData, message.CommonData) &&
                            Enumerable.SequenceEqual(Solutions, message.Solutions));
        }
    }
}
