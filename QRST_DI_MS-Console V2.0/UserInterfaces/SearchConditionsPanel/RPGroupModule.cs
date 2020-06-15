using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.Repository;
using System.IO;
using System.Drawing.Imaging;
 
namespace QRST_DI_MS_Desktop.UserInterfaces.SearchConditionsPanel
{
    public partial class RPGroupModule : UserControl
    {
        DataSet _modularDataSet;
        DataSet _systemDataSet;
        DataTable _dtSysDistinct;
        DataTable _dtSubsysDistinct;
        DataTable _dtContainerDistinct;
        mucDetailViewer _mucDetail;
        public static Dictionary<string, string> SysColumnChar;
        public static Dictionary<string, string> ModuleColumnChar;
        string _searchType;
        public RPGroupModule()
        {
            //    模块名称
            //    模块说明
            //版本
            //系统最低版本要求
            //类名
            //所属动态链接库
            //所属容器名
            //容器类型
            //所属子系统
            //所属系统
            InitializeComponent();
            ModuleColumnChar = new Dictionary<string, string>();
            ModuleColumnChar.Add("模块名称", "NAME");
            ModuleColumnChar.Add("模块说明", "INSTRUCTION");
            ModuleColumnChar.Add("版本", "VERSION");
            ModuleColumnChar.Add("系统最低版本要求", "VERSIONLEAST");
            ModuleColumnChar.Add("类名", "CLASSNAME");
            ModuleColumnChar.Add("所属动态链接库", "DLLNAME");
            ModuleColumnChar.Add("所属容器名", "CONTAINERNAME");
            ModuleColumnChar.Add("容器类型", "CONTAINERTYPE");
            ModuleColumnChar.Add("所属子系统", "SUBSYSTEM");
            ModuleColumnChar.Add("所属系统", "SYSTEM");
            SysColumnChar = new Dictionary<string, string>();
            SysColumnChar.Add("编号", "ID");
            SysColumnChar.Add("中文名", "CHNAME");
            SysColumnChar.Add("英文名", "ENNAME");
            SysColumnChar.Add("版本", "VERSION");
            SysColumnChar.Add("说明", "CONTENT");
            SysColumnChar.Add("图示名", "IMAGENAME");
            SysColumnChar.Add("图示", "IMAGEDATA");
            
            barEdit_ModuleSys.EditValueChanged += new EventHandler(repositoryItemComboBox2_SelectedIndexChanged);
            barEdit_ModuleSubsys.EditValueChanged += new EventHandler(repositoryItemComboBox3_SelectedIndexChanged);
        }

        void repositoryItemComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateContainerSelecter(barEdit_ModuleSys.EditValue.ToString(),barEdit_ModuleSubsys.EditValue.ToString());
        }

