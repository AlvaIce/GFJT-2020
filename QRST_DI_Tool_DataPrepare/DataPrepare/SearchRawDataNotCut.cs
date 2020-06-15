using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace DataPrepare
{
    /// <summary>
    /// 获得所需切片的原始数据
    /// </summary>
    class SearchRawDataNotCut
    {
        private List<List<string>> tileInfos = new List<List<string>>();
        static WS_QDB_Searcher_Sqlite.ServiceSoapClient ssc;
        public static string taskUserName = "";
        private string taskProductName = "";
        private string needTile = "";

        public SearchRawDataNotCut(string message)
        {
            this.tileInfos = resolveRcvMsg(message);
            ssc = new WS_QDB_Searcher_Sqlite.ServiceSoapClient();
        }

        public SearchRawDataNotCut(List<List<string>> tileInfos)
        {
            this.tileInfos = tileInfos;
            ssc = new WS_QDB_Searcher_Sqlite.ServiceSoapClient();
        }

        /// <summary>
        /// 将集成共享发送过来的用户名、所需瓦片等信息存储在数据库的isdb中的app_user、app_need_tile表中
        /// </summary>
        public void InsertNeedTileInfo()
        {
            MysqlOperate operation = new MysqlOperate();
            operation.SubmitTileOrder(taskUserName, taskProductName, needTile);
        }

        /// <summary>
        /// 解析集成共享发过来的消息，提取其中的层级、行列号
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public List<List<string>> resolveRcvMsg(string message)
        {
            List<List<string>> result = new List<List<string>>();
            string[] msgArray = message.Split('#');
            taskUserName = msgArray[1];
            taskProductName = msgArray[2];
            needTile = msgArray[3];
            foreach (String str in needTile.Split(new char[] { ';' }))
            {
                string[] subStrs = str.Split(new char[] { ',' });
                if (subStrs.Length == 3)
                {
                    result.Add(subStrs.ToList());
                }
            }
            return result;
        }

        /// <summary>
        /// 获得已经进行切片的原始数据的DataSourceID
        /// </summary>
        /// <returns></returns>
        public List<string> searchRawDataInSqlite()
        {
            List<string> resultDateSourceID = new List<string>();
            string[][] tileInfosArray = new string[tileInfos.Count][];
            for(int i=0; i<tileInfos.Count;i++)
            {
                tileInfosArray[i] = new string[3];
                tileInfosArray[i][0] = tileInfos[0][0];
                tileInfosArray[i][1] = tileInfos[0][1];
                tileInfosArray[i][2] = tileInfos[0][2];
            }
            DataSet resultDS = ssc.searTileByColAndRowBatch(tileInfosArray);
            if(resultDS.Tables.Count > 0)
            {
                foreach (DataRow mDr in resultDS.Tables[0].Rows)
                {
                    if (mDr["DataSourceID"].ToString() != null && !mDr["DataSourceID"].ToString().Equals(""))
                    {
                        resultDateSourceID.Add(mDr["DataSourceID"].ToString());
                    }
                }
            }
            return resultDateSourceID;
        }

        /// <summary>
        /// 获得所需瓦片的所有原始数据的数据名信息和QRST_CODE
        /// </summary>
        /// <returns></returns>
        public Dictionary<string,string> searchRawDataInMysql()
        {
            Dictionary<string,string> result = new Dictionary<string,string>();
            MysqlOperate operate = new MysqlOperate();
            List<GF1_LanAndLong> allGfData = operate.getRecentInsert();
            foreach (GF1_LanAndLong gf in allGfData)
            {
                foreach(List<string> tileInfo in tileInfos)
                {
                    string tileLevel = tileInfo[0];
                    string row = tileInfo[1];
                    string col = tileInfo[2];
                    double[] LanAndLong = new double[4];
                    LanAndLong = StaticTools.GetLatAndLong(row, col, tileLevel);
                    Intersect inter = new Intersect(gf, LanAndLong);
                    if (!result.ContainsKey(gf.Name)&&StaticTools.isResolutionMatch(gf.SatelliteID,gf.SensorID,tileLevel) && inter.VLL())
                    {
                        result.Add(gf.Name, gf.QRST_CODE);
                    }
                }
              }
             return result;
        }

        /// <summary>
        /// 获得符合条件的原始数据信息不包括已经进行过切片的
        /// </summary>
        /// <returns></returns>
        public List<string> getRawDataNotCutTile()
        {
            List<string> result = new List<string>();
            List<string> tileInSqlite = searchRawDataInSqlite();
            Dictionary<string, string> tileInMySql = searchRawDataInMysql();
            foreach(KeyValuePair<string,string> kvp in tileInMySql)
            {
                Boolean flag = true;
                foreach(string str in tileInSqlite)
                {
                    if (kvp.Key.Contains(str))
                    {
                        tileInSqlite.Remove(str);
                        flag = false;
                        break;
                    }
                }
                if(flag)
                {
                    result.Add(kvp.Key+"#"+kvp.Value);
                }                
            }
            return result;
        }
    }
}
