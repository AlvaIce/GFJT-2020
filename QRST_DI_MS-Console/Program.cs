using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using QRST_DI_MS_Console.UserInterfaces;
using QRST_DI_Resources;

namespace QRST_DI_MS_Console
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
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
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.UserSkins.OfficeSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!Constant.Created)
                Constant.Create();

            using (FrmNewLogin frm = new FrmNewLogin())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    frm.Close();
                    frm.Dispose();  // 不再需要该窗体，释放资源

                    //Application.Run(new frmMSConsole_New());
                    Application.Run(new frm_MSConsole());
                }
            }
        }
    }
}
