using System.Runtime.Serialization;
using System.Collections.Generic;

namespace WebAPI_DataLoader.Common.Enitities
{
    [DataContract]
    public class ColumnInfo
    {
        public ColumnInfo() {
            this.Enable = true;
        }

        [DataMember(Name = "columnName")]
        public string ColumnName { get; set; }

        [DataMember(Name = "columnDataType")]
        public string ColumnDataType { get; set; }

        [DataMember(Name = "enable")]
        public bool Enable { get; set; }

        [DataMember(Name = "referenceColInfo")]
        public ReferenceColInfo ReferenceColInfo { get; set; }
    }

   
}