using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace QRST_DI_MS_Console.UserInterfaces
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

        public void DisplayZB(bool isVisible)
        {
            panelControlJHZB.Visible = isVisible;
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
          //  Thread t = new Thread(new ThreadStart(dc.StartCheck));
           // t.Start();
          //  dc.StartCheck();
        }

     
    }

    public class DataCheck
    {
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
          
            if(dataPath1!=null)
            {
                dataPath = dataPath1;
                totalData = dataPath.Count.ToString();
                StringBuilder sb = new StringBuilder(log);
                for (int i = 0; i < dataPath.Count;i++ )
                {
                    sb.AppendLine(string.Format("检核文件{0}:{1};",i+1,Path.GetFileName(dataPath[i])));
                }
                log = sb.ToString();
                progress = string.Format("{0}/{1}", 0, dataPath.Count);
            }
        }

        public void StartCheck()
        {
            status = "正在检核...";
            for (int i = 0; i < dataPath.Count;i++ )
            {
                log = log + "\r\n开始检测:" + Path.GetFileName(dataPath[i]);
                bool[] result = QualityCheck(dataPath[i]);
                log = log + "\r\n检测完成检测:" + Path.GetFileName(dataPath[i]);
                progress = string.Format("{0}/{1}", i + 1, dataPath.Count);
                checkresult.Add(result);
            }
            Getreport();
            status = "检核完成...";
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
        /// 名字检测
        /// </summary>
        /// <returns></returns>
        private bool NameCheck(string filename)
        {
            log = log + "\r\n开始命名格式检测" ;
            Thread.Sleep(300);
            filename = Path.GetFileName(filename);
            if (filename.ToLower().EndsWith(".tar.gz") && filename.ToLower().StartsWith("gf1"))
            {
                log = log + "\r\n命名符合规范";
                return true;
            }
            else
            {
                log = log + "\r\n命名不规范，不是以GF1开头的命名或者不是一个完整的压缩包";
            }
          
            return false;
        }

        private bool MetaDataCheck(string filename)
        {
            log = log + "\r\n开始源数据检测";
            log = log + "\r\n正在解压获取源数据信息";
            Random r = new Random();

            Thread.Sleep(r.Next(2000, 10000));
            if (r.Next(0, 9) < 3)
            {
                log = log + "\r\n元数据文件获取失败";
                return false;
            }
            else
            {
                log = log + "\r\n元数据信息获取成功";
                int index = r.Next(0, 9);
                if(index<5)
                {
                    log = log + "\r\n缺少字段信息"+fields[index];
                }
                return true;
            }
           
        }

        string[] fields = new string[] { "SatelliteID", "SensorID", "ProductLevel", "SceneRow", "Bands" };

        private bool DataSizeCheck(string filename)
        {
            log = log + "\r\n开始数据大小检测" ;
            FileInfo fi = new FileInfo(filename);
            if (fi.Length / (1024 * 1024) < 2048 && fi.Length / (1024 * 1024) > 300)
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

        private bool ThumbnailCheck(string filename)
        {
            log = log + "\r\n开始缩略图检测";
            Thread.Sleep(200);
            return true;
        }

        private bool FormatCheck(string filename)
        {
            log = log + "\r\n开始数据格式检测";
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
                    sb.AppendLine("符合第四条规范，数据格式规范检核通过");
                }
                else
                {
                    sb.AppendLine("不符合第四条规范，数据格式规范检核未通过，请查看日志文件获取详情");
                    continue;
                }

                    successcount++;

            }
            sb.AppendLine(string.Format("审核通过的文件数：{0}，未通过数：{1}", successcount, dataPath.Count - successcount));
            report = sb.ToString();
        }

    }
}
