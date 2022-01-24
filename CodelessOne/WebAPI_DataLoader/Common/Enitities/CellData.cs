using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Common.Enitities
{
    [DataContract]
    public class CellData
    {
        public CellData(string key, object value)
        {
            Key = key;
            Value = value;
        }

        [DataMember(Name = "key")]
        public string Key { get; set; }

        [DataMember(Name = "value")]
        public object Value { get; set; }
    }
}