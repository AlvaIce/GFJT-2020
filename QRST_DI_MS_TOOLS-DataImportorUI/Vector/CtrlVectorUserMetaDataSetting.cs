using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace QRST_DI_MS_TOOLS_DataImportorUI.Vector
{
    public partial class CtrlVectorUserMetaDataSetting : UserControl, IVectorUserMetaDataSetting
    {
        public CtrlVectorUserMetaDataSetting()
        {
            InitializeComponent();

            //vectorAttributeValues[0] = dt.Rows[index]["元数据名称"].ToString();
            //vectorAttributeValues[1] = dt.Rows[index]["产品名称"].ToString();
            //vectorAttributeValues[2] = dt.Rows[index]["产品生产日期"].ToString();
            //vectorAttributeValues[3] = dt.Rows[index]["产品生产单位"].ToString();
            //vectorAttributeValues[5] = dt.Rows[index]["数据来源"].ToString();
            //vectorAttributeValues[16] = dt.Rows[index]["密级"].ToString();
            //vectorAttributeValues[17] = dt.Rows[index]["数据质量"].ToString();
            //vectorAttributeValues[18] = dt.Rows[index]["地图比例尺"].ToString();
            //vectorAttributeValues[19] = dt.Rows[index]["元数据制作日期"].ToString();  上传日期时间
            //vectorAttributeValues[20] = dt.Rows[index]["元数据制作单位"].ToString();
            //vectorAttributeValues[21] = dt.Rows[index]["元数据制作人"].ToString();  上传人
            //vectorAttributeValues[22] = dt.Rows[index]["备注"].ToString();

            cmbDATAQULITY.SelectedIndex = 0;
            cmbSCALE.SelectedIndex = 0;
            cmbSECURITY.SelectedIndex = 0;


        }

        SingleDataVector _singleDV = null;
        /// <summary>
        /// 用以单个文件导入
        /// </summary>
        /// <param name="sdv"></param>
        public void Create(SingleDataVector sdv)
        {
            _singleDV = sdv;
        }
        /// <summary>
        /// 用以批量导入
        /// </summary>
        public void Create()
        {
            _singleDV = null;
            textMETANAME.Text = "";
            textMETANAME.Enabled = false;
        }

        /// <summary>
        /// 元数据制作者，即数据上传者
        /// </summary>
        public string MetaProductor
        {
            get
            {
                return ctrlVectorDataImport._currentUser.NAME;
            }
        }

        /// <summary>
        /// 元数据制作时间，即数据上传时间
        /// </summary>
        public string MetaProduceDate
        {
            get
            {
                return DateTime.Now.ToString();
            }
        }

        public string MetaName
        {
            get
            {
                if (_singleDV == null)
                {
                    return "";
                }
                return (textMETANAME.Text.Trim() == "") ? Path.GetFileNameWithoutExtension(_singleDV._filepath) : textMETANAME.Text;
            }
        }


        public string PRODUCEORG
        {
            get
            {
                return textPRODUCEORG.Text;
            }
        }

        public string PRODUCTNAME
        {
            get
            {
                return textPRODUCTNAME.Text;
            }
        }

        public string DATASOURCE
        {
            get
            {
                return textDATASOURCE.Text;
            }
        }

        public string Remark
        {
            get
            {
                return rtextRemark.Text;
            }
        }

        public string DATAQULITY
        {
            get
            {
                return cmbDATAQULITY.SelectedText;
            }
        }
        public string SCALE
        {
            get
            {
                return cmbSCALE.SelectedText;
            }
        }
        public string SECURITY
        {
            get
            {
                return cmbSECURITY.SelectedText;
            }
        }
        public string GroupCode
        {
            get
            {
                return "";
            }
        }



        //收集用户输入的元数据信息
        public void SetCustomizedMetaData()
        {
            if (_singleDV != null)
            {
                SetCustomizedMetaData(_singleDV);
            }
        }

        public void SetCustomizedMetaData(SingleDataVector sdv)
        {
            IVectorUserMetaDataSetting userMDSetting = this as IVectorUserMetaDataSetting;

            if (sdv._metaData != null)
            {
                this.Invoke(new EventHandler(delegate
                {
                    if (!string.IsNullOrEmpty(userMDSetting.DATAQULITY))
                    {
                        sdv._metaData.DataQulity = userMDSetting.DATAQULITY;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.DATASOURCE))
                    {
                        sdv._metaData.DataSource = userMDSetting.DATASOURCE;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.MetaName))
                    {
                        sdv._metaData.Name = userMDSetting.MetaName;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.MetaProduceDate))
                    {
                        sdv._metaData.MetaProduceDate = userMDSetting.MetaProduceDate;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.MetaProductor))
                    {
                        sdv._metaData.MetaProductor = userMDSetting.MetaProductor;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.PRODUCEORG))
                    {
                        sdv._metaData.Produceorg = userMDSetting.PRODUCEORG;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.PRODUCTNAME))
                    {
                        sdv._metaData.ProductName = userMDSetting.PRODUCTNAME;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.Remark))
                    {
                        sdv._metaData.Remark = userMDSetting.Remark;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.SCALE))
                    {
                        sdv._metaData.Scale = userMDSetting.SCALE;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.SECURITY))
                    {
                        sdv._metaData.Security = userMDSetting.SECURITY;
                    }
                    if (!string.IsNullOrEmpty(userMDSetting.GroupCode))
                    {
                        sdv._metaData.GroupCode = userMDSetting.GroupCode;
                    }
                }));
            }
        }


    }

    public interface IVectorUserMetaDataSetting
    {
        void SetCustomizedMetaData();
        void SetCustomizedMetaData(SingleDataVector sdv);
        void Create(SingleDataVector sdv);

        /// <summary>
        /// 元数据制作者，即数据上传者
        /// </summary>
        string MetaProductor
        {
            get;
        }

        /// <summary>
        /// 元数据制作时间，即数据上传时间
        /// </summary>
        string MetaProduceDate
        {
            get;
        }

        string MetaName
        {
            get;
        }

        string PRODUCEORG
        {
            get;
        }

        string PRODUCTNAME
        {
            get;
        }

        string DATASOURCE
        {
            get;
        }
        string Remark
        {
            get;
        }

        string DATAQULITY
        {
            get;
        }
        string SCALE
        {
            get;
        }
        string SECURITY
        {
            get;
        }
        string GroupCode
        { get; }
    }
}
