namespace QRST.WorldGlobeTool.Menu
{
    public class LatLonMenuButton : MenuButton
    {
        #region Private Members
        World _parentWorld;
        #endregion

        /// <summary>
        /// 初始化一个LatLonMenuButton实例
        /// </summary>
        /// <param name="buttonIconPath"></param>
        /// <param name="parentWorld"></param>
        public LatLonMenuButton(string buttonIconPath, World parentWorld)
            : base(buttonIconPath)
        {
            this._parentWorld = parentWorld;
            this.Description = "经纬网格线";
            this.SetPushed(World.Settings.showLatLonLines);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public override void Update(DrawArgs drawArgs)
        {
        }

        public override bool IsPushed()
        {
            return World.Settings.showLatLonLines;
        }

        public override void SetPushed(bool isPushed)
        {
            World.Settings.showLatLonLines = isPushed;
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
