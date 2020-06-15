using System;
using System.IO;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITImportVectorFiles : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITImportVectorFiles"; }
            set { }
        }

        public override void Process()
        {
            //Copy Source Data
            string srcFilePath = this.ProcessArgu[0];
            string destFileDir = this.ProcessArgu[1];

            try
            {
                if (System.IO.File.Exists(srcFilePath))
                {
                    if (!Directory.Exists(destFileDir))
                    {
                        Directory.CreateDirectory(destFileDir);
                    }
                    if (CheckShapefile(srcFilePath))
                    {
                        this.ParentOrder.Logs.Add(string.Format("正在入库矢量数据..."));
                        string dir = Path.GetDirectoryName(srcFilePath);
                        string dataname = Path.GetFileNameWithoutExtension(srcFilePath);
                        foreach (string file in Directory.GetFiles(dir, dataname + ".*"))
                        {
                            string filename = Path.GetFileName(file);
                            System.IO.File.Copy(file, destFileDir + filename, true);
                        }
                        this.ParentOrder.Logs.Add(string.Format("完成矢量数据入库。"));
                    }
                    else
                    {
                        throw new Exception("ShapeFile文件信息不全面");
                    }
                }
            }
            catch (Exception ex)
            {
                this.ParentOrder.Logs.Add(string.Format("矢量数据入库出现异常{0}", ex.Message));
            }
        }

        private bool CheckShapefile(string srcFilePath)
        {
            //成果包括Shapefile数据（含shp、prj、dbf、shx）
            string dir = Path.GetDirectoryName(srcFilePath);
            string filename = Path.GetFileNameWithoutExtension(srcFilePath);
            foreach (string file in Directory.GetFiles(dir, filename + ".*"))
            {
                if (!File.Exists(dir + "\\" + filename + ".shp"))
                {
                    return false;
                }
                else if (!File.Exists(dir + "\\" + filename + ".prj"))
                {
                    return false;
                }
                else if (!File.Exists(dir + "\\" + filename + ".dbf"))
                {
                    return false;
                }
                else if (!File.Exists(dir + "\\" + filename + ".shx"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
