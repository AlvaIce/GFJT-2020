using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_MS_Component.Common;
using DevComponents.AdvTree;
 
namespace QRST_DI_MS_Component.VirtualDirUI
{
    public partial class VirtualDirUC : UserControl
    {
        public static List<Node> commonCheckNode = new List<Node>();
        /// <summary>
        /// true表示移动，false表示拷贝或者已执行移动粘帖
        /// </summary>
        public static bool _MoveFlag { get; set; }

        public ctrlVirtualDir MainVirtualDirCtrl
        {
            get { return mainVirtualDirCtrl; }
        }
        public ctrlVirtualDir OtherUseVirtualDirCtrl
        {
            get { return otherUseVirtualDirCtrl; }
        }

        public ctrlVirtualDir currentSelectVDCtrl { get; set; }

        public bool IsInitialized { get; set; }

        public VirtualDirUC()
        {
            InitializeComponent();
            currentSelectVDCtrl = null;
            mainVirtualDirCtrl.advTree1.AfterNodeSelect += new AdvTreeNodeEventHandler(mainUseVirtualDirCtrl_AfterNodeSelect);
            IsInitialized = false;
        }

        //在连接数据库后，获取当前用户的姓名
        public void InitializeVirtualDir(string user, string password)//一个用户的时候使用这个方法
        {
            MainVirtualDirCtrl.Initialize(user, password);
            IsInitialized = true;
        }

        /// <summary>
        /// 将panel2显示出来
        /// </summary>
        /// <param name="username"></param>
        public void OpenOtherUserVD(string otheruser, string password)
        {
            if (otherUseVirtualDirCtrl == null)
            {
                otherUseVirtualDirCtrl = new ctrlVirtualDir();

                this.splitContainer1.Panel2.Controls.Add(otherUseVirtualDirCtrl);
                otherUseVirtualDirCtrl.advTree1.AfterNodeSelect += new AdvTreeNodeEventHandler(otherUseVirtualDirCtrl_AfterNodeSelect);
            }

            this.splitContainer1.Panel2Collapsed = false;
            otherUseVirtualDirCtrl.AutoScroll = true;
            otherUseVirtualDirCtrl.AutoSize = true;
            otherUseVirtualDirCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            otherUseVirtualDirCtrl.Location = new System.Drawing.Point(0, 0);
            otherUseVirtualDirCtrl.Size = new System.Drawing.Size(345, 452);
            otherUseVirtualDirCtrl.Title = "Child";
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();


            otherUseVirtualDirCtrl.Initialize(otheruser, password);

            this.Refresh();
        }

        /// <summary>
        /// 需要修改，只对展开的节点进行更新，优化更新速度
        /// </summary>
        public void UpdateVirtualDir()
        {
            if (!IsInitialized)
            {
                return;
            }
          
            InitializeMainVD(MainVirtualDirCtrl._vde.List());
            if (otherUseVirtualDirCtrl != null && otherUseVirtualDirCtrl._vde.IsRegister)
            {
                InitializeOtherUserVD(otherUseVirtualDirCtrl._vde.List());
            }
        }

        /// <summary>
        /// 单用户登录更新自己的树
        /// </summary>
        /// <param name="roottn"></param>
        public void InitializeMainVD(TreeNode roottn)
        {
            MainVirtualDirCtrl.UpdateContent(roottn);
        }
        /// <summary>
        /// 第二个用户登录更新自己的树
        /// </summary>
        /// <param name="roottn"></param>
        public void InitializeOtherUserVD(TreeNode roottn)
        {
            OtherUseVirtualDirCtrl.UpdateContent(roottn);
        }

        void mainUseVirtualDirCtrl_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            currentSelectVDCtrl = mainVirtualDirCtrl;
        }

        void otherUseVirtualDirCtrl_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {
            currentSelectVDCtrl = otherUseVirtualDirCtrl;
        }

        /// <summary>
        /// 将panel2隐藏起来
        /// </summary>
        public void setPanel2Hide()
        {
            this.splitContainer1.Panel2Collapsed = true;
        }

        public void multiUserCopy()
        {
            if (otherUseVirtualDirCtrl != null && currentSelectVDCtrl != null)
            {
                currentSelectVDCtrl.Copy();
                currentSelectVDCtrl.str = currentSelectVDCtrl.Title;
                currentSelectVDCtrl.UpdateTreeContent();
            }
        }
        public  void multiUserPaste()
        {
            if (VirtualDirUC.commonCheckNode != null)
            {
                List<Node> checkednode = VirtualDirUC.commonCheckNode;
                if (currentSelectVDCtrl != null)
                {
                    if (VirtualDirUC._MoveFlag)
                    {
                        currentSelectVDCtrl.MoveDir();
                        _MoveFlag = false;
                    }
                    else currentSelectVDCtrl.Paste();
                }
                else
                {
                    MessageBox.Show("请选择目标节点！");
                }
            }
            else
            {
                MessageBox.Show("请先选复制或移动！");
            }
        }
        public  void multiUserMove()
        {
            if (otherUseVirtualDirCtrl != null && currentSelectVDCtrl != null)
            {
                currentSelectVDCtrl.Move();
                _MoveFlag = true;
                currentSelectVDCtrl.str = currentSelectVDCtrl.Title;
                currentSelectVDCtrl.UpdateTreeContent();
            }
        }
        public  void multiUserRename()
        {
            if (currentSelectVDCtrl != null)
            {
                currentSelectVDCtrl.ReNameDir();
            }
        }
        public  void multiUserDelete()
        {
            if (currentSelectVDCtrl != null)
            {
                currentSelectVDCtrl.DeleteDir();
            }
        }
        public  void multiUserNewFile()
        {
            if (currentSelectVDCtrl != null)
            {
                currentSelectVDCtrl.NewDir();
            }
        }
        public  void multiUserRefresh()
        {
            if (currentSelectVDCtrl != null)
            {
                currentSelectVDCtrl.Refresh();
            }
        }
        ///// <summary>
        ///// 多用户登录的时候，只能保证一个面板上的Node
        ///// 被选中，才能进行复制、粘贴、移动
        ///// </summary>
        ///// <returns></returns>
        //public bool judgePanelSelectedNode()
        //{
        //    bool var = false;
        //    if (UserReportDisplay.advTree1.SelectedNode != null && TemplateReportDisplay.advTree1.SelectedNode != null)
        //    {
        //        MessageBox.Show("请确保只有一个用户中的节点被选择！");
        //        TemplateReportDisplay.UpdateTreeContent();
        //        UserReportDisplay.UpdateTreeContent();
        //    }
        //    else var = true;
        //    return var;
        //}
        /// <summary>
        ///  判断当前选中的节点
        ///  属于哪一个面板
        /// </summary>
        /// <returns></returns>
        public ctrlVirtualDir judgeBelongPanle()
        {
            return currentSelectVDCtrl;
        }

        public void DownloadSelectFile()
        {
            //瓦片数据的filecode是文件名
            List<string> filecodes = mainVirtualDirCtrl.GetDownLoadFileListQRSTCODE();
            if (filecodes != null && filecodes.Count > 0)
            {
                FrmDownLoad frmDownload = new FrmDownLoad(filecodes);
                frmDownload.Show();
            }

        }

        public void AddFileLink(string parentdir, List<string> filecodelst)
        {
            mainVirtualDirCtrl._vde.AddFileLink(parentdir, filecodelst);
        }
    }
}
