namespace QRST_DI_DS_MetadataQuery.QueryConditionParameter
{
    public class DataTilePara
    {
        public string[] spacialPara = new string[]{"-90","-180","90","180"};
		public string[] ColAndRow = new string[4] {"","","","" };
        public string[] timePara = new string[2];
        public string[] satelliteType;
        public string[] sensorType;
        public string[] dataTileType;
        public string[] level;
		public string otherQuery; 
        public override bool Equals(object obj)
        {
            if (obj is DataTilePara && obj!=null)
            {
                DataTilePara newObj = (DataTilePara)obj;
                for (int i = 0; i < spacialPara.Length;i++ )
                {
                    if(spacialPara[i] != newObj.spacialPara[i])
                    {
                        return false;
                    }
                }
                for (int i = 0; i < timePara.Length; i++)
                {
                    if (timePara[i] != newObj.timePara[i])
                    {
                        return false;
                    }
                }
                if ((satelliteType == null && newObj.satelliteType != null) || (satelliteType != null && newObj.satelliteType == null))
                {
                    return false;
                }
                if (satelliteType != null && newObj.satelliteType!=null)
                {
                      for (int i = 0; i < satelliteType.Length; i++)
                   {
                    if (satelliteType[i] != newObj.satelliteType[i])
                    {
                        return false;
                    }
                    }
                }
                if ((sensorType == null && newObj.sensorType != null) || (sensorType != null && newObj.sensorType == null))
                {
                    return false;
                }
                if (sensorType != null && newObj.sensorType!=null)
                {
                      for (int i = 0; i < sensorType.Length; i++)
                   {
                    if (sensorType[i] != newObj.sensorType[i])
                    {
                        return false;
                    }
                   }
                }
                if ((dataTileType == null && newObj.dataTileType != null) || (dataTileType != null && newObj.dataTileType == null))
                {
                    return false;
                }
                if (dataTileType != null && newObj.dataTileType!=null)
                {
                     for (int i = 0; i < dataTileType.Length; i++)
                   {
                    if (dataTileType[i] != newObj.dataTileType[i])
                    {
                        return false;
                    }
                   }
                }
                if ((level == null && newObj.level != null) || (level != null && newObj.level == null))
                {
                    return false;
                }
                if(level!=null&&newObj.level!=null)
                {
                      for (int i = 0; i < level.Length; i++)
                {
                    if (level[i] != newObj.level[i])
                    {
                        return false;
                    }
                }
                }
              
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
