using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmAlgMake : DevExpress.XtraEditors.XtraForm
    {
        public FrmAlgMake()
        {
            InitializeComponent();
        }

        public string jarPath = string.Empty;
        public List<string> args = new List<string>();
        private void simBtn_Browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "\\\\10.10.10.25\\zhsjk";
            
            ofd.Filter = "可执行Jar文件|*.jar";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txt_jarPath.Text = ofd.FileName;
            }
        }

        private void simBtn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void simBtn_Run_Click(object sender, EventArgs e)
        {
            if (txt_jarPath.Text == null || txt_jarPath.Text.Length == 0)
            {
                MessageBox.Show("请输入Jar包路径！");
                return;
            }
            jarPath = txt_jarPath.Text;
            if (memo_args.Text!=null&&memo_args.Text.Length>0)
            {
                string[] inputs = memo_args.Lines;
                foreach (string input in inputs)
                {
                    if (input.Contains(" "))
                    {
                        MessageBox.Show("参数输入不正确！");
                        return;
                    }
                    else if (input.Length == 0)
                        continue;
                    else
                        args.Add(input);
                }
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}