using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System.IO;
using QRST_DI_MS_Basis.QueryBase;
using SharpMap.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner.Mdl;
using QRST_DI_DS_MetadataQuery.QueryConditionParameter;
using System.Text.RegularExpressions;
using QRST_DI_Resources;
using QRST_DI_DS_Metadata.MetaDataCls;
using System.Net;
using DevExpress.XtraCharts;
using QRST_DI_TS_Basis.DirectlyAddress;
//using QRST_DI_MS_Desktop.WS_QDB_Searcher_MySQL;
using QRST.WorldGlobeTool;
using QRST.WorldGlobeTool.Geometries;
using QRST_DI_MS_Component.VirtualDirUI;
using DevComponents.AdvTree;
using QRST_DI_MS_Component.Common;
using QRST_DI_DS_MetadataQuery.JSONutilty;
using QRST_DI_SS_DBInterfaces.IDBService;
using System.Configuration;

namespace QRST_DI_MS_Desktop.UserInterfaces
{
    public partial class mucDetailViewer : DevExpress.XtraEditors.XtraUserControl
    {
        ruc3DSearcher ruc3DSear;
        const string AddTileTinyView = "Ĵָͼ";
        #region ���������ֶ�
        public static VirtualDirSelector _VirtualDirSelector;

        public metadatacatalognode_Mdl _selectedQueryObj;
        public static DataTable tempTable = null;
        public EnumDisplayEnum displaySchema;                              //��ʾģʽ
        public metadatacatalognode_Mdl selectedQueryObj                   //�洢��Ҫ��ѯ���������Ͷ��� zxw 2013/08/08
        {
            get
            {
                return _selectedQueryObj;
            }
            set
            {
                _selectedQueryObj = value;
                this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                switch (_selectedQueryObj.GROUP_TYPE.ToLower())
                {
                    case "system_vector":
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(MainMapImage);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonZoomIn);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonZoomOut);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonPan);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonFullExtent);
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(labelControlPosition);
                        //�����������Ϣ��ʾ
                        setTileSinglePanel2();
                        SwitchDisplaySchema("normal");       //zxw 20131221 �Զ��л�������ģʽ
                        break;
                    case "system_raster":
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                        setTileSinglePanel2();
                        SwitchDisplaySchema("normal");       //zxw 20131221 �Զ��л�������ģʽ
                        break;
                    case "system_document":
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                        setTileSinglePanel2();
                        SwitchDisplaySchema("alltable");       //zxw 20131221 �Զ��л���ȫ��ģʽ
                        break;
                    case "system_table":
                        if (_selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")
                        {
                            this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.chartControl1);
                            setTileSinglePanel2();
                            SwitchDisplaySchema("normal");       //zxw 20131221 �Զ��л�������ģʽ
                        }
                        else
                        {
                            this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                            setTileSinglePanel2();
                            SwitchDisplaySchema("alltable");       //zxw 20131221 �Զ��л���ȫ��ģʽ
                        }
                        break;
                    case "system_tile":
                        this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                        this.pictureEditImgData.Dock = DockStyle.Fill;
                        //setTileSinglePanel2();
                        SwitchDisplaySchema("normal");       //zxw 20131221 �Զ��л�������ģʽ
                        break;
                    default:
                        break;
                }
            }
        }
         string ChooseDataColumnCaption
        {
            get
            {
                string caption = "";
                switch (Constant.SystemLanguage)
                {
                    case EnumLanguage.ch:
                        caption = "select the data";
                        break;
                    case EnumLanguage.en:
                        caption = "ѡ������";
                        break;
                }
                return caption;

            }
        }
        public QueryPara queryPara;

 /*       WS_QDB_GetDataService.Service getDataclient;    */                 //��Ƭ���ݻ�ȡ�������
        private static IQDB_GetData getDataclient = Constant.IGetDataService;
        private static IQDB_Searcher_Db searcherDbClient = Constant.ISearcherDbServ;
        #endregion

        SharpMap.Forms.MapImage MainMapImage = new SharpMap.Forms.MapImage();

        public mucDetailViewer()
        {
            InitializeComponent();

            //zxw ��Ӷ�ctrlImageEditControl�ĳ�ʼ��  20131221
            this.pictureEditImgData = new QRST_DI_MS_Desktop.UserInterfaces.CtrlImageDisplay(this);
            this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
            this.pictureEditImgData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureEditImgData.Image = null;
            this.pictureEditImgData.Location = new System.Drawing.Point(0, 0);
            this.pictureEditImgData.Name = "pictureEditImgData";
            this.pictureEditImgData.Size = new System.Drawing.Size(881, 396);//381, 396
            this.pictureEditImgData.TabIndex = 11;
             

            MainMapImage.Name = "VectorViewer";
            MainMapImage.Dock = DockStyle.Fill;
            MainMapImage.BackColor = Color.White;
            MainMapImage.MouseMove += MainMapImage_MouseMove;
            MainMapImage.MouseLeave += MainMapImage_MouseLeave;
        }
        /// <summary>
        /// 20170315
        /// ����splitContainerControl ��panel1����ʾ������panel2��
        /// </summary>
        public void setTileSinglePanel2()
        {
            this.splitContainerControl1.PanelVisibility = SplitPanelVisibility.Panel1;
        }
        /// <summary>
        ///��Ƭȫ����ʱ����ʾ
        /// </summary>
        public void setTileSingleBothPanel()
        {
            this.splitContainerControl1.PanelVisibility = SplitPanelVisibility.Both;
        }

        private void MainMapImage_MouseMove(SharpMap.Geometries.Point WorldPos, MouseEventArgs ImagePos)
        {
            labelControlPosition.Text = String.Format("���λ�ã�{0:N5}, {1:N5}", WorldPos.X, WorldPos.Y);
        }

        private void MainMapImage_MouseLeave(object sender, EventArgs e)
        {
            labelControlPosition.Text = "";
        }
        /// <summary>
        /// ������ص�ʱ���ȡ�����������Ͷ�Ӧ��  ��Ե�ַ�� ���ɾ��Ե�ַ���ֶ��б�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mucDetailViewer_Load(object sender, EventArgs e)
        {
            //DLF20130830 ��ӡ�������⣺��Ƭ��ѯʱ����һ�β�ѯ�õ����ݣ��ڶ��β�ѯ�ò�������ʱ����ϸ��Ϣ���δ��ա�
            textDetailnfo.Text = string.Empty;

            SwitchDisplaySchema("alltable");
        }
        /// <summary>
        /// �Զ���ǰ��ֶε����ݣ����¼��� load�¼�֮�󴥷�ִ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMain_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {

        }



        /// <summary>
        /// ���·ָ����ؼ���С�ı��¼����������·ָ�����λ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainerControlTableViewer_SizeChanged(object sender, EventArgs e)
        {
            //if (this.splitContainerControlTableViewer.Height > 600)
            //{
            //    this.splitContainerControlTableViewer.SplitterPosition = this.splitContainerControlTableViewer.Height - 300;
            //}
            //else
            //{
            //    this.splitContainerControlTableViewer.SplitterPosition = this.splitContainerControlTableViewer.Height / 2;
            //}
        }
        /// <summary>
        /// ���ҷָ����ؼ���С�ı��¼����������ҷָ�����λ��
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainerControlImgAndDetail_SizeChanged(object sender, EventArgs e)
        {
            //zxw 20131221 ɾ�����Զ���������
            //if (this.splitContainerControlImgAndDetail.Width>800)
            //{
            //    this.splitContainerControlImgAndDetail.SplitterPosition = this.splitContainerControlImgAndDetail.Width - 400;
            //}
            //else
            //{
            //    this.splitContainerControlImgAndDetail.SplitterPosition = this.splitContainerControlImgAndDetail.Width / 2;
            //}
            if (displaySchema == EnumDisplayEnum.AllPicture)
            {
            }
            else
            {
            }

            if (MainMapImage.Map.Layers.Count > 0)
            {
                MainMapImage.Map.ZoomToExtents();
                //MainMapImage.Refresh();
                simpleButtonZoomIn.BringToFront();
                simpleButtonZoomOut.BringToFront();
                simpleButtonPan.BringToFront();
                simpleButtonFullExtent.BringToFront();
            }

        }

        void SizeToNomal()
        {

            if (this.splitContainerControlTableViewer.Height > 600)
            {
                this.splitContainerControlTableViewer.SplitterPosition = this.splitContainerControlTableViewer.Height - 300;
            }
            else
            {
                this.splitContainerControlTableViewer.SplitterPosition = this.splitContainerControlTableViewer.Height / 2;
            }
            if (this.splitContainerControlImgAndDetail.Width > 800)
            {
                this.splitContainerControlImgAndDetail.SplitterPosition = this.splitContainerControlImgAndDetail.Width - 400;
            }
            else
            {
                this.splitContainerControlImgAndDetail.SplitterPosition = this.splitContainerControlImgAndDetail.Width / 2;
            }

            if (MainMapImage.Map.Layers.Count > 0)
            {
                MainMapImage.Map.ZoomToExtents();
                MainMapImage.Refresh();
                simpleButtonZoomIn.BringToFront();
                simpleButtonZoomOut.BringToFront();
                simpleButtonPan.BringToFront();
                simpleButtonFullExtent.BringToFront();
            }
        }
        /// <summary>
        /// �е����¼�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMain_RowClick(object sender, RowClickEventArgs e)
        {

        }

        private string GetVector(string p)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// �����ϸ��Ϣ��Gridview��,�������ͼ�к�ѡ����
        /// </summary>
        /// <param name="ds"></param>
        public void setGridControl(DataSet ds)
        {
            if (LeftButtonUserControl.button.ToString() != null)
            {
                pictureEditImgData.pfy.comboBoxEdit1.Visible = false;
            }
            if (ds == null)
            {
                return;
            }

            tempTable = ds.Tables[0].Copy();
            gridViewMain.Columns.Clear();
            //����ѡ����
            DataColumn checkDownColumn = new DataColumn() { ColumnName = ChooseDataColumnCaption, DataType = typeof(bool) };

            //DLF 0822���쳣���
            if (ds == null || ds.Tables.Count == 0)
            {
                gridControlDataList.DataSource = null;
                return;
            }

            DataTable dt = ds.Tables[0];
            //���checkDownColumn������
            if (!dt.Columns.Contains(checkDownColumn.ColumnName))
            {
                dt.Columns.Add(checkDownColumn);
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][ChooseDataColumnCaption] = false;
            }
            gridControlDataList.DataSource = dt;
            //ֻ��ʾǰ��ʮ����
            for (int i = 0; i < gridViewMain.Columns.Count; i++)
            {
                if (i < 15)
                {
                    gridViewMain.Columns[i].OptionsColumn.AllowEdit = false;
                    gridViewMain.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else if (i != gridViewMain.Columns.Count - 1)
                {
                    gridViewMain.Columns[i].Visible = false;
                }
            }
            if (gridViewMain.Columns.Count > 1)
            {
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].OptionsColumn.AllowEdit = true;
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].Visible = true;
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].VisibleIndex = 0;
            }
        }      
        public void setGridControl(DataTable dt)
        {
            //  gridControlTable.DataSource = ds;

            tempTable = dt.Copy();
            gridViewMain.Columns.Clear();
            //����ѡ����
            DataColumn checkDownColumn = new DataColumn() { ColumnName = ChooseDataColumnCaption, DataType = typeof(bool) };
            //DLF 0822���쳣���
            if (dt == null)
            {
                gridControlDataList.DataSource = null;
                return;
            }

            dt.Columns.Add(checkDownColumn);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][ChooseDataColumnCaption] = false;
            }
            gridControlDataList.DataSource = dt;            
            //ֻ��ʾǰ��ʮ����
            for (int i = 0; i < gridViewMain.Columns.Count; i++)
            {
                if (i < 15)
                {
                    gridViewMain.Columns[i].OptionsColumn.AllowEdit = false;
                    gridViewMain.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else if (i != gridViewMain.Columns.Count - 1)
                {
                    gridViewMain.Columns[i].Visible = false;
                }
            }
            if (gridViewMain.Columns.Count > 1)
            {
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].OptionsColumn.AllowEdit = true;
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].Visible = true;
                gridViewMain.Columns[gridViewMain.Columns.Count - 1].VisibleIndex = 0;
            }
        }
        /// <summary>
        /// 20170316
        /// ������Ƭȫ���Ǽ���ʱ������ĳһ��Ƭ����ʱ�����Ƭ���
        /// </summary>
        /// <param name="dt"></param>
        public void setFullTileGridControl(DataTable dt)
        {
            tempTable = dt.Copy();
            gridView1.Columns.Clear();
            // ////����ѡ����
            ////DataColumn checkDownColumn = new DataColumn() { ColumnName = ChooseDataColumnCaption, DataType = typeof(bool) };
            //dt.Columns.Add(checkDownColumn);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i][ChooseDataColumnCaption] = false;
            //}
            //Ĵָͼ��
            DataColumn TileTinyViewColumn = new DataColumn() { ColumnName = AddTileTinyView, DataType = typeof(object) };

            if (dt == null)
            {
                gridControlTileList.DataSource = null;
                return;
            }
            //���Ĵָͼ�е������
            //http:\\....\WS_QDB_GetData\GetTileMiniView.aspx?tilename=GF1_PMS2_20131203_L1A0000124162_7_1190_2823-1.tif
            dt.Columns.Add(TileTinyViewColumn);    
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //string url = "http://192.168.10.202/WS_QDB_GetData/GetTileTinyView.aspx?tilename=";

                string url = ConfigurationManager.AppSettings["QRST_DI_MS_Console_WS_QDB_GetDataService_GetTileImgView"];
                //string url = (string)global::QRST_DI_MS_Desktop.Properties.Settings.Default["QRST_DI_MS_Console_WS_QDB_GetDataService_GetTileImgView"];
                string tileName = dt.Rows[i]["TileFileName"].ToString();
                Image img = null;
                string filePath = url + "?tilename=" + tileName;
                    try
                    {
                        //�ж�ͼƬ·���Ƿ�Ϊ����·��
                        if (UrlDiscern(filePath))
                        {
                            //�ж������ļ��Ƿ����
                            if (RemoteFileExists(filePath))
                            {
                                //��ȡ�ļ�
                                using (WebClient wc = new WebClient())
                                {
                                    img = new Bitmap(wc.OpenRead(filePath));
                                }
                            }

                            //pictureEdit�а�ͼƬ
                            dt.Rows[i][AddTileTinyView] = img;
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.ToString());
                    }
            }
            gridControlTileList.DataSource = dt;
           
            //���ý���ʾ�����ǣ���������ʱ��
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                if (i >1 && i < 5)
                {
                    gridView1.Columns[i].Visible = true;
                    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                    gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else if (i == gridView1.Columns.Count - 1)
                {
                    gridView1.Columns[i].ColumnEdit = this.repositoryItemPictureEdit1;
                   gridView1.Columns[i].UnboundType = DevExpress.Data.UnboundColumnType.Object;
                    gridView1.Columns[i].Visible = true;
                    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                    gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                    gridView1.Columns[i].VisibleIndex = 0;
                }
                else
                {
                    gridView1.Columns[i].Visible = false;
                    gridView1.Columns[i].VisibleIndex = -1;

                }
            }
        }
        /// <summary>
        /// ʶ��urlStr�Ƿ�������·��
        /// </summary>
        /// <param name="urlStr"></param>
        /// <returns></returns>
        public bool UrlDiscern(string urlStr)
        {
            if (Regex.IsMatch(urlStr, @"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// �ж�Զ�������ļ��Ƿ����
        /// </summary>
        /// <param name="fileUrl"></param>
        /// <returns></returns>
        public bool RemoteFileExists(string fileUrl)
        {
            HttpWebRequest re = null;
            HttpWebResponse res = null;
            try
            {
                re = (HttpWebRequest)WebRequest.Create(fileUrl);
                res = (HttpWebResponse)re.GetResponse();
                if (res.ContentLength != 0)
                {
                    //MessageBox.Show("�ļ�����");
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (re != null)
                {
                    re.Abort();//���ٹر�����
                }
                if (res != null)
                {
                    res.Close();//���ٹر���Ӧ
                }
            }
            return false;
        }
        /// 20170322
        /// ����������Ƭȫ���Ǽ���ʱ������ĳһ��Ƭ����ʱ�����Ƭ���
        /// </summary>
        /// <param name="dt"></param>
        public void setFullProdGridControl(DataTable dt)
        {
            tempTable = dt.Copy();
            gridView1.Columns.Clear();
            // ////����ѡ����
            ////DataColumn checkDownColumn = new DataColumn() { ColumnName = ChooseDataColumnCaption, DataType = typeof(bool) };
            //dt.Columns.Add(checkDownColumn);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    dt.Rows[i][ChooseDataColumnCaption] = false;
            //}

            if (dt == null)
            {
                gridControlTileList.DataSource = null;
                return;
            }
            gridControlTileList.DataSource = dt;
            //���ý���ʾ���ͣ�ʱ��,��Ƭ�ȼ�
            for (int i = 0; i < gridView1.Columns.Count; i++)
            {
                if (i > 0 && i < 5 && i != 2)
                {
                    gridView1.Columns[i].Visible = true;
                    gridView1.Columns[i].OptionsColumn.AllowEdit = false;
                    gridView1.Columns[i].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
                }
                else
                {
                    gridView1.Columns[i].Visible = false;

                }
            }
        }
        public void SelectAllData(bool isselected)
        {
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if (dt == null)
            {
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i][ChooseDataColumnCaption] = isselected;
            }
        }
        
        /// <summary>
        /// ��ѡ������������
        /// </summary>
        /// <param name="isselected"></param>
        public void SelectPartData(bool isselected)
        {
            if (gridViewMain.FocusedRowHandle <= gridViewMain.DataRowCount)
            {
                DataRow dr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
                dr[ChooseDataColumnCaption] = isselected;
            }
        }


        public void AddVirtualDirSelectedData()
        {
            if (selectedQueryObj == null)
            {
                MessageBox.Show("û��Ҫ��ӵ����ݣ�");
                return;
            }
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;

            List<string> lst = new List<string>();
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))  //������Ƭ
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {
                        lst.Add(dt.Rows[i]["TileFileName"].ToString());
                    }
                }
            }
            else  //���ط���Ƭ����
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {
                        lst.Add(dt.Rows[i][queryPara.QRST_CODE].ToString());
                    }
                }
            }


            if (_VirtualDirSelector == null)
            {
                _VirtualDirSelector = new VirtualDirSelector();
            }
            if (!_VirtualDirSelector.IsCreated)
            {
                _VirtualDirSelector.Create(TheUniversal.currentUser.NAME, TheUniversal.currentUser.PASSWORD);
            }

            if (_VirtualDirSelector.ShowSelectorForm() == DialogResult.OK)
            {
                Node targetNode = _VirtualDirSelector.SelectNode;
                string dircode = targetNode.Tag.ToString();

                WaitForm wf = new WaitForm();
                wf.datask += AddVDDatalink;
                wf.beginShowDialog(new object[]{dircode,lst});
                MessageBox.Show("�����ɣ�");
                if (mucVirtualDirManager.MainInstance != null)
                {
                    mucVirtualDirManager.MainInstance.UpdateVirtualDir();
                }
            }
        }

        public DataTable AddSelectedData()
        {
            if (selectedQueryObj == null)
            {
                return null;
            }
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            DataTable dtSelected = dt.Copy();
            dtSelected.Clear();
            
            for (int i = 0; i < dt.Rows.Count; i++)
            {
               if ((bool)dt.Rows[i][ChooseDataColumnCaption])
               {
                   dtSelected.Rows.Add(dt.Rows[i].ItemArray);
                }
            }
            return dtSelected;
        }

        private void AddVDDatalink(object[] objs)
        {
            TheUniversal._VirtualDirUC.AddFileLink(objs[0].ToString(), (List<string>)objs[1]);
        }

        /// <summary>
        /// xmh 20170519
        /// ��Ƭ��������GFFѹ������������Ƭ�ĵ�ʱ��ȫ����ʱ��ʾ��
        /// </summary>
        public void DownLoadSelectedGFFData()
        {
            if (selectedQueryObj == null)
            {
                MessageBox.Show("û��Ҫ���ص����ݣ�");
                return;
            }
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;

            List<string> lst = new List<string>();
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile") && ruc3DSearcher.clickFlag == true)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {
                        lst.Add(dt.Rows[i]["TileFileName"].ToString());
                    }
                }
                if (lst.Count >0 )
                {
                    FrmDownLoad fullTileDownLoad = new FrmDownLoad(selectedQueryObj, lst, ruc3DSearcher.clickFlag);
                    fullTileDownLoad.Show();
                    
                }
            }
            else
            {
                MessageBox.Show("��֧�ָ����͵���������");
                return;
            }

        }

        public void DownLoadSelectedData()
        {
            if (selectedQueryObj == null)
            {
                MessageBox.Show("û��Ҫ���ص����ݣ�");
                return;
            }
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;

            List<string> lst = new List<string>();
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))  //������Ƭ
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {
                        lst.Add(dt.Rows[i]["TileFileName"].ToString());
                    }
                }
            }
            else if (queryPara.QRST_CODE != "") //���ط���Ƭ����
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {
                        if (selectedQueryObj.GROUP_TYPE.ToUpper() == "SYSTEM_NORMALFILE")
                        {
                            lst.Add(dt.Rows[i][queryPara.QRST_CODE].ToString() + "#" + dt.Rows[i]["�ļ�����"].ToString());
                        }
                        else
                        {
                            lst.Add(dt.Rows[i][queryPara.QRST_CODE].ToString());
                        }
                    }
                }
            }
            if (lst.Count > 0)
            {
                FrmDownLoad frmDownLoad = new FrmDownLoad(selectedQueryObj, lst);
                frmDownLoad.Show();
            }
        }


        /// <summary>
        /// ��ϸ��Ϣ�����ϲ����ؼ�
        /// </summary>
        /// <param name="fileTypeCategory"></param>
        internal void setQuickViewControl(FileTypeCategory fileTypeCategory)
        {
            switch (fileTypeCategory)
            {
                case FileTypeCategory.Vectors:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(MainMapImage);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonZoomIn);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonZoomOut);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonPan);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(simpleButtonFullExtent);
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(labelControlPosition);
                    break;
                case FileTypeCategory.Rasters:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                    break;
                case FileTypeCategory.Documents:
                    break;
                case FileTypeCategory.Sheets:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                    break;
                case FileTypeCategory.Tiles:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                    break;
                case FileTypeCategory.NotDefine:
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Clear();
                    this.splitContainerControlImgAndDetail.Panel1.Controls.Add(this.pictureEditImgData);
                    break;
                default:
                    break;
            }
        }


        private void simpleButtonZoomIn_Click(object sender, EventArgs e)
        {
            MainMapImage.ActiveTool = MapImage.Tools.ZoomIn;
        }

        private void simpleButtonZoomOut_Click(object sender, EventArgs e)
        {
            MainMapImage.ActiveTool = MapImage.Tools.ZoomOut;
        }

        private void simpleButtonPan_Click(object sender, EventArgs e)
        {
            MainMapImage.ActiveTool = MapImage.Tools.Pan;
        }

        private void simpleButtonFullExtent_Click(object sender, EventArgs e)
        {
            if (MainMapImage.Map.Layers.Count > 0)
            {
                MainMapImage.Map.ZoomToExtents();
                MainMapImage.Refresh();

                simpleButtonZoomIn.BringToFront();
                simpleButtonZoomOut.BringToFront();
                simpleButtonPan.BringToFront();
                simpleButtonFullExtent.BringToFront();
            }

        }

        #region   �л���ͼģʽ����
        public void SwitchDisplaySchema(string schema)
        {
            if (schema.ToLower().Equals("allpicture")) //ȫͼչʾ
            {
                displaySchema = EnumDisplayEnum.AllPicture;
                splitContainerControlImgAndDetail.SplitterPosition = splitContainerControlImgAndDetail.Width;
                splitContainerControlTableViewer.SplitterPosition = splitContainerControlTableViewer.Height;
                

            }
            else if (schema.ToLower().Equals("alltable")) //ȫ����ʾ
            {
                //zxw 20131221  ����ȫ�����ʾģʽ
                displaySchema = EnumDisplayEnum.AllTable;
                splitContainerControlImgAndDetail.SplitterPosition = 0;
                splitContainerControlTableViewer.SplitterPosition = 0;
                splitContainerControlgrid.SplitterPosition = splitContainerControlgrid.Width * 3 / 4;

            }
            else if (schema.ToLower().Equals("onlytable")) //������ʾ������Ҫ��ϸ���
            {
                //zxw 20131221  ����ȫ�����ʾģʽ
                displaySchema = EnumDisplayEnum.OnlyTable;
                splitContainerControlImgAndDetail.SplitterPosition = 0;
                splitContainerControlTableViewer.SplitterPosition = 0;
                splitContainerControlgrid.SplitterPosition = splitContainerControlgrid.Width;

            }
            else          //������ʾ
            {
                displaySchema = EnumDisplayEnum.Normal;
                SizeToNomal();
            }
        }
        #endregion
        /// <summary>
        /// ��ʾ��¼����ϸ��Ϣ(����������Ƭȫ����)
        /// </summary>
        /// <param name="dr"></param>
        void DisplayTileDetailInfo(DataRow dr)
        {
            try
            {
                if (dr != null)
                {
                    DataTable dt = (DataTable)gridControlTileList.DataSource;
                    //DataTable dt = (DataTable)gridControlTileList.DataSource;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("��ϸ��Ϣ:");
                    sb.AppendLine("");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!dt.Columns[i].Caption.Equals("ѡ������"))
                        {
                            string info = string.Format("{0}:{1}", dt.Columns[i].Caption, dr[i]);
                            sb.AppendLine(info);
                        }
                    }
                    textDetailnfo.Text = sb.ToString();
                    //memoEditDetail.Text = sb.ToString();
                }
                else
                {
                    textDetailnfo.Text = string.Empty;
                    //memoEditDetail.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Exception e = ex;
            }

        }
        /// <summary>
        /// ��ʾ��¼����ϸ��Ϣ
        /// </summary>
        /// <param name="dr"></param>
        void DisplayDetailInfo(DataRow dr)
        {
            try
            {
                if (dr != null)
                {
                    DataTable dt = (DataTable)gridControlDataList.DataSource;
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("��ϸ��Ϣ:");
                    sb.AppendLine("");
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (!dt.Columns[i].Caption.Equals("ѡ������"))
                        {
                            string info = string.Format("{0}:{1}", dt.Columns[i].Caption, dr[i]);
                            sb.AppendLine(info);
                        }
                    }
                    textDetailnfo.Text = sb.ToString();
                    //zxw 20131221 Ϊ��㣬��ʱ��������ʾ��ϸ��Ϣ�İ�
                    memoEditDetail.Text = sb.ToString();
                }
                else
                {
                    textDetailnfo.Text = string.Empty;
                    //zxw 20131221 Ϊ��㣬��ʱ��������ʾ��ϸ��Ϣ�İ�
                    memoEditDetail.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Exception e = ex;
            }

        }
        //localhostSqlite.Service sqliteclient = new localhostSqlite.Service();
        private static IQDB_Searcher_Tile _tileClient = Constant.ISearcherTileServ;
        /// <summary>
        /// 20170316
        /// ����SearImgTileSingle�������ڻ�ȡĳһ��Ƭ������ʱ�����
        /// (���ɸ��ݷ���SerSearImgTileCountBatch��Ҳ�ɸ��ݷ���serSearImgTileBatch)
        /// </summary>
        /// <param name="resultTable"></param>
        public void serImgTileSingle(string[] singlePar)
        {
            DataTable resSingleTable;
            //����SearImgTileSingle�������ڻ�ȡĳһ��Ƭ������ʱ�����
            System.Data.DataSet dsSingle = new System.Data.DataSet();
            dsSingle = _tileClient.SearImgTileSingle(singlePar[0], singlePar[1], singlePar[2], singlePar[3], singlePar[4], Convert.ToInt32(singlePar[5]), Convert.ToInt32(singlePar[6]));

            resSingleTable = new DataTable();
            resSingleTable.Clear();
            //queryResponse = new QRST_DI_DS_MetadataQuery.QueryResponse();
            //queryResponse.recordSet = dsSingle;
            if (dsSingle != null && dsSingle.Tables.Count > 0)
            {
                try
                {
                    //��dataset�ĵ�һ������ɾ��
                    dsSingle.Tables[0].Rows[0].Delete();
                    dsSingle.Tables[0].AcceptChanges();
                    resSingleTable = dsSingle.Tables[0];

                }
                catch (Exception)
                {

                    throw;
                }

            }
            if (resSingleTable == null || resSingleTable.Rows.Count == 0)
            {
                MessageBox.Show("û�в��ҵ�����!");
                return;
            }
            setFullTileGridControl(resSingleTable);
        }
        /// <summary>
        /// 20170322
        /// ����SearProdTileSingle�������ڻ�ȡĳһ��Ƭ������ʱ�����
        /// (���ɸ��ݷ���SerSearProdTileCountBatch��Ҳ�ɸ��ݷ���serSearProdTileBatch)
        /// </summary>
        /// <param name="resultTable"></param>
        public void serProdTileSingle(string[] singleProdPar)
        {
             DataTable resSingleProdTable;
            //����SearProdTileSingle�������ڻ�ȡĳһ��Ƭ������ʱ�����
            System.Data.DataSet dsSingle = new System.Data.DataSet();
            dsSingle = _tileClient.SearProdTileSingle(singleProdPar[0], singleProdPar[1], singleProdPar[2], singleProdPar[3], Convert.ToInt32(singleProdPar[4]), Convert.ToInt32(singleProdPar[5]));

            resSingleProdTable = new DataTable();
            resSingleProdTable.Clear();
            //queryResponse = new QRST_DI_DS_MetadataQuery.QueryResponse();
            //queryResponse.recordSet = dsSingle;
            if (dsSingle != null && dsSingle.Tables.Count > 0)
            {
                try
                {
                    //��dataset�ĵ�һ������ɾ��
                    dsSingle.Tables[0].Rows[0].Delete();
                    dsSingle.Tables[0].AcceptChanges();
                    resSingleProdTable = dsSingle.Tables[0];

                }
                catch (Exception)
                {

                    throw;
                }

            }
            if (resSingleProdTable == null || resSingleProdTable.Rows.Count == 0)
            {
                MessageBox.Show("û�в��ҵ�����!");
                return;
            }
          
            setFullProdGridControl(resSingleProdTable);
        }
        

        public string tileSelectFileName1 = null;
        string row = null;
        string col = null;

        /// <summary>
        /// ����ʱ����ѡ���е���Ϣ���ص���ϸ��Ϣ�б���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridViewMain_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            RefreshInfo();
        }

        /// <summary>
        /// ˢ�½�����Ϣ
        /// </summary>
        public void RefreshInfo()
        {
            if (gridViewMain.FocusedRowHandle > gridViewMain.DataRowCount)
                gridViewMain.FocusedRowHandle = 0;
            DataRow dr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
            DisplayDetailInfo(dr);
            //�������Ƭ����ʱ����Ҫ���һ��gridView
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")&&ruc3DSearcher.clickFlag == true )
            {
                queryPara = new RasterQueryPara();
                queryPara.QRST_CODE = "TileFileName";
                if (selectedQueryObj.NAME.Equals("���Ӱ������") || selectedQueryObj.NAME.Equals("���Ӱ���������"))
                {
                    if (dr != null)
                    {
                        tileSelectFileName1 = string.Format(dr["TileFileName"].ToString());
                        for (int i = 0; i < gridViewMain.Columns.Count; i++)
                        {
                            row = string.Format("{0}", dr["Row"].ToString());
                            col = string.Format("{0}", dr["Col"].ToString());

                        }
                        //if (ruc3DSear == null)
                        //{
                        //    ruc3DSear = new ruc3DSearcher();
                        //    //ruc3DSear = ((ruc3DSearcher)MSUserInterface.listMSUI[1].uiMainUC);
                        //}
                        string[] singlePar = new string[7];
                        singlePar[0] = ruc3DSearcher.sat;
                        singlePar[1] = ruc3DSearcher.sensor;
                        singlePar[2] = row;
                        singlePar[3] = col;
                        singlePar[4] = ruc3DSearcher.tileLevel;
                        singlePar[5] = "" + ruc3DSearcher.startdate;
                        singlePar[6] = "" + ruc3DSearcher.enddate;
                        //����SearImgTileSingle�������ڻ�ȡĳһ��Ƭ������ʱ�����
                        serImgTileSingle(singlePar);
                        //Task tk3 = new Task(o => serImgTileSingle((string[])o), singlePar);
                        //tk3.Start();

                        //������Ƭ�����ְ�2��gridView ��������
                        if (!string.IsNullOrEmpty(tileSelectFileName1))
                            gridView1.FocusedRowHandle = gridView1.LocateByValue(0, gridView1.Columns["TileFileName"], tileSelectFileName1);

                    }
                    
                }
                else
                {
                    queryPara = new RasterQueryPara();
                    queryPara.QRST_CODE = "TileFileName";
                    if (selectedQueryObj.NAME.Equals("��񻯲�Ʒ����"))
                    {
                        if (dr != null)
                        {
                            tileSelectFileName1 = string.Format(dr["TileFileName"].ToString());
                            for (int i = 0; i < gridViewMain.Columns.Count; i++)
                            {
                                row = string.Format("{0}", dr["Row"].ToString());
                                col = string.Format("{0}", dr["Col"].ToString());

                            }
                            //if (ruc3DSear == null)
                            //{
                            //    ruc3DSear = new ruc3DSearcher();
                            //    //ruc3DSear = ((ruc3DSearcher)MSUserInterface.listMSUI[1].uiMainUC);
                            //}
                            string[] singleProdPar = new string[6];
                            singleProdPar[0] = ruc3DSearcher.prodType;
                            singleProdPar[1] = row;
                            singleProdPar[2] = col;
                            singleProdPar[3] = ruc3DSearcher.tileLevel;
                            singleProdPar[4] = "" + ruc3DSearcher.startdate;
                            singleProdPar[5] = "" + ruc3DSearcher.enddate;
                            //����SearProdTileSingle�������ڻ�ȡĳһ��Ƭ������ʱ�����
                            serProdTileSingle(singleProdPar);
                            ////        Task tk3 = new Task(o => serProdTileSingle((string[])o), paras3);
                            ////        tk3.Start();

                            //������Ƭ�����ְ�2��gridView ��������
                            if (!string.IsNullOrEmpty(tileSelectFileName1))
                                gridView1.FocusedRowHandle = gridView1.LocateByValue(0, gridView1.Columns["TileFileName"], tileSelectFileName1);
                            
                        }
                        
                    }
                } 
            }

            if (dr == null)
            {
              //�����ʾ���
               pictureEditImgData.Image = null;
               return;
            }
                //��ʾ����ͼ��Ϣ
                if (queryPara.QRST_CODE != null)
                {
                    string id;
                    if ((!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector")) && selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() != "rcdb" && (!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")))
                    {
                        id = dr[queryPara.QRST_CODE].ToString();
                        if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))
                        {
                            string qrst_code = ConvertQRSTCODE(id);
                            MetaData metaData = MetaDataFactory.CreateMetaDataInstance(qrst_code);
                            if (metaData is MetaDataImageProd)
                            {
                                //����Ƿǹ�Ӱ���Ʒ��������ͼ��ʾ��Ӧԭʼ���ݵĿ���ͼ
                                try
                                {
                                    string orgQC = dr["ԭʼ���ݱ���"].ToString();
                                    id = orgQC;

                                }
                                catch (Exception ex)
                                { }

                            }
                        }
                        try
                        {
                            GetImageDel del = GetImage;
                            IAsyncResult ar = del.BeginInvoke(id, GetImageCallBack, del);
                        }
                        catch (Exception ex)
                        { } 
                    }
                    else if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")
                    {
                        GetBopuDataDel del = GetBopuData;
                        IAsyncResult ar = del.BeginInvoke(GetBopuDataCallBack, del);
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))          //չʾʸ������
                    {
                        //���ʸ������  zxw 20131020
                        string storePath = MetaData.GetDataAddress(dr[queryPara.QRST_CODE].ToString());
                        if (Directory.Exists(storePath)) //
                        {
                            //�첽ִ���������ݼ���
                            GetShapeFileDel del = GetShapeFile;
                            IAsyncResult ar = del.BeginInvoke(storePath, GetShapeFileCallBack, del);

                        }
                    }
                    else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))             //չʾ��Ƭ���� 
                    {
                        //if (getDataclient == null)
                        //{
                        //    getDataclient = new WS_QDB_GetDataService.Service();
                        //}
                        string str = dr[queryPara.QRST_CODE].ToString();
                        string tileFilename = getDataclient.GetTilesList(new string[] { dr[queryPara.QRST_CODE].ToString() })[0];
                        GetTileImageDel del = GetTileImage;
                        IAsyncResult ar = del.BeginInvoke(tileFilename, GetTileImageCallBack, del);
                    }

                }
                else
                {
                    pictureEditImgData.Image = null;
                }
                //����ά������ʾѡ�м�¼�Ŀռ䷶Χ
                if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))
                {
                    //���ƿռ䷶Χ����ά��
                    if (((RasterQueryPara)queryPara).spacialAvailable)
                    {
                        DrawCheckedExtent((RasterQueryPara)queryPara);
                        DrawRasterExtent((RasterQueryPara)queryPara);
                    }
                }
                else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))
                {
                    //���ƿռ䷶Χ����ά��
                    if (((VectorQueryPara)queryPara).spacialAvailable)
                    {
                        DrawVectorExtent((VectorQueryPara)queryPara);
                    }
                }
                else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                {
                    //���ƿռ䷶Χ����ά��
                    if (((RasterQueryPara)queryPara).spacialAvailable)
                    {
                        DrawRasterExtent((RasterQueryPara)queryPara);
                    }
                }
        }
        /// <summary>
        /// 20170320
        /// (������Ƭ��ȫ���Ǽ���ʱ)����ʱ����ѡ���е���Ϣ���ص���ϸ��Ϣ�б���
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            RefreshSingleInfo();
        }
        public void RefreshSingleInfo() 
        {
            if (gridView1.FocusedRowHandle > gridView1.DataRowCount)
                gridView1.FocusedRowHandle = 0;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            DisplayTileDetailInfo(dr);
            //DisplayDetailInfo(dr);
            if (dr == null)
            {
                //�����ʾ���
                pictureEditImgData.Image = null;
                return;
            }
            //��ʾ����ͼ��Ϣ
            if (queryPara.QRST_CODE != null)
            {
                if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))             //չʾ��Ƭ���� 
                {
                    //if (getDataclient == null)
                    //{
                    //    getDataclient = new WS_QDB_GetDataService.Service();
                    //}
                    //string tileFilename1 = dr[queryPara.QRST_CODE].ToString();
                    //string tileSingleFilename = tileFilename1.Substring(0, tileFilename1.IndexOf("#")) + tileFilename1.Substring(tileFilename1.IndexOf("#") + 2);

                    string tileFilename = getDataclient.GetTilesList(new string[] { dr[queryPara.QRST_CODE].ToString() })[0];
                    GetTileImageDel del = GetTileImage;
                    IAsyncResult ar = del.BeginInvoke(tileFilename, GetTileImageCallBack, del);
                }

            }
            else
            {
                pictureEditImgData.Image = null;
            }
            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
            {
                //���ƿռ䷶Χ����ά��
                if (((RasterQueryPara)queryPara).spacialAvailable)
                {
                    DrawRasterExtent((RasterQueryPara)queryPara);
                    //AddTileLyrOrder((RasterQueryPara)queryPara);
                }
            }
        }

        void DrawCheckedExtent(RasterQueryPara queryPara)
        {
            muc3DSearcher _muc3DSearcher = (muc3DSearcher)MSUserInterface.listMSUI[0].uiMainUC;
            // DataRow dr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);

            if (queryPara.spacialAvailable)
            {
                DataTable dt = (DataTable)gridControlDataList.DataSource;
                List<List<float>> extents1 = new List<List<float>>();
                List<float> list = new List<float>();
                List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                    {

                        DataRow dr = dt.Rows[i];
                        float minLat;
                        float maxLat;
                        float minLon;
                        float maxLon;
                        if (dr != null)
                        {
                            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                            {
                                string level = dr["Level"].ToString();
                                string[] rowandColumn = new string[4];
                                rowandColumn[0] = dr["Row"].ToString();
                                rowandColumn[1] = dr["Col"].ToString();

                                double[] extent = GetLatAndLong(rowandColumn, level);

                                minLat = float.Parse(extent[0].ToString());
                                maxLat = float.Parse(extent[2].ToString());
                                minLon = float.Parse(extent[1].ToString());
                                maxLon = float.Parse(extent[3].ToString());
                                list.Add(minLon);
                                list.Add(maxLat);
                                list.Add(maxLon);
                                list.Add(maxLat);
                                list.Add(maxLon);
                                list.Add(minLat);
                                list.Add(minLon);
                                list.Add(minLat);
                            }
                            else
                            {
                                minLat = Math.Min(float.Parse(dr[queryPara.DATALOWERLEFTLAT].ToString()), float.Parse(dr[queryPara.DATALOWERRIGHTLAT].ToString()));
                                maxLat = Math.Max(float.Parse(dr[queryPara.DATAUPPERLEFTLAT].ToString()), float.Parse(dr[queryPara.DATAUPPERRIGHTLAT].ToString()));
                                minLon = Math.Min(float.Parse(dr[queryPara.DATAUPPERLEFTLONG].ToString()), float.Parse(dr[queryPara.DATALOWERLEFTLONG].ToString()));
                                maxLon = Math.Max(float.Parse(dr[queryPara.DATAUPPERRIGHTLONG].ToString()), float.Parse(dr[queryPara.DATALOWERRIGHTLONG].ToString()));
                                list.Add(float.Parse(dr[queryPara.DATAUPPERLEFTLONG].ToString()));
                                list.Add(float.Parse(dr[queryPara.DATAUPPERLEFTLAT].ToString()));
                                list.Add(float.Parse(dr[queryPara.DATAUPPERRIGHTLONG].ToString()));
                                list.Add(float.Parse(dr[queryPara.DATAUPPERRIGHTLAT].ToString()));
                                list.Add(float.Parse(dr[queryPara.DATALOWERRIGHTLONG].ToString()));
                                list.Add(float.Parse(dr[queryPara.DATALOWERRIGHTLAT].ToString()));
                                list.Add(float.Parse(dr[queryPara.DATALOWERLEFTLONG].ToString()));
                                list.Add(float.Parse(dr[queryPara.DATALOWERLEFTLAT].ToString()));
                            }

                            extents1.Add(list);
                            extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));

                        }
                    }
               
                }
                if (_muc3DSearcher.Is3DViewer)
                {
                    QRSTWorldGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
                    if (_QrstAxGlobeControl != null)
                    {
                        _QrstAxGlobeControl.DrawCheckedExtents(extents1);
                    }
                }
                else
                {
                    uc2DSearcher _uc2DSearcher = this.pictureEditImgData.GetUc2DSearcher();
                    if (_uc2DSearcher != null)
                    {
                        if (extents.Count > 0)
                        {
                            string extent = extents[0].X.ToString() + "," + extents[0].Y.ToString() + "," + (extents[0].X + extents[0].Width).ToString() + "," + (extents[0].Y + extents[0].Height).ToString();
                            _uc2DSearcher.DrawSelectedExtents(extent);
                        }
                    }
                }
            }
        }

        #region ������ƿռ䷶Χ�ķ���
        void DrawRasterExtent(RasterQueryPara queryPara)
        {
            muc3DSearcher _muc3DSearcher = (muc3DSearcher)MSUserInterface.listMSUI[0].uiMainUC;
            DataRow dr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
            List<List<float>> extents1 = new List<List<float>>();
            List<float> list = new List<float>();
            if (queryPara.spacialAvailable)
            {
                List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();

                float minLat;
                float maxLat;
                float minLon;
                float maxLon;
                if (dr != null)
                {
                    if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                    {
                        string level = dr["Level"].ToString();
                        string[] rowandColumn = new string[4];
                        rowandColumn[0] = dr["Row"].ToString();
                        rowandColumn[1] = dr["Col"].ToString();

                        double[] extent = GetLatAndLong(rowandColumn, level);

                        minLat = float.Parse(extent[0].ToString());
                        maxLat = float.Parse(extent[2].ToString());
                        minLon = float.Parse(extent[1].ToString());
                        maxLon = float.Parse(extent[3].ToString());
                        list.Add(minLon);
                        list.Add(maxLat);
                        list.Add(maxLon);
                        list.Add(maxLat);
                        list.Add(maxLon);
                        list.Add(minLat);
                        list.Add(minLon);
                        list.Add(minLat);
                    }
                    else
                    {
                        minLat = Math.Min(float.Parse(dr[queryPara.DATALOWERLEFTLAT].ToString()), float.Parse(dr[queryPara.DATALOWERRIGHTLAT].ToString()));
                        maxLat = Math.Max(float.Parse(dr[queryPara.DATAUPPERLEFTLAT].ToString()), float.Parse(dr[queryPara.DATAUPPERRIGHTLAT].ToString()));
                        minLon = Math.Min(float.Parse(dr[queryPara.DATAUPPERLEFTLONG].ToString()), float.Parse(dr[queryPara.DATALOWERLEFTLONG].ToString()));
                        maxLon = Math.Max(float.Parse(dr[queryPara.DATAUPPERRIGHTLONG].ToString()), float.Parse(dr[queryPara.DATALOWERRIGHTLONG].ToString()));
                        list.Add(float.Parse(dr[queryPara.DATAUPPERLEFTLONG].ToString()));
                        list.Add(float.Parse(dr[queryPara.DATAUPPERLEFTLAT].ToString()));
                        list.Add(float.Parse(dr[queryPara.DATAUPPERRIGHTLONG].ToString()));
                        list.Add(float.Parse(dr[queryPara.DATAUPPERRIGHTLAT].ToString()));
                        list.Add(float.Parse(dr[queryPara.DATALOWERRIGHTLONG].ToString()));
                        list.Add(float.Parse(dr[queryPara.DATALOWERRIGHTLAT].ToString()));
                        list.Add(float.Parse(dr[queryPara.DATALOWERLEFTLONG].ToString()));
                        list.Add(float.Parse(dr[queryPara.DATALOWERLEFTLAT].ToString()));
                    }

                    extents1.Add(list);
                    extents.Add(new System.Drawing.RectangleF(minLon, minLat, maxLon - minLon, maxLat - minLat));
                }
                if (_muc3DSearcher.Is3DViewer)
                {
                    QRSTWorldGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
                    if (_QrstAxGlobeControl != null)
                    {
                        _QrstAxGlobeControl.DrawSelectedExtents(extents1);
                        if (extents1.Count > 0)
                        {
                            double distance = 1026000.48 * Math.Sqrt(extents[0].Height * extents[0].Height + extents[0].Width * extents[0].Width);
                            _QrstAxGlobeControl.SetViewPosition(extents[0].Bottom, extents[0].Left, 0.0, distance, 0.0);
                        }
                    }
                }
                else
                {
                    uc2DSearcher _uc2DSearcher = this.pictureEditImgData.GetUc2DSearcher();
                    if (_uc2DSearcher != null)
                    {
                        if (extents.Count > 0)
                        {
                            string extent = extents[0].X.ToString() + "," + extents[0].Y.ToString() + "," + (extents[0].X + extents[0].Width).ToString() + "," + (extents[0].Y + extents[0].Height).ToString();
                            _uc2DSearcher.DrawSelectedExtents(extent);
                        }
                    }
                }
            }
        }

        void DrawVectorExtent(VectorQueryPara queryPara)
        {
            DataRow dr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
            if (dr == null)
            {
                return;
            }
            if (queryPara.spacialAvailable)
            {
                List<System.Drawing.RectangleF> extents = new List<System.Drawing.RectangleF>();
                extents.Add(new System.Drawing.RectangleF(
                float.Parse(dr[queryPara.extentLeftField].ToString()),
                float.Parse(dr[queryPara.extentDownField].ToString()),
                float.Parse(dr[queryPara.extentRightField].ToString()) - float.Parse(dr[queryPara.extentLeftField].ToString()),
                float.Parse(dr[queryPara.extentUpField].ToString()) - float.Parse(dr[queryPara.extentDownField].ToString())));
                QRSTWorldGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
                if (_QrstAxGlobeControl != null)
                {
                    _QrstAxGlobeControl.DrawSelectedExtents(extents);
                    if (extents.Count > 0)
                    {
                        _QrstAxGlobeControl.SetViewPosition(extents[0].Bottom, extents[0].Left, 0.0, 1000000.0, 0.0);
                    }
                }
            }
        }

        public static double[] GetLatAndLong(string[] rowAndColum, string lv)
        {
            double a = DirectlyAddressing.getLevelRate(lv);
            double[] latAndlong = new double[4];
            latAndlong[0] = Convert.ToDouble(rowAndColum[0]) / a - 90;
            latAndlong[1] = Convert.ToDouble(rowAndColum[1]) / a - 180;
            latAndlong[2] = Convert.ToDouble(Convert.ToInt32(rowAndColum[0]) + 1) / a - 90;
            latAndlong[3] = Convert.ToDouble(Convert.ToInt32(rowAndColum[1]) + 1) / a - 180;
            return latAndlong;
        }
        /// <summary>
        /// xmh  
        /// 20150512
        /// ����ʱ����Ƭ����ͼ
        /// </summary>
        /// <param name="queryPara"></param>
        void AddTileLyrOrder(RasterQueryPara queryPara)
        {
            muc3DSearcher _muc3DSearcher = (muc3DSearcher)MSUserInterface.listMSUI[0].uiMainUC;
            DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
            string str = null;
            string tilePath = null;
            double[] extent = new double[4];
            if (queryPara.spacialAvailable)
            {
                if (dr != null)
                {
                    if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                    {
                        string level = dr["Level"].ToString();
                        string[] rowandColumn = new string[4];
                        rowandColumn[0] = dr["Row"].ToString();
                        rowandColumn[1] = dr["Col"].ToString();

                        extent = GetLatAndLong(rowandColumn, level);

                      
                        str = dr[queryPara.QRST_CODE].ToString();
                        string tileFilename = getDataclient.GetTilesList(new string[] { str })[0];
                        if (tileFilename.EndsWith(".png") || tileFilename.EndsWith(".jpg"))
                        {
                            if (File.Exists(tileFilename))
                            {
                                tilePath = tileFilename;
                            }
                        } 
                        else
                        {
                            if (tileFilename.Contains(".c"))   //��Ƭ����
                            {
                                try
                                {
                                    string thumbnailName = tileFilename.Remove(tileFilename.IndexOf('-')) + ".c.jpg";
                                    string thumbnailPNGName = tileFilename.Remove(tileFilename.IndexOf('-')) + ".c.png";
                                    if (File.Exists(thumbnailPNGName))
                                    {
                                        tilePath = thumbnailPNGName;
                                    }
                                    else if (File.Exists(thumbnailName))
                                    {
                                        tilePath = thumbnailName;
                                    }
                                    else
                                    {
                                        tilePath = tileFilename;
                                    }

                                }
                                catch (Exception)
                                {

                                }

                            }
                        }

                    }
                    
                }
                if (_muc3DSearcher.Is3DViewer)
                {
                    QRSTWorldGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
                    if (_QrstAxGlobeControl != null)
                    {
                       byte opacity = 255;
                       bool addLayer = _QrstAxGlobeControl.QrstGlobe.AddImage(str, 1, extent[0], extent[2], extent[1], extent[3], 0, opacity, tilePath);

                    }
                }

            }
        }

        #endregion

        private void gridViewMain_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        /// <summary>
        /// 20170320 ��ʾ�����
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
        {
            if (e.Info.IsRowIndicator && e.RowHandle >= 0)
            {
                e.Info.DisplayText = (e.RowHandle + 1).ToString();
            }
        }
        //\\192.168.10.202\zhsjk\ʵ����֤���ݿ�\GF1��������\GF1\WFV4\2016\01\23\GF1_WFV4_E115.2_N35.2_20160123_L1A0001363539\GF1_WFV4_E115.2_N35.2_20160123_L1A0001363539.jpg
       public static  string[] TolelPicname = null;
         
        #region �����첽���õķ�ʽ��ȡ����ͼ����

        /// <summary>
        /// ��ʾ����ͼ
        /// ͨ��ID���һ����ļ������Ƿ���ڸ��ļ����������ڣ���ͨ��id���Ҹü�¼���ļ��洢·�����ڶ�Ӧ·�����Ƿ��������ͼ
        /// �������ڣ�����thumbnail�ֶ�
        /// �ж�thumbnail��·����url���Ƕ��������ݣ�
        /// ��Ϊ·������ͨ����·���鿴ͼ�Ƿ���ڣ���������ʾ�����ļ�����������
        /// ��Ϊurl��������õ�ַ��ȡͼƬ������ͼƬ���ص�����
        /// ��Ϊ�����ƣ���ֱ��ת��ΪͼƬ������ʾ
        /// </summary>
        Image GetImage(string id)
        {
            //����λ��
            string cachePath = string.Format(@"{0}\{1}", Application.StartupPath, @"Cache\Thumbnail");//@"Cache\Thumbnail"
            string qrst_code = ConvertQRSTCODE(id);
            Image image = null;
           // pictureEditImgDataFY pfy = pictureEditImgData.pfy;
            string newpath = string.Format(@"{0}\{1}", cachePath, qrst_code);
            string pPath = MetaData.GetDataAddress(qrst_code);
            if (pPath=="-1")
            {
                 //������
                if (pictureEditImgData.InvokeRequired)
                {
                    pictureEditImgData.Invoke(new EventHandler(delegate
                    {
                        try
                        {
                            pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Clear();
                            pictureEditImgData.pfy.comboBoxEdit1.Visible = false;
                        }
                        catch (Exception ex)
                        {
                        }
                    }));
                }
                else
                {
                    try
                    {
                        pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Clear();
                        pictureEditImgData.pfy.comboBoxEdit1.Visible = false;
                    }
                    catch (Exception ex)
                    {


                    }
                }
                return null;
            }
            if (Directory.Exists(newpath) && Directory.GetFiles(newpath).Length > 0)
            {
                //�����ݣ��д��ڻ������ͼ�ļ��� �ӻ����ļ����л�ȡ
                if (pictureEditImgData.InvokeRequired)
                {
                    pictureEditImgData.Invoke(new EventHandler(delegate
                    {
                        try
                        {
                            pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Clear();
                            pictureEditImgData.pfy.comboBoxEdit1.Visible = false;
                            string[] files = Directory.GetFiles(newpath);
                            if (files != null && files.Length > 0)
                            {
                                for (int i = 0; i < files.Length; i++)
                                {
                                    //pictureEditImgData.pfy.list.Add(new ThumbComboItem(files[i]));
                                    try
                                    {
                                        pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Add(new ThumbComboItem(files[i]));
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                                if (pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Count > 0)
                                {
                                    pictureEditImgData.pfy.comboBoxEdit1.SelectedIndex = 0;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }));
                }
                else
                {
                    try
                    {
                        pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Clear();
                        pictureEditImgData.pfy.comboBoxEdit1.Visible = false;
                        string[] files = Directory.GetFiles(newpath);
                        if (files != null && files.Length > 0)
                        {
                            for (int i = 0; i < files.Length; i++)
                            {
                                //pictureEditImgData.pfy.list.Add(new ThumbComboItem(files[i]));
                                try
                                {
                                    pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Add(new ThumbComboItem(files[i]));
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            if (pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Count > 0)
                            {
                                pictureEditImgData.pfy.comboBoxEdit1.SelectedIndex = 0;
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
            else
            {
                //�����ڻ������ͼ �������ļ����в��ң�����ӵ������ļ���
                GetExistPicture(pPath, out TolelPicname);
                if (TolelPicname != null && TolelPicname.Length > 0)
                {
                    Directory.CreateDirectory(newpath);
                    for (int i = 0; i < TolelPicname.Length; i++)
                    {
                        try
                        {
                            string newcachfile = string.Format(@"{0}\{1}", newpath, TolelPicname[i]);
                            string sourcefile = string.Format(@"{0}\{1}", pPath, TolelPicname[i]);
                            if (File.Exists(sourcefile))
                            {
                                File.Copy(sourcefile, newcachfile, true);
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    if (pictureEditImgData.InvokeRequired)
                    {
                        pictureEditImgData.Invoke(new EventHandler(delegate
                        {
                            if (Directory.Exists(newpath))
                            {
                                try
                                {
                                    string[] files = Directory.GetFiles(newpath);
                                    if (files != null && files.Length > 0)
                                    {
                                        pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Clear();
                                        pictureEditImgData.pfy.comboBoxEdit1.Visible = false;
                                        for (int i = 0; i < files.Length; i++)
                                        {
                                            try
                                            {
                                                pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Add(new ThumbComboItem(files[i]));
                                            }
                                            catch (Exception ex)
                                            {
                                            }
                                        }
                                        if (pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Count > 0)
                                        {
                                            pictureEditImgData.pfy.comboBoxEdit1.SelectedIndex = 0;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }));
                    }
                    else
                    {
                        if (Directory.Exists(newpath))
                        {
                            try
                            {
                                string[] files = Directory.GetFiles(newpath);
                                if (files != null && files.Length > 0)
                                {
                                    pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Clear();
                                    pictureEditImgData.pfy.comboBoxEdit1.Visible = false;
                                    for (int i = 0; i < files.Length; i++)
                                    {
                                        try
                                        {
                                            pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Add(new ThumbComboItem(files[i]));
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                    }
                                    if (pictureEditImgData.pfy.comboBoxEdit1.Properties.Items.Count > 0)
                                    {
                                        pictureEditImgData.pfy.comboBoxEdit1.SelectedIndex = 0;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }

                    return null;      //�Ѿ�����ǰ̨��ʾ��
                }
            }
            //������������δ��Чʱ���ж�thumbnail�ֶ�
            if (!string.IsNullOrEmpty(qrst_code) && queryPara.THUMBNAIL != null)
            {
                try
                {
                    string fieldValue = ((DataTable)gridControlDataList.DataSource).Rows[gridViewMain.FocusedRowHandle][queryPara.THUMBNAIL].ToString();
                    if (IsUrl(fieldValue))   //�������ȡͼƬ�����ص����ػ��沢չʾ
                    {
                        image = GetImageFromWeb(fieldValue, qrst_code);
                    }
                    else if (fieldValue.Equals("System.Byte[]"))  //�ж��Ƿ�Ϊ������
                    {
                        byte[] bytes = ((byte[])((DataTable)gridControlDataList.DataSource).Rows[gridViewMain.FocusedRowHandle][queryPara.THUMBNAIL]);
                        image = GetImageFromBlob(bytes, qrst_code);
                    }

                    return image;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            return null;
        }

        public delegate Image GetImageDel(string id);

        void GetImageCallBack(IAsyncResult ar)
        {
            GetImageDel del = (GetImageDel)ar.AsyncState;
            Image image = del.EndInvoke(ar);
            if (image != null)
            {
                if (pictureEditImgData.IsHandleCreated)
                {
                    if (pictureEditImgData.InvokeRequired)
                    {
                        pictureEditImgData.Invoke(new DiaplayDel(DisplayImage), image);
                    }
                    else
                    {
                        DisplayImage(image);
                    }
                }
            }
        }

        void DisplayImage(Image image)
        {
            pictureEditImgData.Image = image;
        }

        public delegate void DiaplayDel(Image image);
        #endregion

        #region  �첽���û�ȡ��������������������
        /// <summary>
        /// ��ȡ��������   ��Tomcat��������ĳ�IIS�������� @jianghua 2015.8.15
        /// </summary>
        /// <returns></returns>
        public string GetBopuData()
        {
            return null;
        }

        /// <summary>
        /// ���������������ݣ�ת��Ϊ����ʾ����������
        /// </summary>
        /// <param name="bpData"></param>
        /// <returns></returns>
        double[,] ResolveBopuData(string bpData)
        {
            try
            {
                BopuData bopuData =JSON.parse<BopuData>(bpData);
                if (bopuData.types != null && bopuData.types.Length > 0)
                {
                    string str = bopuData.types[0].type;
                    string[] strArr = str.Split(";".ToCharArray());

                    double[,] bopudata = new double[(strArr.Length + 100) / 100, 2];
                    int j = 0;
                    for (int i = 0; i < strArr.Length - 1; i++)
                    {
                        string[] doubleata = strArr[i].Split(",".ToCharArray());
                        bopudata[j, 0] = Double.Parse(doubleata[0]);
                        bopudata[j, 1] = Double.Parse(doubleata[1]);
                        j++;
                        i += 100;
                    }
                    return bopudata;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// չʾ��������
        /// </summary>
        /// <param name="bopuSeq"></param>
        void DisplayBopuSeq(double[,] bopuSeq)
        {

            chartControl1.Series[0].Points.Clear();
            if (bopuSeq == null)
            {
                return;
            }
            for (int i = 0; i < bopuSeq.GetLength(0); i++)
            {
                chartControl1.Series[0].Points.Add(new SeriesPoint(bopuSeq[i, 0], bopuSeq[i, 1]));
            }
        }

        void GetBopuDataCallBack(IAsyncResult ar)
        {
            try
            {
                GetBopuDataDel del = (GetBopuDataDel)ar.AsyncState;
                string seq = del.EndInvoke(ar);
                if (chartControl1.IsHandleCreated)
                {
                    double[,] seqArr = ResolveBopuData(seq);
                    chartControl1.Invoke(new DisplayBopuSeqDel(DisplayBopuSeq), seqArr);
                }
            }
            catch (Exception)
            {
            }
        }

        public delegate string GetBopuDataDel();
        public delegate void DisplayBopuSeqDel(double[,] bopuSeq);

        //20170110  ��ղ���ͼ
        public void deleteBop()
        {
            if (chartControl1.IsHandleCreated)
            {
                chartControl1.Series[0].Points.Clear();
            }
            
        }
        #endregion

        #region �첽���û�ȡ��Ƭ��������ͼ
        public delegate Image GetTileImageDel(string tileFileName);

        Image GetTileImage(string _TileFileName)//\\192.168.10.202\QRST_DB_Tile\6\7\1127\2929\PMS1\20160622\GF1_PMS1_20160622_L1A0001658024_7_1127_2929.png
        {
            if (_TileFileName.EndsWith(".png") || _TileFileName.EndsWith(".jpg"))
            {
                if (File.Exists(_TileFileName))
                {
                    return Image.FromFile(_TileFileName);
                }
            }
            else
            {
                if (_TileFileName.Contains(".c"))   //��Ƭ����
                {
                    string thumbnailName = _TileFileName.Remove(_TileFileName.IndexOf('-')) + ".c.jpg";
                    string thumbnailPNGName = _TileFileName.Remove(_TileFileName.IndexOf('-')) + ".c.png";

                    if (File.Exists(thumbnailPNGName))
                    {
                        return Image.FromFile(thumbnailPNGName);
                    }
                    else if (File.Exists(thumbnailName))
                    {
                        return Image.FromFile(thumbnailName);
                    }
                    else if (File.Exists(_TileFileName))
                    {
                        return Image.FromFile(_TileFileName); 
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (_TileFileName.Contains(".p")) //��Ʒ��Ƭ  NDVI_2011041924_L20000521090-L20000521097_6401_4_248_592.p.tif
                {
                    //string removestr = _TileFileName.Substring(_TileFileName.LastIndexOf('.') - 2);
                    //string thumbnailName = _TileFileName.Substring(0, (_TileFileName.Length - removestr.Length)) + ".p.jpg";   
                    string thumbnailName = _TileFileName.Remove(_TileFileName.LastIndexOf('.')) + ".jpg";
                    string thumbnailPNGName = _TileFileName.Remove(_TileFileName.LastIndexOf('.')) + ".png";
                    if (File.Exists(thumbnailPNGName))
                    {
                        return Image.FromFile(thumbnailPNGName);
                    }
                    else if (File.Exists(thumbnailName))
                    {
                        return Image.FromFile(thumbnailName);
                    }
                    else if (File.Exists(_TileFileName))
                    {
                        return Image.FromFile(_TileFileName);
                    }
                    else
                    {
                        return null;
                    }
                }
                else if (_TileFileName.Contains(".s")) //��������  \\172.16.0.185\QRST_DB_Tile\0\4\248\592\CS\20141121\CS_101121_11_20141121_L20000521097_4_248_592.s.tiff
                {
                    string thumbnailName = _TileFileName.Remove(_TileFileName.LastIndexOf('.')) + ".jpg";
                    string thumbnailPNGName = _TileFileName.Remove(_TileFileName.LastIndexOf('.')) + ".png";
                    if (File.Exists(thumbnailPNGName))
                    {
                        return Image.FromFile(thumbnailPNGName);
                    }
                    else if (File.Exists(thumbnailName))
                    {
                        return Image.FromFile(thumbnailName);
                    }
                    else if (File.Exists(_TileFileName))
                    {
                        return Image.FromFile(_TileFileName);
                    }
                    else
                    {
                        return null;
                    }

                }
            }
           
            return null;
            
        }

        void GetTileImageCallBack(IAsyncResult ar)
        {
            GetTileImageDel del = (GetTileImageDel)ar.AsyncState;
            Image image = del.EndInvoke(ar);
            if (pictureEditImgData.IsHandleCreated)
            {
                pictureEditImgData.Invoke(new DiaplayDel(DisplayImage), image);
            }
        }



        #endregion

        #region �첽���û�ȡʸ������
        public delegate string GetShapeFileDel(string fileDir);

        string GetShapeFile(string fileDir)
        {
            string[] shapeFiles = Directory.GetFiles(fileDir, "*.shp");//Ŀǰ��֧����ʾshapefile  
            if (shapeFiles.Length > 0)
            {
                return shapeFiles[0];
            }
            else
                return null;
        }

        void GetShapeFileCallBack(IAsyncResult ar)
        {
            GetShapeFileDel del = (GetShapeFileDel)ar.AsyncState;
            string shapeFile = del.EndInvoke(ar);
            if (MainMapImage.IsHandleCreated)
            {
                MainMapImage.Invoke(new DisplayShpFileDel(DisplayShpFile), shapeFile);
            }
        }

        public delegate void DisplayShpFileDel(string filePath);

        void DisplayShpFile(string filePath)
        {
            //zxw 20131221 
            try
            {
                MainMapImage.Map.Layers.Clear();
                if (!string.IsNullOrEmpty(filePath))
                {
                    SharpMap.Layers.VectorLayer layer = new SharpMap.Layers.VectorLayer(Path.GetFileNameWithoutExtension(filePath));
                    layer.DataSource = new SharpMap.Data.Providers.ShapeFile(filePath);
                    MainMapImage.Map.Layers.Add(layer);
                }
                MainMapImage.Map.ZoomToExtents();
                MainMapImage.Show();
                MainMapImage.Refresh();
                
                simpleButtonZoomIn.BringToFront();
                simpleButtonZoomOut.BringToFront();
                simpleButtonPan.BringToFront();
                simpleButtonFullExtent.BringToFront();
                
            }
            catch (ArgumentException)
            {
            }

        }
        //20170112 ȡ��ʸ��ͼ����ʾ
        public void DeleteQueryShpFile() 
        {
            if (MainMapImage.IsHandleCreated)
            {
                MainMapImage.Map.Layers.Clear();
            }
            MainMapImage.Refresh();
        }
        #endregion


        /// <summary>
        /// ��ȡָ��·��������ͼ
        /// </summary>
        /// <returns></returns>
        Image GetImageFromPath(string filePath)
        {
            if (File.Exists(filePath))
            {
                return Image.FromFile(filePath);
            }
            return null;
        }

        Image GetImageFromBlob(byte[] bytes, string qrst_code)
        {
            try
            {
                MemoryStream ms = new MemoryStream(bytes);
                byte[] arrayByte = new byte[1024];
                int imgLong = (int)ms.Length;
                int l = 0;
                string cachPath = string.Format(@"{0}\{1}", Application.StartupPath, @"Cache\Thumbnail");
                string cachFile = string.Format(@"{0}\{1}.jpg", cachPath, qrst_code);
                FileStream fso = new FileStream(cachFile, FileMode.Create);
                while (l < imgLong)
                {
                    int i = ms.Read(arrayByte, 0, 1024);
                    fso.Write(arrayByte, 0, i);
                    l += i;
                }

                fso.Close();
                ms.Close();
                return Image.FromFile(cachFile);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        Image GetImageFromWeb(string imgUrl, string qrst_code)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(imgUrl);
            request.UserAgent = "Mozilla/6.0 (MSIE 6.0; Windows NT 5.1; Natas.Robot)";
            request.Timeout = 3000;
            string cachPath = string.Format(@"{0}\{1}", Application.StartupPath, @"Cache\Thumbnail");
            string cachFile = string.Format(@"{0}\{1}.jpg", cachPath, qrst_code);
            try
            {
                using (WebResponse response = request.GetResponse())
                {
                    Stream stream = response.GetResponseStream();
                    if (response.ContentType.ToLower().StartsWith("image/"))
                    {
                        byte[] arrayByte = new byte[1024];
                        int imgLong = (int)response.ContentLength;
                        int l = 0;
                        FileStream fso = new FileStream(cachFile, FileMode.Create);
                        while (l < imgLong)
                        {
                            int i = stream.Read(arrayByte, 0, 1024);
                            fso.Write(arrayByte, 0, i);
                            l += i;
                        }
                        fso.Close();
                        stream.Close();
                        response.Close();
                        return Image.FromFile(cachFile);
                    }
                    else
                        return null;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// ��idת��Ϊ��׼��QRSt_CODE��ʽ��0001-MADB-10-9��,������ʶ�����ʽ����
        /// </summary>
        /// <returns></returns>
        string ConvertQRSTCODE(string id)
        {
            Regex rgx1 = new Regex(@"^\d{4}-\w{4}-\d{1,}-\d{1,}$");  //������ʽƥ��qrst_code�ı�׼��ʽ��0001-MADB-10-9
            Regex rgx2 = new Regex(@"^\w{4}-\d{1,}-\d{1,}$");          //������ʽƥ��qrst_code���ϰ汾��ʽ��MADB-10-9
            Regex rgx3 = new Regex(@"^\d{1,}$");                             //ƥ��ID��
            if (rgx1.IsMatch(id))
            {
                return id;
            }
            else if (rgx2.IsMatch(id))
            {
                return string.Format("{0}-{1}", Constant.INDUSTRYCODE, id);
            }
            else if (rgx3.IsMatch(id))
            {
                return string.Format("{0}-{1}-{2}", Constant.INDUSTRYCODE, queryPara.dataCode, id);
            }
            else
                return id;   //ԭ��Ϊ"" zxw 20131223 Ӧ��
        }
        /// <summary>
        /// �ж��ַ��Ƿ�Ϊurl
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        bool IsUrl(string url)
        {
            if (url.StartsWith("http://"))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// �ж�·�����Ƿ����ͼƬ
        /// </summary>
        /// <param name="_path"></param>
        /// <returns></returns>
        bool ExistPicture(string _path, out string _picName)
        {
            _picName = "";
            if (Directory.Exists(_path))
            {
                string[] files = Directory.GetFiles(_path);
                for (int i = 0; i < files.Length; i++)
                {
                    if (IsPic(files[i]))
                    {
                        _picName = files[i];
                        return true;
                    }
                }
                return false;
            }
            else if (File.Exists(_path))
            {
                if (IsPic(_path))
                {
                    _picName = _path;
                    return true;
                }
            }
            return false;
        }
        public static string[] _picName=null;
        public static void GetExistPicture(string _path, out string[] picName)
        {
            picName = null;
            _picName = null;
            List<string> str = new List<string>();
          
            if (Directory.Exists(_path))
            {
                string[] files = Directory.GetFiles(_path);
                for (int i = 0; i < files.Length; i++)
                {
                    if (IsPic(files[i]))
                    {
                        string file = files[i].Substring(files[i].LastIndexOf("\\") + 1);
                        string filethumb = file.Substring(file.LastIndexOf("_")+1);
                        if (filethumb.Contains("thumb"))
                        {
                            continue;
                        }
                        else
                        {
                            //GF1_PMS2_E113.8_N23.8_20160729_L1A0001728289-MSS2_thumb.jpg ���˵�СͼƬ
                            str.Add(file);
                        }
                                          
                     
                    }
                }
                picName = str.ToArray(); 
                _picName = picName;
               
               
            }
            else if (File.Exists(_path))
            {
                if (IsPic(_path))
                {
                    str.Add(_path);
                    _picName = str.ToArray();
               
                }

                _picName = picName;
            }
          
           
        }

        /// <summary>
        /// �ж��ļ��Ƿ�ΪͼƬ
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        static bool IsPic(string _filename)
        {
            string extent = Path.GetExtension(_filename).ToLower();
            if (extent.Contains("bmp") || extent.Contains("jpg") || extent.Contains("gif") || extent.Contains("png"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //�ж��Ƿ��Ǽ�����Ƭ���һ�ν����ҳ�档
        public bool isFirst = true;
        private void mucDetailViewer_VisibleChanged(object sender, EventArgs e)
        {
            muc3DSearcher _muc3DSearcher = (muc3DSearcher)MSUserInterface.listMSUI[0].uiMainUC;

            if (this.Visible)
            {
                if (_muc3DSearcher.Is3DViewer)
                {
                    _muc3DSearcher.globeClickEvent += GlobeClickEvent;
                }
                else
                {
                    _muc3DSearcher.uc2DSearcher1.addMapClickEvent();
                    _muc3DSearcher.uc2DSearcher1.onClickCompletedEvent += new Action<double, double>(GlobeClickEvent);
                }
                //(DataTable)gridControlDataList.DataSource
                if (gridControlDataList.DataSource != null)
                {
                    DataTable dt = (DataTable)gridControlDataList.DataSource;
                    if (dt.Rows.Count > 0) //���ص�һ����Ϣ
                    {
                        DataRow dr = dt.Rows[0];
                        DisplayDetailInfo(dr);
                        if (dr == null)
                        {
                            //�����ʾ���
                            pictureEditImgData.Image = null;
                            return;
                        }
                        //��ʾ����ͼ��Ϣ
                        if (queryPara.QRST_CODE != null)
                        {
                            string id;
                            if ((!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector")) && selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() != "rcdb" && (!selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")))
                            {
                                id = dr[queryPara.QRST_CODE].ToString();
                                //GetImageDel del = GetImage;
                                //IAsyncResult ar = del.BeginInvoke(id, GetImageCallBack, del);
                                if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))
                                {
                                    string qrst_code = ConvertQRSTCODE(id);
                                    MetaData metaData = MetaDataFactory.CreateMetaDataInstance(qrst_code);
                                    if (metaData is MetaDataImageProd)
                                    {
                                        //����Ƿǹ�Ӱ���Ʒ��������ͼ��ʾ��Ӧԭʼ���ݵĿ���ͼ
                                        try
                                        {
                                            string orgQC = dr["ԭʼ���ݱ���"].ToString();
                                            id = orgQC;

                                        }
                                        catch (Exception ex)
                                        { }

                                    }
                                }
                                try
                                {
                                    GetImageDel del = GetImage;
                                    IAsyncResult ar = del.BeginInvoke(id, GetImageCallBack, del);
                                }
                                catch (Exception ex)
                                { } 
                            }
                            else if (selectedQueryObj.GROUP_CODE.Substring(0, 4).ToLower() == "rcdb")
                            {
                                GetBopuDataDel del = GetBopuData;
                                IAsyncResult ar = del.BeginInvoke(GetBopuDataCallBack, del);
                            }
                            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_vector"))          //չʾʸ������
                            {
                                //���ʸ������  zxw 20131020
                                string storePath = MetaData.GetDataAddress(dr[queryPara.QRST_CODE].ToString());
                                if (Directory.Exists(storePath)) //
                                {
                                    //�첽ִ���������ݼ���
                                    GetShapeFileDel del = GetShapeFile;
                                    IAsyncResult ar = del.BeginInvoke(storePath, GetShapeFileCallBack, del);

                                }
                            }
                            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))             //չʾ��Ƭ���� 
                            {
                                string tileFilename = getDataclient.GetTilesList(new string[] { dr[queryPara.QRST_CODE].ToString() })[0];
                                GetTileImageDel del = GetTileImage;
                                IAsyncResult ar = del.BeginInvoke(tileFilename, GetTileImageCallBack, del);
                            }

                        }
                        else
                        {
                            pictureEditImgData.Image = null;
                        }
                    }
                }

                //������ά��
                if (selectedQueryObj != null && (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster") || selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile")))
                {
                    if (_muc3DSearcher.Is3DViewer)
                    {
                        this.pictureEditImgData.SetQrstAxGlobeControl(_muc3DSearcher.qrstAxGlobeControl1);
                    }
                    else
                    {
                        this.pictureEditImgData.SetUc2DSeacher(_muc3DSearcher.uc2DSearcher1);
                    }
                }

            }
            else
            {
                _muc3DSearcher.SetQrstAxGlobeControl();
                if (_muc3DSearcher.Is3DViewer)
                {
                    _muc3DSearcher.globeClickEvent -= GlobeClickEvent;
                }
                else
                {
                    _muc3DSearcher.uc2DSearcher1.removeMapClickEvent();
                }
            }
        }

        public CtrlPage GetCtrlPage()
        {
            return this.ctrlPage;
        }

        public void ExportMetadata()
        {
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if (dt == null)
            {
                XtraMessageBox.Show("û��Ҫ���������ݣ�");
                return;
            }
            DataTable selectedData = dt.Clone();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((bool)dt.Rows[i][ChooseDataColumnCaption])
                {

                    DataRow dr = selectedData.NewRow();

                    for (int j = 0; j < selectedData.Columns.Count; j++)//2017-1-4֮ǰj=0
                    {
                        dr[j] = dt.Rows[i][j];
                    }
                    selectedData.Rows.Add(dr);
                }
            }

            if (selectedData.Rows.Count == 0)
            {
                MessageBox.Show("û����Ҫ������Ԫ������Ϣ��");
                return;
            }
            else
            {
                FrmExportMetadata frmexportDara = new FrmExportMetadata(selectedData);
                frmexportDara.Show();
            }
        }


        /// <summary>
        /// ���񸲸�ͳ�Ʒ���
        /// </summary>
        public void GridStatistic(int Kc, int Kr)
        {
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if (dt == null)
            {
                return;
            }

            Dictionary<System.Drawing.RectangleF, int> extents = new Dictionary<System.Drawing.RectangleF, int>();

            if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))
            {
                RasterQueryPara para = (RasterQueryPara)queryPara;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Point2d upperleft = new Point2d(double.Parse(dt.Rows[i][para.DATAUPPERLEFTLAT].ToString()), double.Parse(dt.Rows[i][para.DATAUPPERLEFTLONG].ToString()));
                    Point2d upperright = new Point2d(double.Parse(dt.Rows[i][para.DATAUPPERRIGHTLAT].ToString()), double.Parse(dt.Rows[i][para.DATAUPPERRIGHTLONG].ToString()));
                    Point2d lowerleft = new Point2d(double.Parse(dt.Rows[i][para.DATALOWERLEFTLAT].ToString()), double.Parse(dt.Rows[i][para.DATALOWERLEFTLONG].ToString()));
                    Point2d lowerright = new Point2d(double.Parse(dt.Rows[i][para.DATALOWERRIGHTLAT].ToString()), double.Parse(dt.Rows[i][para.DATALOWERRIGHTLONG].ToString()));

                    //������ε���Ӿ���
                    Point2d[] outerExtent = SpacialUtil.SpacialUtils.GetOutExtent(upperleft, upperright, lowerleft, lowerright);
                    //������ε����з�Χ
                    int[] rowcolRange = SpacialUtil.SpacialUtils.GetRowColRange(Kr, Kc, outerExtent[0], outerExtent[1]);
                    //�����к�ת��Ϊ��γ����������extents�ֵ�
                    int rowNum = rowcolRange[1] - rowcolRange[0] + 1;
                    int colNum = rowcolRange[3] - rowcolRange[2] + 1;
                    for (int j = 0; j < rowNum; j++)
                    {
                        for (int k = 0; k < colNum; k++)
                        {
                            System.Drawing.RectangleF key;
                            Point2d[] extent = SpacialUtil.SpacialUtils.GetGridExtent(Kr, Kc, rowcolRange[0] + j + 1 + 1, rowcolRange[2] + k + 1);
                            if (ExitRectage(extents, new RectangleF((float)extent[0].Y, (float)extent[0].X, (float)(extent[1].Y - extent[0].Y), (float)(extent[0].X - extent[1].X)), out key))
                            {
                                extents[key] = extents[key] + 1;
                            }
                            else
                            {
                                extents.Add(key, 1);
                            }
                        }
                    }
                }
            }
            else if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))   //��Ƭͳ��
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string level = dt.Rows[i]["Level"].ToString();
                    string[] rowandColumn = new string[4];
                    rowandColumn[0] = dt.Rows[i]["Row"].ToString();
                    rowandColumn[1] = dt.Rows[i]["Col"].ToString();

                    double[] extent = GetLatAndLong(rowandColumn, level);
                    double minLat = double.Parse(extent[0].ToString());
                    double maxLat = double.Parse(extent[2].ToString());
                    double minLon = double.Parse(extent[1].ToString());
                    double maxLon = double.Parse(extent[3].ToString());

                    Point2d upperleft = new Point2d(maxLat, minLon);
                    Point2d upperright = new Point2d(maxLat, maxLon);
                    Point2d lowerleft = new Point2d(minLat, minLon);
                    Point2d lowerright = new Point2d(minLat, maxLon);

                    System.Drawing.RectangleF key;
                    Point2d[] extent1 = new Point2d[] { lowerleft, upperright };

                    if (ExitRectage(extents, new RectangleF((float)extent1[0].Y, (float)extent1[0].X, (float)(extent1[1].Y - extent1[0].Y), (float)(extent1[0].X - extent1[1].X)), out key))
                    {
                        extents[key] = extents[key] + 1;
                    }
                    else
                    {
                        extents.Add(key, 1);
                    }
                    //}
                    // }
                }
            }
            QRSTWorldGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
            if (_QrstAxGlobeControl != null)
            {
                int maxNum = 0;
                _QrstAxGlobeControl.DrawSearchResultExtents(extents, out maxNum);

                ColorRange colorRange = new ColorRange(maxNum, _QrstAxGlobeControl);
                colorRange.Visible = _QrstAxGlobeControl.IsOn("tmpDrawExtentsLayer1");
                //���ͳ��ɫ��
                if (_QrstAxGlobeControl.Controls.ContainsKey("colorRange"))
                {
                    _QrstAxGlobeControl.Controls.RemoveByKey("colorRange");
                }
                _QrstAxGlobeControl.Controls.Add(colorRange);

            }
        }

        public void LayerControl(string layerName, bool _isOn)
        {
            QRSTWorldGlobeControl _QrstAxGlobeControl = this.pictureEditImgData.GetQrstAxGlobeControl();
            _QrstAxGlobeControl.LayerControl(layerName, _isOn);

            if (layerName == "tmpDrawExtentsLayer1")
            {
                //20130921  dlf���쳣����ж�  �쳣�����ڲ�ѯת����ϸ��Ϣ�����ֱ�ӵ����������
                if (_QrstAxGlobeControl.Controls["colorRange"] != null)
                    _QrstAxGlobeControl.Controls["colorRange"].Visible = _isOn;
            }
        }

        /// <summary>
        /// �жϾ��������Ƿ����
        /// </summary>
        /// <param name="extents"></param>
        /// <param name="rectangleF"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        private bool ExitRectage(Dictionary<System.Drawing.RectangleF, int> extents, System.Drawing.RectangleF rectangleF, out System.Drawing.RectangleF key)
        {
            foreach (KeyValuePair<System.Drawing.RectangleF, int> kvp in extents)
            {
                if (kvp.Key.Equals(rectangleF))
                {
                    key = kvp.Key;
                    return true;
                }
            }

            key = rectangleF;
            return false;
        }

        /// <summary>
        /// ��Ӧ�������¼�,���ݵ���ľ�γ�ȣ��ҳ���ϸ��Ϣ�б��еļ�¼����ʾ
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        public void GlobeClickEvent(double lat, double lon)
        {
            if (selectedQueryObj != null && (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile") || selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster")))
            {
                if (((RasterQueryPara)queryPara).spacialAvailable)
                {
                    RasterQueryPara para = (RasterQueryPara)queryPara;
                    //   List<int> rowHandler = new List<int>(); 
                    Dictionary<int, double> rowInfo = new Dictionary<int, double>();  //��¼�����˸õ�������б�ź����

                    //�޸������������λ����ʵ�ʶ�λ��¼�����Ĵ���   zxw 20131018


                    if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_raster"))
                    {
                        for (int i = 0; i < gridViewMain.RowCount; i++)
                        {
                            Point2d upperleft = new Point2d(double.Parse(gridViewMain.GetDataRow(i)[para.DATAUPPERLEFTLAT].ToString()), double.Parse(gridViewMain.GetDataRow(i)[para.DATAUPPERLEFTLONG].ToString()));
                            Point2d upperright = new Point2d(double.Parse(gridViewMain.GetDataRow(i)[para.DATAUPPERRIGHTLAT].ToString()), double.Parse(gridViewMain.GetDataRow(i)[para.DATAUPPERRIGHTLONG].ToString()));
                            Point2d lowerleft = new Point2d(double.Parse(gridViewMain.GetDataRow(i)[para.DATALOWERLEFTLAT].ToString()), double.Parse(gridViewMain.GetDataRow(i)[para.DATALOWERLEFTLONG].ToString()));
                            Point2d lowerright = new Point2d(double.Parse(gridViewMain.GetDataRow(i)[para.DATALOWERRIGHTLAT].ToString()), double.Parse(gridViewMain.GetDataRow(i)[para.DATALOWERRIGHTLONG].ToString()));
                            if (recContains(upperleft, upperright, lowerleft, lowerright, new Point2d(lat, lon)))
                            {
                                rowInfo.Add(i, GetRectangleArea(upperleft, upperright, lowerleft, lowerright));
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < gridViewMain.RowCount; i++)
                        {
                            string level = gridViewMain.GetDataRow(i)["Level"].ToString();
                            string[] rowandColumn = new string[4];
                            rowandColumn[0] = gridViewMain.GetDataRow(i)["Row"].ToString();
                            rowandColumn[1] = gridViewMain.GetDataRow(i)["Col"].ToString();

                            double[] extent = GetLatAndLong(rowandColumn, level);
                            double minLat = double.Parse(extent[0].ToString());
                            double maxLat = double.Parse(extent[2].ToString());
                            double minLon = double.Parse(extent[1].ToString());
                            double maxLon = double.Parse(extent[3].ToString());

                            Point2d upperleft = new Point2d(maxLat, minLon);
                            Point2d upperright = new Point2d(maxLat, maxLon);
                            Point2d lowerleft = new Point2d(minLat, minLon);
                            Point2d lowerright = new Point2d(minLat, maxLon);
                            if (recContains(upperleft, upperright, lowerleft, lowerright, new Point2d(lat, lon)))
                            {
                                rowInfo.Add(i, GetRectangleArea(upperleft, upperright, lowerleft, lowerright));
                            }
                        }
                    }

                    //�������С�ľ��ο���ʾ
                    if (rowInfo.Count > 0)
                    {
                        int minindex = -1;
                        foreach (KeyValuePair<int, double> keyvalue in rowInfo)
                        {
                            if (minindex == -1)
                            {
                                minindex = keyvalue.Key;
                            }
                            else
                            {
                                if (rowInfo[minindex] > keyvalue.Value)
                                {
                                    minindex = keyvalue.Key;
                                }
                            }
                        }
                        if (minindex != -1)
                        {
                            gridViewMain.FocusedRowHandle = minindex;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// ��ȡ�ı������
        /// </summary>
        /// <returns></returns>
        public double GetRectangleArea(Point2d upperleft, Point2d upperright, Point2d lowerleft, Point2d lowerright)
        {
            double a, b, c;
            a = GetDistance(upperleft, lowerright);
            b = GetDistance(upperleft, upperright);
            c = GetDistance(upperright, lowerright);
            double s1 = GetTriangleArea(a, b, c);

            b = GetDistance(upperleft, lowerleft);
            c = GetDistance(lowerleft, lowerright);
            double s2 = GetTriangleArea(a, b, c);
            return s1 + s2;
        }

        double GetDistance(Point2d p1, Point2d p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
        /// <summary>
        /// �������߳��������������
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        double GetTriangleArea(double a, double b, double c)
        {
            double p = (a + b + c) / 2;
            return Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }

        /// <summary>
        /// �ж�objectPoint�Ƿ���ǰ���ĸ����γɵľ��ε����������
        /// </summary>
        /// <param name="upperleft"></param>
        /// <param name="upperright"></param>
        /// <param name="lowerleft"></param>
        /// <param name="lowerright"></param>
        /// <param name="objectPoint"></param>
        /// <returns></returns>
        public bool recContains(Point2d upperleft, Point2d upperright, Point2d lowerleft, Point2d lowerright, Point2d objectPoint)
        {
            if ((objectPoint.X <= Math.Max(upperright.X, upperleft.X)) && (objectPoint.X >= Math.Min(lowerleft.X, lowerright.X)) && (objectPoint.Y >= Math.Min(lowerleft.Y, upperleft.Y)) && (objectPoint.Y <= Math.Max(lowerright.Y, upperright.Y)))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// �仯   zxw 20131221
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void splitContainerControlgrid_SizeChanged(object sender, EventArgs e)
        {
            if (displaySchema == EnumDisplayEnum.AllTable)
            {

                splitContainerControlgrid.SplitterPosition = splitContainerControlgrid.Width * 3 / 4;
                splitContainerControlTableViewer.SplitterPosition = 0;
                splitContainerControlImgAndDetail.SplitterPosition = 0;
                int height = splitContainerControlgrid.Height;
            }
            else
            {
                splitContainerControlgrid.SplitterPosition = splitContainerControlgrid.Width ;
            }

        }
        //20170107 xmh  ��ղ�ѯ���������б�
        public void DeleteQuery()
        {
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if (dt == null)
            {
                gridControlDataList.DataSource = null;
                return;
            }
            gridViewMain.Columns.Clear();
            dt.Rows.Clear();
            gridControlDataList.DataSource = dt;
        }
        /// <summary>
        /// ���gridVIewMain�ı���е����ݣ����б�ͷ��
        /// </summary>
        public void ClearGridView() 
        {
            gridViewMain.CloseEditor();
            DataTable dt = (DataTable)gridControlDataList.DataSource;
            if (dt == null)
            {
                gridControlDataList.DataSource = null;
                return;
            }
            //gridViewMain.Columns.Clear();
            dt.Rows.Clear();
            gridControlDataList.DataSource = dt;

        }
        /// <summary>
        /// 20170331 xmh
        /// ���gridVIew1�ı���е����ݣ����б�ͷ��
        /// </summary>
        public void ClearGridView1()
        {
            gridView1.CloseEditor();
            DataTable dt = (DataTable)gridControlTileList.DataSource;
            if (dt == null)
            {
                gridControlTileList.DataSource = null;
                return;
            }
            //gridView1.Columns.Clear();
            dt.Rows.Clear();
            gridControlTileList.DataSource = dt;

        }
        
        /// <summary>
        ///  xmh  ��Ƭ��ȫ���Ǽ���ʱ��ȷ��ѡ�а�ť
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonTileSubmit_Click(object sender, EventArgs e)
        {
            //row = string.Format("{0}", dr["Row"].ToString());
            if (gridView1.FocusedRowHandle != null)
            {
                DataRow dr = gridView1.GetDataRow(gridView1.FocusedRowHandle);
                DataRow gridViewMaindr = gridViewMain.GetDataRow(gridViewMain.FocusedRowHandle);
                if (selectedQueryObj.GROUP_TYPE.ToLower().Equals("system_tile"))
                {
                    queryPara = new RasterQueryPara();
                    queryPara.QRST_CODE = "TileFileName";
                    if (selectedQueryObj.NAME.Equals("���Ӱ������") || selectedQueryObj.NAME.Equals("���Ӱ���������"))
                    {
                        for (int i = 0; i < gridViewMain.Columns.Count; i++)
                        {
                            if (i < 13 && i != gridViewMain.Columns.Count -1 )
                            {
                                //gridViewMaindr["TileFileName"] = dr["TileFileName"];
                                gridViewMaindr[i] = dr[i].ToString();

                            }
                        }
                    }
                    else
                    {
                        queryPara = new RasterQueryPara();
                        queryPara.QRST_CODE = "TileFileName";
                        if (selectedQueryObj.NAME.Equals("��񻯲�Ʒ����"))
                        {
                            for (int i = 0; i < gridViewMain.Columns.Count; i++)
                            {
                                if ( i != gridViewMain.Columns.Count - 1)
                                {
                                    //gridViewMaindr["TileFileName"] = dr["TileFileName"];
                                    gridViewMaindr[i] = dr[i].ToString();
                                    
                                }
                                
                            }

                        }
                    }
                    
                }
                
                //gridViewMain�޸���֮���޸���ϸ��Ϣ�б�
                DisplayDetailInfo(gridViewMaindr);
                //��ͼ����������ͼ��ťʱ��������ֱ����ͼ�����û�е����ͼ��ť������Ҫ��ͼ����������¼
                if (ruc3DSearcher.clickLyr == true)
                {
                    if (((RasterQueryPara)queryPara).spacialAvailable)
                    {
                        DrawRasterExtent((RasterQueryPara)queryPara);
                        AddTileLyrOrder((RasterQueryPara)queryPara);
                    }

                }
                else
                {
                    if (((RasterQueryPara)queryPara).spacialAvailable)
                    {
                        DrawRasterExtent((RasterQueryPara)queryPara);                        
                    }
                }
            }
            else
            {
                MessageBox.Show("��ѡ��һ������");
            }

        }
        /// <summary>
        /// xmh  20170329
        /// ��ѡ��ĳһ��ʱ������ʾ(CustomDrawCell����Ҳ����)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridView1_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle == gridView1.FocusedRowHandle)
            {
               e.Appearance.BackColor = Color.FromArgb(255,226, 236, 247);
                //e.Appearance.BackColor = Color.CadetBlue;
            }
            else 
            {
                e.Appearance.BackColor = Color.White;
            }

        }
    }

    public enum EnumDisplayEnum
    {
        AllTable = 0,   //ȫ��ģʽ
        OnlyTable = 3,   //����ģʽ�����ȫ��ģʽû����ϸ���
        AllPicture = 1,  //ȫͼ
        Normal = 2,      //����ģʽ

    }

}
