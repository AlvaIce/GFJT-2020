using OSGeo.GDAL;

namespace SharpMap
{
    public static class EnvironmentalGdal
    {

        private static bool makeEnvironment = false;

        /// <summary>
        /// 注册环境变量,用于调用gdal库
        /// </summary>
        public static void MakeEnvironment(string sPathProgram)
        {
            if (makeEnvironment) return;
            setEnvironment(sPathProgram);
            //配置gdal缓存，以MB为单位，不超过2GB
            //Gdal.SetConfigOption("GDAL_CACHEMAX", "1024");
            //禁用PAMDATASET，避免产生aux.xml文件
            //Gdal.SetConfigOption("GDAL_PAM_ENABLED", "NO");
            Gdal.AllRegister();
            OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "YES");
            makeEnvironment = true;

        }

        /// <summary>
        /// 注册系统变量 for GDAL DLL
        /// 若不存在下面的系统变量则创建:
        /// - GDAL_DATA        -> %Program Path% \data
        /// - GEOTIFF_CSV      -> %Program Path% \data       
        /// 
        ///  Add PATH
        ///  - PATH            -> PATH + %Program Path% + %Program Path%\gdal-dll
        /// </summary>
        private static void setEnvironment(string sPathProgram)
        {
            // Check exist variables, else, set variable
            setValueNewVariable("GDAL_DATA", sPathProgram + "\\data");
            setValueNewVariable("GEOTIFF_CSV", sPathProgram + "\\data");
            setValueNewVariable("GDAL_DRIVER_PATH", sPathProgram + "\\gdalplugins");

            // Add variable Path new folders      
            string sVarPath = System.Environment.GetEnvironmentVariable("Path");
            sVarPath += ";" + sPathProgram + ";" + sPathProgram + "\\gdalDepend";
            System.Environment.SetEnvironmentVariable("Path", sVarPath);
        }

        private static void setValueNewVariable(string sVar, string sValue)
        {
            if (System.Environment.GetEnvironmentVariable(sVar) == null)
                System.Environment.SetEnvironmentVariable(sVar, sValue);
        }
    }
}
