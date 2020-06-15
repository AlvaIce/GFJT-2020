using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using DevComponents.DotNetBar.Controls;
using DevComponents.DotNetBar.Rendering;
using QRST_DI_DS_Basis;
using QRST_DI_DS_Basis.DBEngine;
using MySql.Data.MySqlClient;
using System.Configuration;
using QRST_DI_MS_Basis;
using QRST_DI_MS_Basis.UserRole;
using DevComponents.AdvTree;
using QRST_DI_MS_Basis.VirtualDir;
using WindowsFormsApplication2.UI.Report;


namespace WindowsFormsApplication2
{
    //public partial class Ribbon : DevComponents.DotNetBar.Office2007RibbonForm
    //{
    //    public ReportManage reportmanage = ReportManage.GetInstance();
    //    public string strValue;
    //    public string strNewFile;
    //    public static VirtualDirEngine _VirtualDirEngine;
        
    //    public Ribbon()
    //    {
    //        InitializeComponent();
    //        _VirtualDirEngine = new VirtualDirEngine();
         
    //    }

    //    private void Ribbon_Load(object sender, EventArgs e)
    //    {
            
    //        this.panel1.Controls.Add(reportmanage);
    //        ReportManage.TemplateReportDisplay.CopyBtnVisitableChanged += new EventHandler(TemplateReportDisplay_CopyBtnVisitableChanged);
    //        ReportManage.TemplateReportDisplay.PastBtnVisitableChanged += new EventHandler(TemplateReportDisplay_PastBtnVisitableChanged);

    //        _VirtualDirEngine.Register("admin", "123");
    //        //_VirtualDirEngine.Register("dbadmin", "123");
    //       // _VirtualDirEngine.Register(UserLogin.username,UserLogin.pwd);
    //        TreeNode roottn = _VirtualDirEngine.List();



    //        //后期这些都应该改成动态的，从数据库读取当前目录的根目录
    //        reportmanage.Initialize(roottn);
    //        reportmanage.Dock = DockStyle.Fill;
    //        reportmanage.Visible = true;
         
        
            
    //    }

    //    void TemplateReportDisplay_PastBtnVisitableChanged(object sender, EventArgs e)
    //    {
    //        buttonItem5.Enabled = (bool)sender;
    //    }

    //    void TemplateReportDisplay_CopyBtnVisitableChanged(object sender, EventArgs e)
    //    {
    //        buttonItem2.Enabled = (bool)sender;
    //    }

    //    private void ribbonBar1_ItemClick(object sender, EventArgs e)
    //    {

    //    }
    //    //新建文件夹
    //    private void buttonItem1_Click(object sender, EventArgs e)
    //    {
    //        ReportManage.TemplateReportDisplay.NewDir();
    //    }

       

    //    private void buttonItem7_Click(object sender, EventArgs e)
    //    {
            
    //        UserLogin login = new UserLogin();
    //        login.Owner = this;
    //        login.ShowDialog();
    //        if (strValue != null)
    //        {
    //            reportmanage.showPanel2(strValue);
    //            //后期这些都应该改成动态的，从数据库读取当前目录的根目录
    //            //reportmanage.InitializeUserReportDisplay("E:\\Test1");
    //            _VirtualDirEngine.Register("dbadmin","123");
    //            TreeNode rootnode = _VirtualDirEngine.List();
    //            reportmanage.InitializeUserReportDisplay(rootnode);

    //        }
    //    }
    //    //粘贴文件夹
    //    private void buttonItem3_Click(object sender, EventArgs e)
    //    {
    //        ReportManage.TemplateReportDisplay.Paste();
    //    }
    //    //复制文件夹  需要用按钮形式 复制粘贴吗？？
    //    private void buttonItem2_Click(object sender, EventArgs e)
    //    {
    //        ReportManage.TemplateReportDisplay.Copy();
    //        //ReportManage.TemplateReportDisplay.Paste();
    //    }
    //    //移动文件夹
    //    private void buttonItem4_Click(object sender, EventArgs e)
    //    {
    //        ReportManage.TemplateReportDisplay.MoveDir();
    //    }
    //    //删除文件夹
    //    private void buttonItem5_Click(object sender, EventArgs e)
    //    {
           
    //        ReportManage.TemplateReportDisplay.DeleteDir();
    //    }

    //    private void buttonItem6_Click(object sender, EventArgs e)
    //    {
    //        ReportManage.TemplateReportDisplay.ReNameDir();
    //    }

    //}
    public partial class Ribbon : DevComponents.DotNetBar.Office2007RibbonForm
    {
        public ReportManage reportmanage;
        public string strValue;
        public static VirtualDirEngine _VirtualDirEngine;
        public static VirtualDirEngine _VirtualDir;
        public Ribbon()
        {
            InitializeComponent();
            _VirtualDirEngine = new VirtualDirEngine();

        }

