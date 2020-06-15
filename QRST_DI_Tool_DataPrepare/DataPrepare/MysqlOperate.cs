using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using System.Data;

namespace DataPrepare
{
    /// <summary>
    /// mysql查询操作
    /// </summary>
    public class MysqlOperate
    {

        public GF1_LanAndLong gf1;
        public Tile_Info tile;
        public List<Tile_Info> tiles = new List<Tile_Info>(); //切片列表

        /// <summary>
        /// 获取evdb.prod_gf1表中所有的高分影像并存入GF1s中
        /// </summary>
        public List<GF1_LanAndLong> getRecentInsert()
        {
            List<GF1_LanAndLong> result = new List<GF1_LanAndLong>();
            MySqlConnection mycon = new MySqlConnection(Constant.ConnectionEVDBStringMySql);
            mycon.Open();
            string sql = string.Format(@"select Name,DATALOWERLEFTLAT,DATALOWERLEFTLONG,DATALOWERRIGHTLAT,DATALOWERRIGHTLONG,
DATAUPPERRIGHTLAT,DATAUPPERRIGHTLONG,DATAUPPERLEFTLAT,DATAUPPERLEFTLONG,ProductID,QRST_CODE,SatelliteID,
SensorID from prod_gf1");
            MySqlCommand cmd = new MySqlCommand(sql, mycon);
            using (MySqlDataReader reader1 = cmd.ExecuteReader())
            {
                while (reader1.Read())
                {
                    gf1 = new GF1_LanAndLong();
                    gf1.Name = reader1[0].ToString();
                    gf1.DATALOWERLEFTLAT = Convert.ToDouble(reader1[1]);
                    gf1.DATALOWERLEFTLONG = Convert.ToDouble(reader1[2]);
                    gf1.DATALOWERRIGHTLAT = Convert.ToDouble(reader1[3]);
                    gf1.DATALOWERRIGHTLONG = Convert.ToDouble(reader1[4]);
                    gf1.DATAUPPERRIGHTLAT = Convert.ToDouble(reader1[5]);
                    gf1.DATAUPPERRIGHTLONG = Convert.ToDouble(reader1[6]);
                    gf1.DATAUPPERLEFTLAT = Convert.ToDouble(reader1[7]);
                    gf1.DATAUPPERLEFTLONG = Convert.ToDouble(reader1[8]);
                    gf1.ProductID = reader1[9].ToString();
                    gf1.QRST_CODE = reader1[10].ToString();
                    gf1.SatelliteID = reader1[11].ToString();
                    gf1.SensorID = reader1[12].ToString();
                    result.Add(gf1);
                }
            }
            mycon.Close();
            return result;
        }

        /// <summary>
        /// 从isdb中获取所有的层级行列号信息并存入tiles中
        /// </summary>
        public List<Tile_Info> getAllTile()
        {
            List<Tile_Info> result = new List<Tile_Info>();
            MySqlConnection mycon = new MySqlConnection(Constant.ConnectionISDBStringMySql);
            mycon.Open();
            //因为后来没有新建tileinfo表，所以下面的sql语句重写
            string sql = string.Format(@"select level,row,col from app_need_tile");
            MySqlCommand cmd = new MySqlCommand(sql, mycon);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tile = new Tile_Info();
                    tile.level = reader[0].ToString();
                    tile.row = reader[1].ToString();
                    tile.column = reader[2].ToString();
                    result.Add(tile);                 
                }
            }
            mycon.Close();
            return result;
        }




        /// <summary>
        /// 将插入瓦片插入isdb.tileorder请求
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="ProductType"></param>
        /// <param name="strTileInfo"></param>
        /// <returns></returns>
        public bool SubmitTileOrder(string UserName, string ProductType, string strTileInfo)
        {
            MySqlConnection mycon = new MySqlConnection(Constant.ConnectionISDBStringMySql);
            mycon.Open();
            string insertUserSql = string.Format("insert into app_user(username) values('{0}')",UserName);
            MySqlCommand userCommand = new MySqlCommand(insertUserSql, mycon);
            try{
                userCommand.ExecuteNonQuery();
            }
            catch(MySqlException e)
            {

            }
            string searTileIndexSql = string.Format("select TileIndex from app_user where username='{0}'",UserName);
            userCommand = new MySqlCommand(searTileIndexSql,mycon);
            string resultTileIndexs = "";
            using (MySqlDataReader reader = userCommand.ExecuteReader())
            {
                while (reader.Read())
                {
                    resultTileIndexs = reader[0].ToString();
                }
            }
            string[] tiles = strTileInfo.Split(new char[] { ';' });
            foreach(string tile in tiles)
            {
                if (tile.Equals("") || tile == null)
                {

                }
                else
                {
                    string[] strTile = tile.Split(new char[] { ',' });
                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = mycon;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "insert_app_need_tile";
                    cmd.Parameters.Add("in_level", SqlDbType.Int);
                    cmd.Parameters.Add("in_row", SqlDbType.Int);
                    cmd.Parameters.Add("in_col", SqlDbType.Int);
                    cmd.Parameters.Add("result", SqlDbType.Int);
                    cmd.Parameters[3].Direction = ParameterDirection.Output;
                    cmd.Parameters[0].Value = Convert.ToInt32(strTile[0]);
                    cmd.Parameters[1].Value = Convert.ToInt32(strTile[1]);
                    cmd.Parameters[2].Value = Convert.ToInt32(strTile[2]);
                    cmd.ExecuteNonQuery();
                    int result = Convert.ToInt32(cmd.Parameters[3].Value);
                    string[] tileIndexs = resultTileIndexs.ToString().Split(new char[] { ',' });
                    if (!tileIndexs.Contains(result + ""))
                    {
                        resultTileIndexs = resultTileIndexs + (result + ",");
                    }
                }
                    
            }
            string insertTileIndexSql = string.Format("update app_user set TileINdex='{0}' where username='{1}'", resultTileIndexs, UserName);
            userCommand = new MySqlCommand(insertTileIndexSql,mycon);
            userCommand.ExecuteNonQuery();
            return true;
        }

        /// <summary>
        /// 从库中获取IP配号表
        /// </summary>
        /// <returns>IP配号表</returns>
        public static DataSet GetTileDsMod()
        {
            MySqlConnection mycon = new MySqlConnection(Constant.ConnectionEVDBStringMySql);
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            adapter.TableMappings.Add("Table", "IPandMod");
            mycon.Open();
            string sql = "select addressip,modid from tileserversitesinfo where modid is not null";
            MySqlCommand command = new MySqlCommand(sql, mycon);
            adapter.SelectCommand = command;
            DataSet dtMod = new DataSet("IPandMod");
            adapter.Fill(dtMod);
            mycon.Close();
            return dtMod;
        }
    }
}
