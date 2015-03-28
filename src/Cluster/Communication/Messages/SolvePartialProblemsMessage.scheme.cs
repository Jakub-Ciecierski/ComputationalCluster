using Communication.MessageComponents;
using System.Xml.Serialization;

namespace Communication.Messages
{

/// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = SolvePartialProblemsMessage.ELEMENT_NAME)]
    public partial class SolvePartialProblemsMessage : Message
    {

        private string problemTypeField;

        private ulong idField;

        private byte[] commonDataField;

        private ulong solvingTimeoutField;

        private bool solvingTimeoutFieldSpecified;

        private PartialProblem[] partialProblemsField;

        /// <remarks/>
        public string ProblemType
        {
            get
            {
                return this.problemTypeField;
            }
            set
            {
                this.problemTypeField = value;
            }
        }

        /// <remarks/>
        public ulong Id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] CommonData
        {
            get
            {
                return this.commonDataField;
            }
            set
            {
                this.commonDataField = value;
            }
        }

        /// <remarks/>
        public ulong SolvingTimeout
        {
            get
            {
                return this.solvingTimeoutField;
            }
            set
            {
                this.solvingTimeoutField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool SolvingTimeoutSpecified
        {
            get
            {
                return this.solvingTimeoutFieldSpecified;
            }
            set
            {
                this.solvingTimeoutFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("PartialProblem", IsNullable = false)]
        public PartialProblem[] PartialProblems
        {
            get
            {
                return this.partialProblemsField;
            }
            set
            {
                this.partialProblemsField = value;
            }
        }
    }
}