        private void Ribbon_Load(object sender, EventArgs e)
        {
            reportmanage = new ReportManage();
            this.panel1.Controls.Add(reportmanage);
            ReportManage.TemplateReportDisplay.CopyBtnVisitableChanged += new EventHandler(TemplateReportDisplay_CopyBtnVisitableChanged);
            ReportManage.TemplateReportDisplay.PastBtnVisitableChanged += new EventHandler(TemplateReportDisplay_PastBtnVisitableChanged);
            //此处帐号密码应该设置为动态的
            _VirtualDirEngine.Register("admin", "123");
            ReportManage.TemplateReportDisplay.pwd = "123";
            ReportManage.TemplateReportDisplay.user = "admin";
            TreeNode roottn = _VirtualDirEngine.List();

            //后期这些都应该改成动态的，从数据库读取当前目录的根目录
            reportmanage.Initialize(roottn);
            reportmanage.Dock = DockStyle.Fill;
            reportmanage.Visible = true;
        }

        void TemplateReportDisplay_PastBtnVisitableChanged(object sender, EventArgs e)
        {
            buttonItem5.Enabled = (bool)sender;
        }
        void TemplateReportDisplay_CopyBtnVisitableChanged(object sender, EventArgs e)
        {
            buttonItem2.Enabled = (bool)sender;
        }
        private void ribbonBar1_ItemClick(object sender, EventArgs e)
        {

        }
        //新建文件夹
        private void buttonItem1_Click(object sender, EventArgs e)
        {
            ReportManage.multiUserNewFile();
        }
        //刷新20161212
        private void buttonItem7_Click(object sender, EventArgs e)
        {
            ReportManage.multiUserRefresh();
        }
        //粘贴文件夹
        private void buttonItem3_Click(object sender, EventArgs e)
        {
            ReportManage.multiUserPaste();
        }
        //复制文件夹   
        private void buttonItem2_Click(object sender, EventArgs e)
        {
            ReportManage.multiUserCopy();
        }
        //移动文件夹
        private void buttonItem4_Click(object sender, EventArgs e)
        {
            ReportManage.multiUserMove();
        }
        //删除文件夹
        private void buttonItem5_Click(object sender, EventArgs e)
        {
            ReportManage.multiUserDelete();
        }
        //重命名
        private void buttonItem6_Click(object sender, EventArgs e)
        {
            ReportManage.multiUserRename();
        }
         /**12.12 删除/
        //private ReportBodyUCReportDisplay judgeBelongPanle()
        //{
        //    if (ReportManage.UserReportDisplay!=null)
        //    {
        //        if (ReportManage.getAdvtree("Child").SelectedNode != null)
        //        {
        //            return ReportManage.TemplateReportDisplay;
        //        }
        //        else
        //        {
        //            return ReportManage.UserReportDisplay;
        //        }
        //    }
        //    else
        //    {
        //        return ReportManage.TemplateReportDisplay;
        //    }

        //}
        /**12.12 删除*/
        //private bool judgePanelSelectedNode()
        //{
        //    bool var = false;
        //    if (ReportManage.getAdvtree("Child").SelectedNode != null && ReportManage.getAdvtree("Root").SelectedNode != null)
        //    {
        //        MessageBox.Show("请确保只有一个用户中的节点被选择！");
        //        ReportManage.TemplateReportDisplay.UpdateContent(ReportManage.getVirtualDirEngine("Root").List());
        //        ReportManage.UserReportDisplay.UpdateContent(ReportManage.getVirtualDirEngine("Child").List());
        //    }
        //    else
        //    {
        //        var = true;
        //    }
        //    return var;


        //}

        private void buttonItem8_Click(object sender, EventArgs e)
        {
         if (ReportManage.UserReportDisplay!=null)
            {
                this.buttonItem7.Text = "多用户登录";
                reportmanage.setPanel2Hide();
            }
            else
            {
                UserLogin login = new UserLogin();
                login.Owner = this;
                login.ShowDialog();
                if (strValue != null)
                {
                    this.buttonItem7.Text = "多用户退出";
                    reportmanage.showPanel2(strValue);
                    _VirtualDir = new VirtualDirEngine();
                    //此处帐号密码应该设置为动态的
                    _VirtualDir.Register("dbadmin", "123");
                    ReportManage.UserReportDisplay.pwd = "123";
                    ReportManage.UserReportDisplay.user = "dbadmin";
                    TreeNode rootnode = _VirtualDir.List();
                    reportmanage.InitializeUserReportDisplay(rootnode);
                    strValue = null;
                }
            }
         } 
    
    }
        
      
        /*12.12 删除*/
        //
        //private List<Node> getCheckendNode(ReportBodyUCReportDisplay ui)
        //{
        //    List<Node> checkednode = null;
        //    if (ui.Title == "Root") checkednode = ReportManage.getClass("Child");
        //    else checkednode = ReportManage.getClass("Root");
        //    return checkednode;

        }
        //20161212
       



