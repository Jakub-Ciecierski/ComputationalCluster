using Communication.MessageComponents;
using System.Xml.Serialization;

namespace Communication.Messages
{

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.mini.pw.edu.pl/ucc/")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.mini.pw.edu.pl/ucc/", IsNullable=false, ElementName=SolutionsMessage.ELEMENT_NAME)]
    public partial class SolutionsMessage : Message {
    
        private string problemTypeField;
    
        private ulong idField;
    
        private byte[] commonDataField;
    
        private Solution[] solutionsField;
    
        /// <remarks/>
        public string ProblemType {
            get {
                return this.problemTypeField;
            }
            set {
                this.problemTypeField = value;
            }
        }
    
        /// <remarks/>
        public ulong Id {
            get {
                return this.idField;
            }
            set {
                this.idField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary")]
        public byte[] CommonData {
            get {
                return this.commonDataField;
            }
            set {
                this.commonDataField = value;
            }
        }
    
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("Solutions")]
        [System.Xml.Serialization.XmlArrayItemAttribute("Solution", IsNullable=false)]
        public Solution[] Solutions {
            get {
                return this.solutionsField;
            }
            set {
                this.solutionsField = value;
            }
        }
    }

}