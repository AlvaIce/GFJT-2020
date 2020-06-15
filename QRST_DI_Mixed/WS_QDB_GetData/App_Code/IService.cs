interface IService
{
    int DeleteData(string datacode);
    string[] GetAddressFields(string dataCode);
    string GetCorrectedDataAddress(string DataName);
    string GetData(string DataType, string QRST_CODE);
    string GetRalativeAddByDataCode(string dataCode);
    string GetSourceDataPath(string DataCode);
    global::System.Collections.Generic.List<string> GetTilesList(global::System.Collections.Generic.List<string> tileNames);
    string HelloWorld();
    string PushTileZipToDirByName(string tileName, string originalOrderCode);
    string test(string s);
}
