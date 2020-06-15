namespace QRST_DI_MS_Basis.VirtualDir
{
    public class VirtualDirFileLinkClass : VirtualDirObject
    {
        
        //public string Name { get; set; }
        //public string Code { get; set; }
        public VirtualDirClass ParentDir { get; set; }
        private static VirtualDirEngine _vde;

         //文件夹其它属性
        public VirtualDirFileLinkClass(string name, string code)
        {
            if (_vde == null) _vde = new VirtualDirEngine();
            ParentDir = null;
            Name = name;
            Code = code;
            CreatTime = _vde.GetCreatTimeByFileLinkCode(code);
        }


        //文件夹其它属性
        public VirtualDirFileLinkClass(string code)
        {
            if (_vde == null) _vde = new VirtualDirEngine();
            ParentDir = null;
            Name = _vde.GetNameByFileLinkCode(code);
            Code = code;
            CreatTime = _vde.GetCreatTimeByFileLinkCode(code);
        }

    }
}
