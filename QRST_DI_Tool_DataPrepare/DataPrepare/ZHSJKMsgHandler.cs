using System;
using System.Collections.Generic;

namespace DataPrepare
{
    class ZHSJKMsgHandler
    {
        private Form1 mainForm;
        public static NoticeMessager notice; //监听
        static public GF1_LanAndLong gf1 = new GF1_LanAndLong(); //高分影像
        public Tile_Info tile = new Tile_Info(); //切片
        public static MysqlOperate mysqloperate = new MysqlOperate();      

        public void ZHSJKMonitorServiceStart()
        {
            notice = new NoticeMessager(false);
            notice.ReciewedMessage += new RecievedMessageHandle(notice_ReciewedMessage);
        }

        /// <summary>
        /// 当接收到高分影像入库的消息后，将该影像与所有瓦片进行判断与比对，如果符合条件则返回高分影像的ProductID
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="msg"></param>
        /// <returns>高分影像的ProductID</returns>
        static string notice_ReciewedMessage(string sender, string msg)
        {
            Form1.mainForm.AddTextBoxMessege("接收到来自：" + sender+"的消息："+msg);
            string[] sArray = msg.Split('|');
            gf1.Name = sArray[0];
            gf1.DATALOWERLEFTLAT = Convert.ToDouble(sArray[1]);
            gf1.DATALOWERLEFTLONG = Convert.ToDouble(sArray[2]);
            gf1.DATALOWERRIGHTLAT = Convert.ToDouble(sArray[3]);
            gf1.DATALOWERRIGHTLONG = Convert.ToDouble(sArray[4]);
            gf1.DATAUPPERRIGHTLAT = Convert.ToDouble(sArray[5]);
            gf1.DATAUPPERRIGHTLONG = Convert.ToDouble(sArray[6]);
            gf1.DATAUPPERLEFTLAT = Convert.ToDouble(sArray[7]);
            gf1.DATAUPPERLEFTLONG = Convert.ToDouble(sArray[8]);
            gf1.ProductID = sArray[9];
            gf1.SatelliteID = sArray[10];
            gf1.SensorID = sArray[11];

            List<Tile_Info> tileInfos = mysqloperate.getAllTile();
            for (int i = 0; i < tileInfos.Count; i++)
            {
                double[] LanAndLong = new double[4];
                LanAndLong = StaticTools.GetLatAndLong(tileInfos[i].row, tileInfos[i].column, tileInfos[i].level);
                Intersect inter = new Intersect(gf1, LanAndLong);
                if (StaticTools.isResolutionMatch(gf1.SatelliteID, gf1.SensorID, tileInfos[i].level) && inter.VLL())
                {
                    List<string> tempList = new List<string>();
                    tempList.Add(gf1.Name + "#" + gf1.ProductID);
                    string returnMsg = StaticTools.getReturnMsg(tempList,"userName");
                    Form1.mainForm.AddTextBoxMessege("返回消息：" + returnMsg);
                    StaticTools.SendMessage(returnMsg);
                    return returnMsg;
                }
            }
            return "";
        }
    }
}
