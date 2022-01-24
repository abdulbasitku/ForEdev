using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace WebAPI_DataLoader.Common
{
    public class Csv_Xls_DataTypeMapping : DataTypeMapping
    {
        List<DataType> dataTypes = null;
        public Csv_Xls_DataTypeMapping()
        {
            string app_Data_Path = HttpContext.Current.ApplicationInstance.Server.MapPath("~/App_Data");
            string path = app_Data_Path + "\\csv_xls_mapping.json";
            using (StreamReader reader = new StreamReader(path))
            {
                string result = reader.ReadToEnd();
                JavaScriptSerializer scriptSerializer = new JavaScriptSerializer();
                dataTypes = (List<DataType>)scriptSerializer.Deserialize(result, typeof(List<DataType>));
            }
        }
        public override string GetDatype(string sourceValue)
        {
            for (int i = 0; i < dataTypes.Count; i++)
            {
                if (dataTypes[i].sourceValue == sourceValue)
                {
                    return dataTypes[i].targetValue;
                }
            }
            return null;
        }
    }
}