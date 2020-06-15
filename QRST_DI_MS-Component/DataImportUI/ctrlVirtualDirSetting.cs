using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_MS_Component.VirtualDirUI;
using QRST_DI_MS_Basis.VirtualDir;
 
namespace QRST_DI_MS_Component.DataImportUI
{
    public partial class ctrlVirtualDirSetting : UserControl
    {
        public bool UsingVirtualDir { get; private set; }
     
        public VirtualDirSelector _vdSelector
        {
            get;
            private set;
        }
        public string SelectedVDPath { get { return _vdSelector.VDCode; } }
        public VirtualDirEngine _vdEngine;
        public bool IsCreated { get; private set; }
        private string _user;
        private string _pwd;
        public ctrlVirtualDirSetting()
        {
           
            InitializeComponent();
            UsingVirtualDir = false;
            textBox2.Enabled = false;
            textBox2.Text = "";
            _vdSelector = new VirtualDirSelector();
        }

        public void Create(string user, string password)
        {
            _user = user;
            _pwd = password;


            if (_vdSelector == null)
            {
                _vdSelector = new VirtualDirSelector();
            }
            if (!_vdSelector.IsCreated)
            {
                _vdSelector.Create(user, password);
            }
        }

        private void chk_DirImportMode_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = chk_DirImportMode.Checked;
            UsingVirtualDir = chk_DirImportMode.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_vdSelector.ShowSelectorForm() == DialogResult.OK)
            {
                textBox2.Text = _vdSelector.VDPath;
            }
        }

        public void AddFileLink(string filecode)
        {
            if (_vdEngine == null)
            {
                _vdEngine = new VirtualDirEngine();
            }
            if (!_vdEngine.IsRegister)
            {
                _vdEngine.Register(_user, _pwd);                
            }
            _vdEngine.AddFileLink(_vdSelector.VDCode, filecode);
        }

        public void AddFileLink(List<string> filecodes)
        {
            if (_vdEngine == null)
            {
                _vdEngine = new VirtualDirEngine();
            }
            _vdEngine.AddFileLink(_vdSelector.VDCode, filecodes);
        }


        public bool CheckValue()
        {
            if (QRST_DI_MS_Basis.VirtualDir.VirtualDirEngine.IsExistDir(_vdSelector.VDCode))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //2017.3.9判断是否从右键菜单进去并传入相应的路径
        public void changeCtrlVirtualStatus(string name,string target)
        {

            _vdSelector.VDCode = target;
            this.chk_DirImportMode.Checked = true;
            this.textBox2.Text = name;
            this.button1.Enabled = true;
            this.UsingVirtualDir = true;
            this.Refresh();
        }
    }
}
