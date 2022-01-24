using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Response
{
    [DataContract]
    public class EntitiesResponse 
    {
        [DataMember(Name = "Entities")]
        public List<EntityResponse> sheets { get; set; }
    }
}