using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Common.Enitities
{
    [DataContract]
    public class ReferenceColInfo
    {
        [DataMember(Name = "sheetName")]
        public string SheetName { get; set; }

        [DataMember(Name = "columnName")]
        public string ColumnName { get; set; }

        [DataMember(Name = "relationship")]
        public string Relationship { get; set; }
    }
}