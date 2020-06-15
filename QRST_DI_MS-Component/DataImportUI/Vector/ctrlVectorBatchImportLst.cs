using System;
using System.Collections.Generic;
using System.Windows.Forms;
 
namespace QRST_DI_MS_Component_DataImportorUI.Vector
{
    public partial class ctrlVectorBatchImportLst : UserControl
    {
        public delegate void ItemStateChanged(int checkedItemCount);

        public ItemStateChanged itemStateChangedDel;

        public ctrlVectorBatchImportLst()
        {
            InitializeComponent();
            ctrlVectorUserMetaDataSetting1.Create();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            SetAllChecked(CheckState.Checked);
        }

        private void btnRemoveAll_Click(object sender, EventArgs e)
        {
            SetAllChecked(CheckState.Unchecked);
        }

        private void SetAllChecked(CheckState checkState)
        {
            for (int i = 0; i < cbImportDataLst.Items.Count; i++)
            {
                cbImportDataLst.SetItemCheckState(i, checkState);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            cbImportDataLst.Refresh();
        }
        
        /// <summary>
        /// 当check状态发生变化时触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbImportDataLst_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int itemCount = cbImportDataLst.CheckedItems.Count;
            if (e.CurrentValue == CheckState.Checked)
                itemCount--;
            else
                itemCount++;
            if (itemStateChangedDel!=null)
            {
                itemStateChangedDel(itemCount);
            }
        }

        public List<SingleDataVector> GetCheckedItems()
        {
            List<SingleDataVector> singleDataLst = new List<SingleDataVector>();
            for (int i = 0; i < cbImportDataLst.CheckedItems.Count;i++ )
            {
                singleDataLst.Add((SingleDataVector)cbImportDataLst.CheckedItems[i]);
            }

            return singleDataLst;
        }

        public void AddItems(List<SingleDataVector> objLst)
        {
            cbImportDataLst.Items.Clear();
            cbImportDataLst.Items.AddRange(objLst.ToArray());
            SetAllChecked(CheckState.Checked);
        }


        internal void SetCustomizedMetaData(SingleDataVector temp)
        {
            ctrlVectorUserMetaDataSetting1.SetCustomizedMetaData(temp);
            //在元数据中添加用以描述等内容
        }
    }
}
