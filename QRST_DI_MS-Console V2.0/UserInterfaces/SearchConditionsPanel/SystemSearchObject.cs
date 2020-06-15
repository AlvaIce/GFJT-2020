using System.Collections.Generic;
using System.Drawing;
using System.Data;
 
namespace QRST_DI_MS_Desktop.UserInterfaces.SearchConditionsPanel
{
    public class SystemSearchObject
    {
        private string iconName;
        //public string IconName{get;set;}
        public Image 图示 { get; set; }
        public string 中文名 { get; set; }
        public string 英文名 { get; set; }
        public string 版本 { get; set; }
        public string 说明 { get; set; }
        public List<object> 构件 { get; set; }

        public SystemSearchObject()
        {
            
        }

        public SystemSearchObject(List<DataRow> drs)
        {
            if (drs == null || drs.Count==0)
            {
                return;
            }
            DataRow dr = drs[0];

            图示 = RPGroupModule.FromBlobObject(dr["IMAGEDATA"] as byte[]);
            中文名 = dr[RPGroupModule.SysColumnChar["中文名"]].ToString();
            英文名 = dr[RPGroupModule.SysColumnChar["英文名"]].ToString();
            版本 = dr[RPGroupModule.SysColumnChar["版本"]].ToString();
            说明 = dr[RPGroupModule.SysColumnChar["说明"]].ToString();
            构件 = new List<object>();
        }
    }

}
