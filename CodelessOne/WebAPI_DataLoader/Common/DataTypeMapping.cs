using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebAPI_DataLoader.Common
{
    public abstract class DataTypeMapping
    {
        public abstract string GetDatype(string sourceValue);
    }
}