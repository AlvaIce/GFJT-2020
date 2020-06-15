using System.Data;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
 
namespace QRST_DI_DS_Basis
{
    public class SerializerUtil
    {
        /// <summary>
        /// 序列化DataTable
        /// </summary>
        /// <param name="pDt">包含数据的DataTable</param>
        /// <returns>序列化的DataTable</returns>
        public static string GetXmlFormatDs(DataTable dt)
        {
            XmlSerializer ser = new XmlSerializer(dt.GetType());
            System.IO.MemoryStream mem = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(mem, System.Text.Encoding.Default);
            ser.Serialize(writer, dt);
            writer.Close();
            return System.Text.Encoding.Default.GetString(mem.ToArray());
        }

        /// <summary>
        /// 反序列化DataTable
        /// </summary>
        /// <param name="dt">序列化的DataTable</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDsFormatXml(string dt)
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(DataTable));
            StreamReader mem = new StreamReader(new MemoryStream(System.Text.Encoding.Default.GetBytes(dt)), System.Text.Encoding.Default);
            return (DataTable)mySerializer.Deserialize(mem);
        }
    }
}