        void repositoryItemComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdataSubsystemSelecter(barEdit_ModuleSys.EditValue.ToString());
            UpdateContainerSelecter(barEdit_ModuleSys.EditValue.ToString(), barEdit_ModuleSubsys.EditValue.ToString());
        }


        public void UpdateModularDataSet()
        {
            _modularDataSet = TheUniversal.MADB.sqlUtilities.GetDataSet("Select * from mould_gias");
            _systemDataSet = TheUniversal.MADB.sqlUtilities.GetDataSet("Select * from mould_sys");
            DataView dataView = _modularDataSet.Tables[0].DefaultView;
             _dtSysDistinct = dataView.ToTable(true, "SYSTEM");
             _dtSubsysDistinct = dataView.ToTable(true, "SYSTEM", "SUBSYSTEM");
             _dtContainerDistinct = dataView.ToTable(true, "SYSTEM", "SUBSYSTEM", "CONTAINERNAME");

        }

        public void SetSearchViewByType(string searchtypename, mucDetailViewer mucDV)
        {
            _searchType = searchtypename;
            _mucDetail = mucDV;
            //DevExpress.XtraGrid.GridControl gridctrl = _mucDetail.gridControlDataList;
            //((DevExpress.XtraGrid.Views.Grid.GridView)gridctrl.MainView).RowHeight = 32;
            //set conditions
            switch (_searchType)
            {
                case "模块控件":
                    this.barEdit_ModuleKeyword.Visibility = BarItemVisibility.Always;
                    this.barEdit_ModuleType.Visibility = BarItemVisibility.Always;
                    this.barEdit_ModuleSys.Visibility = BarItemVisibility.Never;
                    this.barEdit_ModuleSubsys.Visibility = BarItemVisibility.Never;
                    this.barEdit_ModuleContainer.Visibility = BarItemVisibility.Never;
                    UpdateModuleContainer();
                    break;
                case "系统构件":
                    this.barEdit_ModuleKeyword.Visibility = BarItemVisibility.Always;
                    this.barEdit_ModuleType.Visibility = BarItemVisibility.Always;
                    this.barEdit_ModuleSys.Visibility = BarItemVisibility.Never;
                    this.barEdit_ModuleSubsys.Visibility = BarItemVisibility.Never;
                    this.barEdit_ModuleContainer.Visibility = BarItemVisibility.Never;
                    UpdateSystemContainer();
                    break;
                case "系统构建信息":
                    this.barEdit_ModuleKeyword.Visibility = BarItemVisibility.Never;
                    this.barEdit_ModuleType.Visibility = BarItemVisibility.Never;
                    this.barEdit_ModuleSys.Visibility = BarItemVisibility.Always;
                    this.barEdit_ModuleSubsys.Visibility = BarItemVisibility.Always;
                    this.barEdit_ModuleContainer.Visibility = BarItemVisibility.Always;
                    UpdateSystemInfoContainer();
                    break;
                default:
                    break;
            }

            //set detail view
            _mucDetail.SwitchDisplaySchema("onlytable");
        }

        private void UpdateSystemInfoContainer()
        {
            repositoryItemComboBox2.Items.Clear();
            repositoryItemComboBox2.Items.Add("全部");
            foreach (DataRow dr in _dtSysDistinct.Rows)
            {
                repositoryItemComboBox2.Items.Add(dr[0].ToString());
            }
            barEdit_ModuleSys.EditValue = repositoryItemComboBox2.Items[0];
        }

        private void UpdateModuleContainer()
        {
            repositoryItemComboBox1.Items.Clear();
            foreach (string key in ModuleColumnChar.Keys)
            {
                repositoryItemComboBox1.Items.Add(key);
            }
            barEdit_ModuleKeyword.EditValue = "";
            barEdit_ModuleType.EditValue = repositoryItemComboBox1.Items[0];
        }

        private void UpdateSystemContainer()
        {
            repositoryItemComboBox1.Items.Clear();
            repositoryItemComboBox1.Items.Add("中文名");
            repositoryItemComboBox1.Items.Add("英文名");
            repositoryItemComboBox1.Items.Add("版本");
            repositoryItemComboBox1.Items.Add("说明");

            barEdit_ModuleKeyword.EditValue = "";
            barEdit_ModuleType.EditValue = repositoryItemComboBox1.Items[0];
        }

        private void UpdataSubsystemSelecter(string sysname)
        {
            repositoryItemComboBox3.Items.Clear();
            repositoryItemComboBox3.Items.Add("全部");
            foreach (DataRow dr in _dtSubsysDistinct.Rows)
            {
                if (sysname == "全部" || sysname == dr[0].ToString())
                {
                    repositoryItemComboBox3.Items.Add(dr[1].ToString());
                }
            }
            barEdit_ModuleSubsys.EditValue = repositoryItemComboBox3.Items[0];
        }

        private void UpdateContainerSelecter(string sysname,string subsysname)
        {
            repositoryItemComboBox4.Items.Clear();
            repositoryItemComboBox4.Items.Add("全部");
            foreach (DataRow dr in _dtContainerDistinct.Rows)
            {
                if ((sysname == "全部" && subsysname == "全部") ||
                    (sysname == "全部" && subsysname == dr[1].ToString()) ||
                    (sysname == dr[0].ToString() && subsysname == "全部") ||
                    (sysname == dr[0].ToString() && subsysname == dr[1].ToString()))
                {
                    repositoryItemComboBox4.Items.Add(dr[2].ToString());
                }
            }
           barEdit_ModuleContainer.EditValue = repositoryItemComboBox4.Items[0];
        }

        public static Image FromBlobObject(byte[] imageByte)
        {
            //byte[] imageByte = (byte[])dr[0];   
            //实列化数据流imageStream 
            MemoryStream imageStream = new MemoryStream(imageByte); //二进制流数据重新转成图片  
            Image image = Image.FromStream(imageStream);
            return image;
        }

        public static byte[] FromImage(Image img, ImageFormat imgfmt)
        {
            MemoryStream imageStream = new MemoryStream();   
            img.Save(imageStream, imgfmt); 
            byte[] imageByte = imageStream.ToArray();
            return imageByte;
        }

        public void Search()
        {
            DevExpress.XtraGrid.GridControl gridctrl = _mucDetail.gridControlDataList;
            object rstObj = SearchData();
            gridctrl.DataSource = null;
            gridctrl.DataSource = rstObj;
            ((DevExpress.XtraGrid.Views.Grid.GridView)gridctrl.MainView).RowHeight = 32;
            ((DevExpress.XtraGrid.Views.Grid.GridView)gridctrl.MainView).PopulateColumns();

            foreach (DevExpress.XtraGrid.Columns.GridColumn col in ((DevExpress.XtraGrid.Views.Grid.GridView)gridctrl.MainView).Columns)
            {
                if (col.ColumnType == typeof(Image))
                {
                    RepositoryItemPictureEdit riPicEdit=new RepositoryItemPictureEdit();
                    riPicEdit.SizeMode= DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
                    col.ColumnEdit = riPicEdit;
                }
            }
            ((DevExpress.XtraGrid.Views.Grid.GridView)gridctrl.MainView).BestFitColumns();
            //DataRow dr = ((DevExpress.XtraGrid.Views.Grid.GridView)gridctrl.MainView).cell;
            //for (int i = 0; i < dr.ItemArray.Length; i++)
            //{
            //    if (dr[i] is Image)
            //    {
            //        DevExpress.XtraGrid.Columns.GridColumn col = ((DevExpress.XtraGrid.Views.Grid.GridView)gridctrl.MainView).Columns[i];
            //        col.ColumnEdit = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            //    }

            //}
        }

        private object SearchData()
        {
            switch (_searchType)
            {
                case "系统构件":
                    List<SystemSearchObject> rstobjs = new List<SystemSearchObject>();
                    string keywordsys = barEdit_ModuleKeyword.EditValue.ToString().Trim();
                    List<DataRow> drs = new List<DataRow>();
                    string filterExpress = "";
                    if (keywordsys != "")
                    {
                        filterExpress = string.Format("{0} like '%{1}%'", ModuleColumnChar[barEdit_ModuleType.EditValue.ToString()], keywordsys);
                    }
                    else
                    {
                        filterExpress = "id > 0";
                    }
                    drs.AddRange(_systemDataSet.Tables[0].Select(filterExpress, "CHNAME asc"));
                    List<DataRow> groupdrs = new List<DataRow>();
                    string mname = "";
                    foreach (DataRow dr in drs)
                    {
                        if (mname == "")
                        {
                            mname = dr["CHNAME"].ToString();
                            groupdrs.Add(dr);
                        }
                        else
                        {
                            if (dr["CHNAME"].ToString() == mname)
                            {
                                groupdrs.Add(dr);
                            }
                            else
                            {
                                rstobjs.Add(new SystemSearchObject(groupdrs));
                                groupdrs.Clear();
                                mname = dr["CHNAME"].ToString();
                                groupdrs.Add(dr);
                            }
                        }
                    }
                    rstobjs.Add(new SystemSearchObject(groupdrs));
                    groupdrs.Clear();
                    return rstobjs;
                case "模块控件":
                    List<ModuleSearchObject> rstobjs2 = new List<ModuleSearchObject>();
                    string keyword = barEdit_ModuleKeyword.EditValue.ToString().Trim();
                    List<DataRow> drs2 = new List<DataRow>();
                    string filterExpress2 = "";
                    if (keyword != "")
                    {
                        filterExpress2 = string.Format("{0} like '%{1}%'", ModuleColumnChar[barEdit_ModuleType.EditValue.ToString()], keyword);
                    }
                    else
                    {
                        filterExpress2 = "id > 0";
                    }
                    drs2.AddRange(_modularDataSet.Tables[0].Select(filterExpress2, "NAME asc"));
                    List<DataRow> groupdrs2 = new List<DataRow>();
                    string mname2 = "";
                    foreach (DataRow dr in drs2)
                    {
                        if (mname2 == "")
                        {
                            mname2 = dr["NAME"].ToString();
                            groupdrs2.Add(dr);
                        }
                        else
                        {
                            if (dr["NAME"].ToString() == mname2)
                            {
                                groupdrs2.Add(dr);
                            }
                            else
                            {
                                rstobjs2.Add(new ModuleSearchObject(groupdrs2));
                                groupdrs2.Clear();
                                mname2 = dr["NAME"].ToString();
                                groupdrs2.Add(dr);
                            }
                        }
                    }
                    rstobjs2.Add(new ModuleSearchObject(groupdrs2));
                    groupdrs2.Clear();
                    return rstobjs2;
                case "系统构建信息":
                    List<SystemInfoSearchObject> rstsysobjs = new List<SystemInfoSearchObject>();
                    string Syskeyword = barEdit_ModuleSys.EditValue.ToString();
                    string Subsyskeyword = barEdit_ModuleSubsys.EditValue.ToString();
                    string Containerkeyword = barEdit_ModuleContainer.EditValue.ToString();
                    string SysFilter = (Syskeyword != "全部") ? string.Format("SYSTEM = '{0}'", Syskeyword) : "";
                    string SubsysFilter = (Subsyskeyword != "全部") ? string.Format("SUBSYSTEM = '{0}'", Subsyskeyword) : "";
                    string ContainerFilter = (Containerkeyword != "全部") ? string.Format("CONTAINERNAME = '{0}'", Containerkeyword) : "";
                    filterExpress = string.Format("{0}{1}{2}{3}{4}",
                        SysFilter,
                        (SysFilter != "") ? " and " : "",
                        SubsysFilter,
                        (SubsysFilter != "") ? " and " : "",
                        ContainerFilter);
                    filterExpress = (filterExpress.EndsWith("and ")) ? filterExpress.Substring(0,filterExpress.Length-4) : filterExpress;
                    DataRow[] ddrs = _modularDataSet.Tables[0].Select(filterExpress, "SYSTEM asc, SUBSYSTEM asc, CONTAINERNAME asc");

                    foreach (DataRow dr in ddrs)
                    {
                        rstsysobjs.Add(new SystemInfoSearchObject(dr));
                    }
                    return rstsysobjs;
                default:
                    break;
            }
            return null;
        }

    }


}
