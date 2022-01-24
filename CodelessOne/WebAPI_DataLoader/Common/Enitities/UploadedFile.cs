using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebAPI_DataLoader.Common.Enitities
{
    [DataContract]
    public class UploadedFile
    {

        [DataMember(Name = "filePath")]
        public string FilePath { get; set; }

        [DataMember(Name = "name")]
        public string FileName { get; set; }

        [DataMember(Name = "extension")]
        public string Extension { get; set; }

        [DataMember(Name = "uid")]
        public string Uid { get; set; }
    }
}