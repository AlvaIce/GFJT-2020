using System.Collections.Generic;

namespace QRST_DI_MS_Basis.VirtualDir
{ 
    public class VirtualDirClass : VirtualDirObject
    {
        //public string Name { get; set; }
        //public string Code { get; set; }
        public VirtualDirClass ParentDir { get; set; }
        public List<VirtualDirObject> ChildObjs { get; set; }
        private static VirtualDirEngine _vde;
        private bool isBuilded;
        //文件夹其它属性

        public VirtualDirClass(string name, string code)
        {
            ParentDir = null;
            Name = name;
            Code = code;
            
            ChildObjs = new List<VirtualDirObject>();
            if(_vde==null) _vde = new VirtualDirEngine();
            CreatTime = _vde.GetCreatTimeByVirtualDirCode(code);
            ModityTime = _vde.GetModifyTimeByVirtualDirCode(code);
            Remark = _vde.GetRemarkByVirtualDirCode(code);
            isBuilded = false;
        }

        public VirtualDirClass(string code)
        {
            ParentDir = null;
            Name = _vde.GetNameByVirtualDirCode(code);
            Code = code;
       
            ChildObjs = new List<VirtualDirObject>();
            if (_vde == null) _vde = new VirtualDirEngine();
            CreatTime = _vde.GetCreatTimeByVirtualDirCode(code);
            ModityTime = _vde.GetModifyTimeByVirtualDirCode(code);
            Remark = _vde.GetRemarkByVirtualDirCode(code);
            isBuilded = false;
          
        }

        public void BuildDir()
        {
            if (isBuilded)
            {
                return;
            }
            List<string> ChildLinklistdircode = _vde.Getalldir(Code);

            foreach (string childcode in ChildLinklistdircode)
            {
                if (!VirtualDirEngine.IsDir(childcode))
                {
                    VirtualDirFileLinkClass vdflc = new VirtualDirFileLinkClass(childcode);
                    vdflc.ParentDir = this;
                    ChildObjs.Add(vdflc);
                }
                else
                {
                    VirtualDirClass vdc = new VirtualDirClass(childcode);
                    vdc.ParentDir = this;
                    vdc.BuildDir();
                    ChildObjs.Add(vdc);
                }
            }

            isBuilded = true;
        }

    }
}
