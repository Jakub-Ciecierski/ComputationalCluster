﻿using Communication;
using System.Xml.Serialization;

namespace Communication.Messages
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = DivideProblemMessage.ELEMENT_NAME)]
    public partial class DivideProblemMessage : Message
    {

        private string problemTypeField;

        private ulong idField;

        private byte[] dataField;

        private ulong computationalNodesField;

        private ulong nodeIDField;

        /// <remarks/>
        /// The problem type name as given by TaskSolver and Client
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
        /// The ID of the problem instance assigned by the server
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
        /// The problem data
        [System.Xml.Serialization.XmlElementAttribute(DataType = "base64Binary")]
        public byte[] Data
        {
            get
            {
                return this.dataField;
            }
            set
            {
                this.dataField = value;
            }
        }

        /// <remarks/>
        /// The total number of currently available threads
        public ulong ComputationalNodes
        {
            get
            {
                return this.computationalNodesField;
            }
            set
            {
                this.computationalNodesField = value;
            }
        }

        /// <remarks/>
        /// The ID of the TM that is dividing the problem
        public ulong NodeID
        {
            get
            {
                return this.nodeIDField;
            }
            set
            {
                this.nodeIDField = value;
            }
        }
    }
}