using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Common.Enitities
{
    [DataContract]
    public class Sheet
    {
        [DataMember(Name = "sheetName")]
        public string SheetName { get; set; }
        
        [DataMember(Name = "ColumnInfos")]
        public List<ColumnInfo> ColumnInfos { get; set; }

        [DataMember(Name = "records")]
        public List<Dictionary<string, object>> records { get; set; }

        [DataMember(Name = "selected")]
        public bool selected { get; set; }
    }
}