using System.Xml.Serialization;

namespace Communication.Messages
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName=RegisterMessage.ELEMENT_NAME)]
    
    public partial class RegisterMessage : Message
    {

        private RegisterType typeField;

        private string[] solvableProblemsField;

        private byte parallelThreadsField;

        private bool deregisterField;

        private bool deregisterFieldSpecified;

        private ulong idField;

        private bool idFieldSpecified;

        /// <remarks/>
        public RegisterType Type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("ProblemName", IsNullable = false)]
        public string[] SolvableProblems
        {
            get
            {
                return this.solvableProblemsField;
            }
            set
            {
                this.solvableProblemsField = value;
            }
        }

        /// <remarks/>
        public byte ParallelThreads
        {
            get
            {
                return this.parallelThreadsField;
            }
            set
            {
                this.parallelThreadsField = value;
            }
        }

        /// <remarks/>
        /// If message is used to delete component from the server or Backup server,
        /// this field should be set to true (Id then required too)
        public bool Deregister
        {
            get
            {
                return this.deregisterField;
            }
            set
            {
                this.deregisterField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool DeregisterSpecified
        {
            get
            {
                return this.deregisterFieldSpecified;
            }
            set
            {
                this.deregisterFieldSpecified = value;
            }
        }

        /// <remarks/>
        /// When this message is used to inform backup server
        public ulong Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
                IdSpecified = true;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool IdSpecified
        {
            get
            {
                return this.idFieldSpecified;
            }
            set
            {
                this.idFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public enum RegisterType
    {

        /// <remarks/>
        TaskManager,

        /// <remarks/>
        ComputationalNode,

        /// <remarks/>
        CommunicationServer,

        ///<remarks/>
        CompuationalClient
    }
}