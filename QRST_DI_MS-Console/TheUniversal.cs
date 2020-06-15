using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QRST_DI_DS_Metadata.MetaDataDefiner;
using DevExpress.XtraNavBar;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_MS_Basis.Log;

namespace QRST_DI_MS_Console
{
    /// <summary>
    /// 定义运管系统的全局变量
    /// </summary>
     public static class TheUniversal
    {    
         public static List<SiteDb> subDbLst;   //运管系统控制的数据子库列表
         public static DevExpress.XtraNavBar.NavBarControl nbcMain;   //系统导航栏
         public static userInfo currentUser;            //当前用户

         public static SysLog sysLog;    //log

         public static SiteDb MIDB
         {
             get { return GetsubDbByEngName("MIDB"); }
         }
         public static SiteDb BSDB
         {
             get { return GetsubDbByEngName("BSDB"); }
         }
         public static SiteDb MADB
         {
             get { return GetsubDbByEngName("MADB"); }
         }
         public static SiteDb RCDB
         {
             get { return GetsubDbByEngName("RCDB"); }
         }
         public static SiteDb EVDB
         {
             get { return GetsubDbByEngName("EVDB"); }
         }
         public static SiteDb IPDB
         {
             get { return GetsubDbByEngName("IPDB"); }
         }
         public static SiteDb ISDB
         {
             get { return GetsubDbByEngName("ISDB"); }
         }


         /// <summary>
         /// 根据数据库英语名称获取子库对象
         /// </summary>
         /// <param name="name"></param>
         public static SiteDb GetsubDbByEngName(string name)
         {
             foreach (SiteDb siteDb in subDbLst)
             {
                 if (siteDb.NAME == name)
                 {
                     return siteDb;
                 }
             }
             return null;
         }

         /// <summary>
         /// 根据数据库汉语名称获取子库对象
         /// </summary>
         /// <param name="name"></param>
         public static  SiteDb GetsubDbByName(string  name)
         {
             foreach (SiteDb siteDb in subDbLst)
             {
                 if (siteDb.DESCRIPTION == name)
                 {
                     return siteDb;
                 }
             }
             return null;
         }

         /// <summary>
         /// 根据数据库名英文代号称获取子库对象
         /// </summary>
         /// <param name="name"></param>
         public static SiteDb GetsubDbByCODE(string name)
         {
             foreach (SiteDb siteDb in subDbLst)
             {
                 if (siteDb.NAME == name)
                 {
                     return siteDb;
                 }
             }
             return null;
         }

         /// <summary>
         /// 通过导航栏中的子系统列表获取运管系统所有的子系统
         /// </summary>
         /// <returns></returns>
         public static Dictionary<string, string> GetAllSystemList()
         {
             Dictionary<string, string> dic = new Dictionary<string, string>();
             for (int i = 0 ; i < nbcMain.Groups.Count ;i++ )
             {
                 dic.Add(nbcMain.Groups[i].Caption,nbcMain.Groups[i].Name);
             }
             return dic;
         }

         /// <summary>
         /// 根据子系统名称获取子系统的所有模块
         /// </summary>
         /// <returns></returns>
         public static Dictionary<string, string> GetAllMoudleBySysName(string nbgname)
         {
             Dictionary<string, string> dic = new Dictionary<string, string>();
             //取出对应的nbg
             NavBarGroup nbg = null;
             for (int i = 0 ; i < nbcMain.Groups.Count ; i++)
             {
                 if (nbcMain.Groups[i].Name == nbgname)
                 {
                     nbg = nbcMain.Groups[i];
                     break;
                 }
             }
             if (nbg != null)
             {
                 for (int j = 0 ; j < nbg.ItemLinks.Count ;j++ )
                 {
                     NavBarItemLink itemLink = nbg.ItemLinks[j];
                     dic.Add(itemLink.ItemName,itemLink.Caption);
                 }
             }
             
             return dic;
         }
    }
}
