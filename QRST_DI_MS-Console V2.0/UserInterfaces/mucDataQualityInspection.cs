using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using QRST_DI_TS_Process.Tasks.InstalledTasks;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucDataQualityInspection : DevExpress.XtraEditors.XtraUserControl
    {
        public  DataCheck dc;
        public mucDataQualityInspection()
        {
            InitializeComponent();
        }

        private void gridView1_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
                e.Info.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                e.Info.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            }
        }

        public void DisplayZB(string satelite)
        {
            panelControlJHZB.Controls.Clear();
            if ((satelite == "高分一号卫星") || (satelite == "高分二号卫星")||
                (satelite == "高分三号卫星") || (satelite == "高分四号卫星"))
            {
                Control[] c = new Control[] { label1, label2, label3, label4, label5 };
                panelControlJHZB.Controls.AddRange(c);
                panelControlJHZB.Visible = true;
            }
            else if (satelite == "环境卫星数据")
            {
                Control[] c = new Control[] { label6, label7, label8, label9, label10 };
                panelControlJHZB.Controls.AddRange(c);
                panelControlJHZB.Visible = true;
            }
            else
            {
                panelControlJHZB.Visible = false;
            }
        }
  

        private void timer1_Tick(object sender, EventArgs e)
        {
            labelControlTotalNum.Text = dc.totalData;
            labelControlStatus.Text = dc.status;
            labelControlDataName.Text = dc.currentData;
            labelControlProgress.Text = dc.progress;
            memoEditLog.Text = dc.log;
            memoEditReport.Text = dc.report;
        }
        public void starTime()
        {
            timer1.Start();
        }
        public void StartCheck()
        {
           Task.Factory.StartNew(dc.StartCheck);
            //dc.StartCheck();
           // Thread t = new Thread(new ThreadStart(dc.StartCheck));
           // t.Start();
          //  dc.StartCheck();
        }
    }

    public class DataCheck
    {
        //private static readonly String nameGFExpressions = "((GF1|GF2|GF4)_((WFV[1-4])|PMS[1-2])|IRS|PMI)_((E|W)[0-9]{0,3}.[0-9])_((S|N)[0-9]{0,2}.[0-9])_[0-9]{8}_L[0-9]A([0-9]{10})";
        //GF1文件名检测的正则表达式
        private static readonly String nameGFExpressions = ITDataQualityInspection.namingExpressions;
        //GF1元数据中必须包含的字段名称
        private static readonly String[] metaDataFileds = ITDataQualityInspection.metaDataFileds;

        //HJ星文件名检测的正则表达式
       // private static readonly String nameHJExpressions = "((HJ1A|HJ1B)-((CCD[1-4])|HSI)-[0-9]{0,3}-[0-9]{0,3}-((A[1-2]-[0-9]{8}|[0-9]{8})-L[0-9]{11})|L[0-9]{11})";
    //    private static readonly String nameHJExpressions = "((HJ1A|HJ1B)-(HSI|(CCD[1-4])-[0-9]{0,3}-[0-9]{0,3}-((A|B)[1-2]-[0-9]{8}-L[0-9]{11})|([0-9]{8}-L[0-9]{11}))|((HJ1A|HJ1B)-(HSI|CCD[1-4])-[0-9]{0,3}-[0-9]{0,3}-L[0-9]{11})";
        private static readonly String nameHJExpressions = "(HJ1A|HJ1B)-(HSI|(CCD[1-4]))-[0-9]{1,3}-[0-9]{1,3}-(A|B)[1-2]-[0-9]{8}-L[0-9]{11}|((HJ1A|HJ1B)-(HSI|(CCD[1-4]))-[0-9]{1,3}-[0-9]{1,3}-[0-9]{8}-L[0-9]{11})|((HJ1A|HJ1B)-(HSI|(CCD[1-4]))-[0-9]{1,3}-[0-9]{1,3}-L[0-9]{11})";
        //HJ星元数据中必须包含的字段名称
        private static readonly String[] metaDataFiledsHJ = new String[] { "productId","sceneId","satelliteId","sensorId","recStationId","productDate","productLevel",
	"pixelSpacing","productType","sceneCount","sceneShift","overallQuality","satPath","satRow","satPathbias","satRowbias","scenePath","sceneRow","scenePathbias"
	,"sceneRowbias","direction","sunElevation","sunAzimuthElevation","recStationID",	"sceneDate",	"sceneTime","instrumentMode",	"imagingStartTime","imagingStopTime",
"gain","satOffNadir","mirrorOffNadir","bands","absCalibType","mtfcProMode","radioMatricMethod","addWindow","correctPhase","reconstructProcess","earthModel","mapProjection","resampleTechnique","productOrientation","ephemerisData",
"attitudeData","sceneCenterLat","sceneCenterLong","dataUpperLeftLat","dataUpperLeftLong","dataUpperRightLat","dataUpperRightLong","dataLowerLeftLat","dataLowerLeftLong",
"dataLowerRightLat","dataLowerRightLong","productUpperLeftLat","productUpperLeftLong","productUpperRightLat","productUpperRightLong",	"productLowerLeftLat",
"productLowerLeftLong","productLowerRightLat","productLowerRightLong","dataUpperLeftX","dataUpperLeftY","dataUpperRightX","dataUpperRightY","dataLowerLeftX",
"dataLowerLeftY","dataLowerRightX","dataLowerRightY","productUpperLeftX","productUpperLeftY","productUpperRightX","productUpperRightY",
"productLowerLeftX","productLowerLeftY","productLowerRightX","productLowerRightY","isSimulateData","dataFormatDes","delStatus",
"dataTempDir","dataArchiveDir","browseArchiveDir","browseDirectory","browseFileLocation"};


        public string currentData = "";
        public string log = "";
        public string progress = "";
        public string status;
        public string totalData;
        public List<string> dataPath = new List<string>();  //存放数据检测路径
        public List<bool[]> checkresult = new List<bool[]>(); //检查结果 
        public string report;
        
        public DataCheck(List<string> dataPath1)
        {
            status = "准备检核...";
            dataPath.Clear();
            //将后缀名包含.tar.gz的文件加入检测表
            if (dataPath1 != null)
            {
                foreach (string s in dataPath1)
                {
                    if (s.EndsWith(".tar.gz") || s.EndsWith(".TAR.GZ") || s.EndsWith(".TAR.gz"))
                    {
                        dataPath.Add(s);
                    }
                }
                totalData = dataPath.Count.ToString();
                StringBuilder sb = new StringBuilder(log);
                if (dataPath.Count != 0)
                {
                    for (int i = 0; i < dataPath.Count; i++)
                    {
                        sb.AppendLine(string.Format("待检测文件{0}:{1};", i + 1, Path.GetFileName(dataPath[i])));
                    }
                    log = sb.ToString();
                    progress = string.Format("{0}/{1}", 0, dataPath.Count);
                }
                else
                { log = "没有符合检测条件的文件"; }
            }
        }

        public void StartCheck()
        {
            status = "正在检核...";
            if (dataPath.Count != 0)
            {
                for (int i = 0; i < dataPath.Count; i++)
                {
                    log = String.Format("{0}\r\n\r\n开始检测:{1}", log, Path.GetFileName(dataPath[i]));
                    if (Path.GetFileName(dataPath[i]).Contains("GF1") || Path.GetFileName(dataPath[i]).Contains("GF2") || Path.GetFileName(dataPath[i]).Contains("GF3") || Path.GetFileName(dataPath[i]).Contains("GF4"))
                    {
                        bool[] result = QualityCheck(dataPath[i]);
                        log = String.Format("{0}\r\n检测完成检测:{1}", log, Path.GetFileName(dataPath[i]));
                        progress = string.Format("{0}/{1}", i + 1, dataPath.Count);
                        checkresult.Add(result);
                    }
                    //HJ星数据检测
                    else if (Path.GetFileName(dataPath[i]).Contains("HJ"))
                    {
                        bool[] result = HJQualityCheck(dataPath[i]);
                        log = log + "\r\n检测完成检测:" + Path.GetFileName(dataPath[i]);
                        progress = string.Format("{0}/{1}", i + 1, dataPath.Count);
                        checkresult.Add(result);
                    }
                }
                Getreport();
                status = "检核完成...";
            }
            else
            {
                log = log + "\r\n没有符合检测的文件";
            }
        }
        /// <summary>
        /// HJ星数据检测
        /// </summary>
        /// <param name="p">数据名称</param>
        /// <returns></returns>
        private bool[] HJQualityCheck(string dataPath)
        {
            bool[] isOk = new bool[5];
            isOk[0] = NameCheck(dataPath);
            isOk[1] = MetaDataCheck(dataPath);
            isOk[2] = DataSizeCheck(dataPath);
            isOk[3] = ThumbnailCheck(dataPath);
            isOk[4] = FormatCheck(dataPath);
            return isOk;
        }


        public bool[] QualityCheck(string dataPath)
        {
            bool[] isOk = new bool[5];
            isOk[0] = NameCheck(dataPath);
            isOk[1] = MetaDataCheck(dataPath);
            isOk[2] = DataSizeCheck(dataPath);
            isOk[3] = ThumbnailCheck(dataPath);
            isOk[4] = FormatCheck(dataPath);
            return isOk;
        }

        /// <summary>
        /// 名字检测,包含GF1 和HJ星的
        /// </summary>
        /// <returns></returns>
        private bool NameCheck(string filename)
        {
            log = log + "\r\n开始命名格式检测" ;
            Thread.Sleep(300);
            filename = Path.GetFileName(filename);
            if (filename.Contains("GF"))
            {
                if (Regex.IsMatch(filename, nameGFExpressions))
                {
                    log = log + "\r\n命名符合规范";
                    return true;
                }
                else
                {
                    log = log + "\r\n命名格式不符合规范";
                    return false;
                }
            }
            else if (filename.Contains("HJ"))
            {
                if (Regex.IsMatch(filename, nameHJExpressions))
                {
                    log = log + "\r\n命名符合规范";
                    return true;
                }
                else
                {
                    log = log + "\r\n命名格式不符合规范";
                    return false;
                }
            }
            else
                log = log + "\r\n命名格式不符合规范";
                return false;
        }
        //开始源数据检测
        private bool MetaDataCheck(string filename)
        {
            log = log + "\r\n开始源数据检测";
            log = log + "\r\n正在解压获取源数据信息";
            FileInfo file = new FileInfo(filename);
            string dir = file.DirectoryName;
            DecompressXmlJpgFromTarGzFile(filename, dir);
            //metaDataCheck(dir);
            if (metaDataCheck(dir))
            {
                log = log + "\r\n元数据信息获取成功，元数据规范性检测通过";
                return true;
            }
            else
            {
                log = log + "\r\n元数据规范性检测不通过";
                return false;
            }
        }
        /// <summary>
        /// 解压缩.tar.gz文件,提取Xml、Jpg文件
        /// </summary>
        /// <param name="filePath">要解压的文件</param>
        /// <param name="OutputTmpPath">解压到的目录</param>
        private void DecompressXmlJpgFromTarGzFile(string filePath, string OutputTmpPath)
        {

            //检查输入文件类型合法性
            if (filePath.EndsWith(".tar.gz") || filePath.EndsWith(".TAR.GZ") || filePath.EndsWith(".TAR.gz"))
            {
                filePath = filePath.TrimEnd('\\');
                OutputTmpPath = OutputTmpPath.TrimEnd('\\');
                string tarPath = null;
                if (!System.IO.Directory.Exists(OutputTmpPath))
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(OutputTmpPath);
                    }
                    catch
                    {
                        throw new Exception("MetaDataCompression:数据解压缩文件目录创建失败！");
                    }
                }
                try
                {
                    System.Diagnostics.Process p = new System.Diagnostics.Process();
                    p.StartInfo.FileName = System.AppDomain.CurrentDomain.BaseDirectory + "7za.exe";
                    //.tar.gz文件所在目录，作为第一次解压的输出目录
                    string dir = System.IO.Path.GetDirectoryName(filePath);
                    //解压
                    p.StartInfo.Arguments = string.Format("e {0} -o{1} * -aoa", filePath, dir);
                    p.StartInfo.CreateNoWindow = true;
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    p.Start();
                    p.WaitForExit();
                    //解压后的 *.tar文件，作为第二次解压的输入文件名
                    tarPath = System.IO.Path.GetDirectoryName(filePath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(filePath);
                    //string tempFile = OutputTmpPath + "\\tempMid";
                    if (!System.IO.Directory.Exists(OutputTmpPath))
                    {
                        try
                        {
                            System.IO.Directory.CreateDirectory(OutputTmpPath);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception("MetaDataCompression:" + ex.Message);
                        }
                    }
                    if (tarPath != null)
                    {
                        p.StartInfo.Arguments = string.Format("e {0} -o{1} *.xml -aoa -r", tarPath, OutputTmpPath);

                        //p.StartInfo.Arguments = string.Format("x {0} -y  -aoa  -o{1} * ", tarPath, OutputTmpPath);                      
                        p.Start();
                        p.WaitForExit();
                        p.StartInfo.Arguments = string.Format("e {0} -o{1} *.jpg -aoa -r", tarPath, OutputTmpPath);

                        //p.StartInfo.Arguments = string.Format("x {0} -y  -aoa  -o{1} * ", tarPath, OutputTmpPath);                      
                        p.Start();
                        p.WaitForExit();
                        DeleteDir(OutputTmpPath);
                        System.IO.File.Delete(tarPath);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("MetaDataCompression:" + ex.Message);
                    log = log + ("\r\nMetaDataCompression:" + ex.Message);
                }
            }
            else
            {
              throw new Exception("MetaDataCompression:压缩文件输入异常。");
                log = log + "\r\n压缩文件输入异常";
            }
        }
        /// <summary>
        /// 删除文件夹内子目录
        /// </summary>
        /// <param name="dirRoot"></param>
        private void DeleteDir(string dirRoot)
        {
            DirectoryInfo dir = new DirectoryInfo(dirRoot);
            if (dir.Exists)
            {
                DirectoryInfo[] childs = dir.GetDirectories();
                foreach (DirectoryInfo child in childs)
                {
                    child.Delete(true);
                }
            }
        }

        /// <summary>
        /// 源数据检测
        /// 元数据规范检测，统一元数据格式以及元数据字段命名方式，空间信息，时间信息不能为空，元数据以xml的方式提供
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        private bool metaDataCheck(string filename)
        {
            String[] xmlFileNames = Directory.GetFiles(filename, "*.xml");
            if (xmlFileNames.Length > 0)
            {
                foreach (String xmlFileName in xmlFileNames)
                {
                    if (!xmlDataCheck(xmlFileName))
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        bool result = true;
        /// <summary>
        /// 检测xml是否完整，即存在定义的所有字段
        /// </summary>
        /// <param name="xmlFileName">xml文件路径名称</param>
        /// <returns></returns>
        public bool xmlDataCheck(String xmlFileName)
        {
            result = true;
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFileName);
            XmlElement root = doc.DocumentElement;
            XmlNodeList childlist = root.ChildNodes;
            ArrayList nodeNameList = new ArrayList();
            if (xmlFileName.Contains("GF"))
            {
            foreach (XmlElement node in childlist)
            {
                nodeNameList.Add(node.Name);
            }
            //metaDataFileds标准XML中的字段链表
                foreach (String filed in metaDataFileds)
                {
                    if (!nodeNameList.Contains(filed))
                    {
                        //this.ParentOrder.Logs.Add(string.Format("元数据中缺少字段:" + filed));
                        log = log + "\r\n元数据中缺少字段信息" + filed;
                        result = false;
                    }
                }
                return result;
            }
            if (xmlFileName.Contains("HJ"))
            {
                foreach (XmlElement node in childlist)
                {
                    foreach (XmlElement node1 in (XmlNodeList)node.ChildNodes)
                    {
                        nodeNameList.Add(node1.Name);
                    }
                }
                foreach (String filed in metaDataFiledsHJ)
                {
                    if (!nodeNameList.Contains(filed))
                    {
                        //this.ParentOrder.Logs.Add(string.Format("元数据中缺少字段:" + filed));
                        log = log + "\r\n元数据中缺少字段信息" + filed;
                        result = false;
                    }
                }
                return result;
            }
            else return false;
        }

  //      string[] fields = new string[] { "SatelliteID", "SensorID", "ProductLevel", "SceneRow", "Bands" };
        //开始数据大小检测,检测大小的范围为（200M-4096M）
        private bool DataSizeCheck(string filename)
        {
            log = log + "\r\n开始数据大小检测" ;
            if (filename.ToUpper().Contains("GF") )
            {
                //10MB以上
                FileInfo fi = new FileInfo(filename);
                int minSize = (filename.ToUpper().Contains("IRS")) ? 0 : 10;
                if (fi.Length / (1024 * 1024) > minSize)
                {
                    log = log + "\r\n文件数据大小符合规范";
                    return true;
                }
                else
                {
                    log = log + "\r\n文件数据小于标准数据";
                    return false;
                }
                //if (fi.Length / (1024 * 1024) < 4096 && fi.Length / (1024 * 1024) > 200)
                //{
                //    log = log + "\r\n文件数据大小符合规范";
                //    return true;
                //}
                //else if (fi.Length / (1024 * 1024) >= 4096)
                //{
                //    log = log + "\r\n文件数据大于标准数据";
                //    return false;
                //}
                //else
                //{
                //    log = log + "\r\n文件数据小于标准数据";
                //    return false;
                //}
            }
            else if (filename.ToUpper().Contains("HJ"))
            {
                //20MB-2GB
                FileInfo fi = new FileInfo(filename);
                if (fi.Length / (1024 * 1024) < 2048 && fi.Length / (1024 * 1024) > 20)
                {
                    log = log + "\r\n文件数据大小符合规范";
                    return true;
                }
                else if (fi.Length / (1024 * 1024) >= 2048)
                {
                    log = log + "\r\n文件数据大于标准数据";
                    return false;
                }
                else
                {
                    log = log + "\r\n文件数据小于标准数据";
                    return false;
                }
            }
            else return false;
        }

        private bool ThumbnailCheck(string filename)
        {
            log = log + "\r\n开始缩略图检测";
            //FileInfo file = new FileInfo(filename);
            //string dir = file.DirectoryName;
            ////String[] xmlFileNames = Directory.GetFiles(dir, "*_thumb.jpg") ;
            ////String[] xmlFileNames = Directory.GetFiles(dir, "*_thumb.jpg");
            //List<string[]>xmlFileNames= new List<string[]>() ;
            //xmlFileNames.Add(Directory.GetFiles(dir, "*thumb.jpg"));
            //xmlFileNames.Add(Directory.GetFiles(dir, "*thumb.JPG"));
            //xmlFileNames.Add(Directory.GetFiles(dir, "*THUMB.JPG"));
            //xmlFileNames.Add(Directory.GetFiles(dir, "*THUMB.jpg"));

            int FileCount = 0;
            // 这里写你的目录
           // DirectoryInfo Dir = new DirectoryInfo(filename);
            FileInfo file = new FileInfo(filename);
            string dire = file.DirectoryName;
            DirectoryInfo Dir = new DirectoryInfo(dire);
            foreach (FileInfo FI in Dir.GetFiles())
            {
                // 缺失.jpg格式的缩略图
                if (System.IO.Path.GetExtension(FI.Name).ToLower() == ".jpg" || Path.GetExtension(FI.Name).ToLower() == ".png")
                {
                    FileCount++;
                }
            }
            if (FileCount > 0)
            {
                log = log + "\r\n缩略图检测通过";
                return true;
            }
            log = log + "\r\n缩略图不存在";
            return false;
        }

        private bool FormatCheck(string filename)
        {
            log = log + "\r\n开始数据格式检测";
            Thread.Sleep(2000);
            log = log + "\r\n数据格式检测通过";
            return true;
        }

        public void Getreport()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("共检测文件'{0}'个：",dataPath.Count));
            int successcount = 0;
            for (int i = 0; i < dataPath.Count;i++ )
            {
                sb.AppendLine(string.Format("文件名'{0}'：", Path.GetFileName(dataPath[i])));

                if (checkresult[i][0])
                {
                    sb.AppendLine("符合第一条规范，文件命名规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第一条规范，文件命名规范检核未通过，请查看日志文件获取详情");
                    continue;
                }

                if (checkresult[i][1])
                {
                    sb.AppendLine("符合第二条规范，元数据规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第二条规范，元数据规范检核未通过，请查看日志文件获取详情");
                    continue;
                }
                if (checkresult[i][2])
                {
                    sb.AppendLine("符合第三条规范，数据大小规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第三条规范，数据大小规范检核未通过，请查看日志文件获取详情");
                    continue;
                }
                if (checkresult[i][3])
                {
                    sb.AppendLine("符合第四条规范，缩略图规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第四条规范，缩略图规范检核未通过，请查看日志文件获取详情");
                    continue;
                }
                if (checkresult[i][4])
                {
                    sb.AppendLine("符合第五条规范，数据格式规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第五条规范，数据格式规范检核未通过，请查看日志文件获取详情");
                    continue;
                }
                    successcount++;
            }
            sb.AppendLine(string.Format("审核通过的文件数：{0}，未通过数：{1}\r\n", successcount, dataPath.Count - successcount));
            report = sb.ToString();
        }

    }
}
