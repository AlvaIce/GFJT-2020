namespace QRST_DI_MS_Console.UserInterfaces
{
    partial class mucSpectrumSearch
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.gridControlTable = new DevExpress.XtraGrid.GridControl();
            this.gridViewMain = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.Rel = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.repositoryItemPictureEditThumb = new DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit();
            this.repositoryItemCheckEditSelected = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repositoryItemHyperLinkEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit();
           this.ctrlPage1 = new QRST_DI_MS_Console.UserInterfaces.CtrlPage();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMain)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEditThumb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditSelected)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).BeginInit();
            this.SuspendLayout();
            // 
            // gridControlTable
            // 
            this.gridControlTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControlTable.Location = new System.Drawing.Point(0, 0);
            this.gridControlTable.MainView = this.gridViewMain;
            this.gridControlTable.Name = "gridControlTable";
            this.gridControlTable.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.Rel,
            this.repositoryItemPictureEditThumb,
            this.repositoryItemCheckEditSelected,
            this.repositoryItemHyperLinkEdit1});
            this.gridControlTable.Size = new System.Drawing.Size(743, 220);
            this.gridControlTable.TabIndex = 5;
            this.gridControlTable.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewMain});
            // 
            // gridViewMain
            // 
            this.gridViewMain.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.gridViewMain.GridControl = this.gridControlTable;
            this.gridViewMain.Name = "gridViewMain";
            this.gridViewMain.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewMain.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.gridViewMain.OptionsMenu.EnableColumnMenu = false;
            this.gridViewMain.OptionsView.ColumnAutoWidth = false;
            this.gridViewMain.OptionsView.ShowGroupPanel = false;
            this.gridViewMain.RowHeight = 50;
            // 
            // Rel
            // 
            this.Rel.AutoHeight = false;
            this.Rel.AutoSearchColumnIndex = 2;
            this.Rel.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.Rel.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("FID", "编号"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("FNAME", "姓名"),
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("FSEX", "性别")});
            this.Rel.ImmediatePopup = true;
            this.Rel.Name = "Rel";
            this.Rel.NullText = "";
            this.Rel.SearchMode = DevExpress.XtraEditors.Controls.SearchMode.OnlyInPopup;
            // 
            // repositoryItemPictureEditThumb
            // 
            this.repositoryItemPictureEditThumb.Name = "repositoryItemPictureEditThumb";
            this.repositoryItemPictureEditThumb.PictureStoreMode = DevExpress.XtraEditors.Controls.PictureStoreMode.ByteArray;
            // 
            // repositoryItemCheckEditSelected
            // 
            this.repositoryItemCheckEditSelected.AutoHeight = false;
            this.repositoryItemCheckEditSelected.Name = "repositoryItemCheckEditSelected";
            // 
            // repositoryItemHyperLinkEdit1
            // 
            this.repositoryItemHyperLinkEdit1.AutoHeight = false;
            this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
            // 
            // ctrlPage1
            // 
            this.ctrlPage1.CurrentPage = 1;
            this.ctrlPage1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ctrlPage1.Location = new System.Drawing.Point(0, 220);
            this.ctrlPage1.Name = "ctrlPage1";
            this.ctrlPage1.PageSize = 20;
            this.ctrlPage1.Size = new System.Drawing.Size(743, 30);
            this.ctrlPage1.TabIndex = 6;
            this.ctrlPage1.TotalPage = 1;
            // 
            // mucSpectrumSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridControlTable);
            this.Controls.Add(this.ctrlPage1);
            this.Name = "mucSpectrumSearch";
            this.Size = new System.Drawing.Size(743, 250);
            ((System.ComponentModel.ISupportInitialize)(this.gridControlTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewMain)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Rel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPictureEditThumb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEditSelected)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemHyperLinkEdit1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit Rel;
        private DevExpress.XtraEditors.Repository.RepositoryItemPictureEdit repositoryItemPictureEditThumb;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEditSelected;
        public DevExpress.XtraGrid.GridControl gridControlTable;
        public CtrlPage ctrlPage1;
        private DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;
        public DevExpress.XtraGrid.Views.Grid.GridView gridViewMain;


    }
}
