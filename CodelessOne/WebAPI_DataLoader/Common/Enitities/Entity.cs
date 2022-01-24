using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WebAPI_DataLoader.Common.Enitities
{ 

    [DataContract]
    public class Entity
    {
        [DataMember(Name = "uuid")]
        public string Uuid { get; set; }

        [DataMember(Name = "fileName")]
        public string FileName { get; set; }

        [DataMember(Name = "sheets")]
        public List<Sheet> sheets { get; set; }
    }
}