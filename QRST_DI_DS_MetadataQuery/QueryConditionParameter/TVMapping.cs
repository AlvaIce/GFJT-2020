/*
 * 作者：zxw
 * 创建时间：2013-08-06
 * 描述：用于读取TvMapping中的值
*/ 
using System;
using System.Text;
using System.Configuration;

namespace QRST_DI_DS_MetadataQuery.QueryConditionParameter
{
    public class TVMapping
    {
        /// <summary>
        /// 配置文件名称
        /// </summary>
        readonly static string CONFIG_NAME = string.Format(@"{0}\{1}", AppDomain.CurrentDomain.BaseDirectory, "TVMapping.config");
        Configuration config;

        public TVMapping()
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap() { ExeConfigFilename = CONFIG_NAME };
            config = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
        }

        public string GetValue(string _key)
        {
            if (config.AppSettings.Settings[_key] != null)
            {
                return config.AppSettings.Settings[_key].Value;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                throw new Exception(string.Format("TVMapping配置中不存在'{0}'的节点!",_key));
            }
            
        }

    }
}
