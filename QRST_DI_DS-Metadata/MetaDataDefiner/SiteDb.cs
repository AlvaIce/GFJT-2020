using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Dal;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataDefiner
{
    /// <summary>
    /// 用于描述各大子库的类
    /// </summary>
    public class SiteDb
    {
        private string _name;
        private string _qrst_code;
        private string _description;
        private string _connectstr;

        private bool _state;  //判断数据库连接状态

        public IDbBaseUtilities sqlUtilities=Constant.IdbServerUtilities;

        public metadatacatalognode_Dal metadatacatalognode;    //元数据树节点表对象
        public metadatacatalognode_r_Dal metadatacatalognode_r;//元数据节点关系表对象
        public tablecode_Dal tablecode;                                         //表编码管理对象
        public table_view_Dal tableview_Dal;                                  //视图管理对象
        public MySqlDB mysqlDb;         //用于读取mysql表结构的对象
        public static ImageList DBTreeImageList = new ImageList();
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name">子库名</param>
        /// <param name="qrst_code">子库编码</param>
        /// <param name="connectionStr">连接字符串</param>
        /// <param name="description">描述</param>
        public SiteDb(string name,string qrst_code,string connectionStr,string description)
        {
            InitImageList();
            _name = name;
            _qrst_code = qrst_code;
            _connectstr = connectionStr;
            _description = description;
            //sqlUtilities = new MySqlBaseUtilities(_connectstr);
            sqlUtilities = sqlUtilities.GetSubDbUtilByCon(_connectstr);

            _state = sqlUtilities.ConnectTest();

            metadatacatalognode = new metadatacatalognode_Dal(sqlUtilities);
            metadatacatalognode_r = new metadatacatalognode_r_Dal(sqlUtilities);
            tablecode = new tablecode_Dal(sqlUtilities);
            tableview_Dal = new table_view_Dal(sqlUtilities);
            mysqlDb = new MySqlDB(sqlUtilities);
        }

        public static void InitImageList()
        {
            if (DBTreeImageList == null)
            {
                DBTreeImageList = new ImageList();
            }

            if (DBTreeImageList.Images.Count == 0)
            {
                DBTreeImageList.Images.AddRange(new System.Drawing.Image[]{
                Properties.Resources.db,
                Properties.Resources.dataset,
                Properties.Resources.raster,
                Properties.Resources.vector,
                Properties.Resources.tile,
                Properties.Resources.table,
                Properties.Resources.file,
                Properties.Resources.spectrum});
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string NAME
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 判断子库状态
        /// </summary>
        public bool STATE
        {
            get { return _state; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QRST_CODE
        {
            set { _qrst_code = value; }
            get { return _qrst_code; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DESCRIPTION
        {
            set { _description = value; }
            get { return _description; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ConnectStr
        {
            set { _connectstr = value; }
            get { return _connectstr; }
        }
        
        /// <summary>
        /// 获取该子库的数据类型节点
        /// </summary>
        /// <returns></returns>
        public TreeNode GetDbNode()
        {
            List<metadatacatalognode_Mdl> mdlLst = metadatacatalognode.GetCatalogGroup(string.Format(" NAME = '{0}'", DESCRIPTION));  //查询根节点

            //没有子节点
            if (mdlLst.Count == 0)
            {
                return null;
            }

            TreeNode tn = new TreeNode() { Text = mdlLst[0].NAME, Tag = mdlLst[0], Name = mdlLst[0].GROUP_CODE ,ImageIndex = 0,SelectedImageIndex = 0 };
            InitializeTree(tn);
            return tn;
        }

        /// <summary>
        /// 初始化数据类型树
        /// </summary>
        /// <param name="tn"></param>
        private void InitializeTree(TreeNode tn)
        {
            string groupCode = tn.Name;
            List<string> childCodes = metadatacatalognode_r.GetGroupChild(tn.Name);

            for (int i = 0 ; i < childCodes.Count ; i++)
            {
                List<metadatacatalognode_Mdl> mdlLst = metadatacatalognode.GetCatalogGroup(string.Format("GROUP_CODE = '{0}'", childCodes[i]));
                if (mdlLst.Count != 0)
                {
                    TreeNode tn1 = new TreeNode() { Text = mdlLst[0].NAME, Tag = mdlLst[0], Name = mdlLst[0].GROUP_CODE };
                    if (mdlLst[0].IS_DATASET)
                    {
                        tn1.ImageIndex = 1;
                        tn1.SelectedImageIndex = 1;
                    }
                    else
                    {
                        //矢量数据，特殊处理，修改时间：2013/1/8
                        if (mdlLst[0].GROUP_TYPE == EnumDataKind.System_Vector.ToString())
                        {
                            tn1.ImageIndex = 3;
                            tn1.SelectedImageIndex = 3;
                        }
                       else if (mdlLst[0].GROUP_TYPE == EnumDataKind.System_Tile.ToString())
                            {
                                tn1.ImageIndex = 4;
                                tn1.SelectedImageIndex = 4;
                            }
                        else
                        {
                            tn1.ImageIndex = 2;
                            tn1.SelectedImageIndex = 2;
                        } 
                    }
                    tn.Nodes.Add(tn1);
                    InitializeTree(tn1);
                }
           
            }

        }

         /// <summary>
        /// 定义一种元数据类型，
        /// 1.将数据类型丢到metadatacatalognode中
        /// 2.将元数据节点关系丢到metadatacatalognode_r中
        /// </summary>
        /// <param name="child"></param>
        /// <param name="parent">该元数据的父类</param>
        public void AddMetadata(metadatacatalognode_Mdl child, metadatacatalognode_Mdl parent)
         {
             metadatacatalognode.Add(child);

            metadatacatalognode_r_Mdl  metadatacatalognode_r_mdl = new metadatacatalognode_r_Mdl();
             metadatacatalognode_r_mdl.GROUP_CODE = parent.GROUP_CODE;
             metadatacatalognode_r_mdl.CHILD_CODE = child.GROUP_CODE;

            metadatacatalognode_r.Add(metadatacatalognode_r_mdl);
         }

        /// <summary>
        /// 根据表名获取表结构对象
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public TABLES_Mdl GetTableMdl(string tableName)
        {
           // sqlUtilities.GetDataSet()
           return  mysqlDb.GetTableModel(NAME, tableName);
            
        }

        public TABLES_Mdl GetTableMdlByCode(string tableCode)
        {
            // sqlUtilities.GetDataSet()
            string tableName = tablecode.GetTableName(tableCode);
            return mysqlDb.GetTableModel(NAME, tableName);

        }

        /// <summary>
        /// 将新添加的表注册到table_code中，QRST_CODE自动生成
        /// </summary>
        /// <param name="model"></param>
        public tablecode_Mdl RegistTable(tablecode_Mdl model)
        {
            model.QRST_CODE = String.Format("{0}-{1}", NAME, tablecode.GetMaxID());
            tablecode.Add(model);
            return model;
        }

        /// <summary>
        /// 注册产品类型
        /// </summary>
        public void  RegistProds(tablecode_Mdl mdl,string kindStr)
        {
            EnumDataKind kind;
            Enum.TryParse(kindStr, out kind);
            string prodTable = GetPro_Table(kind);
            //TableLocker dblock = new TableLocker(sqlUtilities);
            Constant.IdbOperating.LockTable(prodTable,EnumDBType.MIDB);
            int maxId = sqlUtilities.GetMaxID("ID",prodTable);
            string insertStr = string.Format("insert into {0} (ID,NAME,DESCRIPTION,QRST_CODE) values ({1},'{2}','{3}','{4}')",prodTable,maxId,mdl.TABLE_NAME,mdl.DESCRIPTION,mdl.QRST_CODE);
            sqlUtilities.ExecuteSql(insertStr);
            Constant.IdbOperating.UnlockTable(prodTable,EnumDBType.MIDB);
        }

        /// <summary>
        /// 删除数据节点,
        /// 1.删除该节点下的所有数据类型
        /// 2.删除所有数据类型的表信息
        /// </summary>
        public void DeleteNode(metadatacatalognode_Mdl mdl)
        {
            if (mdl.IS_DATASET)
            {
                List<metadatacatalognode_Mdl> lst = GetAllChildNode(mdl.GROUP_CODE);
                foreach(metadatacatalognode_Mdl node in lst)
                {
                    DeleteNode(node);
                }
            }
            else
            {
                if (mdl.GROUP_TYPE != EnumDataKind.System_Vector.ToString()&& mdl.GROUP_TYPE != EnumDataKind.System_Tile.ToString())
                {
                    string tablename = tablecode.GetTableName(mdl.DATA_CODE);
                    EnumDataKind dataKind;
                    Enum.TryParse(mdl.GROUP_TYPE, out dataKind);
                    DeleteDataTable(tablename, dataKind);
                }
                else //矢量数据，删除表prods_vector表中对应的记录
                {
                    if (mdl.GROUP_TYPE == EnumDataKind.System_Vector.ToString())
                    {
                        string delSql = string.Format("delete from prods_vector where GROUPCODE = '{0}'", mdl.GROUP_CODE);
                        sqlUtilities.ExecuteSql(delSql);
                    }
                }
            }
            //删除数据树结构关系
            metadatacatalognode_r.Delete(string.Format("child_code = '{0}'",mdl.GROUP_CODE));
            metadatacatalognode.Delete(string.Format("group_code = '{0}'", mdl.GROUP_CODE));
        }

        /// <summary>
        /// 获取指定groupCode的子节点
        /// </summary>
        /// <param name="groupCode"></param>
        /// <returns></returns>
        public List<metadatacatalognode_Mdl> GetAllChildNode(string groupCode)
        {
            List<metadatacatalognode_Mdl> childNodeMdl = new List<metadatacatalognode_Mdl>();
            List<string> childGroupCode = metadatacatalognode_r.GetGroupChild(groupCode);

            for (int i = 0 ; i < childGroupCode.Count ;i++)
            {
                metadatacatalognode_Mdl mdl = metadatacatalognode.GetCatalogGroup(string.Format(" group_code = '{0}'", childGroupCode[i]))[0];
                childNodeMdl.Add(mdl);
            }
            return childNodeMdl;
        }

        
        /// <summary>
        /// 删除一张数据表
        /// 1.删除关系表、视图
        /// 2.注销tablecode编码
        /// 3.删除tableview中表相关记录
        /// 4.删除prods_中记录的
        /// </summary>
        /// <param name="tableName"></param>
        public void DeleteDataTable(string tableName, EnumDataKind kind)
        {
            string delViewStr = string.Format("drop view if exists  {0}",tableName+"_"+"view");
            string delTableStr = string.Format("drop table if exists  {0} ",tableName);

            //删除视图与表
            sqlUtilities.ExecuteSql(delViewStr);
            sqlUtilities.ExecuteSql(delTableStr);

            //注销tablecode代码
            tablecode.Delete(string.Format("table_name = '{0}'", tableName));
            //删除tableview中的相关记录
            tableview_Dal.Delete(string.Format("table_name = '{0}'",tableName));
            //删除prods_中相关记录
            string prodTableName = GetPro_Table(kind);
            sqlUtilities.ExecuteSql(string.Format("delete from {0} where name = '{1}'",prodTableName,tableName));
            
        }
        /// <summary>
        /// 获取表的母表
        /// </summary>
        private string GetPro_Table(EnumDataKind kind )
        {
            switch(kind)
            {
                case EnumDataKind.System_Document:
                    return "prods_doc";
                    break;
                case EnumDataKind.System_Raster:
                    return "prods_raster";
                    break;
                case EnumDataKind.System_Table:
                    return "prods_table";
                        break;
                case EnumDataKind.System_Vector:
                        return "prods_vector";
                        break;
                default:
                        return "";
                        break;
            }
        }

        /// <summary>
        /// 获取数据库中的所有数据种类记录数
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetDataCount()
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            //获取拥有元数据表的节点
            List<metadatacatalognode_Mdl> mdlLst = metadatacatalognode.GetCatalogGroup("data_code is not null and data_code !=''");
            for (int i = 0 ; i < mdlLst.Count ;i++ )
            {
                string dataName = mdlLst[i].NAME;
                string tableName = tablecode.GetTableName(mdlLst[i].DATA_CODE);
                int recordCount = 0;
                if (mdlLst[i].GROUP_TYPE == EnumDataKind.System_Vector.ToString())
                {
                    recordCount = GetVectorDataCount(mdlLst[i]);
                }
                else if (mdlLst[i].GROUP_TYPE == EnumDataKind.System_Tile.ToString())
                    {
                    //再讨论
                        recordCount = 0;
                    }
                else
                {
                      recordCount = sqlUtilities.GetRecordCount("*", tableName, "");
                }
                dic.Add(dataName, recordCount);
            }
            return dic;
        }

        /// <summary>
        /// 获取矢量数据类型的数量
        /// </summary>
        /// <param name="mdl"></param>
        /// <returns></returns>
        public int GetVectorDataCount(metadatacatalognode_Mdl mdl)
        {
            if (mdl == null)
            {
                throw new NullReferenceException("数据类型不能为空！");
            }
            else if (mdl.GROUP_TYPE == EnumDataKind.System_Vector.ToString())
            {
                string tableName = tablecode.GetTableName(mdl.DATA_CODE);
                return sqlUtilities.GetRecordCount("*", tableName, string.Format(" where GROUPCODE = '{0}'",mdl.GROUP_CODE));
            }
            else
            {
                throw new Exception("不是矢量数据类型");
            }
        }

        /// <summary>
        /// 获取数据库总记录数
        /// </summary>
        /// <returns></returns>
        public int GetTotalRecord()
        {
            int num = 0;
            Dictionary<string, int> dic = GetDataCount();
          foreach(var item in dic)
          {
              num = num + item.Value;
          }
            return num;
        }

        /// <summary>
        /// 获取特定数据种类的数据
        /// </summary>
        /// <param name="datakind"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetSpecificDataCount(EnumDataKind datakind)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            
            if (datakind != EnumDataKind.System_DataSet)
            {
                //获取拥有元数据表的节点
                List<metadatacatalognode_Mdl> mdlLst = metadatacatalognode.GetCatalogGroup(string.Format("data_code is not null and group_type = '{0}'",datakind.ToString()));
                for (int i = 0 ; i < mdlLst.Count ; i++)
                {
                    string dataName = mdlLst[i].NAME;
                    string tableName = tablecode.GetTableName(mdlLst[i].DATA_CODE);
                    int recordCount;
                    if (mdlLst[i].GROUP_TYPE == EnumDataKind.System_Vector.ToString())
                    {
                        recordCount = sqlUtilities.GetRecordCount("*", tableName, string.Format(" where GROUPCODE = '{0}'", mdlLst[i].GROUP_CODE));
                    }
                    else if (mdlLst[i].GROUP_TYPE == EnumDataKind.System_Tile.ToString())  //切片
                   {
                        //再讨论吧
                       recordCount = 0;
                   }
                    else
                    {
                        try
                        {
                            recordCount = sqlUtilities.GetRecordCount("*", tableName, "");
                        }
                        catch (Exception)
                        {
                            recordCount = 0;
                        }
                    }
                    dic.Add(dataName, recordCount);
                }
                return dic;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取特定数据种类的数据的总记录数
        /// </summary>
        /// <returns></returns>
        public int GetSpecificDataRecord(EnumDataKind datakind)
        {
            int num = 0;
            Dictionary<string, int> dic = GetSpecificDataCount(datakind);
            foreach (var item in dic)
            {
                num = num + item.Value;
            }
            return num;
        }

        /// <summary>
        ///基础空间数据临时使用，段龙方 2013-1-8 添加
        /// </summary>
        /// <param name="DataCode"></param>
        /// <param name="DataName"></param>
        /// <returns></returns>
        public string GetGroupCodebyName(string DataCode,string DataName)
        {
            string strGroupCode = string.Empty;

            string tableName = "metadatacatalognode";
            //string tableViewName = tablecode.GetTableName(DataCode) + "_view";
            string strSql = string.Format("select GROUP_CODE from {0} where NAME = '{1}'", tableName, DataName);
            DataSet ds = sqlUtilities.GetDataSet(strSql);
            if (ds.Tables.Count!=0&&ds.Tables[0].Rows.Count>0)
            {
                strGroupCode = ds.Tables[0].Rows[0]["GROUP_CODE"].ToString();
            }

            return strGroupCode;
        }
    }
}
