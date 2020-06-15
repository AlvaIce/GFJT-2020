using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tamir.SharpSsh.jsch;
//by csdn id:家猫
namespace QRST_DI_MS_Desktop.HadoopInfo
{
    class ShellHelp
    {
         System.IO.MemoryStream outputstream = new System.IO.MemoryStream();  
         Tamir.SharpSsh.SshStream inputstream = null;  
         Channel channel = null;  
         Session session = null;  
         /// <summary>  
         /// 命令等待标识  
         /// </summary>  
         string waitMark = "]#";
   //      string waitMark = Encoding.ASCII.GetString(new byte[] { 27, 91, 109, 15 });
   
         /// <summary>  
         /// 打开连接  
         /// </summary>  
         /// <param name="host"></param>  
         /// <param name="username"></param>  
         /// <param name="pwd"></param>  
         /// <returns></returns>  
         public bool OpenShell(string host, string username, string pwd, int port)  
         {  
             try  
             {  
                 ////Redirect standard I/O to the SSH channel  
                 inputstream = new Tamir.SharpSsh.SshStream(host, username, pwd, port);  
                 
                 ///别人手动加进去的方法。。为了读取输出信息  
                 inputstream.set_OutputStream(outputstream);  
                  
                 return inputstream != null;  
             }  
             catch
             {
                 throw;
             }  
         }  
         /// <summary>  
         /// 执行命令  
         /// </summary>  
         /// <param name="cmd"></param>  
         public bool Shell(string cmd)  
         {  
             if (inputstream == null) return false;  
   
             string initinfo = GetAllString();  
   
             inputstream.Write(cmd);
             
             inputstream.Flush();
             string currentinfo = GetAllString();
             while (currentinfo == initinfo)
             {
                 System.Threading.Thread.Sleep(100);
                 currentinfo = GetAllString();
             }  
             return true;  
         }

         /// <summary>  
         /// 执行命令  
         /// </summary>  
         /// <param name="cmd"></param>  
         public bool Shell(byte[] b)
         {
             if (inputstream == null) return false;

             string initinfo = GetAllString();

             inputstream.Write(b);

             inputstream.Flush();
             string currentinfo = GetAllString();
             while (currentinfo == initinfo)
             {
                 System.Threading.Thread.Sleep(100);
                 currentinfo = GetAllString();
             }
             return true;
         }  
   
         /// <summary>  
         /// 获取输出信息  
         /// </summary>  
         /// <returns></returns>  
         public string GetAllString()  
         {  
             string outinfo = Encoding.UTF8.GetString(outputstream.ToArray());  
             
             //等待命令结束字符  
             while (!outinfo.Trim().EndsWith(waitMark)&&!outinfo.Trim().EndsWith("logout"))  
             {  
                 System.Threading.Thread.Sleep(200);  
                 outinfo = Encoding.UTF8.GetString(outputstream.ToArray());  
             }  
             outputstream.Flush();  
             return outinfo.ToString();  
         }  
   
         /// <summary>  
         /// 关闭连接  
         /// </summary>  
         public void Close()  
         {  
             if (inputstream != null) inputstream.Close();  
         }

         //public
    }
}
