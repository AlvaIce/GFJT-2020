using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QRST_DI_MS_Console.UserInterfaces
{
    public partial class rucTaskSubmitter : RibbonPageBaseUC
    {
        public rucTaskSubmitter()
        {
            InitializeComponent();
        }
        //  public (object objMUC)
        //    : base(objMUC)
        //{
        //    InitializeComponent();
        //}
        public rucTaskSubmitter(object objMUC)
            : base(objMUC)
        {
            InitializeComponent();
        }


    }
}
