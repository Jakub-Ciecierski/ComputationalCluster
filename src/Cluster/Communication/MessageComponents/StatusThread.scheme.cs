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
    public partial class StatusThread
    {

        private StatusThreadState stateField;

        private ulong howLongField;

        private bool howLongFieldSpecified;

        private ulong problemInstanceIdField;

        private bool problemInstanceIdFieldSpecified;

        private ulong taskIdField;

        private bool taskIdFieldSpecified;

        private string problemTypeField;

        private bool problemTypeFieldSpecified;

        /// <remarks/>
        public StatusThreadState State
        {
            get
            {
                return this.stateField;
            }
            set
            {
                this.stateField = value;
            }
        }

        /// <remarks/>
        public ulong HowLong
        {
            get
            {
                return this.howLongField;
            }
            set
            {
                this.howLongField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool HowLongSpecified
        {
            get
            {
                return this.howLongFieldSpecified;
            }
            set
            {
                this.howLongFieldSpecified = value;
            }
        }

        /// <remarks/>
        public ulong ProblemInstanceId
        {
            get
            {
                return this.problemInstanceIdField;
            }
            set
            {
                this.problemInstanceIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProblemInstanceIdSpecified
        {
            get
            {
                return this.problemInstanceIdFieldSpecified;
            }
            set
            {
                this.problemInstanceIdFieldSpecified = value;
            }
        }

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
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool ProblemTypeFieldSpecified
        {
            get
            {
                return this.problemTypeFieldSpecified;
            }
            set
            {
                this.problemTypeFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    public enum StatusThreadState
    {

        /// <remarks/>
        Idle,

        /// <remarks/>
        Busy,
    }
}
