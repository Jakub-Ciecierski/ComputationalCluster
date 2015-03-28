using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Communication.MessageComponents
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public partial class Solution
    {

        private ulong taskIdField;

        private bool taskIdFieldSpecified;

        private bool timeoutOccuredField;

        private SolutionsSolutionType typeField;

        private ulong computationsTimeField;

        private byte[] dataField;

        /// <remarks/>
        public ulong TaskId
        {
            get
            {
                return this.taskIdField;
            }
            set
            {
                this.taskIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool TaskIdSpecified
        {
            get
            {
                return this.taskIdFieldSpecified;
            }
            set
            {
                this.taskIdFieldSpecified = value;
            }
        }

        /// <remarks/>
        public bool TimeoutOccured
        {
            get
            {
                return this.timeoutOccuredField;
            }
            set
            {
                this.timeoutOccuredField = value;
            }
        }

        /// <remarks/>
        public SolutionsSolutionType Type
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
        public ulong ComputationsTime
        {
            get
            {
                return this.computationsTimeField;
            }
            set
            {
                this.computationsTimeField = value;
            }
        }

        /// <remarks/>
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public enum SolutionsSolutionType
    {

        /// <remarks/>
        Ongoing,

        /// <remarks/>
        Partial,

        /// <remarks/>
        Final,
    }

}
