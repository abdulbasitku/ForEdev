using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Response
{
    [DataContract]
    public class EntityResponse
    {
        [DataMember(Name = "Entity")]
        public string SheetName { get; set; }

        [DataMember(Name = "Attributes")]
        public List<AttributeResponse> Attributes { get; set; }
    }
}