using System;
using System.Collections.Generic;
using QRST_DI_Resources;
using QRST_DI_DS_Metadata.Paths;
using System.IO;
using QRST_DI_TS_Process.JCGXCallBack;
using QRST_DI_TS_Basis.DirectlyAddress;
using ICSharpCode.SharpZipLib.Checksums;
using ICSharpCode.SharpZipLib.Zip;
 
namespace QRST_DI_TS_Process.Tasks.InstalledTasks
{
    public class ITDownLoadDataP2P : TaskClass
    {
        /// <summary>
        /// 任务名,定义唯一标识，不可动态修改
        /// </summary>
        public override string TaskName
        {
            get { return "ITDownLoadDataP2P"; }
            set { }
        }

        public override void Process()
        {
            string dataName = this.ProcessArgu[0];             //要下载数据的数据名称
            string destDir = this.ProcessArgu[1];     //目的地址
            string opID = this.ProcessArgu[2];             //操作号ID
            string webserviceIp = this.ProcessArgu[3];
            string destPath = destDir.TrimEnd('\\');
            try
            {
                //if (dataName.Contains(".gff"))
                if (Path.GetExtension(dataName).ToLower() == ".gff")
                {
                    //判断数据是否已经存在
                    destPath = string.Format(@"{0}\{1}", destPath, dataName);
                    if (File.Exists(destPath))
                    {
                        this.ParentOrder.Logs.Add("目标位置已经存在数据！");
                    }
                    else
                    {
                        DirectlyAddressing da = new DirectlyAddressing(DirectlyAddressingIPMod.IPModDataSet);
                        string tile = Path.GetFileNameWithoutExtension(dataName);
                        TileNameArgs tileArgs = da.GetTileNameArgs(tile + ".png");
                        
                        List<string> tilefiles = new List<string>();
                        switch (tileArgs.Type)
                        {
                            case TileNameArgs.TileType.ProdTile:
                                ProdTileNameArgs ptileArgs = tileArgs as ProdTileNameArgs;
                                if (ptileArgs.IsOldNameStyle)
                                {
                                    //LST_20140220_L1A0000169144_8_474_1176.tif 
                                    tilefiles.AddRange(new string[] { da.GetPathByFileName(tile + ".tif") });
                                }
                                else
                                {
                                    //LST_2014022024_L1A0000169144_6400_8_474_1176.p.tif 
                                    tilefiles.AddRange(new string[] { da.GetPathByFileName(tile + ".tif") });
                                }
                                break;
                            case TileNameArgs.TileType.CorrectedTile:

                                CorrectedTileNameArgs ctileArgs = tileArgs as CorrectedTileNameArgs;

                                if (ctileArgs.IsOldNameStyle)
                                {
                                    //旧式命名法
                                    if (tile.ToUpper().Contains("_MOC"))
                                    {
                                        //GF1_MOC2_20150110_L1A0000580463_8_518_1179-Alpha.tif
                                        tilefiles.AddRange(new string[]{
                                            da.GetPathByFileName(tile + ".png"),
                                            da.GetPathByFileName(tile + ".pgw"),
                                            da.GetPathByFileName(string.Format("{0}-1.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-2.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-3.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-Alpha.tif", tile))});
                                    }
                                    else if (tile.ToUpper().Contains("_GRC_"))
                                    {
                                        //GF1_WFV3_2016082724_L1A0001788435_0C00_7_1300_2957.c
                                        //CP3_GRC_20150110_L1A0000580463_8_518_1179-Alpha.tif old
                                        tilefiles.AddRange(new string[]{
                                            da.GetPathByFileName(tile + ".png"),
                                            da.GetPathByFileName(tile + ".pgw"),
                                            da.GetPathByFileName(string.Format("{0}-1.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-2.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-3.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-4.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-Alpha.tif", tile))});
                                    }
                                    else
                                    {

                                        tilefiles.AddRange(new string[]{
                                            da.GetPathByFileName(tile + ".png"),
                                            da.GetPathByFileName(tile + ".pgw"),
                                            da.GetPathByFileName(string.Format("{0}-1.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-2.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-3.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-4.tif", tile))
                                            //,da.GetPathByFileName(string.Format("{0}-Azimuth.tif", tile))
                                            //,da.GetPathByFileName(string.Format("{0}-Zenith.tif", tile))
                                            });
                                    }
                                }
                                else
                                {
                                    //新式命名法
                                    //GF1_WFV3_2016082724_L1A0001788435_0C00_7_1300_2957.c
                                    tile = Path.GetFileNameWithoutExtension(tile);
                                    if (tile.ToUpper().Contains("_MOC"))
                                    {
                                        //GF1_MOC2_2015011024_L1A0000580463_0C00_8_518_1179-Alpha.c.tif
                                        tilefiles.AddRange(new string[]{
                                            da.GetPathByFileName(tile + ".c.png"),
                                            da.GetPathByFileName(tile + ".c.pgw"),
                                            da.GetPathByFileName(string.Format("{0}-1.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-2.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-3.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-Alpha.c.tif", tile))});
                                    }
                                    else if (tile.ToUpper().Contains("_GRC_"))
                                    {
                                        //GF1_WFV3_2016082724_L1A0001788435_0C00_7_1300_2957.c
                                        //CP3_GRC_20150110_L1A0000580463_8_518_1179-Alpha.tif old
                                        tilefiles.AddRange(new string[]{
                                            da.GetPathByFileName(tile + ".c.png"),
                                            da.GetPathByFileName(tile + ".c.pgw"),
                                            da.GetPathByFileName(string.Format("{0}-1.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-2.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-3.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-4.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-Alpha.c.tif", tile))});
                                    }
                                    else
                                    {

                                        tilefiles.AddRange(new string[]{
                                            da.GetPathByFileName(tile + ".c.png"),
                                            da.GetPathByFileName(tile + ".c.pgw"),
                                            da.GetPathByFileName(string.Format("{0}-1.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-2.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-3.c.tif", tile)),
                                            da.GetPathByFileName(string.Format("{0}-4.c.tif", tile))
                                            //,da.GetPathByFileName(string.Format("{0}-Azimuth.c.tif", tile))
                                            //,da.GetPathByFileName(string.Format("{0}-Zenith.c.tif", tile))
                                            });
                                    }
                                }

                                break;
                            default:
                                throw new Exception("未支持该类型数据！");
                        }

                        //检查源数据完整性
                        foreach (string tilepath in tilefiles)
                        {
                            if (!File.Exists(tilepath))
                            {
                                throw new Exception(string.Format("瓦片数据不完整，缺少{0}！", tilepath));
                            }
                        }

                        //开始数据推送
                        this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                        this.ParentOrder.Logs.Add("开始打包并推送数据！");

                        string strZipPath = Path.Combine(destDir, tile + ".gff_temp");//zip_temp
                        ZipOutputStream s = new ZipOutputStream(File.Create(strZipPath));

                        //P2P 下载瓦片时一定要比对大小，可能数据存在内容不一致；

                        foreach (string tilepath in tilefiles)
                        {
                            zip(tilepath, s, strZipPath);
                        }
                        s.Close();
                        this.ParentOrder.Logs.Add("完成数据打包！");

                        FileInfo gfftmpfile = new FileInfo(strZipPath);
                        gfftmpfile.MoveTo(destPath);
                        this.ParentOrder.Logs.Add("完成数据推送！");
                    }
                    //SendMessageByWebService(opID, destPath);
                }
                else
                {
                    //GF原始数据
                    destPath = string.Format(@"{0}\{1}", destPath, dataName);
                    if (File.Exists(destPath))
                    {
                        this.ParentOrder.Logs.Add("目标位置已经存在数据！");
                    }
                    else
                    {
                        string sourcePath = StoragePath.GetGFDataExistPathByName(dataName, false);

                        if (sourcePath != "-1")      //数据目录存在
                        {
                            this.ParentOrder.Logs.Add("已经获取到源数据路径！");
                            this.ParentOrder.Logs.Add("开始数据推送！");

                            #region 局域网内部拷贝到共享文件夹（FTP文件夹路径）
                            string destdir = Path.GetDirectoryName(destPath);
                            if (!Directory.Exists(destdir))
                            {
                                Directory.CreateDirectory(destdir);
                            }
                            try
                            {
                                File.Copy(sourcePath, destPath);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception("数据推送失败：" + ex.Message);
                            }
                            this.ParentOrder.Logs.Add("完成数据推送！");
                            #endregion
                        }
                        else
                        {
                            throw new Exception("未找到数据！");
                        }
                    }
                }
                SendMessageByWebService(opID, destPath, webserviceIp);
            }
            catch (Exception ex)
            {
                this.ParentOrder.Logs.Add(ex.Message);
                SendMessageByWebService(opID, "false:" + ex.Message, webserviceIp);
            }

        }

