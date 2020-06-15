using System;
using System.Collections.Generic;
using QRST_DI_MS_Basis.UserRole;
using DevExpress.XtraEditors;
using System.Windows.Forms;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public class rucUserManager : RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarButtonItem barButtonItemAddUser;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDeleteUser;
        private DevExpress.XtraBars.BarButtonItem barButtonItemPwd;
        private DevExpress.XtraBars.BarEditItem barEditItemUsername;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemLevel;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.BarEditItem barEditItemEmail;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
        private DevExpress.XtraBars.BarEditItem barEditItemAddress;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit4;
        private DevExpress.XtraBars.BarEditItem barEditItemTrueName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit5;
        private DevExpress.XtraBars.BarEditItem barEditItemTelephone;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit6;
        private DevExpress.XtraBars.BarEditItem barEditItemDescription;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemOldPwd;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit7;
        private DevExpress.XtraBars.BarEditItem barEditItemNewPwd;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit8;
        private DevExpress.XtraBars.BarEditItem barEditItemConfirmPwd;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit9;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSaveUser;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAddRole;
        private DevExpress.XtraBars.BarButtonItem barButtonItemSaveRole;
        private DevExpress.XtraBars.BarButtonItem barButtonItemDeleteRole;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup5;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;

        bool _isAddUser = true;      //判断系统是否在添加用户状态下
        bool _isAddRole = true;
        private DevExpress.XtraBars.BarButtonItem barButtonItemModifyPWD;      //判断系统是否在添加角色状态下

        mucUserManager mucuserManager;
        public bool IsAddUser
        {
            get { return _isAddUser; }
            set
            {
                _isAddUser = value;
                if (_isAddUser)
                {
                    ClearUserInfo();  //清空用户信息界面
                    ribbonPageGroup3.Visible = true;
                    //将角色列表全部设为没有选中
                    barEditItemOldPwd.Enabled = false;
                    ribbonPageGroup2.Visible = true;
                }
                else   //加载用户信息，按照用户拥有的角色勾选角色列表中对应角色
                {
                    barEditItemOldPwd.Enabled = true;
                   
                }
               
            }
        }

        public bool IsAddRole
        {
            get { return _isAddRole; }
            set
            {
                _isAddRole = value;
                if (_isAddRole)
                {
                    mucuserManager.ClearRoleInfo();
                    mucuserManager.textEditRoleName.Enabled =true;
                }
                else
                {
                    if (mucuserManager.checkedListBoxControlRole.Items.Count>0)
                    {
                        mucuserManager.checkedListBoxControlRole.SelectedIndex = 0;
                    }
                    mucuserManager.textEditRoleName.Enabled = false;
                }
            }
        }
    
           public rucUserManager()
            : base()
        {
            InitializeComponent();
        }

           public rucUserManager(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
            mucuserManager = (mucUserManager)ObjMainUC;
            rolesInfo.sqlUtilities = TheUniversal.MIDB.sqlUtilities;
            userInfo.sqlUtilities = TheUniversal.MIDB.sqlUtilities;

            mucuserManager.RoleChanged += RoleChanged;
            mucuserManager.FunChecked += FunChecked;
            mucuserManager.UserChanged += userChanged;
        }

           private void InitializeComponent()
           {
               this.barButtonItemAddUser = new DevExpress.XtraBars.BarButtonItem();
               this.barButtonItemDeleteUser = new DevExpress.XtraBars.BarButtonItem();
               this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
               this.barEditItemUsername = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
               this.barEditItemLevel = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
               this.barEditItemTrueName = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemTextEdit5 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
               this.barEditItemTelephone = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemTextEdit6 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
               this.barEditItemAddress = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemTextEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
               this.barEditItemEmail = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
               this.barEditItemDescription = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
               this.barButtonItemPwd = new DevExpress.XtraBars.BarButtonItem();
               this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
               this.barEditItemOldPwd = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemTextEdit7 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
               this.barEditItemNewPwd = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemTextEdit8 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
               this.barEditItemConfirmPwd = new DevExpress.XtraBars.BarEditItem();
               this.repositoryItemTextEdit9 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
               this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
               this.barButtonItemSaveUser = new DevExpress.XtraBars.BarButtonItem();
               this.ribbonPageGroup5 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
               this.barButtonItemAddRole = new DevExpress.XtraBars.BarButtonItem();
               this.barButtonItemDeleteRole = new DevExpress.XtraBars.BarButtonItem();
               this.barButtonItemSaveRole = new DevExpress.XtraBars.BarButtonItem();
               this.barButtonItemModifyPWD = new DevExpress.XtraBars.BarButtonItem();
               ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).BeginInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit9)).BeginInit();
               this.SuspendLayout();
               // 
               // ribbonControl1
               // 
               // 
               // 
               // 
               this.ribbonControl1.ExpandCollapseItem.Id = 0;
               this.ribbonControl1.ExpandCollapseItem.Name = "";
               this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.barButtonItemAddUser,
            this.barButtonItemDeleteUser,
            this.barButtonItemPwd,
            this.barEditItemUsername,
            this.barEditItemLevel,
            this.barEditItemEmail,
            this.barEditItemAddress,
            this.barEditItemTrueName,
            this.barEditItemTelephone,
            this.barEditItemDescription,
            this.barEditItemOldPwd,
            this.barEditItemNewPwd,
            this.barEditItemConfirmPwd,
            this.barButtonItemSaveUser,
            this.barButtonItemAddRole,
            this.barButtonItemSaveRole,
            this.barButtonItemDeleteRole,
            this.barButtonItemModifyPWD});
               this.ribbonControl1.MaxItemId = 20;
               this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemTextEdit3,
            this.repositoryItemTextEdit4,
            this.repositoryItemTextEdit5,
            this.repositoryItemTextEdit6,
            this.repositoryItemMemoEdit1,
            this.repositoryItemTextEdit7,
            this.repositoryItemTextEdit8,
            this.repositoryItemTextEdit9});
               this.ribbonControl1.Size = new System.Drawing.Size(687, 147);
               // 
               // ribbonPage1
               // 
               this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4,
            this.ribbonPageGroup5});
               // 
               // ribbonPageGroup1
               // 
               this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemAddUser, true);
               this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemDeleteUser, true);
               this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItemModifyPWD, true);
               this.ribbonPageGroup1.Text = "用户操作";
               // 
               // barButtonItemAddUser
               // 
               this.barButtonItemAddUser.Caption = "添加用户";
               this.barButtonItemAddUser.Id = 1;
               this.barButtonItemAddUser.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.添加用户;
               this.barButtonItemAddUser.Name = "barButtonItemAddUser";
               this.barButtonItemAddUser.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
               this.barButtonItemAddUser.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAddUser_ItemClick);
               // 
               // barButtonItemDeleteUser
               // 
               this.barButtonItemDeleteUser.Caption = "删除用户";
               this.barButtonItemDeleteUser.Id = 2;
               this.barButtonItemDeleteUser.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.保存用户;
               this.barButtonItemDeleteUser.Name = "barButtonItemDeleteUser";
               this.barButtonItemDeleteUser.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
               this.barButtonItemDeleteUser.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDeleteUser_ItemClick);
               // 
               // ribbonPageGroup2
               // 
               this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemUsername);
               this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemLevel);
               this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemTrueName, true);
               this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemTelephone);
               this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemAddress, true);
               this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemEmail);
               this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemDescription, true);
               this.ribbonPageGroup2.Name = "ribbonPageGroup2";
               this.ribbonPageGroup2.Text = "用户基本信息";
               // 
               // barEditItemUsername
               // 
               this.barEditItemUsername.Caption = "用户";
               this.barEditItemUsername.Edit = this.repositoryItemTextEdit1;
               this.barEditItemUsername.Id = 4;
               this.barEditItemUsername.Name = "barEditItemUsername";
               this.barEditItemUsername.Width = 100;
               // 
               // repositoryItemTextEdit1
               // 
               this.repositoryItemTextEdit1.AutoHeight = false;
               this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
               // 
               // barEditItemLevel
               // 
               this.barEditItemLevel.Caption = "级别";
               this.barEditItemLevel.Edit = this.repositoryItemTextEdit2;
               this.barEditItemLevel.Id = 5;
               this.barEditItemLevel.Name = "barEditItemLevel";
               this.barEditItemLevel.Width = 100;
               // 
               // repositoryItemTextEdit2
               // 
               this.repositoryItemTextEdit2.AutoHeight = false;
               this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
               // 
               // barEditItemTrueName
               // 
               this.barEditItemTrueName.Caption = "真名";
               this.barEditItemTrueName.Edit = this.repositoryItemTextEdit5;
               this.barEditItemTrueName.Id = 8;
               this.barEditItemTrueName.Name = "barEditItemTrueName";
               this.barEditItemTrueName.Width = 100;
               // 
               // repositoryItemTextEdit5
               // 
               this.repositoryItemTextEdit5.AutoHeight = false;
               this.repositoryItemTextEdit5.Name = "repositoryItemTextEdit5";
               // 
               // barEditItemTelephone
               // 
               this.barEditItemTelephone.Caption = "电话";
               this.barEditItemTelephone.Edit = this.repositoryItemTextEdit6;
               this.barEditItemTelephone.Id = 9;
               this.barEditItemTelephone.Name = "barEditItemTelephone";
               this.barEditItemTelephone.Width = 100;
               // 
               // repositoryItemTextEdit6
               // 
               this.repositoryItemTextEdit6.AutoHeight = false;
               this.repositoryItemTextEdit6.Name = "repositoryItemTextEdit6";
               // 
               // barEditItemAddress
               // 
               this.barEditItemAddress.Caption = "地址";
               this.barEditItemAddress.Edit = this.repositoryItemTextEdit4;
               this.barEditItemAddress.Id = 7;
               this.barEditItemAddress.Name = "barEditItemAddress";
               this.barEditItemAddress.Width = 150;
               // 
               // repositoryItemTextEdit4
               // 
               this.repositoryItemTextEdit4.AutoHeight = false;
               this.repositoryItemTextEdit4.Name = "repositoryItemTextEdit4";
               // 
               // barEditItemEmail
               // 
               this.barEditItemEmail.Caption = "邮箱";
               this.barEditItemEmail.Edit = this.repositoryItemTextEdit3;
               this.barEditItemEmail.Id = 6;
               this.barEditItemEmail.Name = "barEditItemEmail";
               this.barEditItemEmail.Width = 150;
               // 
               // repositoryItemTextEdit3
               // 
               this.repositoryItemTextEdit3.AutoHeight = false;
               this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
               // 
               // barEditItemDescription
               // 
               this.barEditItemDescription.Caption = "描述";
               this.barEditItemDescription.Edit = this.repositoryItemMemoEdit1;
               this.barEditItemDescription.EditHeight = 70;
               this.barEditItemDescription.Id = 10;
               this.barEditItemDescription.Name = "barEditItemDescription";
               this.barEditItemDescription.Width = 150;
               // 
               // repositoryItemMemoEdit1
               // 
               this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
               // 
               // barButtonItemPwd
               // 
               this.barButtonItemPwd.Caption = "密码设置";
               this.barButtonItemPwd.Id = 3;
               this.barButtonItemPwd.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.修改密码;
               this.barButtonItemPwd.Name = "barButtonItemPwd";
               this.barButtonItemPwd.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
               // 
               // ribbonPageGroup3
               // 
               this.ribbonPageGroup3.ItemLinks.Add(this.barEditItemOldPwd);
               this.ribbonPageGroup3.ItemLinks.Add(this.barEditItemNewPwd);
               this.ribbonPageGroup3.ItemLinks.Add(this.barEditItemConfirmPwd);
               this.ribbonPageGroup3.Name = "ribbonPageGroup3";
               this.ribbonPageGroup3.Text = "密码信息";
               // 
               // barEditItemOldPwd
               // 
               this.barEditItemOldPwd.Caption = "当前密码";
               this.barEditItemOldPwd.Edit = this.repositoryItemTextEdit7;
               this.barEditItemOldPwd.Id = 11;
               this.barEditItemOldPwd.Name = "barEditItemOldPwd";
               this.barEditItemOldPwd.Width = 100;
               // 
               // repositoryItemTextEdit7
               // 
               this.repositoryItemTextEdit7.AutoHeight = false;
               this.repositoryItemTextEdit7.Name = "repositoryItemTextEdit7";
               this.repositoryItemTextEdit7.PasswordChar = '*';
               // 
               // barEditItemNewPwd
               // 
               this.barEditItemNewPwd.Caption = "更新密码";
               this.barEditItemNewPwd.Edit = this.repositoryItemTextEdit8;
               this.barEditItemNewPwd.Id = 12;
               this.barEditItemNewPwd.Name = "barEditItemNewPwd";
               this.barEditItemNewPwd.Width = 100;
               // 
               // repositoryItemTextEdit8
               // 
               this.repositoryItemTextEdit8.AutoHeight = false;
               this.repositoryItemTextEdit8.Name = "repositoryItemTextEdit8";
               this.repositoryItemTextEdit8.PasswordChar = '*';
               // 
               // barEditItemConfirmPwd
               // 
               this.barEditItemConfirmPwd.Caption = "确认密码";
               this.barEditItemConfirmPwd.Edit = this.repositoryItemTextEdit9;
               this.barEditItemConfirmPwd.Id = 13;
               this.barEditItemConfirmPwd.Name = "barEditItemConfirmPwd";
               this.barEditItemConfirmPwd.Width = 100;
               // 
               // repositoryItemTextEdit9
               // 
               this.repositoryItemTextEdit9.AutoHeight = false;
               this.repositoryItemTextEdit9.Name = "repositoryItemTextEdit9";
               this.repositoryItemTextEdit9.PasswordChar = '*';
               // 
               // ribbonPageGroup4
               // 
               this.ribbonPageGroup4.ItemLinks.Add(this.barButtonItemSaveUser, true);
               this.ribbonPageGroup4.Name = "ribbonPageGroup4";
               this.ribbonPageGroup4.Text = "保存";
               // 
               // barButtonItemSaveUser
               // 
               this.barButtonItemSaveUser.Caption = "保存用户";
               this.barButtonItemSaveUser.Id = 15;
               this.barButtonItemSaveUser.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.保存用户1;
               this.barButtonItemSaveUser.Name = "barButtonItemSaveUser";
               this.barButtonItemSaveUser.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
               this.barButtonItemSaveUser.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSaveUser_ItemClick);
               // 
               // ribbonPageGroup5
               // 
               this.ribbonPageGroup5.ItemLinks.Add(this.barButtonItemAddRole, true);
               this.ribbonPageGroup5.ItemLinks.Add(this.barButtonItemDeleteRole, true);
               this.ribbonPageGroup5.ItemLinks.Add(this.barButtonItemSaveRole, true);
               this.ribbonPageGroup5.Name = "ribbonPageGroup5";
               this.ribbonPageGroup5.Text = "角色操作";
               // 
               // barButtonItemAddRole
               // 
               this.barButtonItemAddRole.Caption = "添加角色";
               this.barButtonItemAddRole.Id = 16;
               this.barButtonItemAddRole.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.添加角色;
               this.barButtonItemAddRole.Name = "barButtonItemAddRole";
               this.barButtonItemAddRole.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
               this.barButtonItemAddRole.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAddRole_ItemClick);
               // 
               // barButtonItemDeleteRole
               // 
               this.barButtonItemDeleteRole.Caption = "删除角色";
               this.barButtonItemDeleteRole.Id = 18;
               this.barButtonItemDeleteRole.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.删除角色;
               this.barButtonItemDeleteRole.Name = "barButtonItemDeleteRole";
               this.barButtonItemDeleteRole.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
               this.barButtonItemDeleteRole.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemDeleteRole_ItemClick);
               // 
               // barButtonItemSaveRole
               // 
               this.barButtonItemSaveRole.Caption = "保存角色";
               this.barButtonItemSaveRole.Id = 17;
               this.barButtonItemSaveRole.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.保存角色;
               this.barButtonItemSaveRole.Name = "barButtonItemSaveRole";
               this.barButtonItemSaveRole.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
               this.barButtonItemSaveRole.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemSaveRole_ItemClick);
               // 
               // barButtonItemModifyPWD
               // 
               this.barButtonItemModifyPWD.Caption = "修改密码";
               this.barButtonItemModifyPWD.Id = 19;
               this.barButtonItemModifyPWD.LargeGlyph = global::QRST_DI_MS_Desktop.Properties.Resources.修改密码;
               this.barButtonItemModifyPWD.Name = "barButtonItemModifyPWD";
               this.barButtonItemModifyPWD.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
               this.barButtonItemModifyPWD.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemModifyPWD_ItemClick);
               // 
               // rucUserManager
               // 
               this.Name = "rucUserManager";
               ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit5)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit6)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit4)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit7)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit8)).EndInit();
               ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit9)).EndInit();
               this.ResumeLayout(false);

           }
       
        /// <summary>
        /// 清空控件中的用户信息
        /// </summary>
        void ClearUserInfo()
           {
               barEditItemAddress.EditValue = "";
               barEditItemDescription.EditValue = "";
               barEditItemEmail.EditValue = "";
               barEditItemTrueName.EditValue = "";
               barEditItemLevel.EditValue = "";
               barEditItemUsername.EditValue = "";
               barEditItemTelephone.EditValue = "";
               barEditItemConfirmPwd.EditValue = "";
               barEditItemOldPwd.EditValue = "";  //当前密码
               barEditItemNewPwd.EditValue = "";
               mucuserManager.checkedListBoxControlRole.UnCheckAll();
           }

    

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemAddRole_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsAddRole = true;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDeleteRole_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            mucuserManager.DeleteSelectedRole();
        }

        /// <summary>
        /// 保存角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSaveRole_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (IsAddRole)
            {
                rolesInfo role = mucuserManager.GetNewRole();
                if (role != null)
                {
                    rolesInfo.AddRole(role);
                }
            }
            else
            {
                mucuserManager.UpdateRole();
            }
            IsAddRole = false;

            mucuserManager.RefreshRoleList();
        }

        private void RoleChanged(object sender, EventArgs e)
        {
            IsAddRole = false;
        }

        private void FunChecked(object sender, EventArgs e)
        {
        }
 
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemDeleteUser_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (mucuserManager.treeViewUserList.SelectedNode != null)
            {
                userInfo user = (userInfo)mucuserManager.treeViewUserList.SelectedNode.Tag;
                List<rolesInfo> sysroleLst = rolesInfo.GetList(string.Format(" NAME = '{0}'", "系统管理员"));
                if(sysroleLst.Count > 0)
                {
                    if (user.HasRole(sysroleLst[0]))
                    {
                        XtraMessageBox.Show("无法删除带有系统管理权限的用户！");
                        return;
                    }
                 
                }
            
                string msg = string.Format("确定要删除用户'{0}'?", user.NAME);
                System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(this, msg, "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    userInfo.deleteUser((int)user.ID);
                    mucuserManager.RefreshUsersLst();
                    IsAddUser = true;
                }
                 
            }

            //string msg = string.Format("确定要删除角色'{0}'?这将导致拥有该角色的用户丧失该角色的全部权限！", role.NAME);
            //System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(this, msg, "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
            //if (result == System.Windows.Forms.DialogResult.Yes)
            //{
            //    rolesInfo.DeleteRole((int)role.ID);
            //}
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemAddUser_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            IsAddUser = true;
        }

        /// <summary>
        /// 保存用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemSaveUser_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (!ribbonPageGroup2.Visible) //保存密码
            {
                userInfo user = (userInfo)mucuserManager.treeViewUserList.SelectedNode.Tag;
                if (barEditItemOldPwd.EditValue== null ||user.PASSWORD != barEditItemOldPwd.EditValue.ToString())
                {
                    XtraMessageBox.Show("当前密码输入错误！");
                    return;
                }
                else
                {
                    if (barEditItemConfirmPwd.EditValue==null||barEditItemNewPwd.EditValue == null || barEditItemNewPwd.EditValue.ToString() != barEditItemConfirmPwd.EditValue.ToString())
                    {
                        XtraMessageBox.Show("两次输入的密码不一致或为空！");
                        return;
                    }
                    else
                    {
                        user.PASSWORD = barEditItemNewPwd.EditValue.ToString();
                        userInfo.updateUser(user);
                        mucuserManager.RefreshUsersLst();
                        IsAddRole = false;
                        return;
                    }
                }
            }


            if (Check())
                {
                    if (IsAddUser)
                    {
                        userInfo user = GetNewUser();
                        userInfo.AddUser(user);
                        mucuserManager.RefreshUsersLst();
                        IsAddUser = true;
                    }
                    else
                    {
                        userInfo user = GetNewUser();
                        userInfo.updateUser(user);
                        mucuserManager.RefreshUsersLst();
                        IsAddUser = false;
                    }
                }
        }

        bool Check()
        {
            if (barEditItemUsername.EditValue== null ||string.IsNullOrEmpty(barEditItemUsername.EditValue.ToString()))
            {
                XtraMessageBox.Show("请输入用户名！");
                return false;
            }


            if ((barEditItemConfirmPwd.EditValue==null||string.IsNullOrEmpty(barEditItemConfirmPwd.EditValue.ToString()))&&IsAddUser)
            {
                XtraMessageBox.Show("请输入用户密码！");
                return false;
            }
            if ((barEditItemNewPwd.EditValue== null ||string.IsNullOrEmpty(barEditItemNewPwd.EditValue.ToString()))&&IsAddUser)
            {
                XtraMessageBox.Show("请确认密码！");
                return false;
            }
            if ((barEditItemConfirmPwd.EditValue== null||barEditItemNewPwd.EditValue.ToString() != barEditItemConfirmPwd.EditValue.ToString()) && IsAddUser)
            {
                XtraMessageBox.Show("两次输入的密码不一致，请重新输入！");
                return false;
            }
        if (mucuserManager.checkedListBoxControlRole.CheckedItems.Count==0)
        {
            XtraMessageBox.Show("您还没有为用户赋予任何角色！");
            return false;
        }
            return true;
        }

        private userInfo GetNewUser()
        {
            userInfo user = new userInfo();
            user.ADDRESS = barEditItemAddress.EditValue.ToString();
            user.CELLPHONE = barEditItemTelephone.EditValue.ToString();
            user.DESCRIPTION = barEditItemDescription.EditValue.ToString();
            user.EMAIL = barEditItemEmail.EditValue.ToString();
            if (!string.IsNullOrEmpty(barEditItemLevel.EditValue.ToString()))
            {user.LEVELS = Int64.Parse(barEditItemLevel.EditValue.ToString()) ;
            }
            if (IsAddUser)
            {
                user.PASSWORD = barEditItemNewPwd.EditValue.ToString();
            }
            else
            {
                user.ID = ((userInfo)mucuserManager.treeViewUserList.SelectedNode.Tag).ID;
                user.PASSWORD = ((userInfo)mucuserManager.treeViewUserList.SelectedNode.Tag).PASSWORD;
            }
            user.NAME = barEditItemUsername.EditValue.ToString();
            user.REALNAME = barEditItemTrueName.EditValue.ToString();
            user.ROLES = userInfo.RoleLst2RoleStr(mucuserManager.GetCheclkedRolesCode());
            return user;
        }

        void userChanged(object sender, TreeViewEventArgs e)
        {
            if (e.Node != null)
            {
                userInfo user = (userInfo)e.Node.Tag;
                displayUser(user);
                IsAddUser = false;
            }
        }

        void displayUser(userInfo user)
        {
            ribbonPageGroup2.Visible = true;
            barEditItemAddress.EditValue = user.ADDRESS;
            barEditItemTelephone.EditValue = user.CELLPHONE;
            barEditItemDescription.EditValue = user.DESCRIPTION;
            barEditItemEmail.EditValue =user.EMAIL ;
            barEditItemLevel.EditValue =user.LEVELS ;
            barEditItemUsername.EditValue=user.NAME  ;
            barEditItemTrueName.EditValue =user.REALNAME ;
            ribbonPageGroup3.Visible = false;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemModifyPWD_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ribbonPageGroup3.Visible = true;
            ribbonPageGroup2.Visible = false;
        }

    }
}
