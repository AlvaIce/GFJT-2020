using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using QRST_DI_DS_MetadataQuery;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_SS_Basis;
using QRST_DI_TS_Basis.DirectlyAddress;
using QRST_DI_TS_Basis.DBEngine;
// 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucDataMaintainer : DevExpress.XtraEditors.XtraUserControl
    {
        private DataTable dt = null;
        bool isSqlLite = false;
        private List<int> deleteIndex = new List<int>();
        private List<string> strQrstCode = new List<string>();
        private List<List<string>> sqlLiteInfo = new List<List<string>>();
        string ChooseDataColumnCaption = "选择数据";
        //MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
        //MySql.Data.MySqlClient.MySqlCommand cmd;
        string tableName;
        int count;
        string destPath;
        List<string> tileslist = new List<string>();
        TileIndexUpdateUtilities tileIndexUpdate = new TileIndexUpdateUtilities();
    
        public mucDataMaintainer()
        {
            InitializeComponent();
            try
            {
                FieldViewBasedQuerySchema queryInfo = (FieldViewBasedQuerySchema)ruc3DSearcher.querySchema;
                //SQLite 系统重构
                //conn.ConnectionString = queryInfo.baseUtilities.connString;
                //tableName = queryInfo.tableName;
                //conn.Open();
                //cmd = new MySql.Data.MySqlClient.MySqlCommand();
                //cmd.Connection = conn;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void setGridControl()
        {
            gridControlMaintainTable.DataSource = null;
            gridView1.Columns.Clear();
            dt = mucDetailViewer.tempTable;
            DataColumn checkDownColumn = new DataColumn()
            {
                ColumnName = ChooseDataColumnCaption,
                DataType = typeof(bool)
            };
            //DLF 0822因异常添加
            if (dt == null)
            {
                gridControlMaintainTable.DataSource = null;
                return;
            }
            if (!dt.Columns.Contains(ChooseDataColumnCaption))
                dt.Columns.Add(checkDownColumn);
            gridControlMaintainTable.DataSource = dt;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][ChooseDataColumnCaption] = false;
            }
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                if (i < 15)
                {
                    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                    gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else
                    if (i != gridView1.Columns.Count - 1)
                    {
                        gridView1.Columns[i].Visible = false;
                    }
            }
            if (gridView1.Columns.Count > 1)
            {
                gridView1.Columns[gridView1.Columns.Count - 1].OptionsColumn.AllowEdit = true;
                gridView1.Columns[gridView1.Columns.Count - 1].Visible = true;
                gridView1.Columns[gridView1.Columns.Count - 1].VisibleIndex = 0;
            }
        }
        /// <summary>
        /// 20170412 把数据写入gridControlMaintainTable中
        /// </summary>
        /// <param name="dt"></param>
        public  void setGridControl(DataTable dt)
        {
            gridControlMaintainTable.DataSource = null;
            gridView1.Columns.Clear();
            //dt = mucDetailViewer.tempTable;
            DataColumn checkDownColumn = new DataColumn()
            {
                ColumnName = ChooseDataColumnCaption,
                DataType = typeof(bool)
            };
            //DLF 0822因异常添加
            if (dt == null)
            {
                gridControlMaintainTable.DataSource = null;
                return;
            }
            if (!dt.Columns.Contains(ChooseDataColumnCaption))
                dt.Columns.Add(checkDownColumn);
            gridControlMaintainTable.DataSource = dt;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][ChooseDataColumnCaption] = false;
            }
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                if (i < 15)
                {
                    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                    gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else
                    if (i != gridView1.Columns.Count - 1)
                    {
                        gridView1.Columns[i].Visible = false;
                    }
            }
            if (gridView1.Columns.Count > 1)
            {
                gridView1.Columns[gridView1.Columns.Count - 1].OptionsColumn.AllowEdit = true;
                gridView1.Columns[gridView1.Columns.Count - 1].Visible = true;
                gridView1.Columns[gridView1.Columns.Count - 1].VisibleIndex = 0;
            }
        }
        void mucDataMaintainer_VisibleChanged(object sender, System.EventArgs e)
        {
            if (this.Visible == true)
                setGridControl();
        }
        public void deleteRecord()
        {
            gridView1.CloseEditor();
            if (dt == null)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((bool)dt.Rows[i]["选择数据"])
                {
                    deleteIndex.Add(i);
                    if (!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                    {
                        isSqlLite = false;

                    if (dt.Columns.Contains("数据编码"))
                    {
                        strQrstCode.Add(dt.Rows[i]["数据编码"].ToString());
                        }
                        else if (dt.Columns.Contains("QRST_CODE"))
                        {
                            //isSqlLite = false;
                            strQrstCode.Add(dt.Rows[i]["QRST_CODE"].ToString());

                        }
                        else  //波普数据
                        {
                            //strQrstCode.Add(dt.Rows[i].ItemArray.ToString());
                            strQrstCode.Add(dt.Rows[i]["序号"].ToString()); 
                        }
                        
                    }
                    else
                    {
                        isSqlLite = true;
                        List<string> sqlRecordInfo = new List<string>();
                        object[] obj = dt.Rows[i].ItemArray;
                        for (int j = 0; j < obj.Length; j++)
                        {
                            sqlRecordInfo.Add(obj[j].ToString());
                        }
                        sqlLiteInfo.Add(sqlRecordInfo);
                        //tileslist.Add(dt.Rows[i][0].ToString());   
                        //tileslist.Add(sqlRecordInfo[0]); 
                    }
                }
                else
                {
                    continue;
                }
            }
            count = 0;
            foreach (var item in deleteIndex)
            {
                try
                {
                    dt.Rows.RemoveAt(item - count);
                    count++;
                }
                catch
                {
                    continue;
                }
            }

            deleteIndex.Clear();
            //setGridControl();
            MessageBox.Show("记录移除成功！");
        }

        public void ExcuteDelete(int n)
        {
            if (!isSqlLite)
            {
                if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")
                {
                    
                }
                else
                {
                //MySql.Data.MySqlClient.MySqlConnection conn1 = new MySql.Data.MySqlClient.MySqlConnection();
                //MySql.Data.MySqlClient.MySqlCommand cmd1 = new MySql.Data.MySqlClient.MySqlCommand();
                try
                {
                    FieldViewBasedQuerySchema queryInfo = (FieldViewBasedQuerySchema)ruc3DSearcher.querySchema;
                    //conn1.ConnectionString = queryInfo.baseUtilities.connString;
                    tableName = queryInfo.tableName;
                    //conn.Open();
                    //cmd1.Connection = conn1;

                    string sql = string.Format("delete from {0} where QRST_CODE ='{1}'", tableName, strQrstCode[n]);
                    //conn1.Open();
                    //cmd1.CommandText = sql;
                    //cmd1.ExecuteNonQuery();
                    queryInfo.baseUtilities.ExecuteSql(sql);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    //conn1.Close();
                    }
                }
            }
            else
            {
                //20170221 解决因利用sql语句删除瓦片效率低的问题
                List<string> rowList = sqlLiteInfo[n];
                List<string> tilesnames = new List<string>();
                tilesnames.Add(rowList[0]);
                tileIndexUpdate.TileIndexUpdate(TileIndexUpdateType.Delete, tilesnames);
                //tileIndexUpdate.TileIndexUpdate(TileIndexUpdateType.Delete, tileslist);

                //List<string> rowList = sqlLiteInfo[n];
                //string sql = string.Format("delete from correctedTiles where DataSourceID ='{0}' and Satellite ='{1}' and Sensor ='{2}' and Date ='{3}' and Level ='{4}' and Row ='{5}' and Col ='{6}' and type ='{7}'", rowList[1], rowList[2], rowList[3], rowList[4], rowList[5], rowList[6], rowList[7], rowList[8]);
                //service.ExecuteNonQuery(sql);
            }
        }
        //20170220删除本地共享文件夹的数据
        public void DeleteLocalFile(int n)
        {
            selectedQueryObj = muc3DSearcher._metadatacatalognode_Mdl;
            if (selectedQueryObj == null)
            {
                return;
            }

            if (!isSqlLite)
            {
                if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")
                {
                    destPath = null;
                }
                else if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "madb" && !selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_normalfile"))
                {
                    destPath = null;
                }
                else
                {
                    string tableCode = StoragePath.GetTableCodeByQrstCode(strQrstCode[n]);
                    StoragePath storePath = new StoragePath(tableCode);
                    destPath = storePath.GetDataPath(strQrstCode[n]);

                }

            }
            else
            {
                DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                TileNameArgs tnargs = da.GetTileNameArgs(sqlLiteInfo[n][0]);
                //TileNameArgs tnargs = da.GetTileNameArgs(tileslist[n]);
                string ipmod = da.GetStorageIPMod(int.Parse(tnargs.Row), int.Parse(tnargs.Col)).ToString();
                string storeip = da.GetIPbyMod(ipmod);
                //destPath = tnargs.GetFilePath(storeip, ipmod);
                destPath = da.GetPathByFileName(sqlLiteInfo[n][0], out storeip);
                //destPath = da.GetPathByFileName(tileslist[n], out storeip);
            }
            deleteTmpFiles(destPath);
        }

        //2016/12/25删除指定目录下的所有文件及文件夹及本目录
        public void deleteTmpFiles(string dirPath)
        {
            if (Directory.Exists(dirPath))
            {
                foreach (string content in Directory.GetFileSystemEntries(dirPath))
                {
                    if (Directory.Exists(content))
                    {
                        try
                        {
                        Directory.Delete(content, true);
                        }
                        catch (Exception)
                        {
                     
                        }
                    }
                    else if (File.Exists(content))
                    {
                        try
                        {
                        File.Delete(content);
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                try
                {
                Directory.Delete(dirPath, true);
                }
                catch (Exception)
                {
                }
            }
            else if (File.Exists(dirPath))
            {
                try
                {
                 File.Delete(dirPath);
                }
                catch (Exception)
                {
                }
            }
            else
            {

            }
        }

        public void SelectAllData(bool isselected)
        {
            gridView1.CloseEditor();
            DataTable dt = (DataTable)gridControlMaintainTable.DataSource;
            if (dt == null)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["选择数据"] = isselected;
            }
        }
        public void SaveDeleteResult()
        {
            DialogResult result = MessageBox.Show("是否确定删除？", "提示", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
                return;
            int Sum = count;
            int TaskNum = Sum / 5;
            if (TaskNum != 0)
            {
                Task[] tasks = new Task[5];
                tasks[0] = new Task(() =>
                {
                    for (int i = 0; i < TaskNum; i++)
                    {
                        this.DeleteLocalFile(i);
                        this.ExcuteDelete(i);
                    }
                });
                tasks[1] = new Task(() =>
                {
                    for (int i = TaskNum; i < TaskNum * 2; i++)
                    {
                        this.DeleteLocalFile(i);
                        this.ExcuteDelete(i);
                    }
                });
                tasks[2] = new Task(() =>
                {
                    for (int i = TaskNum * 2; i < TaskNum * 3; i++)
                    {
                        this.DeleteLocalFile(i);
                        this.ExcuteDelete(i);
                    }
                });
                tasks[3] = new Task(() =>
                {
                    for (int i = TaskNum * 3; i < TaskNum * 4; i++)
                    {
                        this.DeleteLocalFile(i);
                        this.ExcuteDelete(i);
                    }
                });
                tasks[4] = new Task(() =>
                {
                    for (int i = TaskNum * 4; i < TaskNum * 5; i++)
                    {
                        this.DeleteLocalFile(i);
                        this.ExcuteDelete(i);
                    }
                });
                tasks[0].Start();
                tasks[1].Start();
                tasks[2].Start();
                tasks[3].Start();
                tasks[4].Start();
                Task.WaitAll(tasks);
                tasks[0] = new Task(() =>
                {
                    for (int i = TaskNum * 5; i < Sum; i++)
                    {
                        this.DeleteLocalFile(i);
                        this.ExcuteDelete(i);
                    }
                });
                tasks[0].Start();
                tasks[0].Wait();
            }
            else
            {
                for (int i = 0; i < Sum; i++)
                {
                    this.DeleteLocalFile(i);
                    this.ExcuteDelete(i);
                    //Thread.Sleep(1000);
                }
            }
            //conn.Close();
            strQrstCode.Clear();
            sqlLiteInfo.Clear();
            
            MessageBox.Show("数据删除成功！");
        }
        /// <summary>
        /// xmh 20170411 根据检索的结果创建一个DataTable 
        /// </summary>
        /// <returns></returns>
        public DataTable getNewTable() 
        {
            gridView1.CloseEditor();
             DataTable newTable = mucDetailViewer.tempTable.Clone();
            if (newTable == null)
            {
                gridControlMaintainTable.DataSource = null;
                return newTable;
            }
            newTable.Rows.Clear();
            gridControlMaintainTable.DataSource = newTable;

            return newTable;
        }
        /// <summary>
        /// 20170412  用于获取同名冗余的数据记录，并把记录存入表中
        /// </summary>
        /// <param name="str"></param>
        public void getSameItems(String str) 
        {
            gridView1.CloseEditor();
            dt = mucDetailViewer.tempTable;
            if (dt == null)
            {
                return;
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                //group by 数据名称     找出 数据名称重复的记录（分组）
                var query = (from t in dt.AsEnumerable()
                             group t by new { name = t.Field<string>(str) } into m
                             select new
                             {
                                 name = m.Key.name,
                                 rowcount = m.Count()
                             } into c
                             where c.rowcount > 1
                             select c).ToList();
                
                if (query.Count > 0)  //有同名冗余的数据
                {
                    DataTable newTable = getNewTable();
                    //遍历分组结果集
                    foreach (var q in query)
                    {
                        //DataRow[] dr_finds = dt.Select("文件名称 =  '" + q.name + " '");
                        DataRow[] dr_finds = dt.Select(str + " = '" + q.name + " '");;
                        foreach (DataRow dr in dr_finds)
                        {
                            DataRow drNew = newTable.NewRow();
                            drNew.ItemArray = (object[])dr.ItemArray.Clone(); 
                            newTable.Rows.Add(drNew);
                            
                        }
                    }
                    dt = newTable;
                    //gridControlMaintainTable.DataSource = newTable;
                    setGridControl(dt);
                }
                else     //没有同名冗余的数据
                {
                    gridView1.CloseEditor();
                    DataTable newTable = mucDetailViewer.tempTable.Copy();
                    if (newTable == null)
                    {
                        gridControlMaintainTable.DataSource = null;
                        return;
                    }
                    newTable.Rows.Clear();
                    gridControlMaintainTable.DataSource = newTable;
                    gridView1.Columns.Clear();
                    MessageBox.Show("没有同名冗余的数据记录！！");
                }
               
            }
        }
        static metadatacatalognode_Mdl selectedQueryObj = null; 
        /// <summary>
        /// 20170412 xmh  
        /// 显示同名冗余记录
        /// </summary>
        public void ShowSameItems()
        {
            string str;
            selectedQueryObj = muc3DSearcher._metadatacatalognode_Mdl;
            if (selectedQueryObj == null)
            {
                return;
            }
            if (!selectedQueryObj.IS_DATASET)
            {
                switch (selectedQueryObj.GROUP_TYPE.ToLower())
                {
                    case "system_vector":
                        str = "元数据名称";
                        getSameItems(str);
                        break;
                    case "system_raster":
                        if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "evdb")
                        {
                            str = "数据名称";
                            getSameItems(str);
                        }
                        else if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "indb")
                        {
                            str = "产品名称";
                            getSameItems(str);
                        }
                        else if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "isdb")
                        {
                            str = "dataName";
                            getSameItems(str);
                        }

                        break;
                    case "system_document":
                        if (selectedQueryObj.GROUP_CODE.Substring(0,4).ToLower() == "isdb")
                        {
                            str = "文档名称";
                            getSameItems(str);

                        }
                        else if (selectedQueryObj.GROUP_CODE.Substring(0,4).ToLower() == "madb")
                        {
                            if (selectedQueryObj.GROUP_CODE.ToLower() == "madb-2-10")   //标准工具
                            {
                                str = "toolName";
                                getSameItems(str);
                            }
                            else if (selectedQueryObj.GROUP_CODE.ToLower() == "madb-2-3") //算法组件
                            {
                                str = "AlgorithmName";
                                getSameItems(str);
                            }
                            
                        }
                        
                        break;
                    case "system_normalfile":
                         str = "文件名称";
                        getSameItems(str);
                        break;
                    case "system_table":
                        if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")
                        {
                            if (selectedQueryObj.NAME == "北方植被"||selectedQueryObj.NAME == "南方植被")
                            {
                                str = "植被名称";
                                getSameItems(str);
                            }
                            else if (selectedQueryObj.NAME == "土壤")
                            {
                                str = "土壤名称";
                                getSameItems(str); 
                            }
                            else if (selectedQueryObj.NAME == "岩矿")
                            {
                                str = "岩矿名称";
                                getSameItems(str);
                            }
                            else if (selectedQueryObj.NAME == "城市目标")
                            {
                                str = "人工目标名称";
                                getSameItems(str);
                            }
                            else if (selectedQueryObj.NAME == "地表大气")
                            {
                                str = "站点名称";
                                getSameItems(str);
                            }
                            else if (selectedQueryObj.NAME == "水体")
                            {
                                str = "水域名称";
                                getSameItems(str);
                            }
                        }
                        else
                        {

                        }
                        break;
                    case "system_tile":
                        str = "TileFileName";
                        getSameItems(str);
                        break;
                    default:
                        break;
                }
            }
           
        }

        /// <summary>
        ///  xmh 20170411 还原记录
        /// </summary>
        public void ResetItems()
        {
            dt = mucDetailViewer.tempTable;
            setGridControl(dt);
            //setGridControl();
           
        }


    }
}
