using System;
using System.Collections.Generic;
using System.Data;
using DevExpress.XtraGrid;
using QRST_DI_DS_MetadataQuery.PagingQuery;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class CtrlPage : DevExpress.XtraEditors.XtraUserControl
    {
        public delegate void QueryFinishedEventHandle();         //定义一种委托类型
        public delegate void DeviceQueryEventHandle();
        public QueryFinishedEventHandle queryFinishedEventHandle;  //查询结束后执行此委托
        public event DeviceQueryEventHandle devicequeryevent;
        public DataSet ds
        {
            get;
            set;
        }

        public DataTable dt  //存储当前页的数据  
        {
            get;
            set;
        }  

        public int _recordNum = 0;                               //记录总数

        private int[] pageSizeArr = new int[] { 5, 10, 15, 20, 25,50,100,500,1000,2000,5000,10000,30000,50000,100000};

        public int PageSize { get; set; }

        public int CurrentPage { get; set; }

        public int TotalPage { get; set; }

        private IPagingQuery queryObj;

        public CtrlPage()
        {

            InitializeComponent();

            cmbPageSize.Properties.Items.AddRange(pageSizeArr);
            cmbPageSize.SelectedIndex = 3;

            //RefreshInterface();
        }

        public void SetPageSize(int size)
        {
            switch (size)
            {
                case 5:
                    cmbPageSize.SelectedIndex = 0;
                    break;
                case 10:
                    cmbPageSize.SelectedIndex = 1;
                    break;
                case 15:
                    cmbPageSize.SelectedIndex = 2;
                    break;
                case 20:
                    cmbPageSize.SelectedIndex = 3;
                    break;
                case 25:
                    cmbPageSize.SelectedIndex = 4;
                    break;
                case 50:
                    cmbPageSize.SelectedIndex = 5;
                    break;
                case 100:
                    cmbPageSize.SelectedIndex = 6;
                    break;
                case 500:
					cmbPageSize.SelectedIndex = 7;
					break;
				case 1000:
                    cmbPageSize.SelectedIndex = 8;
                    break;
				case 2000:
					cmbPageSize.SelectedIndex = 9;
					break;
                case 5000:
                    cmbPageSize.SelectedIndex = 10;
                    break;
                case 10000:
                    cmbPageSize.SelectedIndex = 11;
					break;
				case 30000:
					cmbPageSize.SelectedIndex = 12;
					break;
				case 50000:
					cmbPageSize.SelectedIndex = 13;
                    break;
                case 100000:
                    cmbPageSize.SelectedIndex = 14;
                    break;
                default:
                        break;
            }

        }

        public void SetPageSizeIndex(int i)
        {
            if (i < 0 || i > 10)
            {
                return;
            }
            cmbPageSize.SelectedIndex = i;
        }

     
        
        /// <summary>
        /// 将查询对象绑定到CtrlPage上
        /// </summary>
        /// <param name="_gridControl"></param>
        /// <param name="_queryObj"></param>
        public void Binding(IPagingQuery _queryObj)
        {
            //try
            //{
                queryObj = _queryObj;
        }

        public void UpdatePageUC()
        {
            _recordNum = queryObj.GetTotalRecordNum();
             //   cmbPageSize.SelectedIndex = 6; // 每页默认显示100条
                RefreshInterface();
            //}
            //catch(Exception e)
            //{
            //    throw new Exception("查询对象绑定失败:"+e.ToString());
            //}
        }

        /// <summary>
        /// 切换页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinCurrentPage_EditValueChanged(object sender, EventArgs e)
        {
            CurrentPage = int.Parse(spinCurrentPage.Value.ToString());
            Query();
        }

        /// <summary>
        /// 设置页大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageSize = pageSizeArr[cmbPageSize.SelectedIndex];
            if (int.Parse(spinCurrentPage.Value.ToString()) != 1)
            {
                spinCurrentPage.Value = 1;
            }
            else
            {
                Query();
            }
            
        }

        private void btnFirstPage_Click(object sender, EventArgs e)
        {
            if (int.Parse(spinCurrentPage.Value.ToString()) != 1)
            {
                spinCurrentPage.Value = 1;
            }
            else
            {
                Query();
            }
            CurrentPage = 1;
        }

        /// <summary>
        /// 刷新当前页
        /// </summary>
        public void RefreshCurrentPage()
        {
            Query();
        }

        public void FirstQuery()
        {
            CurrentPage = 1;
            PageSize = (_recordNum == 0) ? (int)cmbPageSize.EditValue : _recordNum;
            if (int.Parse(spinCurrentPage.Value.ToString()) != 1)
            {
                spinCurrentPage.Value = 1;
            }
            else
            {
                Query();
            }
           
        }

        private void btnPrePage_Click(object sender, EventArgs e)
        {
            spinCurrentPage.Value = --CurrentPage;
        }

        private void btnNextPage_Click(object sender, EventArgs e)
        {
            spinCurrentPage.Value = ++CurrentPage;
        }

        private void btnLastPage_Click(object sender, EventArgs e)
        {
            CurrentPage = TotalPage;
            if (int.Parse(spinCurrentPage.Value.ToString()) != TotalPage)
            {
                spinCurrentPage.Value = TotalPage;
            }
            else
            {
                Query();
            }
            
        }

        public void Query()
        {
            try
            {
                if(queryObj == null)
                {
                    return;
                }
                dt = queryObj.GetCurrentPageData((CurrentPage-1)*PageSize,PageSize);
				//int numcount = dt.DataSet.Tables[0].Rows.Count;   异常 @jianghua 2015.8.1
            }
            catch (System.Exception ex)
            {
                dt = null;
            }
			if (queryFinishedEventHandle != null)
			{
				queryFinishedEventHandle();
			}
            if (devicequeryevent != null)
            {
                devicequeryevent();
            }
            RefreshInterface();
        }



        void RefreshInterface()
        {
            PageSize = pageSizeArr[cmbPageSize.SelectedIndex];
            CurrentPage = int.Parse(spinCurrentPage.Value.ToString());

            if (PageSize != 0 && _recordNum % PageSize == 0 && _recordNum != 0)
                TotalPage = _recordNum / PageSize;
            else
                TotalPage = _recordNum / PageSize + 1;

            //lblPage.Text = CurrentPage.ToString();
            //lblTotalPage.Text = TotalPage.ToString();

            if (CurrentPage == 1)
                btnPrePage.Enabled = false;
            else
                btnPrePage.Enabled = true;
            if (CurrentPage == TotalPage)
                btnNextPage.Enabled = false;
            else
                btnNextPage.Enabled = true;
            lblPageInfo.Text = string.Format("第{0}页|共{1}页|共{2}条记录",CurrentPage,TotalPage,_recordNum);

            spinCurrentPage.Properties.MaxValue = TotalPage;
        }

        //public DataTable GetTable(int startposition, int pagesize)
        //{
        //    pagedDT = dt.Clone();
        //    for (int i = startposition; i < ((dt.Rows.Count - startposition) >= pagesize ? (startposition + pagesize) : dt.Rows.Count); i++)
        //    {
        //        pagedDT.ImportRow(dt.Rows[i]);
        //    }
        //    return pagedDT;
        //}
    }
}
