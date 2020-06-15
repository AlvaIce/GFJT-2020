using System;

namespace QRST.WorldGlobeTool.Utility
{
    /// <summary>
    /// 屏幕上的消息
    /// </summary>
    public sealed class OnScreenMessage
    {
        private String message;
        private double x;
        private double y;

        /// <summary>
        /// 
        /// </summary>
        public OnScreenMessage() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="message"></param>
        public OnScreenMessage(double x, double y, String message)
        {
            this.x = x;
            this.y = y;
            this.message = message;
        }

        public String Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        public double X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public double Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

    }
}
