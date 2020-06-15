using System;
using System.Collections.Generic;
using System.Data;
using QRST_DI_SS_Basis;

namespace QRST_DI_TS_Process.Site
{
     
    public  class TServerSiteMoniter
    {
        public static DataTable monitorInfoDataSource;

        public static List<TServerSite> runningSite = new List<TServerSite>();

        /// <summary>
        /// 初始化一个信息监控数据源
        /// </summary>
        /// <returns></returns>
        public static DataTable InitializeMonitorDataTable()
        {
            DataTable dt = new DataTable();
            DataColumn dc1 = new DataColumn() { ColumnName = "IPAddress", DataType = Type.GetType("System.String") };
            dt.Columns.Add(dc1);
            DataColumn dc2 = new DataColumn() { ColumnName = "SiteName", DataType = Type.GetType("System.String") };
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn() { ColumnName = "CPUVersion", DataType = Type.GetType("System.String") };
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn() { ColumnName = "StorageSpace", DataType = Type.GetType("System.Double") };
            dt.Columns.Add(dc4);
            DataColumn dc5 = new DataColumn() { ColumnName = "MemorySize", DataType = Type.GetType("System.Double") };
            dt.Columns.Add(dc5);
            DataColumn dc6 = new DataColumn() { ColumnName = "RunningState", DataType = Type.GetType("System.String") };
            dt.Columns.Add(dc6);

            return dt;
        }

        /// <summary>
        /// 根据TServerSiteManager中的StorageSites获取各个站点的基本信息。
        /// </summary>
        /// <returns></returns>
        public static DataTable GetBasicSiteInfo()
        {
            TServerSiteManager.UpdateStorageSiteList(); 
            monitorInfoDataSource = InitializeMonitorDataTable();
            TServerSiteManager.UpdateOptimalStorageSiteList();
            for (int i = 0 ; i < TServerSiteManager.StorageSites.Count ;i++ )
            {
                monitorInfoDataSource.Rows.Add(GetSiteInfo(TServerSiteManager.StorageSites[i]));
            }
            return monitorInfoDataSource;
        }

        public static DataRow GetSiteInfo(TServerSite site)
        {
            DataRow dr = monitorInfoDataSource.NewRow();
            dr["IPAddress"] = site.IPAdress;
            dr["SiteName"] = site.SiteName;
            TServerSite tss = TServerSiteManager.getSiteFromSiteIP(site.IPAdress);

            if (TServerSiteManager.optimalStorageSites.Contains(tss))
            {
                try
                {
                    bool isrunning = site.TCPService.IsRunning;
                    dr["CPUVersion"] = site.Status.CPUVersion;
                    dr["StorageSpace"] = site.Status.GetDivToatalsize();
                    dr["MemorySize"] = site.Status.TotalPhysicalMemory;
                    dr["RunningState"] = "正在运行";
                    runningSite.Add(site);
                }
                catch (Exception ex)
                {
                    dr["RunningState"] = "运行出错";
                }
            }
            else if (site.RunningState  == EnumSiteStatus.Stopped)
            {
                dr["CPUVersion"] = "";
                dr["StorageSpace"] = 0;
                dr["MemorySize"] = 0;
                dr["RunningState"] = "运行停止";
            }
            return dr;
        }

    }

    
}
