﻿using Communication.MessageComponents;
using System.Xml.Serialization;

namespace Communication.Messages
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://www.mini.pw.edu.pl/ucc/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://www.mini.pw.edu.pl/ucc/", IsNullable = false, ElementName = RegisterResponseMessage.ELEMENT_NAME)]
    public partial class RegisterResponseMessage : Message
    {

        private ulong idField;

        private uint timeoutField;

        private BackupCommunicationServer[] backupCommunicationServersField;

        /// <remarks/>
        /// The ID assigned by the Communication Server
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
        /// The communication timeout configured on Communication Server
        public uint Timeout
        {
            get
            {
                return this.timeoutField;
            }
            set
            {
                this.timeoutField = value;
            }
        }

        /// <remarks/>
        /// List of backup servers
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