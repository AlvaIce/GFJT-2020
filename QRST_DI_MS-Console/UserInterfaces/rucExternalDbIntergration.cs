using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using DevExpress.XtraEditors;

namespace QRST_DI_MS_Console.UserInterfaces
{
    class rucExternalDbIntergration : RibbonPageBaseUC
    {
        private DevExpress.XtraBars.BarButtonItem barItemDeleteLink;
        private DevExpress.XtraBars.BarButtonItem barItemUpdateLink;
        private DevExpress.XtraBars.BarEditItem barEditItemName;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemUrl;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
        private DevExpress.XtraBars.BarEditItem barEditItemDescription;
        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit repositoryItemMemoEdit1;
        private DevExpress.XtraBars.BarEditItem barEditItemImage;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEdit1;
        private DevExpress.XtraBars.BarButtonItem barButtonItemUploadImage;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem barButtonSave;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem barItemAddLink;

        private void InitializeComponent()
        {
            this.barItemAddLink = new DevExpress.XtraBars.BarButtonItem();
            this.barItemDeleteLink = new DevExpress.XtraBars.BarButtonItem();
            this.barItemUpdateLink = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItemName = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemUrl = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.barEditItemDescription = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
            this.barEditItemImage = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemPictureEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.barButtonItemUploadImage = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonSave = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).BeginInit();
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
            this.barItemAddLink,
            this.barItemDeleteLink,
            this.barItemUpdateLink,
            this.barEditItemName,
            this.barEditItemUrl,
            this.barEditItemDescription,
            this.barEditItemImage,
            this.barButtonItemUploadImage,
            this.barButtonSave});
            this.ribbonControl1.MaxItemId = 10;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repositoryItemTextEdit2,
            this.repositoryItemMemoEdit1,
            this.repositoryItemPictureEdit1});
            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barItemAddLink, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItemUpdateLink, true);
            this.ribbonPageGroup1.ItemLinks.Add(this.barItemDeleteLink, true);
            this.ribbonPageGroup1.Text = "操作";
            // 
            // barItemAddLink
            // 
            this.barItemAddLink.Caption = "新建链接";
            this.barItemAddLink.Id = 1;
            this.barItemAddLink.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.新建连接;
            this.barItemAddLink.Name = "barItemAddLink";
            this.barItemAddLink.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barItemAddLink.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItemAddLink_ItemClick);
            // 
            // barItemDeleteLink
            // 
            this.barItemDeleteLink.Caption = "删除链接";
            this.barItemDeleteLink.Enabled = false;
            this.barItemDeleteLink.Id = 2;
            this.barItemDeleteLink.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.删除链接;
            this.barItemDeleteLink.Name = "barItemDeleteLink";
            this.barItemDeleteLink.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barItemDeleteLink.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItemDeleteLink_ItemClick);
            // 
            // barItemUpdateLink
            // 
            this.barItemUpdateLink.Caption = "修改链接";
            this.barItemUpdateLink.Enabled = false;
            this.barItemUpdateLink.Id = 3;
            this.barItemUpdateLink.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.修改链接;
            this.barItemUpdateLink.Name = "barItemUpdateLink";
            this.barItemUpdateLink.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barItemUpdateLink.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barItemUpdateLink_ItemClick);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemName);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemUrl);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemDescription, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItemImage, true);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItemUploadImage, true);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "信息设置";
            // 
            // barEditItemName
            // 
            this.barEditItemName.Caption = "数据库名";
            this.barEditItemName.Edit = this.repositoryItemTextEdit1;
            this.barEditItemName.Id = 4;
            this.barEditItemName.Name = "barEditItemName";
            this.barEditItemName.Width = 200;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barEditItemUrl
            // 
            this.barEditItemUrl.Caption = "链接地址";
            this.barEditItemUrl.Edit = this.repositoryItemTextEdit2;
            this.barEditItemUrl.Id = 5;
            this.barEditItemUrl.Name = "barEditItemUrl";
            this.barEditItemUrl.Width = 200;
            // 
            // repositoryItemTextEdit2
            // 
            this.repositoryItemTextEdit2.AutoHeight = false;
            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // barEditItemDescription
            // 
            this.barEditItemDescription.Caption = "描述信息";
            this.barEditItemDescription.Edit = this.repositoryItemMemoEdit1;
            this.barEditItemDescription.EditHeight = 70;
            this.barEditItemDescription.Id = 6;
            this.barEditItemDescription.Name = "barEditItemDescription";
            this.barEditItemDescription.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barEditItemDescription.Width = 250;
            // 
            // repositoryItemMemoEdit1
            // 
            this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
            // 
            // barEditItemImage
            // 
            this.barEditItemImage.AutoHideEdit = false;
            this.barEditItemImage.CanOpenEdit = false;
            this.barEditItemImage.Caption = "图片";
            this.barEditItemImage.Edit = this.repositoryItemPictureEdit1;
            this.barEditItemImage.EditHeight = 70;
            this.barEditItemImage.Id = 7;
            this.barEditItemImage.Name = "barEditItemImage";
            this.barEditItemImage.Width = 70;
            // 
            // repositoryItemPictureEdit1
            // 
            this.repositoryItemPictureEdit1.InitialImage = global::QRST_DI_MS_Console.Properties.Resources.北师大数据库;
            this.repositoryItemPictureEdit1.Name = "repositoryItemPictureEdit1";
            this.repositoryItemPictureEdit1.PictureStoreMode = DevExpress.XtraEditors.Controls.PictureStoreMode.ByteArray;
            this.repositoryItemPictureEdit1.ShowMenu = false;
            this.repositoryItemPictureEdit1.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            // 
            // barButtonItemUploadImage
            // 
            this.barButtonItemUploadImage.Caption = "上传图片";
            this.barButtonItemUploadImage.Id = 8;
            this.barButtonItemUploadImage.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.上传图片1;
            this.barButtonItemUploadImage.Name = "barButtonItemUploadImage";
            this.barButtonItemUploadImage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemUploadImage_ItemClick);
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonSave, true);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "保存";
            // 
            // barButtonSave
            // 
            this.barButtonSave.Caption = "保存";
            this.barButtonSave.Id = 9;
            this.barButtonSave.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.保存2;
            this.barButtonSave.Name = "barButtonSave";
            this.barButtonSave.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonSave.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonSave_ItemClick);
            // 
            // rucExternalDbIntergration
            // 
            this.Name = "rucExternalDbIntergration";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEdit1)).EndInit();
            this.ResumeLayout(false);

        }
        private bool isAdd = true;
        private mucExternalDbIntergration mucObject;
        public rucExternalDbIntergration(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
            mucObject = (mucExternalDbIntergration)ObjMainUC;
            mucObject.ClickEvent += ItemClick;
        }
        
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barItemAddLink_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isAdd = true;
            ClearInterface();
        }

        /// <summary>
        /// 删除
        /// 2013/2/22 zxw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barItemDeleteLink_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
              System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(this, "确定要删除选中的外部数据库？", "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
              if (result == System.Windows.Forms.DialogResult.Yes)
              {
                  if (mucObject.externalDb != null)
                {
                Externaldb.Delete(mucObject.externalDb.ID, TheUniversal.MIDB.sqlUtilities);
                ClearInterface();
                mucObject.ClearInterface();
              //  mucObject.externalDb = null;
                mucObject.flowLayoutPanel1.Controls.RemoveAt(mucObject.selectedIndex);
                if (mucObject.flowLayoutPanel1.Controls.Count > 0)
                {
                    //如果列表不为空，则手动触发一次flowLayoutPanel1.Controls[0]的click事件
                    ((CtrlExternalDBMdl)mucObject.flowLayoutPanel1.Controls[0]).ItemClick();
                }
                else
                {
                    barItemDeleteLink.Enabled = false;
                    barItemUpdateLink.Enabled = false;
                }
                      
                 }
              }
     
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barItemUpdateLink_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            isAdd = false;
            Externaldb db = mucObject.externalDb;
            barEditItemName.EditValue = db.NAME;
            barEditItemDescription.EditValue = db.DESCRIPTION;
            barEditItemUrl.EditValue = db.URL;
            barEditItemImage.EditValue = db.IMAGE;
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemUploadImage_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Stream myStream;
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "img files (*.jpg)|*.jpg|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = openFileDialog1.OpenFile()) != null)
                {
                    // Insert code to read the stream here.
                    byte[] bytes = new byte[myStream.Length];
                    myStream.Read(bytes, 0, bytes.Length);
                    barEditItemImage.EditValue = bytes;
                    myStream.Close();
                }
            }
        }

        /// <summary>
        /// 清理界面
        /// </summary>
        void ClearInterface()
        {
            barEditItemDescription.EditValue = "";
            barEditItemImage.EditValue = null;
            barEditItemName.EditValue = "";
            barEditItemUrl.EditValue = "";
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (Check())
            {
                Externaldb externaldb = GetExternalDbFromInterface();
                if (isAdd)
                {  
                   Externaldb.Add(externaldb,TheUniversal.MIDB.sqlUtilities);
                   
                }
                else
                {
                    externaldb.ID = mucObject.externalDb.ID;
                    Externaldb.Update(externaldb,TheUniversal.MIDB.sqlUtilities);
                }
                ClearInterface();
                mucObject.InitializeLst();
            }
        }

        bool Check()
        {
            if(string.IsNullOrEmpty(barEditItemName.EditValue.ToString()) )
            {
                XtraMessageBox.Show("请输入外部数据库名称！");
                return false;
            }
            if(barEditItemDescription.EditValue.ToString().Length > 2000)
            {
                XtraMessageBox.Show("对外部数据库的描述太长！");
                return false;
            }
            return true;
        }

        Externaldb GetExternalDbFromInterface()
        {
            Externaldb externaldb = new Externaldb();
            externaldb.NAME = barEditItemName.EditValue.ToString();
            externaldb.DESCRIPTION = barEditItemDescription.EditValue.ToString();
            externaldb.URL = barEditItemUrl.EditValue.ToString();
            externaldb.IMAGE = (byte[])barEditItemImage.EditValue;
            return externaldb;
        }

        //列表单击事件
        void ItemClick(object sender, EventArgs e)
        {
            barItemDeleteLink.Enabled = true;
            barItemUpdateLink.Enabled = true;
            ClearInterface();
        }

    }
}
