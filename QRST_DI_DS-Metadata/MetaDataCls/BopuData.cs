using System.Runtime.Serialization;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_DS_Metadata.MetaDataCls
{
    public class MetaDataRcBopu : MetaData
    {
        public string _typeName = "";
        public string _dataName = "";
        public MetaDataRcBopu(string typename)
        {
            _typeName = typename;
            this._dataType = EnumMetadataTypes.RcBopuData;
        }

        public override void GetModel(string qrst_code,IDbBaseUtilities sqlBase)
        {
            _dataName = qrst_code;
            base.GetModel(qrst_code, sqlBase);
        }

        public override string GetRelateDataPath()
        {
            string relatePath = "";
            switch (_typeName)
            {
                case "土壤":
                    relatePath = string.Format("波普特征库\\soil\\{0}.zip", _dataName);
                    break;
                case "南方植被":
                    relatePath = string.Format("波普特征库\\vsouth\\{0}.zip", _dataName);
                    break;
                case "北方植被":
                    relatePath = string.Format("波普特征库\\vnorth\\{0}.zip", _dataName);
                    break;
                case "城市目标":
                    relatePath = string.Format("波普特征库\\city\\{0}.zip", _dataName);
                    break;
                case "地表大气":
                    relatePath = string.Format("波普特征库\\atmosphere\\{0}.zip", _dataName);
                    break;
                case "水体":
                    relatePath = string.Format("波普特征库\\water\\{0}.zip", _dataName);
                    break;
                case "岩矿":
                    relatePath = string.Format("波普特征库\\rock\\{0}.zip", _dataName);
                    break;
                default:
                    break;
            }
            return relatePath;
        }
    }

     [DataContract]
    public class BopuData
    {
         public BopuData()
         {
         }

         [DataMember]
         public Bopu[] types { get; set; }
    }

       [DataContract]
     public class Bopu
     {
         public Bopu()
         {
         }

         [DataMember]
         public  string type { get; set; }
     }

}
