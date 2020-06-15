namespace QRST_DI_MS_Basis.QueryBase
{
    /// <summary>
    /// 描述数据类型信息的对象，数据类型是在各个库中可操作的数据对象。
    /// </summary>
    public class DataTypeClass
    { 
        private string hostDBCode;
        /// <summary>
        /// 数据类型所属的数据库的编码；各个数据库的Host 是Root；Root的此项是空。
        /// </summary>
        public string HostDBCode
        {
            get
            {
                return hostDBCode != null ? categoryName : string.Empty;
            }
            set
            {
                hostDBCode = value;
            }
        }

        private string categoryName;
        /// <summary>
        /// 数据类型名称，用于显示等
        /// </summary>
        public string CatagoryName
        {
            get
            {
                return categoryName != null ? categoryName : string.Empty;
            }
            set
            {
                categoryName = value;
            }
        }

        private string catagoryCode;
        /// <summary>
        /// 数据类型的编码，用于数据库中操作
        /// </summary>
        public string CatagoryCode
        {
            get
            {
                return catagoryCode != null ? catagoryCode : string.Empty;
            }
            set
            {
                catagoryCode = value;
            }
        }

        private FileTypeCategory dataFileType;
        /// <summary>
        /// 数据类型所属的文件类型。包括栅格，矢量，文档，表格四种。
        /// </summary>
        public FileTypeCategory DataFileType
        {
            get
            {
                //return dataFileType != null ? dataFileType : FileTypeCategory.NotDefine;
                return dataFileType;
            }
            set
            {
                dataFileType = value;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataTypeClass()
        {

        }
    }
    public enum FileTypeCategory
    {
        /// <summary>
        /// 栅格数据
        /// </summary>
        Rasters,
        /// <summary>
        /// 切片数据
        /// </summary>
        Tiles,
        /// <summary>
        /// 矢量数据
        /// </summary>
        Vectors,
        /// <summary>
        /// 文档数据
        /// </summary>
        Documents,
        /// <summary>
        /// 表格数据
        /// </summary>
        Sheets,
        /// <summary>
        /// 未定义文件类型
        /// </summary>
        NotDefine
    }
}
