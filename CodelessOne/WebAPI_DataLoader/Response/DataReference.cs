using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebAPI_DataLoader.Common.Enitities;

namespace WebAPI_DataLoader.Response
{
    [DataContract]
    public class DataReference
    {
        [DataMember(Name = "subEntity")]
        public string SubEntity { get; set; }

        [DataMember(Name = "relationName")]
        public string RelationName { get; set; }

        [DataMember(Name = "referenceRecords")]
        public List<CellData> ReferenceRecords { get; set; }
    }
}