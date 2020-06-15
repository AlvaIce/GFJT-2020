using System.IO;

namespace QRST_DI_DataImportTool.DataImport.MetaData
{
    public abstract class MetaData
    {
        //IMetaDataProvider metaDataProvider;
        
        /// <summary>
        /// 导入数据
        /// </summary>
        /// 
        public string QRST_CODE;


        public virtual void ReadAttributes(string fileName) 
        {
            if (!File.Exists(fileName))
                return;
        }   //读取属性数据
        public virtual void ImportData() { }//存储属性数据
    }
}
