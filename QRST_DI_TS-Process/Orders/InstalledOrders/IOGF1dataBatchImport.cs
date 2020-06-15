using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using System.Windows.Forms;
using System.Data;
using QRST_DI_Resources;
using QRST_DI_SS_Basis;
using QRST_DI_SS_Basis.MetaData;
using QRST_DI_SS_DBClient.DBEngine;
using QRST_DI_SS_DBInterfaces.IDBEngine;

namespace QRST_DI_TS_Process.Orders.InstalledOrders
{
    class IOGF1dataBatchImport
	{
		public List<OrderClass> CreateGF1BatchImport(string FilePath)
		{
			List<OrderClass> orderList = new List<OrderClass>();
			Thread th = new Thread(importThread);
			th.Start(FilePath);
			return orderList;
		}
		private void importThread(object obj)
		{			
			string filePath = (string)obj;
            //if (!TSPCommonReference.isCreated)
            //{
            //	TSPCommonReference.Create();
            //}
            if (!Constant.ServiceIsConnected)
            {
                Constant.InitializeTcpConnection();
            }
            if (!Directory.Exists(filePath))
			{
				MessageBox.Show("路径不合法");
				return;
			}

			FileInfo[] FiltedFileNames1 = searNewData(filePath);
			////遍历tar.gz文件列表，每个文件进行入库操作
			for (int i = 0; i < FiltedFileNames1.Length; i++)
			{

				//FileInfo fi = new FileInfo(FiltedFileNames1[i]);
				try
				{
					importTarGz(FiltedFileNames1[i]);
				}
				catch
				{
					continue;
				}
			}
			List<string> errorImport = new List<string>();
			
		}
		private void importTarGz(FileInfo fifi)
		{
			//FileInfo fifi = new FileInfo(fileName);
			string orderCode = "";			
			QRST_DI_TS_Process.Orders.OrderClass orderClass1 = null;				
			orderClass1 = new IOGF1DataImport().Create();						
			OrderManager.SubmitOrder(orderClass1);
			orderCode = orderClass1.OrderCode;
			string ws = "%OrderWorkspace%";

			while (ws == "%OrderWorkspace%")
			{
				////循环监控订单是否被执行
				OrderClass orderClass2 = OrderManager.GetOrderByCode(orderCode);
				ws = orderClass2.OrderWorkspace;
				if (ws == "%OrderWorkspace%")
				{
					System.Threading.Thread.Sleep(1000);
				}
			}

			//Copy .tar.gz 到workspace下
			string sourceFilePath = fifi.FullName;
			string destFilePath = Path.Combine(ws, fifi.Name);

			int num = 1;
			while (num < 4)
			{
				try
				{
					File.Copy(sourceFilePath, destFilePath);
					break;
				}
				catch (Exception ex)
				{
					num++;
					if (num == 4)
					{
						//Addmessege1(String.Format("数据提交失败: {0}{1}", fifi.Name, Environment.NewLine));
						return;
						//throw ex;
					}
				}
			}
			//File.Copy(sourceFilePath, destFilePath);
			//继续处理点单消息
			//submitWS.ResumeGF1DataImportOrder(code);

			////循环监控订单是否被执行
			num = 1;
			while (num < 4)
			{
				OrderClass orderClass3 = OrderManager.GetOrderByCode(orderCode);
				if (orderClass3 != null && orderClass3.Status == EnumOrderStatusType.Suspended)
				{
					OrderManager.UpdateOrderStatus(orderClass3.OrderCode, EnumOrderStatusType.Waiting);
					break;
				}
				Thread.Sleep(1000);
				num++;
			}			
		}
		public FileInfo[] searNewData(string DirPath)
		{

			//string DirPath = this.textBox1.Text;
			FileInfo[] fileInfos;
			DirectoryInfo di_data = new DirectoryInfo(DirPath);
			fileInfos = di_data.GetFiles("*.tar.gz", SearchOption.AllDirectories);
			//遍历path 获取tar.gz文件
			Dictionary<string, FileInfo> dicNameFi = new Dictionary<string, FileInfo>();

			foreach (FileInfo fi in fileInfos)
			{
				if (dicNameFi.Keys.Contains(fi.Name))
				{
					dicNameFi.Remove(fi.Name);
				}
				dicNameFi.Add(fi.Name, fi);
			}

			string[] filenames = dicNameFi.Keys.ToArray();

			int blockSize = 1000;

			List<string> resultFilted = new List<string>();

			int blockNum = filenames.Length / blockSize;
			for (int i = 0; i < blockNum + 1; i++)
			{
				int beginIndex = i * blockSize;
				int len = blockSize;
				if (i == blockNum)
				{
					len = filenames.Length % blockSize;
				}
				if (len > 0)
				{
					string[] blockArr = new string[len];
					for (int j = 0; j < len; j++)
					{
						blockArr[j] = filenames[beginIndex + j];
					}
					List<string> blockList = new List<string>();
					blockList = blockArr.ToList();
					string[] returnNotInDB = DataCertificate_Multi(blockList).ToArray();
					resultFilted.AddRange(returnNotInDB);
				}
			}
			List<FileInfo> files=new List<FileInfo> ();
			foreach (var item in dicNameFi)
			{
				if(resultFilted.Contains(item.Key))
					files.Add(item.Value);
			}
			return files.ToArray();
		}
		public List<string> DataCertificate_Multi(List<string> dataNames)
		{
			List<string> result = new List<string>();
			if (dataNames == null || dataNames.Count == 0)
			{
				return result;
			}
			result.AddRange(dataNames.ToArray());
			string tableName = string.Empty;
			tableName = "prod_gf1";
				
			string SQLString = string.Format("select name from {0} where name in (", tableName);
			for (int i = 0; i < dataNames.Count; i++)
			{
				SQLString = string.Format("{0}'{1}',", SQLString, dataNames[i]);
			}
			SQLString = SQLString.TrimEnd().TrimEnd(',');
			SQLString += ")";
			//DBMySqlOperating mySQLOperator;
		    IDbOperating sqLiteOperating = Constant.IdbOperating;
			//MySqlBaseUtilities MySqlBaseUti;
		    IDbBaseUtilities sqLiteBaseUtilities = sqLiteOperating.GetSubDbUtilities(EnumDBType.EVDB);

            DataSet returnDS = null;
			//根据数据库名称获取库访问实例
			//mySQLOperator = new DBMySqlOperating();
			//MySqlBaseUti = mySQLOperator.EVDB;

			returnDS = sqLiteBaseUtilities.GetDataSet(SQLString);

			if (returnDS != null && returnDS.Tables.Count > 0)
			{
				foreach (DataRow dr in returnDS.Tables[0].Rows)
				{
					result.Remove(dr["name"].ToString());
				}
			}
			return result;
		}
	}
}
