using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Common.Enitities
{
    [DataContract]
    public class FileOption
    {
        [DataMember(Name = "uploadedFiles")]
        public List<UploadedFile> UploadedFiles { get; set; }

        [DataMember(Name = "sepratedOption")]
        public string SepratedOption { get; set; }

        [DataMember(Name = "headerRow")]
        public int HeaderRow { get; set; }

        [DataMember(Name = "dataRow")]
        public int DataRow { get; set; }
    }
}