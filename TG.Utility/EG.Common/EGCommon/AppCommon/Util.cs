using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;
using Newtonsoft.Json.Serialization;


namespace EG.Utility.AppCommon
{
    public class Util
    {
        /*
        /// <summary>     
        /// dataTable to Json --by edgar on 2013-9-3
        /// </summary>     
        /// <param name="datatable"></param>     
        /// <returns></returns>     
        public string DataTableToJson(DataTable datatable)
        { 
            StringBuilder jsonBuilder = new StringBuilder();
            jsonBuilder.Append("{\"");
            jsonBuilder.Append(datatable.TableName.ToString());
            jsonBuilder.Append("\":[");
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                jsonBuilder.Append("{");
                for (int j = 0; j < datatable.Columns.Count; j++)
                {
                    jsonBuilder.Append("\"");
                    jsonBuilder.Append(datatable.Columns[j].ColumnName);
                    jsonBuilder.Append("\":\"");
                    jsonBuilder.Append(datatable.Rows[i][j].ToString());
                    jsonBuilder.Append("\",");
                }
                jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
                jsonBuilder.Append("},");
            }
            jsonBuilder.Remove(jsonBuilder.Length - 1, 1);
            jsonBuilder.Append("]}");
            return jsonBuilder.ToString();
        }
        */
        private static JsonSerializerSettings jsonSetting4DataTable = new JsonSerializerSettings() {
            ContractResolver = new LowercaseContractResolver(),
            
            Converters = new List<JsonConverter> { 
                new DataTableConverter(), 
                new IsoDateTimeConverter() {DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss" } }
                
        };

        /// <summary>
        /// Serialize a Object to Json formate string,
        /// if there has a property which type is DataTable, this property can be Serialize also,
        /// if there has a property which type is DateTime, this property value will be output as 'yyyy-MM-dd HH:mm:ss' format.
        /// 
        /// and all output property name will be convert to lowercase
        /// 
        /// </summary>
        /// <param name="dataObject">the data object will be Serialize</param>
        /// <returns>Json formate String</returns>
        public static string Json(Object dataObject)
        {
            return JsonConvert.SerializeObject(dataObject, Formatting.None, jsonSetting4DataTable);  
        }


    }

    /// <summary>
    /// JsonConvert can use this Resolver to conver property name to lowercase
    /// </summary>
    public class LowercaseContractResolver : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
