using System;
using System.Data;
using QRST_DI_SS_Basis.MetadataQuery;
using QRST_DI_DS_MetadataQuery.PagingQuery;
 
namespace QRST_DI_MS_Desktop.UserInterfaces
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

        private string[] pageSizeArr = new string[] {
            "100",
            "1000",
            "5000",
            "10000",
            "25000",
            "50000",
            "100000",
            "250000",
            "500000",
            "1000000",
            "全部记录"};

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
                case -1:
                    cmbPageSize.SelectedIndex = 10;
                    break;
                case 100:
                    cmbPageSize.SelectedIndex = 0;
                    break;
                case 1000:
                    cmbPageSize.SelectedIndex = 1;
                    break;
                case 5000:
                    cmbPageSize.SelectedIndex = 2;
                    break;
                case 10000:
                    cmbPageSize.SelectedIndex = 3;
                    break;
                case 25000:
                    cmbPageSize.SelectedIndex = 4;
                    break;
                case 50000:
                    cmbPageSize.SelectedIndex = 5;
                    break;
                case 100000:
                    cmbPageSize.SelectedIndex = 6;
                    break;
                case 250000:
					cmbPageSize.SelectedIndex = 7;
					break;
				case 500000:
                    cmbPageSize.SelectedIndex = 8;
                    break;
				case 1000000:
					cmbPageSize.SelectedIndex = 9;
					break;
                default:
                        break;
            }

        }

        //public void SetPageSizeIndex(int i)
        //{
        //    if (i < 0 || i > 10)
        //    {
        //        return;
        //    }
        //    cmbPageSize.SelectedIndex = i;
        //}

     
        
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
            if (_recordNum >= PageSize||(_recordNum<PageSize&&CurrentPage!=1))
            {
                _recordNum = queryObj.GetTotalRecordNum();
            }
             //   cmbPageSize.SelectedIndex = 6; // 每页默认显示100条
                RefreshInterface();
            //}
            //catch(Exception e)
            //{
            //    throw new Exception("查询对象绑定失败:"+e.ToString());
            //}
        }

        private void spinPage(int currentPage)
        {
            foreach (var item in MetaDataPagingQuery.dic)
            {
                if (item.Key == currentPage)
                {
                    dt = item.Value;
                    if (queryFinishedEventHandle != null)
                    {
                        queryFinishedEventHandle();
                    }
                    if (devicequeryevent != null)
                    {
                        devicequeryevent();
                    }
                    RefreshInterface();
                    break;
                }
            }
        }
        /// <summary>
        /// 切换页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void spinCurrentPage_EditValueChanged(object sender, EventArgs e)
        {
            CurrentPage = int.Parse(spinCurrentPage.Value.ToString());
            if (CurrentPage == 1)
            {
                MetaDataPagingQuery.record = 0;
            }
            if (ComplexCondition._usingGeometry)
            {
                if (isPrePage)
                {
                    spinPage(CurrentPage);
                }
                else if (isLastP)
                {
                    if (ComplexCondition._usingGeometry)
                    {
                        if (CurrentPage > MetaDataPagingQuery.dic.Count)
                        {
                            while (true)
                            {
                                Query(MetaDataPagingQuery.record);
                                if (dt == null || dt.Rows.Count < PageSize)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            spinPage(CurrentPage);
                        }
                    }
                }
                else if (isNextP)
                {
                    if (CurrentPage <= MetaDataPagingQuery.dic.Count)
                    {
                        spinPage(CurrentPage);
                    }
                    else
                    {
                        Query(MetaDataPagingQuery.record);
                    }
                }
                else
                {
                    if (CurrentPage <= MetaDataPagingQuery.dic.Count)
                    {
                        spinPage(CurrentPage);
                    }
                    else
                    {
                        while (MetaDataPagingQuery.dic.Count<CurrentPage)
                        {
                            Query(MetaDataPagingQuery.record);
                            if (dt == null || dt.Rows.Count < PageSize)
                            {
                                break;
                            }
                        }
                    }

                }               
            }
            else
            {
                Query();
            }
        }

        int GetPageSize()
        {
            int size;
            if (pageSizeArr[cmbPageSize.SelectedIndex].ToString() == "全部记录")
            {
                return -1;
            }
            else if (int.TryParse(pageSizeArr[cmbPageSize.SelectedIndex], out size))
            {
                if (!isRefresh)
                {
                    MetaDataPagingQuery.dic.Clear();
                    MetaDataPagingQuery.record = 0;
                }
                return size;
            }
            else
            {
                return 1000;
            }
        }

        /// <summary>
        /// 设置页大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            PageSize = GetPageSize();
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
            else if(CurrentPage!=1)
            {
                spinPage(1);
                //Query();
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
            MetaDataPagingQuery.dic.Clear();
            CurrentPage = 1;
            PageSize = GetPageSize();
            if (int.Parse(spinCurrentPage.Value.ToString()) != 1)
            {
                spinCurrentPage.Value = 1;
            }
            else
            {
                Query();
            }
           
        }
        bool isPrePage = false;
        private void btnPrePage_Click(object sender, EventArgs e)
        {
            isPrePage = true;
            spinCurrentPage.Value = --CurrentPage;
            isPrePage = false;
        }
        bool isNextP = false;
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            isNextP = true;
            spinCurrentPage.Value = ++CurrentPage;
            isNextP = false;
        }
        bool isLastP = false;
        private void btnLastPage_Click(object sender, EventArgs e)
        {
            CurrentPage = TotalPage;
            isLastP = true;
            if (int.Parse(spinCurrentPage.Value.ToString()) != TotalPage)
            {
                spinCurrentPage.Value = TotalPage;
            }
            else
            {
                //Query();
            }
            isLastP = false;
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
            _recordNum = queryObj.GetTotalRecordNum();
            if (dt!=null&&dt.Rows.Count < PageSize&&CurrentPage==1)
            {
                _recordNum = dt.Rows.Count;
            }
            RefreshInterface();
        }

        public void Query(int record)
        {
            try
            {
                if (queryObj == null)
                {
                    return;
                }
                dt = queryObj.GetCurrentPageData(record, PageSize);
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
            if (dt!=null&& dt.Rows.Count < PageSize)
            {
                _recordNum = (CurrentPage-1)*PageSize+dt.Rows.Count;
            }
            RefreshInterface();
        }
        bool isRefresh = false;
         void RefreshInterface()
        {
            isRefresh = true;
            PageSize = GetPageSize();
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
            isRefresh = false;
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

        /// <summary>
        /// 20170317 
        /// xmh
        /// 根据数据总数，获取当前的页面等信息
        /// </summary>
        /// <param name="_recordNum"></param>
        public void RefreshPage(int _recordNum )
        {
            isRefresh = true;
            PageSize = GetPageSize();
            CurrentPage = int.Parse(spinCurrentPage.Value.ToString());

            if (PageSize != 0 && _recordNum % PageSize == 0 && _recordNum != 0)
                TotalPage = _recordNum / PageSize;
            else
                TotalPage = _recordNum / PageSize + 1;

            if (CurrentPage == 1)
                btnPrePage.Enabled = false;
            else
                btnPrePage.Enabled = true;
            if (CurrentPage == TotalPage)
                btnNextPage.Enabled = false;
            else
                btnNextPage.Enabled = true;
            lblPageInfo.Text = string.Format("第{0}页|共{1}页|共{2}条记录", CurrentPage, TotalPage, _recordNum);

            spinCurrentPage.Properties.MaxValue = TotalPage;
            isRefresh = false;
        }
    }
}
