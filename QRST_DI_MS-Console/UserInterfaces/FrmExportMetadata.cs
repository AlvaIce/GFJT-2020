using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.IO;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class FrmExportMetadata : DevExpress.XtraEditors.XtraForm
    {
        public FrmExportMetadata(DataTable _dt)
        {
            InitializeComponent();
            dt = _dt;
            if (dt != null)
            {
                for (int i = 0; i < dt.Columns.Count;i++ )
                {
                    checkedListBoxColumns.Items.Add(dt.Columns[i].ColumnName, dt.Columns[i].ColumnName, CheckState.Checked, true);
                }
            }
            else
            {
                checkedListBoxColumns.Items.Clear();
            }
        }

        private DataTable dt = null;

        private void btnSelectPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "文本文件(*.txt)|*.txt";
            if(sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtStorePath.Text = sfd.FileName;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            checkedListBoxColumns.CheckAll();
        }

        private void btnUnselectAll_Click(object sender, EventArgs e)
        {
            checkedListBoxColumns.UnCheckAll();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                  if(string.IsNullOrEmpty(txtStorePath.Text))
            {
                MessageBox.Show("请选择导出文件！");
                return;
            }
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("没有导出的记录！");
                return;
            }
            else if(checkedListBoxColumns.CheckedItemsCount == 0)
            {
                MessageBox.Show("请选择需要导出的列！");
                return;
            }

            
            progressBarControl1.Properties.Maximum = dt.Rows.Count-1;
            progressBarControl1.Properties.Maximum = 0;



            FileStream fs = new FileStream(txtStorePath.Text, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            StringBuilder header = new StringBuilder();
            for (int i = 0; i < checkedListBoxColumns.CheckedItemsCount; i++)
            {
                header.AppendFormat("{0}{1}", checkedListBoxColumns.CheckedItems[i].ToString(),radioGroupDivideChar.Properties.Items[radioGroupDivideChar.SelectedIndex]);
            }
            sw.WriteLine(header.ToString());

             for (int i = 0; i < dt.Rows.Count; i++)
             {
                 lblExportInfo.Text = string.Format("导出记录数：{0}条，共{1}条记录！", i+1, dt.Rows.Count);
                 progressBarControl1.EditValue = i;
                 StringBuilder row = new StringBuilder();
                 for (int j = 0; j < checkedListBoxColumns.CheckedItemsCount; j++)
                 {
                     row.AppendFormat("{0}{1}", dt.Rows[i][checkedListBoxColumns.CheckedItems[j].ToString()].ToString(), radioGroupDivideChar.Properties.Items[radioGroupDivideChar.SelectedIndex]);
                 }
                 sw.WriteLine(row.ToString());
             }
             sw.Close();
             fs.Close();
            }
            catch(Exception ex)
            {
                lblExportInfo.Text = "数据导出出错:" + ex.ToString();
            }

        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //public void ExportMetadata(string filename)
        //{
        //    DataTable dt = (DataTable)gridControlDataList.DataSource;
        //    if (dt == null || dt.Rows.Count == 0)
        //    {
        //        MessageBox.Show("没有需要导出的元数据信息！");
        //        return;
        //    }
        //    else
        //    {
        //        FileStream fs = new FileStream(filename, FileMode.Create);
        //        StreamWriter sw = new StreamWriter(fs);
        //        StringBuilder header = new StringBuilder();
        //        for (int i = 0; i < dt.Columns.Count; i++)
        //        {
        //            header.AppendFormat("{0}#;#", dt.Columns[i].ColumnName);
        //        }
        //        sw.WriteLine(header.ToString());
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            StringBuilder row = new StringBuilder();
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                row.AppendFormat("{0}#;#", dt.Rows[i][j].ToString());
        //            }
        //            sw.WriteLine(row.ToString());
        //        }
        //        sw.Close();
        //        fs.Close();

        //    }
        //}
    }
}