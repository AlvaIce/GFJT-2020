using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;

namespace QRST_DI_DS_MetadataQuery.JSONutilty
{
    public class JSON
    {
        public static T parse<T>(string jsonString)
        {
            if (string.IsNullOrEmpty(jsonString))
            {
                throw new Exception("没有数据！");
            }
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString)))
            {
                return (T)new DataContractJsonSerializer(typeof(T)).ReadObject(ms);
            }
        }

        public static string stringify(object jsonObject)
        {
            using (var ms = new MemoryStream())
            {
                new DataContractJsonSerializer(jsonObject.GetType()).WriteObject(ms, jsonObject);
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }
    }
}
