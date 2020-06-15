using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using WorldWind.BaseTool;
using Qrst;

namespace DrawTools.Plugins
{
    public abstract class DrawBaseTool: BaseWwTool
    {
        public Polygon Polygon;
    }
}
