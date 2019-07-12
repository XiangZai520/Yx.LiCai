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

namespace  YxLiCai.Model.UploadConfig
{
    public class Setting : ConfigurationElement
    {
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["name"] as string;
            }
        }


        /// <summary>
        /// 文件路径
        /// </summary>
        [ConfigurationProperty("filePath", IsRequired = true)]
        public string FilePath
        {
            get
            {
                return this["filePath"] as string;
            }
        } 
        /// <summary>
        /// 允许上传文件,例：jpg|jpeg|gif
        /// </summary>
        [ConfigurationProperty("allowUpload", IsRequired = true)]
        public string AllowUpload
        {
            get
            {
                return this["allowUpload"] as string;
            }
        }

        /// <summary>
        /// 允许最大上载文件
        /// </summary>
        [ConfigurationProperty("maxFileSize", IsRequired = true)]
        public int MaxFileSize
        {
            get
            {
                return int.Parse(this["maxFileSize"].ToString());
            }
        }

        /// <summary>
        /// 是否生成小图
        /// </summary>
        [ConfigurationProperty("createSmallPic", IsRequired = true)]
        public bool CreateSmallPic
        {
            get
            {
                return bool.Parse(this["createSmallPic"].ToString());
            }
        } 
        /// <summary>
        /// 小图的尺寸数组(200x300,300x222,333x222)
        /// </summary>
        [ConfigurationProperty("smallPicSize", IsRequired = true)]
        public string SmallPicSize
        {
            get
            {
                return this["smallPicSize"] as string;
            }
        }

        /// <summary>
        /// 是否打水印
        /// </summary>
        [ConfigurationProperty("addWaterMark", IsRequired = true)]
        public bool AddWaterMark
        {
            get
            {
                return bool.Parse(this["addWaterMark"].ToString());
            }
        }


        /// <summary>
        /// 水印类型
        /// </summary>
        [ConfigurationProperty("waterMarkType", IsRequired = true)]
        public string WaterMarkType
        {
            get
            {
                return this["waterMarkType"] as string;
            }
        }

        /// <summary>
        /// 水印类型
        /// </summary>
        [ConfigurationProperty("waterMarkImgOrTxt", IsRequired = true)]
        public string WaterMarkImgOrTxt
        {
            get
            {
                return this["waterMarkImgOrTxt"] as string;
            }
        }

        /// <summary>
        /// 水印透明度
        /// </summary>
        [ConfigurationProperty("transparency", IsRequired = true)]
        public float Transparency
        {
            get
            {
                return float.Parse(this["transparency"].ToString());
            }
        }



    }
}
