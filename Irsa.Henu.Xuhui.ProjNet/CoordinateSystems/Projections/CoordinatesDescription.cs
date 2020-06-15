namespace ProjNet.CoordinateSystems.Projections
{
    public class CoordinatesDescription
    {
        private static string _GCS = "GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.2572326660126,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0],UNIT[\"degree\",0.0174532925199433],AUTHORITY[\"EPSG\",\"4326\"]]";
        public static string GCS
        {
            get
            {
                return _GCS;
            }
        }
    }
}
