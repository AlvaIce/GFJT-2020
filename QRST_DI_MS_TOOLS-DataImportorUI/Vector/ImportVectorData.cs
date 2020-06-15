using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using QRST_DI_DS_Metadata.Paths;
using log4net;
using QRST_DI_MS_TOOLS_DataImportorUI.Vector;
using QRST_DI_MS_TOOLS_DataImportorUI.Common;
using QRST_DI_DS_Basis.DBEngine;

namespace QRST_DI_MS_TOOLS_DataImportorUI.Vector
{
    public class ImportVectorData
    {
        ILog log = LogManager.GetLogger(typeof(ImportVectorData));

        List<SingleDataVector> singleDataLst = new List<SingleDataVector>();

        public ImportVectorData()
        {
        }

        public void Add(SingleDataVector _singleData)
        {
            singleDataLst.Add(_singleData);
        }

        public void AddRange(List<SingleDataVector> _singleDataLst)
        {
            singleDataLst.AddRange(_singleDataLst);
        }

        public void ClearAll()
        {
            singleDataLst.Clear();
        }

        public void DataImport(MySqlBaseUtilities bsdbUtil)
        {
            foreach (var temp in singleDataLst)
            {
                try
                {
                    log.Info(string.Format("###########开始导入数据{0}###############", temp._filepath));
                    temp.DataImport(bsdbUtil);
                    log.Info(string.Format("数据导入成功：{0}！", temp._filepath));
                }
                catch(Exception ex)
                {
                    log.Error(string.Format("数据导入失败:{0}！", temp._filepath), ex);
                }
            }
        }

        public List<SingleDataVector> GetImportDataLst()
        {
            return singleDataLst;
        }
        
    }

}
