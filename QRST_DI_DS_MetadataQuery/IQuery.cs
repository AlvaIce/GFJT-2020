using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetadataQuery;

namespace QRST_DI_DS_MetadataQuery
{ 
    public interface IQuery
    {
        QueryResponse Query();
        QueryResponse GetTableStruct();
    }
}
