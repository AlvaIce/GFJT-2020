using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_TS_Process.Tasks;
using System.IO;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using System.Reflection;
using QRST_DI_DS_Metadata.Paths;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class FrmTaskDef : DevExpress.XtraEditors.XtraForm
    {
        private taskdef taskClass;
        public delegate void AddFinishedDel();
        public AddFinishedDel taskFinishedEvent;       //添加或者更新完成后执行

        public FrmTaskDef()
        {
            InitializeComponent();
            labelControlDes.Text = "创建一个新的自定义数据入库组件";
        }

        public FrmTaskDef(taskdef _taskclass)
        {
            InitializeComponent();
            taskClass = _taskclass;
            labelControlDes.Text = "更新数据入库组件";
            DisplayTask();
        }

        private void FrmTaskDef_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < TheUniversal.subDbLst.Count; i++)
            {
                comboBoxEditImportDataType.Properties.Items.AddRange(TheUniversal.subDbLst[i].metadatacatalognode.GetCatalogGroup(" group_type <> 'System_DataSet' and data_code <> ''").ToArray());
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (Check())
            {
                if (taskClass == null)   //添加新组件
                {
                    //注册组件
                    //将组件导入到文件服务器，共享给站点
                    taskClass = GetData();
                    taskdef.AddtaskDef(taskClass);
                    //创建组件描述文件
                    taskClass.CreateDllDesFile(textEditDllPath.Text);
                    string srcDir = Path.GetDirectoryName(textEditDllPath.Text);
                    string destDir = StoragePath.GetPluginDir(textEditDllPath.Text);
                    if (Directory.Exists(destDir))
                    {
                        Directory.Delete(destDir);
                    }
                    Directory.CreateDirectory(destDir);
                    //拷贝组件
                    string[] files = Directory.GetFiles(srcDir);
                    for (int i = 0; i < files.Length; i++)
                    {
                        File.Copy(files[i], string.Format(@"{0}\{1}", destDir, Path.GetFileName(files[i])));
                    }
                }
                else   //更新组件
                {

                }
                if (taskFinishedEvent != null)
                {
                    taskFinishedEvent();
                }
                this.Close();
            }
            else
            {
                XtraMessageBox.Show(labelControlMsg.Text);
            }
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// 查找组件路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnChooseDllPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "(数据入库组件*.dll)|*.dll";
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textEditDllPath.Text = ofd.FileName;
            }
        }

        private taskdef GetData()
        {
            taskdef _taskClass = new taskdef();
            _taskClass.DESCRIPTION = textEditDescription.Text;
            _taskClass.NAME = ((metadatacatalognode_Mdl)comboBoxEditImportDataType.SelectedItem).DATA_CODE;
            _taskClass.ProcessExec = Path.GetFileName(textEditDllPath.Text);
            _taskClass.Type = "Customized";
            StringBuilder parasStr = new StringBuilder();

            for (int i = 0; i < memoEditPara.Lines.Length; i++)
            {
                parasStr.Append(memoEditPara.Lines[i] + ",");
            }

            _taskClass.Params = parasStr.ToString().TrimEnd(",".ToCharArray());
            return _taskClass;
        }

        private void DisplayTask()
        {
            if (taskClass != null)
            {
                textEditDescription.Text = taskClass.DESCRIPTION;
                for (int i = 0; i < comboBoxEditImportDataType.Properties.Items.Count; i++)
                {
                    if (comboBoxEditImportDataType.Properties.Items[i].ToString().Split(":".ToCharArray())[1] == taskClass.NAME)
                    {
                        comboBoxEditImportDataType.SelectedIndex = i;
                        break;
                    }
                }

                string[] paras = taskClass.Params.Split(",".ToCharArray());
                memoEditPara.Text = "";
                memoEditPara.Lines = paras;
            }
        }

        bool Check()
        {
            if (string.IsNullOrEmpty(textEditDescription.Text))
            {
                labelControlMsg.Text = "警告：请填写任务组件描述！";
                return false;
            }
            if (string.IsNullOrEmpty(comboBoxEditImportDataType.Text))
            {
                labelControlMsg.Text = "警告：请选择导入数据类型！";
                return false;
            }
            if (string.IsNullOrEmpty(textEditDllPath.Text))
            {
                labelControlMsg.Text = "警告：请选择导入数据组件！";
                return false;
            }
            //判断是否有重复的组件存在
            if (taskClass == null)  //添加组件，不允许有重复组件存在
            {
                string dllname = Path.GetFileName(textEditDllPath.Text);
                List<taskdef> lst = taskdef.GetTaskdefLst("");
                for (int i = 0; i < lst.Count; i++)
                {
                    if (lst[i].ProcessExec == dllname)
                    {
                        labelControlMsg.Text = string.Format("警告：组件'{0}'已经存在！", dllname);
                        return false;
                    }
                }
            }

            //判断该组件是否实现数据入库的基本接口IDataImport
            Type interfaceType = null;
            try
            {
                Assembly assembil = Assembly.LoadFile(textEditDllPath.Text);
                Type[] types = assembil.GetTypes();
                //查找实现了IDataImport接口的类型

                for (int i = 0; i < types.Length; i++)
                {
                    Type[] interfacetypes = types[i].GetInterfaces();
                    for (int j = 0; j < interfacetypes.Length; j++)
                    {
                        if (interfacetypes[j].Name == "IDataImport")
                        {
                            interfaceType = interfacetypes[j];
                            break;
                        }
                    }
                    if (interfaceType != null)
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                interfaceType = null;
            }

            if (interfaceType == null)
            {
                labelControlMsg.Text = "警告：没能找到实现数据导入接口的组件！";
                return false;
            }
            return true;
        }

    }
}