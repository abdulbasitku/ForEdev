using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Response
{
    [DataContract]
    public class DataEntitiesResponse
    {
        [DataMember(Name = "Entities")]
        public List<DataEntityResponse> sheets { get; set; }
    }
}