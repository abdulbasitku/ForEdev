using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using WebAPI_DataLoader.Common.Enitities;

namespace WebAPI_DataLoader.Response
{
    [DataContract]
    public class DataRecords
    {
        [DataMember(Name = "data")]
        public List<CellData> Data { get; set; }

        [DataMember(Name = "reference", EmitDefaultValue = false)]
        public List<DataReference> DataReferences { get; set; }
    }
}