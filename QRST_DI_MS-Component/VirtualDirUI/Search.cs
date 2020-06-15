using System;

namespace QRST_DI_MS_Component.VirtualDirUI
{
    public partial class SearchFile : DevComponents.DotNetBar.Office2007Form
    {
         
        public string searchInfo{get;set;}
         
        public SearchFile()
        {
           
            InitializeComponent();
             this.labelControl1.Visible = false;
        }
        private void buttonX2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonX1_Click(object sender, EventArgs e)
        {
            this.searchInfo = this.textEdit1.Text.Trim();
            this.Close();
         
          
        }


     

        private void textEdit1_TextChanged_1(object sender, EventArgs e)
        {
            if (this.textEdit1.Text == "" || this.textEdit1.Text == null)
            {
                this.labelControl1.Visible = true;
            }
            else this.labelControl1.Visible = false;

        }


    }
}