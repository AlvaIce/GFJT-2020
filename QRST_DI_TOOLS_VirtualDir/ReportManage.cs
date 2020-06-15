using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.AdvTree;
using WindowsFormsApplication2.UI.Report;
using QRST_DI_MS_Basis.VirtualDir;


namespace WindowsFormsApplication2
{
    //
    //public partial class ReportManage : UserControl
    //{
    //    public static ReportManage instance;
    //    public static ReportManage GetInstance()
    //    {
    //        if (instance == null)
    //        {
    //            instance = new ReportManage();
    //        }
    //        return instance;
    //    }
    //    public static WindowsFormsApplication2.UI.Report.ReportBodyUCReportDisplay TemplateReportDisplay
    //    {
    //        get { return templateReportDetailDspl; }
    //    }
    //    public static WindowsFormsApplication2.UI.Report.ReportBodyUCReportDisplay UserReportDisplay
    //    {
    //        get { return userReportDetailDspl; }
    //    }
        
        
       
        

    //    public ReportManage()
    //    {
    //        InitializeComponent();
    //    }
    //    //在连接数据库后，获取当前用户的姓名
    //    public void Initialize(TreeNode roottn)//一个用户的时候使用这个方法
    //    {
    //        TemplateReportDisplay.targetDir = roottn.Tag.ToString();
    //        TemplateReportDisplay.UpdateContent(roottn);
           
    //    }
    //    public void InitializeUserReportDisplay(TreeNode roottn)//两个用户的时候需要使用这个方法
    //    {


    //        userReportDetailDspl.UpdateContent(roottn);
    //    }
    //    public void showPanel2(String username)
    //    {
    //        userReportDetailDspl = new WindowsFormsApplication2.UI.Report.ReportBodyUCReportDisplay();
    //        this.splitContainer1.Panel2.Controls.Add(userReportDetailDspl);
    //        this.splitContainer1.Panel2Collapsed = false;
    //        userReportDetailDspl.AutoScroll = true;
    //        userReportDetailDspl.AutoSize = true;
    //        userReportDetailDspl.Dock = System.Windows.Forms.DockStyle.Fill;
    //        userReportDetailDspl.Location = new System.Drawing.Point(0, 0);
         
    //        userReportDetailDspl.Size = new System.Drawing.Size(345, 452);
    //        userReportDetailDspl.Title = "Child";
    //        this.splitContainer1.Panel2.ResumeLayout(false);
    //        this.splitContainer1.Panel2.PerformLayout();
    //        userReportDetailDspl.setGrouPanleName(username);
    //    }
    //    public static WindowsFormsApplication2.UI.Report.ReportBodyUCReportDisplay getClass(string ordername)
    //    {
    //        if (ordername == "Root")
    //        {
    //            return TemplateReportDisplay;
    //        }
    //        else
    //        {
    //            return UserReportDisplay;
    //        }

    //    }
        
    //}
    public partial class ReportManage : UserControl
    {
        public static List<Node> commonCheckNode = new List<Node>();
        public static bool _MoveFlag { get; set; }
        public static ReportBodyUCReportDisplay reportUC;

        public static ReportBodyUCReportDisplay TemplateReportDisplay
        {
            get { return templateReportDetailDspl; }
        }
        public static ReportBodyUCReportDisplay UserReportDisplay
        {
            get { return userReportDetailDspl; }
        }

