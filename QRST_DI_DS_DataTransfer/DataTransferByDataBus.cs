using System;
using System.Text;
using System.IO;
 
namespace QRST_DI_DS_DataTransfer
{
    public class DataTransferByDataBus
    {
        const string jarName = "gf-databus-client.jar";

        /// <summary>
        /// 将数据上传到数据总线
        /// </summary>
        /// <param name="url">服务器url地址,例如：http://192.168.1.200:8080</param>
        /// <param name="pathID">由集成共享端传递过来的数据路径ID(应该包括QRST_CODE信息)例如: ic1/201304101713/user1/ab1234 </param>
        /// <param name="dataPath">运管系统中数据的路径，例如：./tmp/abc.tar.gz</param>
        /// <param name="appDirecroty">数据传输工具包所在路径</param>
        /// <param name="errorMsg">如果传输工具包执行出错，输出错误信息</param>
        /// <returns>返回上传结果信息,上传成功则返回true,否则返回false</returns>
        public static bool UpLoadData(string serverUrl, string pathID, string dataPath, string appDirecroty,out string errorMsg, out string dcOutMsg)
        {
            try
            {
                dcOutMsg = "null";
                StringBuilder sbcommand = new StringBuilder();
                sbcommand.AppendFormat("java -jar {0} upload {3} /{1} {2}", jarName, pathID, dataPath, serverUrl);
                //appDirecroty = appDirecroty + "\\";
                if (!File.Exists(appDirecroty + jarName))
                {
                    throw new Exception("数据传输工具包不存在!");
                }
                else
                {
                    String command = sbcommand.ToString();
                    string msg = Cmd.StartCmd(appDirecroty, command,out errorMsg);
                    dcOutMsg = msg;
                    //解析命令行的输出信息
                    string []msgArr = msg.Split(Environment.NewLine.ToCharArray());
                    if(msgArr.Length <2)
                    {
                        return false;
                    }
                    //寻找倒数第二条不为空的输出信息
                    int index = 0;
                    string outputMsg = "";
                    for (int i = msgArr.Length-1; i > 0;i-- )
                    {
                        if(!string.IsNullOrEmpty(msgArr[i]))
                        {
                            if(++index == 2)
                            {
                                outputMsg = msgArr[i];
                                break;
                            }
                        }
                    }
                    if(outputMsg.StartsWith("http"))
                    {
                        dcOutMsg = "success";
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据传输失败！" + ex.ToString());
            }
        }

        /// <summary>
        /// 将数据从数据总线下载到数据库
        /// </summary>
        /// <param name="dataBusUrl">数据总线服务器url,例如："http://192.168.1.200:8080"</param>
        /// <param name="pathID">由集成共享端传递过来的数据路径ID，例如："icz/201304101011/abc/1234567"</param>
        /// <param name="fileName">要下载的文件名称,例如："12345.txt"</param>
        /// <param name="targetFolder">数据库中存放该文件的路径，例如：@"D:\tester"</param>
        /// <param name="appDirecroty">数据传输工具包所在路径</param>
        /// <returns>返回下载结果信息，上传成功，返回true,否则返回false</returns>COMPLETE/CLIENT_ERROR
        public static bool DownLoadData(string dataBusUrl, string pathID, string fileName, string targetFolder, string appDirecroty,out string errorMsg)
        {
            try
            {
                StringBuilder sbcommand = new StringBuilder();
                sbcommand.AppendFormat("java -jar {0} download {1} /{2} {3} {4}", jarName, dataBusUrl, pathID, fileName, targetFolder);
                //appDirecroty = appDirecroty + "\\";
                if (!File.Exists(appDirecroty + jarName))
                {
                    throw new Exception("数据传输工具包不存在!");
                }
                else
                {
                    String command = sbcommand.ToString();
                    string msg = Cmd.StartCmd(appDirecroty, command,out errorMsg);

                       //解析命令行的输出信息
                    string []msgArr = msg.Split(Environment.NewLine.ToCharArray());
                    if(msgArr.Length <2)
                    {
                        return false;
                    }
                    //寻找倒数第二条不为空的输出信息
                    int index = 0;
                    string outputMsg = "";
                    for (int i = msgArr.Length-1; i > 0;i-- )
                    {
                        if(!string.IsNullOrEmpty(msgArr[i]))
                        {
                            if(++index == 2)
                            {
                                outputMsg = msgArr[i];
                                break;
                            }
                        }
                    }
                    if(outputMsg.StartsWith("COMPLETE"))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("数据传输失败！" + ex.ToString());
            }
        }

        /// <summary>
        /// 获取数据总线上指定的文件列表
        /// </summary>
        /// <param name="dataBusUrl">http://192.168.1.200:8080</param>
        /// <param name="pathId">由集成共享端传递过来的数据路径ID,例如：icz/201304101011/abc/1234567</param>
        /// <returns></returns>
        public static FileListInfo GetFileList(string dataBusUrl, string pathId)
        {
            string requestUrl = string.Format(@"{0}/download/{1}",dataBusUrl,pathId);
            try
            {
                FileListInfo fileInfoLst;
                System.Net.HttpWebRequest Myrq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(requestUrl);
                System.Net.HttpWebResponse myrp = (System.Net.HttpWebResponse)Myrq.GetResponse();
                StreamReader sr = new StreamReader(myrp.GetResponseStream());
                string jsonStr = sr.ReadToEnd();
                fileInfoLst = JSON.parse<FileListInfo>(jsonStr);
                sr.Close();
                myrp.Close();
                Myrq.Abort();
                return fileInfoLst;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }

    }
}
