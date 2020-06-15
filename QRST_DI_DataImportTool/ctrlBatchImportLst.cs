using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_DataImportTool.DataImport;

namespace QRST_DI_DataImportTool
{
    public partial class ctrlBatchImportLst : UserControl
    {
        public delegate void ItemStateChanged(int checkedItemCount);

        public ItemStateChanged itemStateChangedDel;

        public ctrlBatchImportLst()
        {
            InitializeComponent();
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

        public List<SingleData> GetCheckedItems()
        {
            List<SingleData> singleDataLst = new List<SingleData>();
            for (int i = 0; i < cbImportDataLst.CheckedItems.Count;i++ )
            {
                singleDataLst.Add((SingleData)cbImportDataLst.CheckedItems[i]);
            }

            return singleDataLst;
        }

        public void AddItems(List<SingleData> objLst)
        {
            cbImportDataLst.Items.Clear();
            cbImportDataLst.Items.AddRange(objLst.ToArray());
            SetAllChecked(CheckState.Checked);
          
        }

    }
}
