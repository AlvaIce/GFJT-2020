using System;

namespace QRST.WorldGlobeTool.Renderable
{
    /// <summary>
    /// 可绘制内容接口
    /// </summary>
    interface IRenderable : IDisposable
    {
        void Initialize(DrawArgs drawArgs);
        void Update(DrawArgs drawArgs);
        void Render(DrawArgs drawArgs);
    }
}
