using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Response
{
    [DataContract]
    public class DataEntityResponse
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "records")]
        public List<DataRecords> Records { get; set; }
    }
}