using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QRST_DI_MS_Basis.UserRole;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class mucUserManager : DevExpress.XtraEditors.XtraUserControl
    {
        bool isFirstLoad = true;
        //子系统系统列表
        Dictionary<string, string> sysDic;

        private int[][] _sysMouldArr;   

        //自定义类型改变事件，便于和ribbon进行通信
        public delegate void RoleChangedEventHandler(object sender, EventArgs e);  
        public event RoleChangedEventHandler RoleChanged;

        public delegate void FunCheckedEventHandler(object sender, EventArgs e);  
        public event FunCheckedEventHandler FunChecked;

        //用户列表选择项发生变化
        public delegate void UserChangedEventHandler(object sender, TreeViewEventArgs e);
        public event UserChangedEventHandler UserChanged;      

        public mucUserManager()
        {
            InitializeComponent();
        }

        private void mucUserManager_VisibleChanged(object sender, EventArgs e)
        {
            if (isFirstLoad && Visible)
            {

                //加载子系统列表
               sysDic = TheUniversal.GetAllSystemList();
               string[] strArr = new string[sysDic.Count];
               sysDic.Keys.CopyTo(strArr, 0);
               listBoxControlSystem.Items.AddRange(strArr);
           
               //初始化sysMouldArr
                _sysMouldArr = new int[sysDic.Count][];
                for (int i = 0 ; i < _sysMouldArr.Length ;i++ )
                {
                    string sysName;
                    sysDic.TryGetValue(strArr[i], out sysName);
                    Dictionary<string, string> funDic = TheUniversal.GetAllMoudleBySysName(sysName);
                    _sysMouldArr[i] = new int[funDic.Count+1];
                    for (int j = 0 ; j < _sysMouldArr[i].Length ;j++ )
                    {
                        _sysMouldArr[i][j] = 0;
                    }
                }

                if (listBoxControlSystem.Items.Count > 0)
                {
                    listBoxControlSystem.SelectedIndex = 0;
                }
         
                //加载角色列表
                RefreshRoleList();
                //加载用户列表
                RefreshUsersLst();

                isFirstLoad = false;

            }
        }

        /// <summary>
        /// 系统列表选择变化时，将该子系统中相应的功能模块加载到listBoxControlSystem中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listBoxControlSystem_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxControlSystem.SelectedIndex >= 0)
            {
                checkedListBoxControlFunction.Items.Clear();
                string sysName;
                if (sysDic.TryGetValue(listBoxControlSystem.SelectedItem.ToString(),out sysName))
                {
                    Dictionary<string, string> funDic = TheUniversal.GetAllMoudleBySysName(sysName);
                    string[] strArr = new string[funDic.Count];
                    funDic.Values.CopyTo(strArr, 0);
                    checkedListBoxControlFunction.Items.AddRange(strArr);
                }
                for (int i = 1 ; i < _sysMouldArr[listBoxControlSystem.SelectedIndex].Length ;i++ )
                {
                    if (_sysMouldArr[listBoxControlSystem.SelectedIndex][i] == 1)
                    {
                        checkedListBoxControlFunction.Items[i - 1].CheckState = CheckState.Checked;
                    }
                    else
                    {
                        checkedListBoxControlFunction.Items[i - 1].CheckState = CheckState.Unchecked;
                    }
                }
            }
        }

        /// <summary>
        /// 选中一个模块，改变_sysMouldArr中的值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxControlFunction_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
            if (e.State == CheckState.Checked)
            {
                _sysMouldArr[listBoxControlSystem.SelectedIndex][e.Index + 1] = 1;
            }
            else
            {
                _sysMouldArr[listBoxControlSystem.SelectedIndex][e.Index + 1] = 0;
            }
            _sysMouldArr[listBoxControlSystem.SelectedIndex][0] = 0;
            for (int i = 1 ; i < _sysMouldArr[listBoxControlSystem.SelectedIndex].Length ;i++ )
            {
                _sysMouldArr[listBoxControlSystem.SelectedIndex][0] = _sysMouldArr[listBoxControlSystem.SelectedIndex][0] + _sysMouldArr[listBoxControlSystem.SelectedIndex][i];
            }
            if (FunChecked!= null)
            {
                FunChecked(sender,e);
            }
        }

        /// <summary>
        /// 清空角色信息
        /// </summary>
        public void ClearRoleInfo()
        {
            for (int i = 0 ; i < _sysMouldArr.Length ;i++ )
            {
                for (int j = 0 ; j < _sysMouldArr[i].Length ;j++ )
                {
                    _sysMouldArr[i][j] = 0;
                }
            }
            for (int k = 0 ; k < checkedListBoxControlFunction.Items.Count ;k++ )
            {
                checkedListBoxControlFunction.Items[k].CheckState = CheckState.Unchecked;
            }

            textEditRoleName.Text = "";
        }

        /// <summary>
        /// 加载角色信息
        /// </summary>
        /// <param name="role"></param>
        public void LoadRoleInfo(rolesInfo role)
        {
            textEditRoleName.Text = role.NAME;
            _sysMouldArr = role.RoleArray;
            listBoxControlSystem.SelectedIndex = 0;
        }

        /// <summary>
        /// 检查role信息是否符合入库条件
        /// </summary>
        /// <returns></returns>
        public bool CheckRole()
        {
            if (string.IsNullOrEmpty(textEditRoleName.Text))
            {
                XtraMessageBox.Show("请输入角色名称！");
                return false;
            }
            if (rolesInfo.ExistRole(textEditRoleName.Text))
            {
                string msg = string.Format("角色名'{0}'已经存在", textEditRoleName.Text);
                XtraMessageBox.Show(msg);
                return false;
            }
            return true;
        }

        public rolesInfo GetNewRole()
        {
            if(CheckRole())
            {
                rolesInfo role = new rolesInfo();
                role.DESCRIPTION = rolesInfo.RoleArr2Str(_sysMouldArr);
                role.NAME = textEditRoleName.Text;
                return role;
            }
            return null;
        }

        /// <summary>
        /// 刷新角色列表
        /// </summary>
        public void RefreshRoleList()
        {
            checkedListBoxControlRole.Items.Clear();
            List<rolesInfo> rolesLst = rolesInfo.GetList("");

            checkedListBoxControlRole.DataSource = rolesLst;
            checkedListBoxControlRole.DisplayMember = "NAME";
           

            //勾选当前用户勾选的角色信息,初始化_roleArr 
            if (treeViewUserList.SelectedNode != null)
            {
                userInfo user = (userInfo)treeViewUserList.SelectedNode.Tag;
                CheckRolesWithSelectedUser(user);
            }
        }

        /// <summary>
        /// 当选择角色发生变化时，加载对应的角色信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxControlRole_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (checkedListBoxControlRole.SelectedItem != null)
            {
                rolesInfo role = (rolesInfo)checkedListBoxControlRole.SelectedItem;
                LoadRoleInfo(role);
            }
            if (RoleChanged != null)
            {
                RoleChanged(sender,e);
            }
        }
        public void DeleteSelectedRole()
        {
            if (checkedListBoxControlRole.SelectedItem == null)
            {
                XtraMessageBox.Show("没有可以删除的角色！");
                return;
            }
             rolesInfo role = (rolesInfo)checkedListBoxControlRole.SelectedItem;
             if (role.NAME == "系统管理员")
            {
                XtraMessageBox.Show("无法删除‘系统管理员’角色！");
                return;
            }
            string msg = string.Format("确定要删除角色'{0}'?这将导致拥有该角色的用户丧失该角色的全部权限！",role.NAME);
            System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(this, msg, "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                rolesInfo.DeleteRole((int)role.ID);
            }
            RefreshRoleList();
        }

        public void UpdateRole()
        {
            rolesInfo role = (rolesInfo)checkedListBoxControlRole.SelectedItem;
            role.DESCRIPTION = rolesInfo.RoleArr2Str(_sysMouldArr);
            rolesInfo.updateRole(role);
        }

