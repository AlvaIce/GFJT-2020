using System;
using System.Collections.Generic;
using System.Windows.Forms;
using QRST_DI_MS_Basis.UserRole;
using QRST_DI_MS_Desktop.UserInterfaces;
using QRST_DI_Resources;

namespace QRST_DI_MS_Desktop
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.TFS
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RunApplication();
        }

        private static void RunApplication()
        {
            try
            {
                if (!Constant.Created)
                {
                    Constant.InitializeTcpConnection();
                    Constant.Create();
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show("通信进程启动异常，请联系管理员！\r\n"+ex.Message);
                return;
            }
             
            userInfo.sqlUtilities = Constant.IdbServerUtilities;
            List<userInfo> userLst = userInfo.GetList(string.Format("NAME = '超级用户'"));
            TheUniversal.currentUser = userLst[0];

            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                using (FrmNewLogin frm = new FrmNewLogin())
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        frm.Close();
                        frm.Dispose();  // 不再需要该窗体，释放资源

                        //Application.Run(new frmMSConsole_New());
                        Application.Run(new frm_MSConsole2());
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message + e.TargetSite.DeclaringType + " " + e.Source + " " + e.StackTrace+" "+e.Data);
            }
        }
    }
}
