/*
 * 图片上传配置类
 * 作者：平扬
 * 时间：2014-1-1
 * 版本：1.0.0.0
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace YxLiCai.Model.UploadConfig
{
    public class UploadConfig : ConfigurationSection
    {
        [ConfigurationProperty("Settings")]
        public SettingsCollection Settings
        {
            get
            {
                return this["Settings"] as SettingsCollection;
            }
        }
    }
}
