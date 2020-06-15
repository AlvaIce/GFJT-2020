using System.Drawing;
using System.Data;

namespace QRST_DI_MS_Desktop.UserInterfaces.SearchConditionsPanel
{ 
    public class SystemInfoSearchObject
    {
        public Image 图标 { get; set; }
        private string iconName;
        //public string IconName{get;set;}
        public string 模块名称 { get; set; }
        public string 模块说明 { get; set; }
        public string 版本 { get; set; }
        public string 系统最低版本要求 { get; set; }
        public string 类名 { get; set; }
        public string 所属动态链接库 { get; set; }
        public string 所属容器名 { get; set; }
        public string 容器类型 { get; set; }
        public string 所属子系统 { get; set; }
        public string 所属系统 { get; set; }

        public SystemInfoSearchObject()
        {
            图标 = global::QRST_DI_MS_Desktop.Properties.Resources.三维可视化检索;
            模块名称 = "二维可视化检索";
            模块说明 = "三维可视化检索";
            版本 = "1.0.0.0";
            系统最低版本要求 = "1.0.0.0";
            类名 = "LegendControl";
            所属动态链接库 = "QRST_GIAS_Control.dll";
        }

        public SystemInfoSearchObject(DataRow dr)
        {
            if (dr == null)
            {
                return;
            }

            图标 = RPGroupModule.FromBlobObject(dr["ICONDATA"] as byte[]);
            模块名称 = dr[RPGroupModule.ModuleColumnChar["模块名称"]].ToString();
            模块说明 = dr[RPGroupModule.ModuleColumnChar["模块说明"]].ToString();
            版本 = dr[RPGroupModule.ModuleColumnChar["版本"]].ToString();
            系统最低版本要求 = dr[RPGroupModule.ModuleColumnChar["系统最低版本要求"]].ToString();
            类名 = dr[RPGroupModule.ModuleColumnChar["类名"]].ToString();
            所属动态链接库 = dr[RPGroupModule.ModuleColumnChar["所属动态链接库"]].ToString();
            所属容器名 = dr[RPGroupModule.ModuleColumnChar["所属容器名"]].ToString();
            容器类型 = dr[RPGroupModule.ModuleColumnChar["容器类型"]].ToString();
            所属子系统 = dr[RPGroupModule.ModuleColumnChar["所属子系统"]].ToString();
            所属系统 = dr[RPGroupModule.ModuleColumnChar["所属系统"]].ToString();
        }
    }

    //public class SystemSearchObject
    //{
    //    public string 系统名 { get; set; }
    //    public List<SubsystemSearchObject> 子系统 { get; set; }

    //    public SystemSearchObject()
    //    {
    //        系统名 = "神华专题系统";
    //        子系统 = new List<SubsystemSearchObject>();
    //        子系统.Add(new SubsystemSearchObject());
    //        子系统.Add(new SubsystemSearchObject());
    //        子系统.Add(new SubsystemSearchObject());
    //        子系统.Add(new SubsystemSearchObject());
    //        子系统.Add(new SubsystemSearchObject());
    //    }
    //}

    //public class SubsystemSearchObject
    //{
    //    public string 子系统名 { get; set; }
    //    public List<ContainerSearchObject> 容器 { get; set; }

    //    public SubsystemSearchObject()
    //    {
    //        子系统名 = "产品生产分系统";
    //        容器 = new List<ContainerSearchObject>();
    //        容器.Add(new ContainerSearchObject());
    //        容器.Add(new ContainerSearchObject());
    //        容器.Add(new ContainerSearchObject());
    //        容器.Add(new ContainerSearchObject());
    //        容器.Add(new ContainerSearchObject());
    //        容器.Add(new ContainerSearchObject());
    //    }
    //}

    //public class ContainerSearchObject
    //{
    //    public string 容器名 { get; set; }
    //    public string 容器类型 { get; set; }
    //    public List<ModuleselfSearchObject> 模块 { get; set; }

    //    public ContainerSearchObject()
    //    {
    //        容器名 = "地图浏览";
    //        容器类型 = "工具条";
    //        模块 = new List<ModuleselfSearchObject>();
    //        模块.Add(new ModuleselfSearchObject());
    //        模块.Add(new ModuleselfSearchObject());
    //        模块.Add(new ModuleselfSearchObject());
    //        模块.Add(new ModuleselfSearchObject());
    //        模块.Add(new ModuleselfSearchObject());
    //    }
    //}


    //public class ModuleselfSearchObject
    //{
    //    public Image 图标 { get; set; }
    //    //public string IconName{get;set;}
    //    public string 名称 { get; set; }
    //    public string 说明 { get; set; }
    //    public string 版本 { get; set; }
    //    public string 最低版本 { get; set; }
    //    public string 类名 { get; set; }
    //    public string 所属动态链接库 { get; set; }

    //    public ModuleselfSearchObject()
    //    {
    //        图标 = global::QRST_DI_MS_Desktop.Properties.Resources.三维可视化检索;
    //        名称 = "二维可视化检索";
    //        说明 = "三维可视化检索";
    //        版本 = "1.0.0.0";
    //        最低版本 = "1.0.0.0";
    //        类名 = "LegendControl";
    //        所属动态链接库 = "QRST_GIAS_Control.dll";
    //    }
    //}
}
