using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Response
{
    [DataContract]
    public class LookUpAttributeResponse
    {
        [DataMember(Name = "Entity")]
        public string Entity { get; set; }

        [DataMember(Name = "AttributeName")]
        public string AttributeName { get; set; }

        [DataMember(Name = "Relationship")]
        public string Relationship { get; set; }
    }
}