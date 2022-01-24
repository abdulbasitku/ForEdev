using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Response
{
    [DataContract]
    public class AttributeResponse
    {
        [DataMember(Name = "AttributeName")]
        public string AttributeName { get; set; }

        [DataMember(Name = "DataType")]
        public string DataType { get; set; }

        [DataMember(Name = "IsMap")]
        public bool IsMap { get; set; }

        [DataMember(Name = "LookUpAttribute", EmitDefaultValue = false)]
        public LookUpAttributeResponse LookUpAttribute { get; set; }
    }
}