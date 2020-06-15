using System.Data;
 
namespace QRST_DI_MS_Desktop.UserInterfaces.SearchConditionsPanel
{
    public class SysModuleSearchObj
    {
        public string 所属容器名 { get; set; }
        public string 容器类型 { get; set; }
        public string 所属子系统 { get; set; }
        public string 所属系统 { get; set; }

        public SysModuleSearchObj()
        {
            所属容器名 = "地图漫游";
            容器类型 = "工具条";
            所属子系统 = "矸石堆人工分类子系统";
            所属系统 = "神东矿区卫星遥感业务化运行系统";
        }
        public SysModuleSearchObj(DataRow dr)
        {
            所属容器名 = dr[RPGroupModule.ModuleColumnChar["所属容器名"]].ToString();
            容器类型 = dr[RPGroupModule.ModuleColumnChar["容器类型"]].ToString();
            所属子系统 = dr[RPGroupModule.ModuleColumnChar["所属子系统"]].ToString();
            所属系统 = dr[RPGroupModule.ModuleColumnChar["所属系统"]].ToString();
        }
        //public string Container { get; set; }
        //public string ContainerType { get; set; }
        //public string Subsystem { get; set; }
        //public string System { get; set; }

        //public SysModuleSearchObj()
        //{
        //    Container = "地图漫游";
        //    ContainerType = "工具条";
        //    Subsystem = "矸石堆人工分类子系统";
        //    System = "神东矿区卫星遥感业务化运行系统";
        //}
    }
}
