using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
 
namespace QRST_DI_MS_Component.VirtualDirUI
{
    public partial class Preview : DevComponents.DotNetBar.Office2007Form
    {
        public Preview()
        {
            InitializeComponent();
        }
        public Preview(string path)
        {
            InitializeComponent();
            this.pictureBox1.Image=Image.FromFile(path); 
        }
    }
}
