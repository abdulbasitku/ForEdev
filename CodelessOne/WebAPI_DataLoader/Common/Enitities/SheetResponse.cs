using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Common.Enitities
{
    public class SheetResponse
    {
        [DataMember(Name = "sheetName")]
        public string SheetName { get; set; }

        [DataMember(Name = "ColumnInfos")]
        public List<ColumnInfo> ColumnInfos { get; set; }

    }
}