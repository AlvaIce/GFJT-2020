namespace QRST.WorldGlobeTool.Menu
{
    /// <summary>
    /// 位置信息菜单按钮
    /// </summary>
    public class PositionMenuButton : MenuButton
    {
        /// <summary>
        /// 初始化一个PositionMenuButton实例
        /// </summary>
        /// <param name="buttonIconPath"></param>
        public PositionMenuButton(string buttonIconPath)
            : base(buttonIconPath)
        {
            this.Description = "位置信息";
        }

        public override void Dispose()
        {
            base.Dispose();
        }
        public override bool IsPushed()
        {
            return World.Settings.showPosition;
        }
        public override void SetPushed(bool isPushed)
        {
            World.Settings.showPosition = isPushed;
            World.Settings.showCrosshairs = isPushed;
        }

        public override bool OnMouseWheel(System.Windows.Forms.MouseEventArgs e)
        {
            return false;
        }

        public override void OnKeyDown(System.Windows.Forms.KeyEventArgs keyEvent)
        {

        }
        public override void OnKeyUp(System.Windows.Forms.KeyEventArgs keyEvent)
        {

        }
        public override void Update(DrawArgs drawArgs)
        {

        }

        public override bool OnMouseDown(System.Windows.Forms.MouseEventArgs e)
        {
            return false;
        }
        public override bool OnMouseMove(System.Windows.Forms.MouseEventArgs e)
        {
            return false;
        }
        public override bool OnMouseUp(System.Windows.Forms.MouseEventArgs e)
        {
            return false;
        }
        public override void Render(DrawArgs drawArgs)
        {

        }
    }
}
