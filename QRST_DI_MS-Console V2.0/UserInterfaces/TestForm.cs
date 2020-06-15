using System;
using System.Data;
using System.Windows.Forms;

namespace QRST_DI_MS_Desktop.UserInterfaces
{ 
    public partial class TestForm : Form
    {
        DataTable dataTable;
        public TestForm(DataTable _dataTable)
        {
            InitializeComponent();
            dataTable = _dataTable;
            if (dataTable.Rows.Count!=0)
            {
                DataView dv = dataTable.DefaultView;
                dv.Sort = "接收时间";
                dataTable = dv.ToTable();
            }
            BindingSource myBindingSource = new BindingSource();
            myBindingSource.DataSource = dataTable;
            dataGridView1.DataSource = dataTable;
            bindingNavigator1.BindingSource=myBindingSource  ;
        }
        private void TestForm_Load(object sender, EventArgs e)
        {
         
        }

    }
}
