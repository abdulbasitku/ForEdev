using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Common.Enitities
{
    [DataContract]
    public class RowData
    {
        [DataMember(Name = "record")]
       public Dictionary<string, object> record { get; set; }
        //public List<CellData> record { get; set; }
    }
}