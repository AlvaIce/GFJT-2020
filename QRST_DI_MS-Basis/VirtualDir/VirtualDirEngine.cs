using System;
using System.Collections.Generic;
using System.Text;
//using QRST_DI_MS_Basis.Model;
using System.Data;
using System.Windows.Forms;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_DS_Metadata.Paths;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;
 

namespace QRST_DI_MS_Basis.VirtualDir
{
    public class VirtualDirEngine
    {
        private static IDbBaseUtilities isdbutil;
        private static IDbBaseUtilities _isDBUtil
        {
            get
            {
                if (isdbutil == null)
                {
                    //isdbutil = mySqlOperater.ISDB;
                    isdbutil = mySqlOperater.GetSubDbUtilities(EnumDBType.ISDB);
                }
                return isdbutil;
            }
            set
            {
                isdbutil = value;
            }
        }
        private static IDbOperating mysqlopr;
        private static IDbOperating mySqlOperater
        {
            get
            {
                if (mysqlopr == null)
                {
                    mysqlopr = Constant.IdbOperating;
                }
                return mysqlopr;
            }
            set {
                mysqlopr = value;
            }
        }
        public static string _vdtc;
        public static string virtualdirtablecode
        {
            get
            {
                if (_vdtc == null)
                {

                    _vdtc = GetVDTableCode();
                }
                return _vdtc;
            }
          
        }
        public static string _vdrtc;
        private static string virtualdirrelationtablecode
        {
            get
            {
                if (_vdrtc == null)
                {

                    _vdrtc = GetVDRTableCode();
                }
                return _vdrtc;
            }
          
        }
        public string _User;
        public string _Password;
        private bool _isRegister;
        public bool IsRegister
        {
            get { return _isRegister; }
        }

        public VirtualDirEngine()
        {
        }

        public static string GetVDTableCode()
        {
            string strSql = "select QRST_CODE from tablecode where TABLE_NAME = 'VirtualDir'";
            DataSet ds = _isDBUtil.GetDataSet(strSql);
            return ds.Tables[0].Rows[0][0].ToString();
        }

        public static string GetVDRTableCode()
        {
            string strSql = "select QRST_CODE from tablecode where TABLE_NAME = 'VirtualDirRelation'";
            DataSet ds = _isDBUtil.GetDataSet(strSql);
            return ds.Tables[0].Rows[0][0].ToString();

        }

