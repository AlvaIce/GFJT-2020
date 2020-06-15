using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace QRST_DI_MS_Desktop.HadoopInfo
{
    class HttpRequestHelper
    {
        public static string DoGet(string url, IDictionary<string, string> paras)
        {
            StringBuilder sb = new StringBuilder();
            bool firstPara = true;
            if (paras != null)
            {
                foreach (KeyValuePair<string, string> ky in paras)
                {
                    if (!string.IsNullOrEmpty(ky.Key) && !string.IsNullOrEmpty(ky.Value))
                    {
                        if (!firstPara)
                            sb.Append("&");
                        sb.AppendFormat("{0}={1}", ky.Key, ky.Value);
                        firstPara = false;
                    }
                }
                url = url + "?" + sb.ToString();
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            HttpWebResponse rep = null;

            try
            {
                rep = (HttpWebResponse)req.GetResponse();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                rep = null;
            }

            if (rep != null)
            {
                Encoding encoding;
                if (rep.CharacterSet == null || rep.CharacterSet.Length == 0)
                    encoding = Encoding.GetEncoding("UTF-8");
                else
                    encoding = Encoding.GetEncoding(rep.CharacterSet);

                Stream stream = null;
                StreamReader reader = null;
                string result = string.Empty;
                try
                {
                    stream = rep.GetResponseStream();
                    reader = new StreamReader(stream, encoding);
                    result = reader.ReadToEnd();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    result = string.Empty;
                }
                finally
                {
                    if (reader != null)
                        reader.Close();
                    if (stream != null)
                        stream.Close();
                    if (rep != null)
                        rep.Close();
                }
                return result;
            }
            return string.Empty;
        }
    }
}
