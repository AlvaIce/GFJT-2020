using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_DS_Basis.DBEngine;
using QRST_DI_DS_MetadataQuery;
using System.Threading.Tasks;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucDataMaintainer : DevExpress.XtraEditors.XtraUserControl
    {
        private DataTable dt = null;
        bool isSqlLite = false;
        private List<int> deleteIndex = new List<int>();
        private List<string> strName = new List<string>();
        private List<List<string>> sqlLiteInfo = new List<List<string>>();
        string ChooseDataColumnCaption = "选择数据";
        MySql.Data.MySqlClient.MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
        MySql.Data.MySqlClient.MySqlCommand cmd;
        string tableName;
        localhostSqlite.Service service = new localhostSqlite.Service();
        int count;
        public mucDataMaintainer()
        {
            InitializeComponent();
            try
            {
                FieldViewBasedQuerySchema queryInfo = (FieldViewBasedQuerySchema)ruc3DSearcher.querySchema;
                conn.ConnectionString = queryInfo.baseUtilities.connString;
                tableName = queryInfo.tableName;
                conn.Open();
                cmd = new MySql.Data.MySqlClient.MySqlCommand();
                cmd.Connection = conn;
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
                else if (i != gridView1.Columns.Count - 1)
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
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((bool)dt.Rows[i]["选择数据"])
                {
                    deleteIndex.Add(i);
                    if (dt.Columns.Contains("数据名称"))
                    {
                        isSqlLite = false;
                        strName.Add(dt.Rows[i]["数据名称"].ToString());
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
                    }
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
                string sql = string.Format("delete from {0} where Name ='{1}'", tableName, strName[n]);
                cmd.CommandText = sql;
                cmd.ExecuteNonQuery();
            }
            else
            {
                List<string> rowList = sqlLiteInfo[n];
                string sql = string.Format("delete from correctedTiles where DataSourceID ='{0}' and Satellite ='{1}' and Sensor ='{2}' and Date ='{3}' and Level ='{4}' and Row ='{5}' and Col ='{6}' and type ='{7}'", rowList[1], rowList[2], rowList[3], rowList[4], rowList[5], rowList[6], rowList[7], rowList[8]);
                service.ExecuteNonQuery(sql);
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
                        this.ExcuteDelete(i);
                    }
                });
                tasks[1] = new Task(() =>
                {
                    for (int i = TaskNum; i < TaskNum * 2; i++)
                    {
                        this.ExcuteDelete(i);
                    }
                });
                tasks[2] = new Task(() =>
                {
                    for (int i = TaskNum * 2; i < TaskNum * 3; i++)
                    {
                        this.ExcuteDelete(i);
                    }
                });
                tasks[3] = new Task(() =>
                {
                    for (int i = TaskNum * 3; i < TaskNum * 4; i++)
                    {
                        this.ExcuteDelete(i);
                    }
                });
                tasks[4] = new Task(() =>
                {
                    for (int i = TaskNum * 4; i < TaskNum * 5; i++)
                    {
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
                    this.ExcuteDelete(i);
                }
            }
            conn.Close();
            strName.Clear();
            sqlLiteInfo.Clear();
            MessageBox.Show("数据删除成功！");
        }
    }
}