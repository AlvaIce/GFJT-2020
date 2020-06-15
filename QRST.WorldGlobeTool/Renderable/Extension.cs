namespace QRST.WorldGlobeTool.Renderable
{
    public class Extension
    {
        public Extension()
        {
            North = -90;
            South = 90;
            West = 180;
            East = -180;
        }
        public Extension(double n, double s, double w, double e)
        {
            North = n;
            South = s;
            West = w;
            East = e;
        }
        public double North { get; set; }
        public double South { get; set; }
        public double West { get; set; }
        public double East { get; set; }

        public void Include(Extension ext)
        {
            North = (North > ext.North) ? North : ext.North;
            South = (South < ext.South) ? South : ext.South;
            West = (West < ext.West) ? West : ext.West;
            East = (East > ext.East) ? East : ext.East;
        }
    }
}
