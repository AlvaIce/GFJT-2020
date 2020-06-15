using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using QRST_DI_MS_Component.Common;
using QRST_DI_Resources;
using QRST_DI_MS_Basis.Taskinfo;
using System.Configuration;


namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmTaskDistribution : Form
    {
        public metadatacatalognode_Mdl selectedQueryObj;
        public QueryPara queryPara;
        public List<string> lst = new List<string>();
        public List<string> datalst = new List<string>();
        private string dataPath;

        public FrmTaskDistribution()
        {
            InitializeComponent();

        }

        public FrmTaskDistribution(DataTable dt, metadatacatalognode_Mdl _selectedQueryObj, QueryPara _queryPara)
        {
            selectedQueryObj = _selectedQueryObj;
            queryPara = _queryPara;
            InitializeComponent();
            setGridControl(dt);
        }

        //动态加载子系统的服务分发模块
        private void radioGroup1_TabIndexChanged(object sender, EventArgs e)
        {
            int tabIndex = radioGroup1.SelectedIndex;
            checkedListBox.Items.Clear();
            taskName.Text = "";
            switch (tabIndex)
            {
                case 0: 
                    checkedListBox.Items.Add("unchecked", "道路网提取");
                    checkedListBox.Items.Add("unchecked", "道路附属设施信息提取");
                    break;
                case 1:
                    checkedListBox.Items.Add("unchecked", "道路三维建模");
                    checkedListBox.Items.Add("unchecked", "公路设计多因素综合评价");
                    checkedListBox.Items.Add("unchecked", "不良地质体时空分布分析");
                    break;
                case 2:
                    checkedListBox.Items.Add("unchecked", "道路灾害损毁评估模块");
                    checkedListBox.Items.Add("unchecked", "交通用地监测模块");
                    checkedListBox.Items.Add("unchecked", "公路灾害多源立体监测模块");
                    break;
                case 3:
                    checkedListBox.Items.Add("unchecked", "城市路网运行指标评价");
                    checkedListBox.Items.Add("unchecked", "路域能见度监测");
                    checkedListBox.Items.Add("unchecked", "居民出行分布预测");
                    break;
                case 4:
                    checkedListBox.Items.Add("unchecked", "冲刷岸区提取和多时相分析");
                    checkedListBox.Items.Add("unchecked", "基于地面控制点和水上浮动控制点的水位线提取");
                    checkedListBox.Items.Add("unchecked", "基于高光谱影像的含沙量反演");
                    checkedListBox.Items.Add("unchecked", "面向水上交通事故调查的船舶自动分类识别");
                    checkedListBox.Items.Add("unchecked", "有害物质扩散范围监测");
                    break;
                case 5:
                    checkedListBox.Items.Add("unchecked", "机场基础设施信息提取");
                    checkedListBox.Items.Add("unchecked", "机场环境参量信息提取");
                    break;
                case 6:
                    checkedListBox.Items.Add("unchecked", "高分交通计算服务运行管理");
                    checkedListBox.Items.Add("unchecked", "高分交通计算服务流建模");
                    checkedListBox.Items.Add("unchecked", "高分交通多元数据综合检索");
                    checkedListBox.Items.Add("unchecked", "高分交通数据多尺度无缝集成");
                    checkedListBox.Items.Add("unchecked", "高分交通统一专题图制图"); break;
                default: break;
            }
        }

        //动态更新任务名称
        private void checkedListBox_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            for (int i = 0; i < checkedListBox.Items.Count; i++)
            {
                if (checkedListBox.GetItemChecked(i))
                {
                    taskName.Text = checkedListBox.GetItemText(i);
                    break;
                }
            }
        }

        //初始化设置已选择数据表格内容
        public void setGridControl(DataTable dt)
        {
            gridControl1.DataSource = null;
            gridView1.Columns.Clear();

            gridControl1.DataSource = dt;
            gridView1.Columns.ColumnByFieldName("选择数据").Visible = false;
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                if (i < 10)
                {
                    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                    gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else if (i != gridView1.Columns.Count - 1)
                {
                    gridView1.Columns[i].Visible = false;
                }
            }
        }

        //取消
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //确认
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (checkedListBox.SelectedIndex == -1)
            {
                MessageBox.Show("没有选择要分发的模块！");
                return;
            }
            else if (taskName.Text == "" || taskName.Text == null)
            {
                MessageBox.Show("请填写任务名称！");
                return;
            }

            distributionData();
            executeTask();
            
            this.Close();
        }

        //分发数据列表
        private void distributionData()
        {
            DataTable dt = (DataTable)gridControl1.DataSource;
            gridView1.CloseEditor();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (selectedQueryObj.GROUP_TYPE.ToUpper() == "SYSTEM_NORMALFILE")
                {
                    lst.Add(dt.Rows[i][queryPara.QRST_CODE].ToString() + "#" + dt.Rows[i]["文件名称"].ToString());
                    datalst.Add(dt.Rows[i]["数据名称"].ToString());
                }
                else
                {
                    lst.Add(dt.Rows[i][queryPara.QRST_CODE].ToString());
                    datalst.Add(dt.Rows[i]["数据名称"].ToString());
                    //datalst.Add(dt.Rows[i][0].ToString());
                }
            }
        }
        
        //执行分发任务
        private void executeTask()
        {
            try
            {
                taskinfo model = new taskinfo();
                int index = radioGroup1.SelectedIndex;
                string taskPath;
                string SubTaskDataPath = ConfigurationSettings.AppSettings["SubTaskDataPath"];
                string tempPath = "\\\\" + SubTaskDataPath + "\\gfjtdata\\子系统任务分配\\" + radioGroup1.Properties.Items[index].Description + "\\";
                int taskcount = 0;
                string sourcename = "";

                for (int m = 0; m < datalst.Count; m++)
                {
                    sourcename = sourcename + datalst[m] + ";";
                }

                for (int i = 0; i < checkedListBox.Items.Count; i++)
                {
                    if (checkedListBox.GetItemChecked(i))
                    {
                        taskcount++;
                        string mid = model.MethodID(checkedListBox.GetItemText(i));
                        string sid = model.Add(taskName.Text, index + 1, Convert.ToInt32(mid), sourcename, tempPath,taskDescription.Text);
                        taskPath = tempPath + sid + taskName.Text;

                        if (taskcount == 1)//sid != null)
                        {
                            dataPath = taskPath;
                            FrmDownLoadLst.TGetInstance().Show();
                            FrmDownLoadLst.TGetInstance().AddDownLoadTaskThreads(lst, taskPath);
                            createTXT(taskPath);
                        }
                        else
                        {
                            createTXT(taskPath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyConsole.WriteLine("任务分发失败:" + "###Exception:" + ex.Message);
                MessageBox.Show("任务分发失败！");
            }
        }

        //创建数据说明txt
        private void createTXT(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            FileStream fileStream = new FileStream(path + "\\数据说明.txt", FileMode.Create);
            StreamWriter streamWriter = new StreamWriter(fileStream);
            streamWriter.WriteLine("任务数据存储路径："+ dataPath);
            streamWriter.WriteLine("");
            streamWriter.WriteLine("任务数据列表：");
            for (int i = 0; i < datalst.Count; i++)
            {
                streamWriter.WriteLine("数据"+Convert.ToString(i+1)+"：" + datalst[i]);
            }
            streamWriter.Flush();
            streamWriter.Close();
            fileStream.Close();
        }
    }
}
