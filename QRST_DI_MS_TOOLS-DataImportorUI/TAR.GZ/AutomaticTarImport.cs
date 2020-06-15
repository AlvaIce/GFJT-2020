using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using QRST_DI_TS_Process.Orders;
 
namespace QRST_DI_MS_Component_DataImportorUI.TAR.GZ
{
    class AutomaticTarImport
    {
        private String monitorPath = null;
        private ConcurrentQueue<String> importDataQueue = new ConcurrentQueue<string>();
        private SourceDataDelete dataDelete = new SourceDataDelete();
        private List<String> needDeleteList = new List<string>();
        private FileInfo[] files = null;
        private UCImportTarGZ form;
        FileSystemWatcher fsWatcher = null;

        public AutomaticTarImport(string path,UCImportTarGZ form)
        {
            this.monitorPath = path;
            this.form = form;
            files = dataDelete.getAllFiles(this.monitorPath);
        }

        public void startMonitor()
        {
            TraversePath(monitorPath);
            watcher(monitorPath, "*.tar.gz");
            form.ImportMessage.Clear();
            Thread t = new Thread(monitorImportQueue);
            t.Start();
        }

        public void TraversePath(string path)
        {
            if(files != null && files.Length > 0)
            {
                needDeleteList = dataDelete.TraverseSourceDataInDB(files);
                foreach (FileInfo file in files)
                {
                    if (!needDeleteList.Contains(file.Name))
                    {
                        importDataQueue.Enqueue(file.FullName);
                    }
                    else
                    {
                        file.Delete();
                    }
                }
            }
        }

        void monitorImportQueue()
        {
            while(true)
            {
                if(!importDataQueue.IsEmpty)
                {
                    String dataFilePath = null;
                    importDataQueue.TryDequeue(out dataFilePath);
                    importData(dataFilePath);
                }
            }
        }

        public void importData(String filePath)
        {
            form.importTarGz(filePath);
            List<string> NoProcessing = new List<string>();
            foreach (var item in form.ImportMessage)
            {
                OrderClass orderClass3 = OrderManager.GetOrderByCode(item.Key);
                if (orderClass3.Status == EnumOrderStatusType.Error)
                { 
                    NoProcessing.Add(item.Key);
                }
                else if (orderClass3.Status == EnumOrderStatusType.Completed)
                { 
                    FileInfo file = new FileInfo(item.Value);
                    file.Delete();
                    NoProcessing.Add(item.Key);
                }
                //    else if (orderClass3.Status == EnumOrderStatusType.Processing)
                //    {
                //        Thread.Sleep(10000);
                //        num++;
                //        isProcessed = false;
                //    }
            }
            foreach (string noProcesing in NoProcessing)
            {
                form.ImportMessage.Remove(noProcesing);
            }
        }

        /// <summary>
        /// 文件夹的监视器 @zhangfeilong
        /// </summary>
        /// <param name="path">监视路径</param>
        /// <param name="filter">过滤器</param>
        public void watcher(string path, string filter)
        {
            fsWatcher = new FileSystemWatcher();
            fsWatcher.Path = path;
            //watcher.Filter = filter;
            fsWatcher.Created += new FileSystemEventHandler(OnProcess);
            fsWatcher.EnableRaisingEvents = true;
            fsWatcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName | NotifyFilters.LastAccess
                                   | NotifyFilters.LastWrite | NotifyFilters.Security | NotifyFilters.Size;
            fsWatcher.IncludeSubdirectories = true;
        }

        private void OnProcess(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                OnCreated(source, e);
            }
        }

        private void OnCreated(object source, FileSystemEventArgs e)
        {
            if (File.Exists(e.FullPath))
            {
                FileInfo file = new FileInfo(e.FullPath);
                while (true)
                {
                    if (!isFileInUse(e.FullPath) && e.FullPath.EndsWith(".tar.gz"))
                    {
                        if (!form.newFileInDB(file.Name))
                        {
                            form.Addmessege1("文件：" + e.FullPath + "已经存在数据库中！" + Environment.NewLine);
                        }
                        else
                        {
                            importDataQueue.Enqueue(file.FullName);
                        }
                        break;
                    }
                }
            }
            else if (Directory.Exists(e.FullPath))
            {

            }
            else
            {
                Console.WriteLine("****");
            }
        }

        /// <summary>
        /// 判断文件是否正在使用 @zhangfeilong
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool isFileInUse(string fileName)
        {
            bool inUse = true;
            FileStream fs = null;
            try
            {

                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                FileShare.None);
                inUse = false;
            }
            catch
            {

            }
            finally
            {
                if (fs != null)

                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }

        public void stopMonitor()
        {
            if (fsWatcher != null)
            {
                fsWatcher.EnableRaisingEvents = false;
            }
            form.Addmessege1("停止监控此目录......\n");
        }
    }

    

    
}