        public string GetcodeBydirnameandparentcode(string parentcode, string name)
        {
            string code = "";
            StringBuilder sqlStrr = new StringBuilder();
            sqlStrr.Append("SELECT virtualdirrelation.ChildLink,virtualdir.`Name` FROM `virtualdirrelation` LEFT JOIN  virtualdir on virtualdirrelation.ChildLink=virtualdir.QRST_CODE");
            sqlStrr.AppendFormat(" where virtualdirrelation.`ParentLink` = '{0}' and virtualdir.`Name`='{1}' ", parentcode, name);
            DataSet dsd = _isDBUtil.GetDataSet(sqlStrr.ToString());
            if (dsd != null && dsd.Tables[0] != null && dsd.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drd in dsd.Tables[0].Rows)
                {
                    code = drd["ChildLink"].ToString();
                    break;
                }
            }
            return code;
        }
        /// <summary>
        /// 根据code得到文件夹名称
        /// </summary>
        /// <param name="dircode"></param>
        /// <returns></returns>
        public string GetNameByVirtualDirCode(string dircode)
        {
            string sourcedirname = "";//sourcedircode的文件夹名字
            string sqlStrd = string.Format("select Name from VirtualDir where QRST_CODE= '{0}' ", dircode);
            DataSet dsd = _isDBUtil.GetDataSet(sqlStrd.ToString());
            if (dsd != null && dsd.Tables[0] != null && dsd.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drd in dsd.Tables[0].Rows)
                {
                    sourcedirname = drd["Name"].ToString();
                    break;
                }
            }
            return sourcedirname;
        }
        /// <summary>
        /// 根据code得到文件名称
        /// </summary>
        /// <param name="filelinkcode"></param>
        /// <returns></returns>
        public string GetNameByFileLinkCode(string filelinkcode)
        {
            string filename = "";
            DataRow datarowinfo = GetDataRowInfo(filelinkcode);
            filename = datarowinfo["NAME"].ToString();
            return filename;

        }
        /// <summary>
        /// 根据code得到文件夹新建时间
        /// </summary>
        /// <param name="dircode"></param>
        /// <returns></returns>
        public string GetCreatTimeByVirtualDirCode(string dircode)
        {
            string creattime = "";
            string sqlStr = string.Format("select CreateTime from VirtualDir where  QRST_CODE= '{0}' ", dircode);
            DataSet dsd = _isDBUtil.GetDataSet(sqlStr);

            if (dsd != null && dsd.Tables[0] != null && dsd.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drd in dsd.Tables[0].Rows)
                {
                    creattime = drd["CreateTime"].ToString();
                    break;
                }
            }
            return creattime;

        }

        /// <summary>
        /// 根据code得到文件夹备注
        /// </summary>
        /// <param name="dircode"></param>
        /// <returns></returns>
        public string GetRemarkByVirtualDirCode(string dircode)
        {
            string creattime = "";
            string sqlStr = string.Format("select Remark from VirtualDir where  QRST_CODE= '{0}' ", dircode);
            DataSet dsd = _isDBUtil.GetDataSet(sqlStr);

            if (dsd != null && dsd.Tables[0] != null && dsd.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drd in dsd.Tables[0].Rows)
                {
                    creattime = drd["Remark"].ToString();
                    break;
                }
            }
            return creattime;

        }

        /// <summary>
        /// 根据code得到文件夹修改时间
        /// </summary>
        /// <param name="dircode"></param>
        /// <returns></returns>
        public string GetModifyTimeByVirtualDirCode(string dircode)
        {
            string modifytime = "";
            string sqlStr = string.Format("select ModifyTime from VirtualDir where  QRST_CODE= '{0}' ", dircode);
            DataSet dsd = _isDBUtil.GetDataSet(sqlStr);

            if (dsd != null && dsd.Tables[0] != null && dsd.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drd in dsd.Tables[0].Rows)
                {
                    modifytime = drd["ModifyTime"].ToString();
                    break;
                }
            }
            return modifytime;

        }
        /// <summary>
        /// 根据code得到文件新建时间
        /// </summary>
        /// <param name="filelinkcode"></param>
        /// <returns></returns>
        public string GetCreatTimeByFileLinkCode(string filelinkcode)
        {
            string creattime = "";
            string sqlStr = string.Format("select CreateTime from VirtualDirRelation where  ChildLink= '{0}' ", filelinkcode);
            DataSet dsd = _isDBUtil.GetDataSet(sqlStr);

            if (dsd != null && dsd.Tables[0] != null && dsd.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drd in dsd.Tables[0].Rows)
                {
                    creattime = drd["CreateTime"].ToString();
                    break;
                }
            }
            return creattime;

        }
 /// <summary>

        /// 根据code得到文件信息code
      /// </summary>
      /// <param name="parentcode"></param>
      /// <param name="filelinkcode"></param>
      /// <returns></returns>
        public string GetInfoByFCodeandPcode(string parentcode,string filelinkcode)
        {
            string code = "";
            string sqlStr = string.Format("select QRST_CODE from VirtualDirRelation where ParentLink='{0}' and ChildLink= '{1}' ",parentcode, filelinkcode);
           DataSet dsd = _isDBUtil.GetDataSet(sqlStr);

            if (dsd != null && dsd.Tables[0] != null && dsd.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow drd in dsd.Tables[0].Rows)
                {
                    code = drd["QRST_CODE"].ToString();
                    break;
                }
            }
            return code;

        }


        /// <summary>
        /// 最后写
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public void Register(string username, string password)
        {
            _isRegister = Login(username, password);
            _User=username;
            _Password=password;
        }

        /// <summary>
        /// 最后写
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool Login(string username, string password)
        {
            string sqlStr = string.Format("select * from userinfo where NAME= '{0}' and PASSWORD='{1}'", username, Secret.Encrypt(password));         //加密码
            if (mySqlOperater==null)
            {
                mySqlOperater = Constant.IdbOperating;
            }
            DataSet ds = mySqlOperater.GetSubDbUtilities(EnumDBType.MIDB).GetDataSet(sqlStr);
            if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 用户目录列表
        /// </summary>
        /// <param name="topdir"></param>
        public TreeNode List()//有问题？
        {
            if (IsRegister)
            {
                return List(_User, _Password);
            }
            else
            {
                //throw new Exception("用户没有该权限！");
                return null;
            }
        }
        

        /// <summary>
        /// 用户目录列表
        /// </summary>
        /// <param name="topdir"></param>
        public TreeNode List(string user, string pwd)
        {
            bool isLogin = false;
            if (user == _User && pwd == _Password && IsRegister)
            {
                isLogin = true;
            }
            else
            {
                isLogin = Login(user, pwd);
            }

            if (!isLogin)
            {
                //throw new Exception("用户没有该权限！");
                return null;
            }


            StringBuilder strSql = new StringBuilder();
            strSql.Append("select QRST_CODE from VirtualDir");
            strSql.AppendFormat(" where User = '{0}' and Name = 'root' ", user);
            DataSet ds = _isDBUtil.GetDataSet(strSql.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string rootQC = ds.Tables[0].Rows[0][0].ToString();

                TreeNode tn = new TreeNode("root");
                tn.Tag = rootQC;

               //return List(tn);
               return FirstList(tn,0); //20161212
            }
            else
            {
                //每个用户必须要有root，如果不存在，创建一个
                string rootQC = InitRootDir(user);
                TreeNode tn = new TreeNode("root");
                tn.Tag = rootQC;

                return tn;
            }
        }
        

        /// <summary>
        /// 用户目录列表
        /// </summary>
        /// <param name="topdir"></param>
        public TreeNode List(TreeNode parentTreeNode)
        {
            string parentDirQC=parentTreeNode.Tag.ToString();
            string parentDirName = parentTreeNode.Name;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT virtualdirrelation.ChildLink,virtualdir.`Name` FROM `virtualdirrelation` LEFT JOIN  virtualdir on virtualdirrelation.ChildLink=virtualdir.QRST_CODE");
            strSql.AppendFormat(" where virtualdirrelation.`ParentLink` = '{0}' ", parentDirQC);
            DataSet ds = _isDBUtil.GetDataSet(strSql.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string qrstcode=dr[0].ToString();
                    string vdname=dr[1].ToString();
                    TreeNode childtn = new TreeNode(vdname);
                    childtn.Tag = qrstcode;

                    //判断是否是虚拟文件夹，通过QRSTCODE开头判断
                    if (IsDir( qrstcode))
                    {
                        //补充文件夹信息 info

                        //迭代
                        List(childtn);
                    }
                    else
                    {
                        //文件，获取文件信息
                        if (!qrstcode.StartsWith(QRST_DI_Resources.Constant.INDUSTRYCODE))
                        {
                            childtn.Text = qrstcode;
                        }
                        else
                        {
                            DataRow datarowinfo = GetDataRowInfo(qrstcode);
                            vdname = datarowinfo["NAME"].ToString();
                            childtn.Text = vdname;
                        }
                    }

                    parentTreeNode.Nodes.Add(childtn);
                }
            }

            return parentTreeNode;
        }

        /// <summary>
        /// 获取前两级目录
        /// </summary>
        /// <param name="parentTreeNode"></param>
        /// <returns></returns>
        public TreeNode FirstList(TreeNode parentTreeNode,int i) //20161212
        {
            string parentDirQC = parentTreeNode.Tag.ToString();
            string parentDirName = parentTreeNode.Name;

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT virtualdirrelation.ChildLink,virtualdir.`Name` FROM `virtualdirrelation` LEFT JOIN  virtualdir on virtualdirrelation.ChildLink=virtualdir.QRST_CODE");
            strSql.AppendFormat(" where virtualdirrelation.`ParentLink` = '{0}' ", parentDirQC);
            DataSet ds = _isDBUtil.GetDataSet(strSql.ToString());
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string qrstcode = dr[0].ToString();
                    string vdname = dr[1].ToString();
                    TreeNode childtn = new TreeNode(vdname);
                    childtn.Tag = qrstcode;

                    if (IsDir(qrstcode))
                    {
                        if (i < 2)
                        {
                            parentTreeNode.Nodes.Add(childtn);
                            FirstList(childtn, ++i);
                            i = 0;
                        }                        
                        continue;                      
                    }
                    else
                    {
                        //文件，获取文件信息
                        if (i < 2)
                        {
                            if (!qrstcode.StartsWith(QRST_DI_Resources.Constant.INDUSTRYCODE))
                            {
                                childtn.Text = qrstcode;
                            }
                            else
                            {
                                try
                                {

                                    DataRow datarowinfo = GetDataRowInfo(qrstcode);
                                    if (datarowinfo==null)
                                   {
                                        return parentTreeNode;//加这个判断是防止熊明豪删除数据库数据后，用户在我的空间里就查询不到这条数据了。不用她在删除数据库数据后再删除virtual表记录了。
                                    }
                                    bool isExist = datarowinfo.Table.Columns.Contains("NAME");
                                    if(isExist)
                                    {
                                        vdname = datarowinfo["NAME"].ToString();
                                    }
                                    else
                                    {
                                        vdname = datarowinfo["TITLE"].ToString();
                                    }
                                    childtn.Text = vdname;
                                    parentTreeNode.Nodes.Add(childtn);
                                }
                                catch(Exception ex)
                                {
                                    MessageBox.Show(string.Format("构建虚拟目录异常QRSTCODE：{0}，请联系管理员！\r\n异常信息：{1}", qrstcode, ex.Message));
                                }                               
                            }
                        }
                        
                    }
                    //parentTreeNode.Nodes.Add(childtn);
                }
            }

            return parentTreeNode;
        }
       

        private string curtablecode = "";
        private string curtablename = "";
        private StoragePath curstoragepath = null; 
        private DataRow GetDataRowInfo(string datacode)
        {
            DataRow dr = null;
            try
            {
                string tableCode = StoragePath.GetTableCodeByQrstCode(datacode);
                if (curstoragepath == null || curtablecode != tableCode)
                {
                    curstoragepath = new StoragePath(tableCode);
                    curtablecode = tableCode;

                    string sql = string.Format("select table_name from tablecode where QRST_CODE = '{0}'", tableCode);
                    DataSet tbds = curstoragepath.GetMysqlBaseUtilities().GetDataSet(sql);
                    curtablename = tbds.Tables[0].Rows[0][0].ToString();
                }

                string sql2 = string.Format("select * from {0} where QRST_CODE = '{1}'", curtablename, datacode);
                DataSet ds = curstoragepath.GetMysqlBaseUtilities().GetDataSet(sql2);
                dr = ds.Tables[0].Rows[0];
            }
            catch { }
            return dr;
        }


        private string InitRootDir(string user)
        {

            DateTime CreateTime = DateTime.Now;
            DateTime ModifyTime = DateTime.Now;
            //TableLocker dblock = new TableLocker(_isDBUtil);
            Constant.IdbOperating.LockTable("VirtualDir",EnumDBType.MIDB);
            int ID = _isDBUtil.GetMaxID("ID", "VirtualDir");
            string QRST_CODE = virtualdirtablecode + "-" + ID.ToString();
            string strSql = string.Format("insert into VirtualDir(ID,Name,User,CreateTime,ModifyTime,Remark,QRST_CODE) values ('{0}','root','{1}','{2}','{3}','{4}','{5}');", ID, user, CreateTime, ModifyTime, "", QRST_CODE);
            _isDBUtil.ExecuteSql(strSql);
            Constant.IdbOperating.UnlockTable("VirtualDir",EnumDBType.MIDB);
            return QRST_CODE;
        }

        
        /// <summary>
        /// 新建文件夹(指定字段插入数据)
        /// </summary>
        /// <param name="parentdir"></param>
        /// <param name="newdirname"></param>
        public string NewDir(string parentdirCode, string newdirname, string remark)
        {
            if (IsRegister)
            {
                //判断该文件夹下是否存在要新建的文件夹（名称）
               // string sqlStr = string.Format("select QRST_CODE from VirtualDir where Name= '{0}' and User='{1}'", newdirname, _User);
               
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SELECT virtualdirrelation.ChildLink,virtualdir.`Name` FROM `virtualdirrelation` LEFT JOIN  virtualdir on virtualdirrelation.ChildLink=virtualdir.QRST_CODE");
                strSql.AppendFormat(" where virtualdirrelation.`ParentLink` = '{0}' and virtualdir.`Name`='{1}' ", parentdirCode, newdirname);
                DataSet ds = _isDBUtil.GetDataSet(strSql.ToString());
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                   // throw new Exception("该目标文件下存在要新建的文件夹！");
                    return "";
                }
                else
                {
                    return NewDir(parentdirCode, newdirname, _User, _Password, remark);
                }
             
            }
            else
            {
                throw new Exception("用户没有该权限！");
            }
        }

        public string TrueCopyNewDir(string parentdirCode, string newdirname, string remark)
        {
            if (IsRegister)
            {

                return NewDir(parentdirCode, newdirname, _User, _Password, remark);
            }
            else
            {
                throw new Exception("用户没有该权限！");
            }
        }
        /// <summary>
        /// 新建文件夹(指定字段插入数据)
        /// </summary>
        /// <param name="parentdirCode">文件夹名称Name的QRST_CODE</param>
        /// <param name="newdirname">新文件夹的名称</param>
        public string NewDir(string parentdirCode, string newdirname, string user, string pwd, string remark)//,string user,string pwd
        {
            
            DateTime CreateTime = DateTime.Now;
            DateTime ModifyTime = DateTime.Now;
            //TableLocker dblock = new TableLocker(_isDBUtil);
            Constant.IdbOperating.LockTable("VirtualDir",EnumDBType.MIDB);
            StringBuilder strSql = new StringBuilder();
            int ID = _isDBUtil.GetMaxID("ID", "VirtualDir");
            string QRST_CODE = virtualdirtablecode + "-" + ID.ToString();
            strSql.Append("insert into VirtualDir(");
            strSql.Append("ID,Name,User,CreateTime,ModifyTime,Remark,QRST_CODE,IsVisible)");
            strSql.Append(" values (");
            strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}')", ID, newdirname, _User, CreateTime.ToString(), ModifyTime.ToString(), remark, QRST_CODE, ""));
            _isDBUtil.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("VirtualDir",EnumDBType.MIDB);

            Constant.IdbOperating.LockTable("VirtualDirRelation",EnumDBType.MIDB);
            StringBuilder strSqll = new StringBuilder();
            int ID1 = _isDBUtil.GetMaxID("ID", "VirtualDirRelation");
            string QRST_CODE1 = virtualdirrelationtablecode + "-" + ID1.ToString();
            strSqll.Append("insert into VirtualDirRelation(");
            strSqll.Append("ID,ParentLink,ChildLink,CreateTime,QRST_CODE)");
            strSqll.Append(" values (");
            strSqll.Append(string.Format("{0},'{1}','{2}','{3}','{4}')", ID1, parentdirCode, QRST_CODE, CreateTime.ToString(), QRST_CODE1));
       
            _isDBUtil.ExecuteSql(strSqll.ToString());
            Constant.IdbOperating.UnlockTable("VirtualDirRelation",EnumDBType.MIDB);
            return QRST_CODE;

        }

         public string MulNewDir(string parentdirCode, string newdirname, string user, string remark)//,string user,string pwd
        {

            DateTime CreateTime = DateTime.Now;
            DateTime ModifyTime = DateTime.Now;
            //TableLocker dblock = new TableLocker(_isDBUtil);
            Constant.IdbOperating.LockTable("VirtualDir", EnumDBType.MIDB);
            StringBuilder strSql = new StringBuilder();
            int ID = _isDBUtil.GetMaxID("ID", "VirtualDir");
            string QRST_CODE = virtualdirtablecode + "-" + ID.ToString();
            strSql.Append("insert into VirtualDir(");
            strSql.Append("ID,Name,User,CreateTime,ModifyTime,Remark,QRST_CODE,IsVisible)");
            strSql.Append(" values (");
            strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}','{5}','{6}','{7}')", ID, newdirname, user, CreateTime.ToString(), ModifyTime.ToString(), remark, QRST_CODE, ""));
            _isDBUtil.ExecuteSql(strSql.ToString());
            Constant.IdbOperating.UnlockTable("VirtualDir", EnumDBType.MIDB);

            Constant.IdbOperating.LockTable("VirtualDirRelation", EnumDBType.MIDB);
            StringBuilder strSqll = new StringBuilder();
            int ID1 = _isDBUtil.GetMaxID("ID", "VirtualDirRelation");
            string QRST_CODE1 = virtualdirrelationtablecode + "-" + ID1.ToString();
            strSqll.Append("insert into VirtualDirRelation(");
            strSqll.Append("ID,ParentLink,ChildLink,CreateTime,QRST_CODE)");
            strSqll.Append(" values (");
            strSqll.Append(string.Format("{0},'{1}','{2}','{3}','{4}')", ID1, parentdirCode, QRST_CODE, CreateTime.ToString(), QRST_CODE1));

            _isDBUtil.ExecuteSql(strSqll.ToString());
            //dblock.UnlockTable("VirtualDirRelation");
            Constant.IdbOperating.UnlockTable("VirtualDirRelation", EnumDBType.MIDB);
            return QRST_CODE;

        }


        //public void fuzhiNewDir(string newparentdir, string dircode)//,string user,string pwd
        //{

        //    DateTime CreateTime = DateTime.Now;
        //    DateTime ModifyTime = DateTime.Now;
        //    TableLocker dblock = new TableLocker(_isDBUtil);
        //    dblock.LockTable("VirtualDirRelation");
        //    StringBuilder strSqll = new StringBuilder();
        //    int ID1 = _isDBUtil.GetMaxID("ID", "VirtualDirRelation");
        //    string QRST_CODE1 = virtualdirrelationtablecode + "-" + ID1.ToString();
        //    strSqll.Append("insert into VirtualDirRelation(");
        //    strSqll.Append("ID,ParentLink,ChildLink,CreateTime,QRST_CODE)");
        //    strSqll.Append(" values (");
        //    strSqll.Append(string.Format("{0},'{1}','{2}','{3}','{4}')", ID1, newparentdir, dircode, CreateTime.ToString(), QRST_CODE1));
          
        //    _isDBUtil.ExecuteSql(strSqll.ToString());
        //    dblock.UnlockTable("VirtualDirRelation");

        //}


        /// <summary>
        /// 添加文件到指定文件夹
        /// </summary>
        /// <param name="parentdir"></param>
        /// <param name="filecode"></param>

        public void AddFileLink(string parentdir, List<string> filecodelst)
        {
            if (IsRegister)
            {
                //排除已经存在的相同数据
                string sqlStrr = string.Format("select ChildLink from VirtualDirRelation where ParentLink = '{0}' ", parentdir);
                DataSet existlinkds = _isDBUtil.GetDataSet(sqlStrr.ToString());
                if (existlinkds != null && existlinkds.Tables[0] != null && existlinkds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in existlinkds.Tables[0].Rows)
                    {
                        string existlink = dr[0].ToString();
                        int idx= filecodelst.IndexOf(existlink);
                        while (idx != -1)
                        {
                            filecodelst.RemoveAt(idx);
                            idx = filecodelst.IndexOf(existlink);
                        }
                    }
                }

                if (filecodelst.Count > 0)
                {
                    //TableLocker dblock = new TableLocker(_isDBUtil);
                    Constant.IdbOperating.LockTable("VirtualDirRelation",EnumDBType.MIDB);
                    foreach (string filecode in filecodelst)
                    {
                        DateTime CreateTime = DateTime.Now;

                        StringBuilder strSql = new StringBuilder();
                        int ID = _isDBUtil.GetMaxID("ID", "VirtualDirRelation");
                        string QRST_CODE = virtualdirrelationtablecode + "-" + ID.ToString();
                        strSql.Append("insert into VirtualDirRelation(");
                        strSql.Append("ID,ParentLink,ChildLink,CreateTime,QRST_CODE)");//ID,ParentLink,ChildLink,CreateTime,
                        strSql.Append(" values (");
                        //strSql.Append("@ID,@ParentLink,@ChildLink,@CreateTime,@QRST_CODE)");//@ID,@ParentLink,@ChildLink,@CreateTime,
                        strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}')", ID, parentdir, filecode, CreateTime, QRST_CODE));
                        //MySqlParameter[] parameters = {
                        //                      new MySqlParameter("@ID", MySqlDbType.Int64,10),                              					                          new MySqlParameter("@ParentLink", MySqlDbType.Text), 
                        //                      new MySqlParameter("@ChildLink", MySqlDbType.Text),
                        //                      new MySqlParameter("@CreateTime", MySqlDbType.DateTime),
                        //                      new MySqlParameter("@QRST_CODE", MySqlDbType.Text)};
                        //parameters[0].Value = ID;
                        //parameters[1].Value = parentdir;
                        //parameters[2].Value = filecode;
                        //parameters[3].Value = CreateTime;
                        //parameters[4].Value = QRST_CODE;
                        _isDBUtil.ExecuteSql(strSql.ToString());
                    }
                    Constant.IdbOperating.UnlockTable("VirtualDirRelation",EnumDBType.MIDB);
                }
                    
                
            }
            else
            {
                return;
                //throw new Exception("用户没有该权限！");         
            }
        }
       
        /// <summary>
        /// 添加文件到指定文件夹
        /// </summary>
        /// <param name="parentdir"></param>
        /// <param name="filecode"></param>

        public void AddFileLink(string parentdir, string filecode)
        {
            if (IsRegister)
            {
                string sqlStrr = string.Format("select * from VirtualDirRelation where ParentLink = '{0}' and ChildLink = '{1}'", parentdir, filecode);
                DataSet dss = _isDBUtil.GetDataSet(sqlStrr.ToString());
                if (dss != null && dss.Tables[0] != null && dss.Tables[0].Rows.Count > 0)
                {
                    return;
                    // throw new Exception("该目标文件下存在要移动的文件夹！"); // break;//存在sourcedir跳出
                }
                else
                {
                    DateTime CreateTime = DateTime.Now;
                    //TableLocker dblock = new TableLocker(_isDBUtil);
                    Constant.IdbOperating.LockTable("VirtualDirRelation",EnumDBType.MIDB);
                    StringBuilder strSql = new StringBuilder();
                    int ID = _isDBUtil.GetMaxID("ID", "VirtualDirRelation");
                    string QRST_CODE = virtualdirrelationtablecode + "-" + ID.ToString();
                    strSql.Append("insert into VirtualDirRelation(");
                    strSql.Append("ID,ParentLink,ChildLink,CreateTime,QRST_CODE)");//ID,ParentLink,ChildLink,CreateTime,
                    strSql.Append(" values (");
                    //strSql.Append("@ID,@ParentLink,@ChildLink,@CreateTime,@QRST_CODE)");//@ID,@ParentLink,@ChildLink,@CreateTime,
                    // strSql.AppendFormat(" where ParentLink = '{0}' ", parentdir);
                    strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}')", ID, parentdir, filecode, CreateTime, QRST_CODE));
                    //MySqlParameter[] parameters = {
                    //                          new MySqlParameter("@ID", MySqlDbType.Int64,10),                                            
					               //           new MySqlParameter("@ParentLink", MySqlDbType.Text), 
                    //                          new MySqlParameter("@ChildLink", MySqlDbType.Text),
                    //                          new MySqlParameter("@CreateTime", MySqlDbType.DateTime),
                    //                          new MySqlParameter("@QRST_CODE", MySqlDbType.Text)};
                    //parameters[0].Value = ID;
                    //parameters[1].Value = parentdir;
                    //parameters[2].Value = filecode;
                    //parameters[3].Value = CreateTime;
                    //parameters[4].Value = QRST_CODE;
                    _isDBUtil.ExecuteSql(strSql.ToString());
                    Constant.IdbOperating.UnlockTable("VirtualDirRelation",EnumDBType.MIDB);
                }
            }
            else
            {
                return;
                //throw new Exception("用户没有该权限！");         
            }
        }

        /// <summary>
        /// 删除文件夹下文件
        /// </summary>
        /// <param name="parentdir"></param>
        /// <param name="filecode"></param>
        public void DelFileLink(string parentdir, string filecode)
        {
            if (IsRegister)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from VirtualDirRelation ");
                strSql.AppendFormat(" where ParentLink = '{0}' and ChildLink='{1}' ", parentdir, filecode);
                _isDBUtil.ExecuteSql(strSql.ToString());
            }
            else
            {
                return;
            }
        }


        /// <summary>
        /// 删除文件夹（遍历删除VirtualDir与VirtualDirRelation表中相关记录？？）
        /// </summary>
        /// <param name="dircode">文件夹名称Name的QRST_CODE</param>
        public void DelDir(string dircode)
        {

            if (IsRegister)
            {
                //遍历得到dircode(ParentLink)下的所有目录filecode(ChildLink)
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ChildLink from VirtualDirRelation ");
                strSql.AppendFormat(" where  ParentLink = '{0}'", dircode);//添加如何是否注册过的name是文件夹名称
                DataSet ds = _isDBUtil.GetDataSet(strSql.ToString());

                List<string> folders = new List<string>();
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        try
                        {
                            string filecode = dr["ChildLink"].ToString();
                            string[] filecodelist = new string[] { filecode };
                            foreach (string filecoded in filecodelist)
                            {
                                //判断是否是虚拟文件夹，通过QRSTCODE开头判断
                                if (filecoded.StartsWith(virtualdirtablecode))//virtualdirtablecode是ISDB-37-12形式就是文件夹
                                {
                                    //迭代
                                    DelDir(filecoded);
                                }                               
                                else  //如果遍历是文件,删除文件
                                {                                    
                                    DelFileLink(dircode,filecoded);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }            
                StringBuilder strSqll = new StringBuilder();
                strSqll.Append("delete from VirtualDir ");
                strSqll.AppendFormat(" where QRST_CODE='{0}' ", dircode);
                _isDBUtil.ExecuteSql(strSqll.ToString());


                StringBuilder strSqlll = new StringBuilder();
                strSqlll.Append("delete from VirtualDirRelation ");
                strSqlll.AppendFormat(" where ParentLink='{0}' ", dircode);
                _isDBUtil.ExecuteSql(strSqlll.ToString());

                StringBuilder strSqllll = new StringBuilder();
                strSqllll.Append("delete from VirtualDirRelation ");
                strSqllll.AppendFormat(" where ChildLink='{0}' ", dircode);
                _isDBUtil.ExecuteSql(strSqllll.ToString());

            }

            else
            {
                return;
            }
          
           
        }
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="oldparentdir"></param>
        /// <param name="newparentdir"></param>
        /// <param name="filecode"></param>
        public void MoveFileLink(string oldparentdir, string newparentdir, string filecode)
        {
            if (IsRegister)
            {
                string sqlStrr = string.Format("select * from VirtualDirRelation where ParentLink = '{0}' and ChildLink = '{1}'", newparentdir, filecode);
                DataSet dss = _isDBUtil.GetDataSet(sqlStrr.ToString());
                if (dss != null && dss.Tables[0] != null && dss.Tables[0].Rows.Count > 0)
                {
                    return;
                    // throw new Exception("该目标文件下存在要移动的文件！"); // break;//存在sourcedir跳出
                }

                string messaged = GetInfoByFCodeandPcode(oldparentdir, filecode);
                if (messaged == "")
                {
                    MessageBox.Show("复制的东西不存在，请重新选择复制！！！");
                    return;
                }
                string sqlStr = string.Format("update VirtualDirRelation set  ParentLink= '{0}', ChildLink='{1}'  where ParentLink = '{2}' and ChildLink='{3}' ", newparentdir, filecode, oldparentdir, filecode);
                _isDBUtil.ExecuteSql(sqlStr);
            }
            else
            {
                //throw new Exception("用户没有该权限！");
                return;
            }
        }

        public void ReName(string dircode,string dirname)
        {
            if (IsRegister)
            {
                DateTime ModifyTime = DateTime.Now;
                string sqlStr = string.Format("update VirtualDir set  Name= '{0}', ModifyTime='{1}'  where QRST_CODE = '{2}'  ", dirname, ModifyTime, dircode);
               _isDBUtil.ExecuteSql(sqlStr);
            }
            else
            {
             
                return;
            }
        
        }
        /// <summary>
        /// 移动文件夹
        /// </summary>
        /// <param name="destdircode">目的文件夹的code</param>
        /// <param name="sourcedircode">要移动文件夹的code</param>
     
        public void MoveDir(string sourcedircode, string destdircode)
        {
            if (IsRegister)
            {
                CopyDir(destdircode, sourcedircode);
                DelDir(sourcedircode);
            }

            else
            {
                throw new Exception("用户没有该权限！");
            }


        }

        public void CopyFileLink(string newparentdir, string filecode)
        {

            string sqlStrr = string.Format("select * from VirtualDirRelation where ParentLink = '{0}' and ChildLink = '{1}'", newparentdir, filecode);
                DataSet dss = _isDBUtil.GetDataSet(sqlStrr.ToString());
                if (dss != null && dss.Tables[0] != null && dss.Tables[0].Rows.Count > 0)
                {
                    return;
                    // throw new Exception("该目标文件下存在要移动的文件！"); // break;//存在sourcedir跳出
                }
                else
                {
                    string messaged = GetNameByFileLinkCode(filecode);
                    if (messaged == "")
                    {
                        MessageBox.Show("复制的东西不存在，请重新选择复制！！！");
                        return;
                    }
                    DateTime CreateTime = DateTime.Now;
                    StringBuilder strSql = new StringBuilder();
                    int ID = _isDBUtil.GetMaxID("ID", "VirtualDirRelation");
                    string QRST_CODE = virtualdirrelationtablecode + "-" + ID.ToString();

                    strSql.Append("insert into VirtualDirRelation(");
                    strSql.Append("ID,ParentLink,ChildLink,CreateTime,QRST_CODE)");
                    strSql.Append(" values (");
                    //strSql.Append("@ID,@ParentLink,@ChildLink,@CreateTime,@QRST_CODE)");
                // strSql.AppendFormat(" where ParentLink = '{0}' and ChildLink='{1}' ", oldparentdir, filecode);
                    strSql.Append(string.Format("{0},'{1}','{2}','{3}','{4}')", ID, newparentdir, filecode, CreateTime,
                        QRST_CODE));
                //MySqlParameter[] parameters = {
                //                              new MySqlParameter("@ID", MySqlDbType.Int64,10),                                            
					           //               new MySqlParameter("@ParentLink", MySqlDbType.Text), 
                //                              new MySqlParameter("@ChildLink", MySqlDbType.Text),
                //                              new MySqlParameter("@CreateTime", MySqlDbType.DateTime),
                //                              new MySqlParameter("@QRST_CODE", MySqlDbType.Text)};
                //    parameters[0].Value = ID;
                //    parameters[1].Value = newparentdir;
                //    parameters[2].Value = filecode;
                //    parameters[3].Value = CreateTime;
                //    parameters[4].Value = QRST_CODE;
                    _isDBUtil.ExecuteSql(strSql.ToString());
                    //throw new NotImplementedException();
                }
        }

     
        public void CopyDir(string newparentdir, string dircode)
        {
            if (IsRegister)
            {
                string sourcedirname = GetNameByVirtualDirCode(dircode);
                if (sourcedirname == "")
                {
                    MessageBox.Show("复制的东西不存在，请重新选择复制！！！");
                    return;
                } 
                StringBuilder sqlStrr = new StringBuilder();
                sqlStrr.Append("SELECT virtualdirrelation.ChildLink,virtualdir.`Name` FROM `virtualdirrelation` LEFT JOIN  virtualdir on virtualdirrelation.ChildLink=virtualdir.QRST_CODE");
                sqlStrr.AppendFormat(" where virtualdirrelation.`ParentLink` = '{0}' and virtualdir.`Name`='{1}' ", newparentdir, sourcedirname);
                DataSet dss = _isDBUtil.GetDataSet(sqlStrr.ToString());
                //说明新目录下有原来的目录 然后判断是否要覆盖原来的目录，如果不覆盖就else，覆盖呢就继续执行
                if (dss != null && dss.Tables[0] != null && dss.Tables[0].Rows.Count > 0)
                {
                    string newdirxiasamefilenamecode = "";
                    foreach (DataRow drdd in dss.Tables[0].Rows)
                    {
                        newdirxiasamefilenamecode = drdd["ChildLink"].ToString();
                        break;
                    }
                    if (MessageBox.Show("相同文件夹已经存在，是否合并？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                         //复制的文件夹在newparentdir下有相同的文件夹名
                        #region
                        //List<string> DircodeChildLinklist = Getalldir(dircode);










                        //List<string> SameNameChildLinklist = Getalldir(newdirxiasamefilenamecode);













                        //foreach (string dircodechildlink in DircodeChildLinklist.ToArray())
                        //{
                        //    if (IsDir(dircodechildlink))
                        //    {
                        //        string dircodename = GetNameByVirtualDirCode(dircodechildlink);
                        //        if (IsHas(newdirxiasamefilenamecode, dircodename))
                        //        {
                        //            continue;
                        //        }
                        //        else
                        //        {
                        //            CopyDir(newdirxiasamefilenamecode, dircodechildlink);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        if (IsHasfile(newdirxiasamefilenamecode, dircodechildlink))
                        //        {
                        //            continue;
                        //        }
                        //        else
                        //        {
                        //            CopyFileLink(newdirxiasamefilenamecode, dircodechildlink);
                        //        }
                        //    }
                        //}
                        #endregion
                        #region
                        //string destxiasourcedircode = TrueCopyNewDir(newparentdir, sourcedirname);
                        //List<string> ChildLinklist = Getalldir(dircode);//获取源目录下的所有目录  但是结果有可能是文件或者文件夹 我该怎么做??
                        //foreach (string childlink in ChildLinklist.ToArray())
                        //{
                        //    if (IsDir(childlink))//virtualdirtablecode是ISDB-37-12形式就是文件夹
                        //    {
                        //        //迭代
                        //        CopyDir(destxiasourcedircode, childlink);
                        //    }
                        //    else  //如果遍历是文件夹再次遍历该文件夹
                        //    {
                        //        CopyFileLink(destxiasourcedircode, childlink);

                        //    }
                        //}
                        //List<string> hebingchilelinklist = Getalldir(newdirxiasamefilenamecode);
                        //foreach (string chilelink in hebingchilelinklist)
                        //{
                        //    if (IsDir(chilelink))
                        //    {
                        //        CopyDir(destxiasourcedircode, chilelink);
                        //    }
                        //    else
                        //    {
                        //        CopyFileLink(destxiasourcedircode, chilelink);
                        //    }

                        //}
                        //DelDir(newdirxiasamefilenamecode);
                        ////把复制的文件夹 复制过来后，把同名的文件夹移动到了复制过来的文件夹下了
                        ////MoveDir(newdirxiasamefilenamecode, destxiasourcedircode);
                        #endregion
                        Combine(newdirxiasamefilenamecode, dircode);
                    }
                    else
                    {
                        if (MessageBox.Show("是否继续复制/移动？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            string remarked = GetRemarkByVirtualDirCode(dircode);
                            string destxiasourcedircode =  TrueCopyNewDir(newparentdir, sourcedirname,remarked);
                            List<string> ChildLinklist = Getalldir(dircode);//获取源目录下的所有目录  但是结果有可能是文件或者文件夹 我该怎么做??
                            foreach (string childlink in ChildLinklist.ToArray())
                            {
                                if (IsDir(childlink))//virtualdirtablecode是ISDB-37-12形式就是文件夹
                                {
                                    //迭代
                                    CopyDir(destxiasourcedircode, childlink);
                                }
                                else  //如果遍历是文件夹再次遍历该文件夹
                                {
                                    CopyFileLink(destxiasourcedircode, childlink);

                                }
                            }
                            
                        }
                        else
                        {
                            return;

                        }
                    }
                }
                else
                {
                    //遍历dircode，获得节点列表
                    VirtualDirClass vdcls = new VirtualDirClass(sourcedirname, dircode);
                    vdcls.BuildDir();
                    CopyDir(newparentdir, vdcls);
                }
            }
            else
            {
                return;
            }
        }

        public void CopyDir(string newparentdir, VirtualDirClass vdcls)
        {
            string newdircode = NewDir(newparentdir, vdcls.Name,vdcls.Remark);

     
            foreach (VirtualDirObject childobj in vdcls.ChildObjs)
            {
                if ((childobj as VirtualDirClass) != null)//virtualdirtablecode是ISDB-37-12形式就是文件夹
                {
                    //迭代
                    CopyDir(newdircode, childobj as VirtualDirClass);
                }
                else  //如果遍历是文件夹再次遍历该文件夹
                {
                    CopyFileLink(newdircode, childobj.Code);

                }
            }

        }
        public void Combine(string newcode,string dircode)
        {
            List<string> DircodeChildLinklist = Getalldir(dircode);

            List<string> SameNameChildLinklist = Getalldir(newcode);

            foreach (string dircodechildlink in DircodeChildLinklist.ToArray())



            {
                if (IsDir(dircodechildlink))
                {
                    string dircodename = GetNameByVirtualDirCode(dircodechildlink);
                    if (IsHas(newcode, dircodename))
                    {
                        string childfilecode = GetcodeBydirnameandparentcode(newcode, dircodename);
                        Combine(childfilecode, dircodechildlink);
                    }
                    else
                    {
                        CopyDir(newcode, dircodechildlink);
                    }
                }
                else
                {
                    if (IsHasfile(newcode, dircodechildlink))
                    {
                        continue;
                    }
                    else
                    {
                        CopyFileLink(newcode, dircodechildlink);
                    }
                }
            }
        


        }
        public static bool IsExistDir(string code)
        {
            string sqlStrd = string.Format("select Name from VirtualDir where QRST_CODE= '{0}' ", code);
            DataSet ds = _isDBUtil.GetDataSet(sqlStrd);
            List<string> parentlist = new List<string>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }

            return false;
            
        }

        public static bool IsDir(string code)
        {
            bool isdir = code.StartsWith(virtualdirtablecode);
            return isdir;
        }
        public List<string> GetPlByCl(string code)
        { 
             StringBuilder strSql = new StringBuilder();
            strSql.Append("select ParentLink from VirtualDirRelation ");
            strSql.AppendFormat(" where  ChildLink = '{0}'", code);
            DataSet ds = _isDBUtil.GetDataSet(strSql.ToString());
            List<string> ChildLinklist = new List<string>();
            
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ChildLinklist.Add(ds.Tables[0].Rows[i][0].ToString());
               
            }
            return ChildLinklist;        
        }

        /// <summary>
        /// 判断在一个目录下是否有该name的文件夹
        /// </summary>
        /// <param name="pcode"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public  bool IsHas(string pcode, string name)
        {
            StringBuilder sqlStrr = new StringBuilder();
            sqlStrr.Append("SELECT virtualdirrelation.ChildLink,virtualdir.`Name` FROM `virtualdirrelation` LEFT JOIN  virtualdir on virtualdirrelation.ChildLink=virtualdir.QRST_CODE");
            sqlStrr.AppendFormat(" where virtualdirrelation.`ParentLink` = '{0}' and virtualdir.`Name`='{1}' ", pcode, name);
            DataSet dss = _isDBUtil.GetDataSet(sqlStrr.ToString());
            if (dss != null && dss.Tables[0] != null && dss.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断在一个目录下是否有该clcode的文件
        /// </summary>
        /// <param name="pcode"></param>
        /// <param name="clcode"></param>
        /// <returns></returns>
        public bool IsHasfile(string pcode, string clcode)
        {
            string sqlStrr = string.Format("select * from VirtualDirRelation where ParentLink = '{0}' and ChildLink = '{1}'", pcode, clcode);
            DataSet dss = _isDBUtil.GetDataSet(sqlStrr.ToString());
            if (dss != null && dss.Tables[0] != null && dss.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CopyToDestUserDir(string user, string newparentdir, string dircode)
        {
            if (IsRegister)
            {
                string sourcedirname = GetNameByVirtualDirCode(dircode);
                if (sourcedirname == "")
                {
                    MessageBox.Show("复制的东西不存在，请重新选择复制！！！");
                    return;
                }
                StringBuilder sqlStrr = new StringBuilder();
                sqlStrr.Append("SELECT virtualdirrelation.ChildLink,virtualdir.`Name` FROM `virtualdirrelation` LEFT JOIN  virtualdir on virtualdirrelation.ChildLink=virtualdir.QRST_CODE");
                sqlStrr.AppendFormat(" where virtualdirrelation.`ParentLink` = '{0}' and virtualdir.`Name`='{1}' ", newparentdir, sourcedirname);
                DataSet dss = _isDBUtil.GetDataSet(sqlStrr.ToString());
                if (dss != null && dss.Tables[0] != null && dss.Tables[0].Rows.Count > 0)
                {
                    string newdirxiasamefilenamecode = "";
                    foreach (DataRow drdd in dss.Tables[0].Rows)
                    {
                        newdirxiasamefilenamecode = drdd["ChildLink"].ToString();
                        break;
                    }
                    if (MessageBox.Show("相同文件夹已经存在，是否合并？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Combine(newdirxiasamefilenamecode, dircode);
                    }
                    else
                    {
                        if (MessageBox.Show("是否继续复制/移动？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            string remarked = GetRemarkByVirtualDirCode(dircode);
                            string destxiasourcedircode = MulNewDir(newparentdir, sourcedirname, user, remarked);
                            List<string> ChildLinklist = Getalldir(dircode);//获取源目录下的所有目录  但是结果有可能是文件或者文件夹 我该怎么做??
                            foreach (string childlink in ChildLinklist.ToArray())
                            {
                                if (IsDir(childlink))//virtualdirtablecode是ISDB-37-12形式就是文件夹
                                {
                                    CopyToDestUserDir(user, destxiasourcedircode, childlink);
                                }
                                else  //如果遍历是文件夹再次遍历该文件夹
                                {
                                    CopyFileLink(destxiasourcedircode, childlink);
                                }
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    VirtualDirClass vdcls = new VirtualDirClass(sourcedirname, dircode);
                    vdcls.BuildDir();
                    CopyDir(newparentdir, vdcls,user);
                }
            }
            else
            {
                return;
            }
        }

        public void CopyDir(string newparentdir, VirtualDirClass vdcls,string user)
        {
            string newdircode = MulNewDir(newparentdir, vdcls.Name, user,vdcls.Remark);

            foreach (VirtualDirObject childobj in vdcls.ChildObjs)
            {
                if ((childobj as VirtualDirClass) != null)//virtualdirtablecode是ISDB-37-12形式就是文件夹
                {
                    //迭代
                    CopyDir(newdircode, childobj as VirtualDirClass,user);
                }
                else  //如果遍历是文件夹再次遍历该文件夹
                {
                    CopyFileLink(newdircode, childobj.Code);

                }
            }

        }
        
        /// <summary>
        /// 跨用户间移动文件
        /// </summary>
        public void MultiUserMoveFileLink(string user, string password, string sourcedir, string destdir, string filecode)
        {
            
            if (!IsRegister || !Login(user, password))
            {
                return;
                //throw new Exception("用户不存在！");
            }
            if (IsRegister && Login(user, password))
            {
                string sqlStrr = string.Format("select * from VirtualDirRelation where ParentLink = '{0}' and ChildLink = '{1}'", destdir, filecode);
               DataSet dss = _isDBUtil.GetDataSet(sqlStrr.ToString());
               if (dss != null && dss.Tables[0] != null && dss.Tables[0].Rows.Count > 0)
               {
                  // throw new Exception("目标用户下存在该文件！"); ;//存在sourcedir跳出
                   return;
               }
               else
               {
                 MoveFileLink(sourcedir, destdir, filecode);
               }
            }
           
              
        }

        /// <summary>
        /// 公共方法获取某个文件夹或者文件的父文件夹
        /// </summary>
        /// <param name="onedir"></param>
        /// <returns>父文件夹code</returns>
        public List<string> GetParentDir(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ParentLink from VirtualDirRelation ");
            strSql.AppendFormat(" where  ChildLink = '{0}'", code);
            DataSet ds = _isDBUtil.GetDataSet(strSql.ToString());
            List<string> parentlist = new List<string>();
            if (ds!=null&&ds.Tables.Count>0&&ds.Tables[0].Rows.Count>0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    parentlist.Add(ds.Tables[0].Rows[i][0].ToString());
                }
            }
           
            return parentlist;
        }



        /// <summary>
        /// 公共方法获取某个文件夹下的文件夹或者是文件
        /// </summary>
        /// <param name="onedir"></param>
        /// <returns></returns>
        public List<string> Getalldir(string onedir)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ChildLink from VirtualDirRelation ");
            strSql.AppendFormat(" where  ParentLink = '{0}'", onedir);
            DataSet ds = _isDBUtil.GetDataSet(strSql.ToString());
            List<string> ChildLinklist = new List<string>();
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ChildLinklist.Add(ds.Tables[0].Rows[i][0].ToString());

                }
            }
            return ChildLinklist;        
        }

        /// <summary>
        /// 跨用户间移动文件夹
        /// 返回值为1 成功 0 异常 -1用户1未登录 -2用户2未登录
        /// 吧sourcedircode的文件夹移动到destdircode文件夹下
        /// </summary>


        public int MultiUserMoveDir(string destuser, string password, string newdircode, string dircode)
        {
            //权限判断
            //自己权限判断及目标用户的权限
            if (!IsRegister || !Login(destuser, password))
            {
                return -2;
            }
            if (IsRegister)
            {
                MultiUserCopyDir(destuser, password, newdircode, dircode);
                DelDir(dircode);
                return 1;
            }
            else
            {
                throw new Exception("用户没有该权限！");
            }
        }

        /// <summary>
        /// 跨用户间复制文件
        /// </summary>
        /// <param name="destuser"></param>
        /// <param name="password"></param>
        /// <param name="newparentdir"></param>
        /// <param name="filecode"></param>
        public void MultiUserCopyFileLink(string destuser, string password, string newparentdir, string filecode)
        {
            if (!IsRegister || !Login(destuser, password))
            {
                return;
                //throw new Exception("用户不存在！");
            }
            if (IsRegister && Login(destuser, password))
            {
                string sqlStrr = string.Format("select * from VirtualDirRelation where ParentLink = '{0}' and ChildLink = '{1}'", newparentdir, filecode);
                DataSet dss = _isDBUtil.GetDataSet(sqlStrr.ToString());
                if (dss != null && dss.Tables[0] != null && dss.Tables[0].Rows.Count > 0)
                {
                    // throw new Exception("目标用户下存在该文件！"); ;//存在sourcedir跳出
                    return;
                }
                else
                {
                    CopyFileLink(newparentdir, filecode);
                }
            }
        }

        /// <summary>
        /// 跨用户间复制文件夹
        /// </summary>
        /// <param name="destuser"></param>
        /// <param name="password"></param>
        /// <param name="newparentdir"></param>
        /// <param name="dircode"></param>
        public void MultiUserCopyDir(string destuser, string password, string newparentdir, string dircode)
        {
            if (!IsRegister || !Login(destuser, password))
            {
                throw new Exception("用户不存在！");
            }
            if (IsRegister && Login(destuser, password))
            {
                CopyToDestUserDir(destuser, newparentdir, dircode);
            }
        }

        public void Import()
        {
            //完全没思路
            throw new NotImplementedException();
        }

        public void Export()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 文件夹内检索文件名
        /// </summary>
        /// <param name="dircode"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public DataSet Search(string dircode, string keyword)
        {
            if (IsRegister)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select * from VirtualDirRelation ");
                strSql.AppendFormat(" where  ParentLink = '{0}' and ChildLink = '{1}'", dircode, keyword);
                DataSet ds = _isDBUtil.GetDataSet(strSql.ToString());
                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    return ds;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new Exception("用户没有该权限！");
            }


        }
    }
}
