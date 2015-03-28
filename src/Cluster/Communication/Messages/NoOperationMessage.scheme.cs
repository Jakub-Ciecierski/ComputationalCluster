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
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = NoOperationMessage.ELEMENT_NAME)]
    public partial class NoOperationMessage : Message
    {

        private BackupCommunicationServer[] backupCommunicationServersField;

        /// <remarks/>
        public BackupCommunicationServer[] BackupCommunicationServers
        {
            get
            {
                return this.backupCommunicationServersField;
            }
            set
            {
                this.backupCommunicationServersField = value;
            }
        }
    }
}