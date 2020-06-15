using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using QRST_DI_TS_Process.Orders;
using QRST_DI_TS_Process.Tasks;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmAddOrderPara : DevExpress.XtraEditors.XtraForm
    {
        private static FrmAddOrderPara instance = null;
        private bool isFirstLoad = true;

        public OrderClass orderClass;
        int preIndex = -1;      //记录listBox的前一个索引号
        string[][] tasksPara ; 
        //构造订单列表数据源
        DataTable dtOrderPara;
        //任务流参数列表数据源
        DataTable dtTaskPara;

        public FrmAddOrderPara()
        {
            InitializeComponent();

 
        }

        private void FrmAddOrderPara_Load(object sender, EventArgs e)
        {
            if (!isFirstLoad)
                return;

            //构造订单列表数据源
            dtOrderPara = ConstructOrderParaDataSource();
            gridControlOrder.DataSource = dtOrderPara;

            tasksPara = new string[orderClass.Tasks.Count][];
            //构造任务列表
            for (int i = 0 ; i < orderClass.Tasks.Count ; i++)
            {
                if (orderClass.Tasks[i] != null)
                    listBoxTask.Items.Add(orderClass.Tasks[i].Description);
            }

            //如果订单类型为installed类型，则不需要输入任务参数
            groupBox2.Enabled = false;
            isFirstLoad = false;
        }

        /// <summary>
        /// 确定参数设置，收集参数信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            gridView1.CloseEditor();
            gridView1.UpdateCurrentRow();

            if (orderClass.Type != EnumOrderType.Installed)
            {
                gridView2.CloseEditor();
                gridView2.UpdateCurrentRow();
                //收集任务参数信息
                List<string[]> tasksParaLst = new List<string[]>();
                for (int i = 0 ; i < tasksPara.Length ; i++)
                {
                    tasksParaLst.Add(tasksPara[i]);
                }
                orderClass.TaskParams = tasksParaLst;
            }
            //收集订单参数信息
            string[] orderPara = new string[dtOrderPara.Rows.Count];
            for (int j = 0 ; j < dtOrderPara.Rows.Count ;j++ )
            {
                orderPara[j] = dtOrderPara.Rows[j]["ParaValue"].ToString();
            }
            orderClass.OrderParams = orderPara;
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private DataTable ConstructDataSource()
        {
            DataTable dt = new DataTable();
            DataColumn dc = new DataColumn() { ColumnName = "ParaName", DataType = Type.GetType("System.String") };
            dt.Columns.Add(dc);
            DataColumn dc1 = new DataColumn() { ColumnName = "ParaValue", DataType = Type.GetType("System.String") };
            dt.Columns.Add(dc1);
            return dt;
        }
        
        /// <summary>
        /// 构造订单参数数据源
        /// </summary>
        /// <returns></returns>
        private DataTable ConstructOrderParaDataSource()
        {
            DataTable dtOrderPara = ConstructDataSource();
            if (orderClass != null)
            {
                for (int i = 0 ; i < orderClass.OrderParams.Length ; i++)
                {
                    DataRow dr = dtOrderPara.NewRow();
                    dr["ParaName"] = orderClass.OrderParams[i];
                    dtOrderPara.Rows.Add(dr);
                }
            }
            return dtOrderPara;
        }

        /// <summary>
        /// 构造任务参数数据源
        /// </summary>
        /// <returns></returns>
        private DataTable ConstructTaskParaDataSource(TaskClass taskClass)
        {
            DataTable dtTaskPara = ConstructDataSource();
            if (taskClass != null)
            {
                string[] paras = taskClass.paraMemo.Split(",".ToCharArray(),StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0 ; i < paras.Length ;i++ )
                {
                    DataRow dr = dtTaskPara.NewRow();
                    dr["ParaName"] = paras[i];
                    dtTaskPara.Rows.Add(dr);
                }
            }

            return dtTaskPara;
        }

        /// <summary>
        /// 切换不同的任务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            //将输入的数据写入参数数组 
           if (preIndex != -1)
          {
               if (tasksPara[preIndex] == null)
               {
                   tasksPara[preIndex] = new string[dtTaskPara.Rows.Count];
               }
               for (int i = 0 ; i < dtTaskPara.Rows.Count ;i++ )
               {
                   tasksPara[preIndex][i] = dtTaskPara.Rows[i]["ParaValue"].ToString();
               }
           }
           //构造任务参数列表
           dtTaskPara = ConstructTaskParaDataSource(orderClass.Tasks[listBoxTask.SelectedIndex]);
           gridControlTaskPara.DataSource = dtTaskPara;

           if (tasksPara[listBoxTask.SelectedIndex] != null)
            {
                for (int i = 0 ; i < dtTaskPara.Rows.Count ;i++ )
                {
                    dtTaskPara.Rows[i]["ParaValue"] = tasksPara[listBoxTask.SelectedIndex][i];
                }
            }

            preIndex = listBoxTask.SelectedIndex;
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DataTable dt = gridControlOrder.DataSource as DataTable;
            DataRow dr = dt.Rows[gridView1.FocusedRowHandle];
            if (dr["ParaName"].ToString().Trim() == "元数据信息")
            {
                switch (orderClass.OrderName)
                {
                    case "IORasterDataImport":
                        FrmMetEdit rasterfrmMetEdit = new FrmMetEdit("BSDB-R");
                        if (rasterfrmMetEdit.ShowDialog() == DialogResult.OK)
                        {
                            dtOrderPara.Rows[gridView1.FocusedRowHandle][1] = rasterfrmMetEdit.Metadata;
                        }
                        break;
                    case "IOVectorImport":
                        FrmMetEdit vectorfrmMetEdit = new FrmMetEdit("BSDB-V");
                        if (vectorfrmMetEdit.ShowDialog() == DialogResult.OK)
                        {
                            dtOrderPara.Rows[gridView1.FocusedRowHandle][1] = vectorfrmMetEdit.Metadata;
                        }
                        break;

                }

            }
        }


        /// <summary>
        /// 检核参数输入情况
        /// </summary>
        /// <returns></returns>
        //bool Check()
        //{
        //    for (int i = 0 ; i < tasksPara.Length ;i++ )
        //    {
        //        if (tasksPara[i] == null&& tasksPara[i] )
        //        {
        //            return false;
        //        }
        //    }
        //}

    }
}