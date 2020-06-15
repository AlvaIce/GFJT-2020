using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraEditors;
using System.Data;
using QRST_DI_DS_Metadata.MetaDataDefiner;

namespace QRST_DI_MS_Console.UserInterfaces
{
    class rucSatalliteSensor : RibbonPageBaseUC
    {
        private DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup repositoryItemRadioGroup1;
        private DevExpress.XtraBars.BarEditItem barEditItem2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextKeyWord;
        private DevExpress.XtraBars.BarButtonItem barButtonItemAdd;
        private DevExpress.XtraBars.BarButtonItem barButtonDelete;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit repositoryItemButtonEdit1;
        private DevExpress.XtraBars.BarButtonItem barButtonAddSensor;
        private DevExpress.XtraBars.BarButtonItem barButtonDeleteSensor;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup4;
        private DevExpress.XtraBars.BarEditItem barEditItemInfoType;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBox1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;

             public rucSatalliteSensor()
            : base()
        {
            InitializeComponent();
        }

             public rucSatalliteSensor(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
            this.barEditItemInfoType.EditValue = "卫星数据";
        }
    
        private void InitializeComponent()
        {
            this.repositoryItemRadioGroup1 = new DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barEditItem2 = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemTextKeyWord = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonItemAdd = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonDelete = new DevExpress.XtraBars.BarButtonItem();
            this.repositoryItemButtonEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.ribbonPageGroup4 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.barButtonAddSensor = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonDeleteSensor = new DevExpress.XtraBars.BarButtonItem();
            this.barEditItemInfoType = new DevExpress.XtraBars.BarEditItem();
            this.repositoryItemComboBox1 = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextKeyWord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).BeginInit();
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
            this.barEditItem2,
            this.barButtonItemAdd,
            this.barButtonDelete,
            this.barButtonAddSensor,
            this.barButtonDeleteSensor,
            this.barEditItemInfoType});
            this.ribbonControl1.MaxItemId = 14;
            this.ribbonControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemRadioGroup1,
            this.repositoryItemTextKeyWord,
            this.repositoryItemButtonEdit1,
            this.repositoryItemComboBox1});
            this.ribbonControl1.Size = new System.Drawing.Size(687, 149);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2,
            this.ribbonPageGroup3,
            this.ribbonPageGroup4});
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.barEditItemInfoType);
            this.ribbonPageGroup1.Text = "信息类型";
            // 
            // repositoryItemRadioGroup1
            // 
            this.repositoryItemRadioGroup1.Columns = 1;
            this.repositoryItemRadioGroup1.Items.AddRange(new DevExpress.XtraEditors.Controls.RadioGroupItem[] {
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "卫星数据"),
            new DevExpress.XtraEditors.Controls.RadioGroupItem(null, "传感器数据")});
            this.repositoryItemRadioGroup1.Name = "repositoryItemRadioGroup1";
            this.repositoryItemRadioGroup1.SelectedIndexChanged += new System.EventHandler(this.repositoryItemRadioGroup1_SelectedIndexChanged);
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.barEditItem2);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "检索";
            // 
            // barEditItem2
            // 
            this.barEditItem2.Caption = "关键字";
            this.barEditItem2.Edit = this.repositoryItemTextKeyWord;
            this.barEditItem2.Id = 3;
            this.barEditItem2.Name = "barEditItem2";
            this.barEditItem2.Width = 150;
            // 
            // repositoryItemTextKeyWord
            // 
            this.repositoryItemTextKeyWord.AutoHeight = false;
            this.repositoryItemTextKeyWord.Name = "repositoryItemTextKeyWord";
            this.repositoryItemTextKeyWord.EditValueChanged += new System.EventHandler(this.repositoryItemTextKeyWord_EditValueChanged);
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonItemAdd);
            this.ribbonPageGroup3.ItemLinks.Add(this.barButtonDelete, true);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "卫星数据操作";
            // 
            // barButtonItemAdd
            // 
            this.barButtonItemAdd.Caption = "添加";
            this.barButtonItemAdd.Id = 5;
            this.barButtonItemAdd.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.卫星数据添加;
            this.barButtonItemAdd.Name = "barButtonItemAdd";
            this.barButtonItemAdd.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonItemAdd.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItemAdd_ItemClick);
            // 
            // barButtonDelete
            // 
            this.barButtonDelete.Caption = "删除";
            this.barButtonDelete.Id = 7;
            this.barButtonDelete.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.卫星数据删除;
            this.barButtonDelete.Name = "barButtonDelete";
            this.barButtonDelete.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonDelete.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonDelete_ItemClick);
            // 
            // repositoryItemButtonEdit1
            // 
            this.repositoryItemButtonEdit1.AutoHeight = false;
            this.repositoryItemButtonEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton()});
            this.repositoryItemButtonEdit1.Name = "repositoryItemButtonEdit1";
            // 
            // ribbonPageGroup4
            // 
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonAddSensor);
            this.ribbonPageGroup4.ItemLinks.Add(this.barButtonDeleteSensor, true);
            this.ribbonPageGroup4.Name = "ribbonPageGroup4";
            this.ribbonPageGroup4.Text = "传感器数据操作";
            // 
            // barButtonAddSensor
            // 
            this.barButtonAddSensor.Caption = "添加";
            this.barButtonAddSensor.Enabled = false;
            this.barButtonAddSensor.Id = 9;
            this.barButtonAddSensor.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.传感器数据添加;
            this.barButtonAddSensor.Name = "barButtonAddSensor";
            this.barButtonAddSensor.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonAddSensor.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonAddSensor_ItemClick);
            // 
            // barButtonDeleteSensor
            // 
            this.barButtonDeleteSensor.Caption = "删除";
            this.barButtonDeleteSensor.Enabled = false;
            this.barButtonDeleteSensor.Id = 11;
            this.barButtonDeleteSensor.LargeGlyph = global::QRST_DI_MS_Console.Properties.Resources.传感器数据删除;
            this.barButtonDeleteSensor.Name = "barButtonDeleteSensor";
            this.barButtonDeleteSensor.RibbonStyle = DevExpress.XtraBars.Ribbon.RibbonItemStyles.Large;
            this.barButtonDeleteSensor.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonDeleteSensor_ItemClick);
            // 
            // barEditItemInfoType
            // 
            this.barEditItemInfoType.Caption = "信息类型";
            this.barEditItemInfoType.Edit = this.repositoryItemComboBox1;
            this.barEditItemInfoType.Id = 13;
            this.barEditItemInfoType.Name = "barEditItemInfoType";
            this.barEditItemInfoType.Width = 80;
            this.barEditItemInfoType.EditValueChanged += new System.EventHandler(this.barEditItemInfoType_EditValueChanged);
            // 
            // repositoryItemComboBox1
            // 
            this.repositoryItemComboBox1.AutoHeight = false;
            this.repositoryItemComboBox1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBox1.Items.AddRange(new object[] {
            "卫星数据",
            "传感器数据"});
            this.repositoryItemComboBox1.Name = "repositoryItemComboBox1";
            // 
            // rucSatalliteSensor
            // 
            this.Name = "rucSatalliteSensor";
            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemRadioGroup1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextKeyWord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemButtonEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        /// <summary>
        /// 添加卫星
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonItemAdd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucSatalliteSensor)ObjMainUC).isAdd = true;
            ((mucSatalliteSensor)ObjMainUC).InitializeAddControl();
        }

        /// <summary>
        /// 保存更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonSaveUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucSatalliteSensor)ObjMainUC).SaveUpdate();
        }

        /// <summary>
        /// 卫星删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonDelete_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (((mucSatalliteSensor)ObjMainUC).gridView1.FocusedRowHandle < 0)
            {
                XtraMessageBox.Show("没有需要删除的数据！");
                return;
            }
            string name = ((DataTable)((mucSatalliteSensor)ObjMainUC).gridControl1.DataSource).Rows[((mucSatalliteSensor)ObjMainUC).gridView1.FocusedRowHandle]["NAME"].ToString();
            int id = Convert.ToInt32(((DataTable)((mucSatalliteSensor)ObjMainUC).gridControl1.DataSource).Rows[((mucSatalliteSensor)ObjMainUC).gridView1.FocusedRowHandle]["ID"].ToString());
            System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(this, string.Format("确定要删除'{0}'", name), "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Satellite.Delete(id, TheUniversal.EVDB.sqlUtilities);
                ((mucSatalliteSensor)ObjMainUC).GetSatalliteDataSource("");
            }
        }

        /// <summary>
        /// 添加传感器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonAddSensor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucSatalliteSensor)ObjMainUC).isAdd = true;
            ((mucSatalliteSensor)ObjMainUC).InitializeAddControl();
        }

        /// <summary>
        /// 保存传感器更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonSaveUpdateSnesor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ((mucSatalliteSensor)ObjMainUC).SaveUpdate();
        }

        /// <summary>
        /// 删除传感器
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barButtonDeleteSensor_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (((mucSatalliteSensor)ObjMainUC).gridView1.FocusedRowHandle < 0)
            {
                XtraMessageBox.Show("没有需要删除的数据！");
                return;
            }

            string name = ((DataTable)((mucSatalliteSensor)ObjMainUC).gridControl1.DataSource).Rows[((mucSatalliteSensor)ObjMainUC).gridView1.FocusedRowHandle]["NAME"].ToString();
            int id = Convert.ToInt32(((DataTable)((mucSatalliteSensor)ObjMainUC).gridControl1.DataSource).Rows[((mucSatalliteSensor)ObjMainUC).gridView1.FocusedRowHandle]["ID"].ToString());
            System.Windows.Forms.DialogResult result = DevExpress.XtraEditors.XtraMessageBox.Show(this, string.Format("确定要删除'{0}'", name), "提示", System.Windows.Forms.MessageBoxButtons.YesNo);
            if (result == System.Windows.Forms.DialogResult.Yes)
            {
                Sensors.Delete(id,TheUniversal.EVDB.sqlUtilities);
                ((mucSatalliteSensor)ObjMainUC).GetSensorsDataSource("");
            }
          
        }

        /// <summary>
        /// 切换卫星与传感器列表显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemRadioGroup1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = "";
            if (((RadioGroup)sender).SelectedIndex == 0)
            {
                ((mucSatalliteSensor)ObjMainUC).datatype = "satellites";
                ((mucSatalliteSensor)ObjMainUC).GetSatalliteDataSource("");

                barButtonItemAdd.Enabled = true;
                barButtonDelete.Enabled = true;
                //barButtonSaveUpdate.Enabled = true;

                barButtonAddSensor.Enabled = false;
                barButtonDeleteSensor.Enabled = false;
               // barButtonSaveUpdateSnesor.Enabled = false;

            }
            else
            {
                barButtonItemAdd.Enabled = false;
                barButtonDelete.Enabled = false;
                //barButtonSaveUpdate.Enabled = false;

                barButtonAddSensor.Enabled = true;
                barButtonDeleteSensor.Enabled = true;
               // barButtonSaveUpdateSnesor.Enabled = true;

                ((mucSatalliteSensor)ObjMainUC).datatype = "sensors";
                ((mucSatalliteSensor)ObjMainUC).GetSensorsDataSource("");
            }
         //   repositoryItemRadioGroup
        }

        /// <summary>
        /// 关键字查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void repositoryItemTextKeyWord_EditValueChanged(object sender, EventArgs e)
        {
            string keyWord = ((TextEdit)sender).Text;
            if ( ((mucSatalliteSensor)ObjMainUC).datatype == "satellites")
            {
                  ((mucSatalliteSensor)ObjMainUC).GetSatalliteDataSource(string.Format("where NAME like '%{0}%'",keyWord));
            }
            else
            {
                ((mucSatalliteSensor)ObjMainUC).GetSensorsDataSource(string.Format("where NAME like '%{0}%'", keyWord));
            }
        }

        /// <summary>
        /// 当切换管理内容时,触发该事件
        /// 2013/2/21 zxw
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void barEditItemInfoType_EditValueChanged(object sender, EventArgs e)
        {
           // XtraMessageBox.Show("数据切换！");
            //object obj = sender;
            DevExpress.XtraBars.BarEditItem obj = (DevExpress.XtraBars.BarEditItem)sender;
            if (obj.EditValue.ToString().Trim() == "卫星数据")
            {
                ((mucSatalliteSensor)ObjMainUC).datatype = "satellites";
                ((mucSatalliteSensor)ObjMainUC).GetSatalliteDataSource("");

                barButtonItemAdd.Enabled = true;
                barButtonDelete.Enabled = true;
               // barButtonSaveUpdate.Enabled = true;

                barButtonAddSensor.Enabled = false;
                barButtonDeleteSensor.Enabled = false;
              //  barButtonSaveUpdateSnesor.Enabled = false;
            }
            else if (obj.EditValue.ToString().Trim() == "传感器数据")
            {
                barButtonItemAdd.Enabled = false;
                barButtonDelete.Enabled = false;
              //  barButtonSaveUpdate.Enabled = false;

                barButtonAddSensor.Enabled = true;
                barButtonDeleteSensor.Enabled = true;
               // barButtonSaveUpdateSnesor.Enabled = true;

                ((mucSatalliteSensor)ObjMainUC).datatype = "sensors";
                ((mucSatalliteSensor)ObjMainUC).GetSensorsDataSource("");
            }
        }
    }
}
