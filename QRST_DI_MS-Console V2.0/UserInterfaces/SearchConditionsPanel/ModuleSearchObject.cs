using System.Collections.Generic;
using System.Drawing;
using System.Data;
 
namespace QRST_DI_MS_Desktop.UserInterfaces.SearchConditionsPanel
{
    public class ModuleSearchObject
    {
        public Image 图标 { get; set; }
        //public string IconName{get;set;}
        public string 模块名称 { get; set; }
        public string 模块说明 { get; set; }
        public string 版本 { get; set; }
        public string 系统最低版本要求 { get; set; }
        public string 类名 { get; set; }
        public string 所属动态链接库 { get; set; }
        public List<SysModuleSearchObj> 系统用例 { get; set; }
        public int 模块使用次数 { get { return 系统用例.Count; } }

        //public Image Icon{get;set;}
        ////public string IconName{get;set;}
        //public string Name{get;set;}
        //public string Desciption{get;set;}
        //public string Version {get;set;}
        //public string RequiredVersion{get;set;}
        //public string ClassName{get;set;}
        //public string DllName{get;set;}
        //public List<SysModuleSearchObj> SysUtils{get;set;}

        public ModuleSearchObject()
        {
            //for testing
            //SysUtils = new List<SysModuleSearchObj>();
            //Icon = global::QRST_DI_MS_Desktop.Properties.Resources.三维可视化检索;
            //Name = "三维可视化检索";
            //Desciption = "三维可视化检索";
            //Version = "1.0.0.0";
            //RequiredVersion = "1.0.0.0";
            //ClassName = "LegendControl";
            //DllName = "QRST_GIAS_Control.dll";

            //SysUtils.Add(new SysModuleSearchObj());
            //SysUtils.Add(new SysModuleSearchObj());
            //SysUtils.Add(new SysModuleSearchObj());
            //SysUtils.Add(new SysModuleSearchObj());

            图标 = global::QRST_DI_MS_Desktop.Properties.Resources.三维可视化检索;
            模块名称 = "二维可视化检索";
            模块说明 = "三维可视化检索";
            版本 = "1.0.0.0";
            系统最低版本要求 = "1.0.0.0";
            类名 = "LegendControl";
            所属动态链接库 = "QRST_GIAS_Control.dll";

            系统用例 = new List<SysModuleSearchObj>();
            系统用例.Add(new SysModuleSearchObj());
            系统用例.Add(new SysModuleSearchObj());
            系统用例.Add(new SysModuleSearchObj());
            系统用例.Add(new SysModuleSearchObj());
        }


        public ModuleSearchObject(List<DataRow> drs)
        {
            系统用例 = new List<SysModuleSearchObj>();
            //DataRow[] drsdata = null;
            //drs.CopyTo(drsdata);
            if (drs==null||drs.Count==0)
            {
                return;
            }

            图标 = RPGroupModule.FromBlobObject(drs[0]["ICONDATA"] as byte[]);
            模块名称 = drs[0][RPGroupModule.ModuleColumnChar["模块名称"]].ToString();
            模块说明 = drs[0][RPGroupModule.ModuleColumnChar["模块说明"]].ToString();
            版本 = drs[0][RPGroupModule.ModuleColumnChar["版本"]].ToString();
            系统最低版本要求 = drs[0][RPGroupModule.ModuleColumnChar["系统最低版本要求"]].ToString();
            类名 = drs[0][RPGroupModule.ModuleColumnChar["类名"]].ToString();
            所属动态链接库 = drs[0][RPGroupModule.ModuleColumnChar["所属动态链接库"]].ToString();

            系统用例 = new List<SysModuleSearchObj>();
            foreach (DataRow dr in drs)
            {
                系统用例.Add(new SysModuleSearchObj(dr));
            }
        }
    }
}