        private void zip(string strFile, ZipOutputStream s, string staticFile)
        {
            if (File.Exists(strFile))
            {
                Crc32 crc = new Crc32();
                //string[] filenames = Directory.GetFileSystemEntries(strFile);
                FileStream fs = File.OpenRead(strFile);

                byte[] buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
                string tempfile = strFile.Substring(strFile.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(tempfile);

                entry.DateTime = DateTime.Now;
                entry.Size = fs.Length;
                fs.Close();
                crc.Reset();
                crc.Update(buffer);
                entry.Crc = crc.Value;
                s.PutNextEntry(entry);
                s.Write(buffer, 0, buffer.Length);
            }
        }

        public void SendMessageByWebService(string opID, string msg, string webserviceip)
        {
            if (webserviceip == "")
            {
                webserviceip = Constant.JcgxEndPointAddress;
            }

            try
            {
                string address = String.Format("http://{0}/JCGXService/services/WS_DB_Callback.WS_DB_CallbackHttpSoap12Endpoint/", Constant.JcgxEndPointAddress);
                using (WS_DB_CallbackPortTypeClient client = new WS_DB_CallbackPortTypeClient(Constant.JcgxEndPointName, address))
                {
                    int i = client.CallBack_Down(opID, msg);
                    this.ParentOrder.Logs.Add("完成集成共享P2P消息发送！" + i.ToString());
                }
            }
            catch (Exception ex)
            {
                this.ParentOrder.Logs.Add("集成共享(P2P下载)反馈消息发送失败：" + ex.ToString());
            }
        }
    }
}
