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
    public partial class PartialProblem
    {

        private ulong taskIdField;

        private byte[] dataField;

        private ulong nodeIDField;

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
