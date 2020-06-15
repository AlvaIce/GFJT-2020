using System;

namespace DotSpatial.Mono
{
    public class Mono
    {
        public static bool IsRunningOnMono()
        {
            Type t = Type.GetType("Mono.Runtime");
            return (t != null);
        }
    }
}
