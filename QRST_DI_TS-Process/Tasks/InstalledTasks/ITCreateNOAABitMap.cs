using System;
using System.Linq;
using System.IO;
using System.Drawing;
using DotSpatial.Data.Rasters.GdalExtension.QRSTUtilities;

namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITCreateNOAABitMap:TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITCreateNOAABitMap"; }
            set { }
        }

        public override void Process()
        {
            string srcFilePath = this.ProcessArgu[0];
            string ImageSaveFilePath = this.ProcessArgu[1];

            //将.sig文件拷贝进去  zxw 2013/06/16
            string filename = Path.GetFileName(srcFilePath);
            string path = Path.GetDirectoryName(srcFilePath);
            string sigFile = string.Format(@"{0}\{1}.sig", path, filename);
            if (File.Exists(sigFile))
            {
                if (!Directory.Exists(Path.GetDirectoryName(ImageSaveFilePath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(ImageSaveFilePath));
                }
                string destPath = string.Format(@"{0}\{1}",Path.GetDirectoryName(ImageSaveFilePath),Path.GetFileName(sigFile));
                File.Copy(sigFile,destPath,true);
            }

            CreateNOAABitMap.CreateGrayImage(srcFilePath, ImageSaveFilePath);
        }

    }
}