        public ReportManage()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 单用户登录更新自己的树
        /// </summary>
        /// <param name="roottn"></param>
        public void Initialize(TreeNode roottn)
        {
            TemplateReportDisplay.UpdateContent(roottn);
        }
        /// <summary>
        /// 第二个用户登录更新自己的树
        /// </summary>
        /// <param name="roottn"></param>
        public void InitializeUserReportDisplay(TreeNode roottn)
        {
            userReportDetailDspl.UpdateContent(roottn);
        }
        /// <summary>
        /// 将panel2显示出来
        /// </summary>
        /// <param name="username"></param>
        public void showPanel2(String username)
        {
            userReportDetailDspl = new WindowsFormsApplication2.UI.Report.ReportBodyUCReportDisplay();
            this.splitContainer1.Panel2.Controls.Add(userReportDetailDspl);
            this.splitContainer1.Panel2Collapsed = false;
            userReportDetailDspl.AutoScroll = true;
            userReportDetailDspl.AutoSize = true;
            userReportDetailDspl.Dock = System.Windows.Forms.DockStyle.Fill;
            userReportDetailDspl.Location = new System.Drawing.Point(0, 0);
            userReportDetailDspl.Size = new System.Drawing.Size(345, 452);
            userReportDetailDspl.Title = "Child";
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            userReportDetailDspl.setGrouPanleName(username);
        }
        /// <summary>
        /// 将panel2隐藏起来
        /// </summary>
        public void setPanel2Hide()
        {
            userReportDetailDspl = null;
            this.splitContainer1.Panel2Collapsed = true;
        }
        public static void multiUserCopy()
        {
            if (UserReportDisplay != null)
            {
                judgePanelSelectedNode();
            }
            reportUC = judgeBelongPanle();
            reportUC.Copy();
            reportUC.str = reportUC.Title;
            reportUC.UpdateTreeContent();
        }
        public static void multiUserPaste()
        {
            if (ReportManage.commonCheckNode != null)
            {
                reportUC = ReportManage.judgeBelongPanle();
                List<Node> checkednode = ReportManage.commonCheckNode;
                if (ReportManage._MoveFlag)
                {
                    reportUC.MoveDir();
                    _MoveFlag = false;
                }
                else reportUC.Paste();
            }
            else
            {
                MessageBox.Show("请先选复制或移动！");
            }
        }
        public static void multiUserMove()
        {
            if (UserReportDisplay != null)
            {
                ReportManage.judgePanelSelectedNode();
            }
            reportUC = judgeBelongPanle();
            reportUC.Move();
            _MoveFlag = true;
            reportUC.str = reportUC.Title;
            reportUC.UpdateTreeContent();

        }
        public static void multiUserRename()
        {
            judgeBelongPanle().ReNameDir();
        }
        public static void multiUserDelete()
        {
            judgeBelongPanle().DeleteDir();
        }
        public static void multiUserNewFile()
        {
            judgeBelongPanle().NewDir();
        }
        public static void multiUserRefresh()
        {
            judgeBelongPanle().Refresh();
        }
        /// <summary>
        /// 多用户登录的时候，只能保证一个面板上的Node
        /// 被选中，才能进行复制、粘贴、移动
        /// </summary>
        /// <returns></returns>
        public static bool judgePanelSelectedNode()
        {
            bool var = false;
            if (UserReportDisplay.advTree1.SelectedNode != null && TemplateReportDisplay.advTree1.SelectedNode != null)
            {
                MessageBox.Show("请确保只有一个用户中的节点被选择！");
                TemplateReportDisplay.UpdateTreeContent();
                UserReportDisplay.UpdateTreeContent();
            }
            else var = true;
            return var;
        }
        /// <summary>
        ///  判断当前选中的节点
        ///  属于哪一个面板
        /// </summary>
        /// <returns></returns>
        public static ReportBodyUCReportDisplay judgeBelongPanle()
        {
            if (UserReportDisplay != null)
            {
                if (TemplateReportDisplay.advTree1.SelectedNode != null)
                {
                    return TemplateReportDisplay;
                }
                else
                {
                    return UserReportDisplay;
                }
            }
            else
            {
                return TemplateReportDisplay;
            }

        }
        /*12.12 删除*/
        //public static List<Node> getClass(string ordername)
        //{
        //    if (ordername == "Root")
        //    {
        //        return templateReportDetailDspl.checkednode;
        //    }
        //    else
        //    {
        //        return userReportDetailDspl.checkednode;
        //    }

        //}

        /// <summary>
        /// 判断当前系统上有几个用户登录
        /// </summary>
        /// <returns></returns>
        //public static AdvTree getRootNode(string ordername)
        //{
        //    if (ordername == "Root")
        //    {
        //        return templateReportDetailDspl.advTree1;

        //    }
        //    else
        //    {
        //        return userReportDetailDspl.advTree1;
        //    }

        //}
        /*12.12 删除*/
        //public static bool judgeCurrentUserNum()
        //{
        //    bool var = false;
        //    if (userReportDetailDspl != null) var = true;
        //    return var;

        //}
        /*12.12 删除*/
        //public static VirtualDirEngine getVirtualDirEngine(string title)
        //{
        //    VirtualDirEngine virtualdir = null;
        //    if (title == "Root")
        //    {
        //        virtualdir = _vde;
        //    }
        //    else
        //    {
        //        virtualdir = Ribbon._VirtualDir;
        //    }
        //    return virtualdir;

        //}
        /*12.12 删除*/
        //public static string getOtherUserName(string title)
        //{
        //    if (title == "Root")
        //    {
        //        return userReportDetailDspl.user;
        //    }
        //    else
        //    {
        //        return templateReportDetailDspl.user;
        //    }


        //}
       /*12.12 删除*/
        //public static void updateOtherPanle(string title)
        //{
        //    if (title == "Root")
        //    {
        //        userReportDetailDspl.UpdateContent(getVirtualDirEngine("Child").List());
        //    }
        //    else
        //    {
        //        templateReportDetailDspl.UpdateContent(getVirtualDirEngine("Root").List());
        //    }

        //}
       /*12.12删除*/
        //public static AdvTree getAdvtree(string title)
        //{ 
        //    if (title == "Root")
        //    {
        //        return userReportDetailDspl.advTree1;
        //    }
        //    else
        //    {
        //        return templateReportDetailDspl.advTree1;
        //    }

        //}
         

    }
}
