using QRST_DI_SS_Basis.TileSearch;
using System.Collections.Generic;
 
namespace QRST_DI_TS_Basis.Search
{
    public class SearchCondition
    {
        public SearchCondition()
        {

        }
        /// <summary>
        /// 检索correctedTiles 纠正数据（切片）
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public string QueryCollectionForSqlite(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype)
        {
            string tablename = "correctedTiles";
            string QueryStr = "";
            if (position.Count != 0)
            {

                string minRow = position[0];
                string minColum = position[1];
                string maxRow = position[2];
                string maxColum = position[3];


                QueryStr += string.Format("( Row>={1} and Row<={3})and (Col>={2} and Col<={4})", tablename, minRow, minColum, maxRow, maxColum);

                QueryStr += " and";
            }
            if (datetime.Count != 0)
            {
                QueryStr += "(";
                int dt1 = datetime[0]*100;
                int dt2 = datetime[1]*100 +24;
                QueryStr += string.Format("Date between {0} and {1}", dt1, dt2);

                QueryStr += ") and";
            }
            if (satellite.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in satellite)
                {
                    QueryStr += string.Format(" Satellite = '{0}' or ", s);


                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            if (sensor.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in sensor)
                {
                    QueryStr += string.Format(" Sensor = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";

            }

            if (datatype.Count != 0)
            {
                QueryStr += " (";
                foreach (string type in datatype)
                {
                    QueryStr += string.Format(" type = '{0}' or ", type);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;


        }
        /// <summary>
        /// 检索correctedTiles HJ纠正数据,实现了同时查询多个层级,DLF 0703
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public string QueryCollectionForSqlite2(List<TileLevelPosition> levelPositions, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype)
        {
            //string tablename = "correctedTiles";
            string QueryStr = "";

            if (levelPositions.Count != 0)
            {
                QueryStr += "(";
                foreach (TileLevelPosition levelposition in levelPositions)
                {

                    if (levelposition.tileRowandColumn.Length >= 4)
                    {

                        int minRow = levelposition.tileRowandColumn[0];
                        int minColum = levelposition.tileRowandColumn[1];
                        int maxRow = levelposition.tileRowandColumn[2];
                        int maxColum = levelposition.tileRowandColumn[3];

                        QueryStr += string.Format("( Level='{0}' and Row>={1} and Row<={3} and Col>={2} and Col<={4} ) or ", levelposition.TileLevel, minRow, minColum, maxRow, maxColum);
                    }
                }

                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }

            if (datetime.Count != 0)
            {
                QueryStr += "(";
                int dt1 = datetime[0]*100;
                int dt2 = datetime[1]*100 + 24;
                QueryStr += string.Format("Date between {0} and {1}", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }
            if (satellite.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in satellite)
                {
                    QueryStr += string.Format(" Satellite = '{0}' or ", s);

                    //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            if (sensor.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in sensor)
                {
                    QueryStr += string.Format(" Sensor = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";

            }

            if (datatype.Count != 0)
            {
                QueryStr += " (";
                foreach (string type in datatype)
                {
                    QueryStr += string.Format(" type = '{0}' or ", type);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            //if (tileLevel.Count!=0)
            //{
            //    QueryStr += " (";
            //    foreach (int level in tileLevel)
            //    {
            //        QueryStr += string.Format(" Level = '{0}' or ", level);
            //    }
            //    QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
            //}
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;


        }
        
        /// <summary>
        /// 检索productTiles 产品（切片）
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public string QueryCollectionForSqlite_Prod(List<string> position, List<int> datetime, List<string> datatype)
        {
            string tablename = "productTiles";
            string QueryStr = "";
            if (position.Count != 0)
            {

                string minRow = position[0];
                string minColum = position[1];
                string maxRow = position[2];
                string maxColum = position[3];


                QueryStr += string.Format("( Row>={1} and Row<={3})and (Col>={2} and Col<={4})", tablename, minRow, minColum, maxRow, maxColum);

                QueryStr += " and";
            }
            if (datetime.Count != 0)
            {
                QueryStr += "(";
                int dt1 = datetime[0]*100;
                int dt2 = datetime[1]*100 +24;
                QueryStr += string.Format("Date between {0} and {1}", dt1, dt2);

                QueryStr += ") and";
            }

            if (datatype.Count != 0)
            {
                QueryStr += " (";
                foreach (string type in datatype)
                {
                    QueryStr += string.Format(" ProdType = '{0}' or ", type);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;


        }

        /// <summary>
        /// 检索productTiles HJ产品（切片）,实现了同时查询多个层级,DLF 0703
        /// </summary>
        /// <param name="position"></param>
        /// <param name="datetime"></param>
        /// <param name="satellite"></param>
        /// <param name="sensor"></param>
        /// <param name="datatype"></param>
        /// <returns></returns>
        public string QueryCollectionForSqlite2_Prod(List<TileLevelPosition> levelPositions, List<int> datetime, List<string> datatype)
        {
            //string tablename = "productTiles";
            string QueryStr = "";
            if (levelPositions.Count != 0)
            {
                QueryStr += "(";
                foreach (TileLevelPosition levelposition in levelPositions)
                {

                    if (levelposition.tileRowandColumn.Length >= 4)
                    {

                        int minRow = levelposition.tileRowandColumn[0];
                        int minColum = levelposition.tileRowandColumn[1];
                        int maxRow = levelposition.tileRowandColumn[2];
                        int maxColum = levelposition.tileRowandColumn[3];

                        QueryStr += string.Format("( Level='{0}' and Row>={1} and Row<={3} and Col>={2} and Col<={4} ) or ", levelposition.TileLevel, minRow, minColum, maxRow, maxColum);
                    }
                }

                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            if (datetime.Count != 0)
            {
                QueryStr += "(";
                int dt1 = datetime[0] *100;
                int dt2 = datetime[1]*100 +24;
                QueryStr += string.Format("Date between {0} and {1}", dt1, dt2);
                //QueryStr += searchHJ.getTime(Convert.ToString(dt1), Convert.ToString(dt2), "SCENEDATE");
                QueryStr += ") and";
            }

            if (datatype.Count != 0)
            {
                QueryStr += " (";
                foreach (string type in datatype)
                {
                    QueryStr += string.Format(" ProdType = '{0}' or ", type);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="orderCode">对应SQLite中DataSourceID</param>
        /// <returns></returns>
        public string QueryCollectionForSqlite_Raster(List<string> position, List<string> orderCode)
        {
            string tablename = "productTiles";
            string QueryStr = "";
            if (position.Count != 0)
            {

                string minRow = position[0];
                string minColum = position[1];
                string maxRow = position[2];
                string maxColum = position[3];

                QueryStr += string.Format("( Row>={1} and Row<={3})and (Col>={2} and Col<={4})", tablename, minRow, minColum, maxRow, maxColum);

                QueryStr += " and";
            }
            if (orderCode.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in orderCode)
                {
                    QueryStr += string.Format("DataSourceID = '{0}' or", s);
                    //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";

            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;

        }


        public string QueryCollectionForOracle(List<string> position, List<int> datetime, List<string> satellite, List<string> sensor, List<string> datatype)
        {
            string tablename = "tester6";
            string QueryStr = "";
            if (position.Count != 0)
            {

                string minRow = position[0];
                string minColum = position[1];
                string maxRow = position[2];
                string maxColum = position[3];

                QueryStr += string.Format("( Row>={1} and Row<={3})and (Col>={2} and Col<={4})", tablename, minRow, minColum, maxRow, maxColum);

                QueryStr += " and";
            }
            if (datetime.Count != 0)
            {
                QueryStr += "(";
                int dt1 = datetime[0]*100;
                int dt2 = datetime[1]*100 + 24;
                QueryStr += string.Format("proddate between {0} and {1}", dt1, dt2);

                QueryStr += ") and";
            }
            if (satellite.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in satellite)
                {
                    QueryStr += string.Format(" satellite = '{0}' or ", s);

                    //addQueryStr = addQueryStr.TrimEnd(" or".ToCharArray()) + ")";
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";
            }
            if (sensor.Count != 0)
            {
                QueryStr += " (";
                foreach (string s in sensor)
                {
                    QueryStr += string.Format(" sensor = '{0}' or ", s);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
                QueryStr += " and ";

            }

            if (datatype.Count != 0)
            {
                QueryStr += " (";
                foreach (string type in datatype)
                {
                    QueryStr += string.Format(" type = '{0}' or ", type);
                }
                QueryStr = QueryStr.TrimEnd(" or ".ToCharArray()) + ")";
            }
            QueryStr = QueryStr.TrimEnd(" and ".ToCharArray());
            return QueryStr;


        }
    }
}
