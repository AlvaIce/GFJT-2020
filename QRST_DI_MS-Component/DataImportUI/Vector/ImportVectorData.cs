using System;
using System.Collections.Generic;
using log4net;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_MS_Component_DataImportorUI.Vector
{ 
    public class ImportVectorData
    {
        ILog log = LogManager.GetLogger(typeof(ImportVectorData));

        List<SingleDataVector> singleDataLst = new List<SingleDataVector>();

        public bool UsingVirtualDir { get; set; }
        public ctrlVectorDataImport VectorImportCtrl;

        public ImportVectorData()
        {
            UsingVirtualDir = false;
            VectorImportCtrl = null; 
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

        public void DataImport(IDbBaseUtilities bsdbUtil)
        {
            foreach (var temp in singleDataLst)
            {
                try
                {
                    log.Info(string.Format("###########开始导入数据{0}###############", temp._filepath));
                    temp.DataImport(bsdbUtil);

                    if (UsingVirtualDir && VectorImportCtrl != null)
                    {
                        VectorImportCtrl.ctrlVirtualDirSetting1.AddFileLink(temp._metaData.QRST_CODE);
                    }
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
