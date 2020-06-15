using System.Web.Services;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]

public class DispatcherService : System.Web.Services.WebService
{
    public DispatcherService()
    {
        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 

    }

    //[WebMethod]
    //public string HelloWorld() {
    //    return "Hello World";
    //}

    [WebMethod(Description = "创建数据工作空间")]
    public string CreateWorkSpace()
    {
       
        return "";
    }
    
}