#region  用户操作
        /// <summary>
        /// 根据特定的用户check拥有的角色
        /// </summary>
        /// <param name="user"></param>
        public void CheckRolesWithSelectedUser(userInfo user)
        {
            List<rolesInfo> roleLst = (List<rolesInfo>)checkedListBoxControlRole.DataSource;
            for (int i = 0 ; i < roleLst.Count ;i++ )
            {
                rolesInfo role = roleLst[i]; // (rolesInfo)checkedListBoxControlRole.Items[i];
                if (user.HasRole(role))
                {
                    checkedListBoxControlRole.SetItemChecked(i, true);
                }
                else
                {
                    checkedListBoxControlRole.SetItemChecked(i, false);
                }
            }
        }

        public void RefreshUsersLst()
        {
            treeViewUserList.Nodes.Clear();

            List<userInfo> userLst = userInfo.GetList("");
           foreach (userInfo var in userLst)
           {
               TreeNode tn = new TreeNode() {Name = var.NAME,Text = var.NAME,Tag = var,ImageIndex = 0 };
               treeViewUserList.Nodes.Add(tn);
           }
           if (treeViewUserList.Nodes.Count>0)
            {
                treeViewUserList.SelectedNode = treeViewUserList.Nodes[0];
            }
           treeViewUserList.ExpandAll();
        }

        /// <summary>
        /// 选中用户节点后在角色列表中勾选对应的角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewUserList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CheckRolesWithSelectedUser((userInfo)e.Node.Tag);

            if (UserChanged != null)
            {
                UserChanged(sender, e);
            }
        }
#endregion

        /// <summary>
        /// 为用户选择角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkedListBoxControlRole_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
        {
       
        }



        /// <summary>
        /// 获取checked的角色
        /// </summary>
        /// <returns></returns>
        public  string[] GetCheclkedRolesCode()
        {
            string [] rolesCodeArr = new string [checkedListBoxControlRole.CheckedItems.Count];
            for (int i = 0 ; i < rolesCodeArr.Length ;i++ )
            {
                rolesCodeArr[i] = ((rolesInfo)checkedListBoxControlRole.CheckedItems[i]).QRST_CODE; 
            }
            return rolesCodeArr;
        }

      

    }
}
