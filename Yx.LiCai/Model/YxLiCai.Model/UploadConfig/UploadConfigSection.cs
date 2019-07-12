/*
 * 图片上传配置类
 * 作者：平扬
 * 时间：2014-1-1
 * 版本：1.0.0.0
 * 
 */
using System;
using System.Configuration;
using System.ComponentModel;

namespace YxLiCai.Model.UploadConfig
{
    public class UploadConfigSection : ConfigurationSection
    {
        private static readonly ConfigurationProperty s_property
            = new ConfigurationProperty(string.Empty, typeof(MyKeyValueCollection), null,
                                            ConfigurationPropertyOptions.IsDefaultCollection);

        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        public MyKeyValueCollection KeyValues
        {
            get
            {
                return (MyKeyValueCollection)base[s_property];
            }
        }
    }


    [ConfigurationCollection(typeof(MyKeyValueSetting))]
    public class MyKeyValueCollection : ConfigurationElementCollection		// 自定义一个集合
    {
        // 基本上，所有的方法都只要简单地调用基类的实现就可以了。

        public MyKeyValueCollection()
            : base(StringComparer.OrdinalIgnoreCase)	// 忽略大小写
        {
        }

        // 其实关键就是这个索引器。但它也是调用基类的实现，只是做下类型转就行了。
        new public MyKeyValueSetting this[string name]
        {
            get
            {
                return (MyKeyValueSetting)base.BaseGet(name);
            }
        }

        // 下面二个方法中抽象类中必须要实现的。
        protected override ConfigurationElement CreateNewElement()
        {
            return new MyKeyValueSetting();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MyKeyValueSetting)element).name;
        }

        // 说明：如果不需要在代码中修改集合，可以不实现Add, Clear, Remove
        public void Add(MyKeyValueSetting setting)
        {
            this.BaseAdd(setting);
        }
        public void Clear()
        {
            base.BaseClear();
        }
        public void Remove(string name)
        {
            base.BaseRemove(name);
        }
    }

    public class MyKeyValueSetting : ConfigurationElement	// 集合中的每个元素
    {
        /// <summary>
        /// 配置名
        /// </summary>
        [Description("配置名")]
        [ConfigurationProperty("name", IsRequired = true)]
        public string name
        {
            get { return this["name"].ToString(); }
            set { this["name"] = value; }
        }
        /// <summary>
        /// 文件存放路径 (可以相对路径也可以绝对路径)
        /// </summary>
        [Description("文件存放路径 (可以相对路径也可以绝对路径)")]
        [ConfigurationProperty("filePath", IsRequired = true)]
        public string filePath
        {
            get { return this["filePath"].ToString(); }
            set { this["filePath"] = value; }
        }
        /// <summary>
        /// 允许上载文件的后缀名
        /// </summary>
        [Description("允许上载文件的后缀名)")]
        [ConfigurationProperty("allowUpload", IsRequired = true)]
        public string allowUpload
        {
            get { return this["allowUpload"].ToString(); }
            set { this["allowUpload"] = value; }
        }
        /// <summary>
        /// 上传最大文件质量
        /// </summary>
        [Description("上传最大文件质量)")]
        [ConfigurationProperty("maxFileSize", IsRequired = true)]
        public int maxFileSize
        {
            get { return Convert.ToInt32(this["maxFileSize"]); }
            set { this["maxFileSize"] = value; }
        }
        /// <summary>
        /// 是否生成小图 (只对 bmp,jpg,jpeg,gif 产生作用)
        /// </summary>
        [Description("是否生成小图 (只对 bmp,jpg,jpeg,gif 产生作用))")]
        [ConfigurationProperty("createSmallPic", IsRequired = true)]
        public string createSmallPic
        {
            get { return this["createSmallPic"].ToString(); }
            set { this["createSmallPic"] = value; }
        }
        /// <summary>
        /// 小图尺寸数组
        /// </summary>
        [Description("小图尺寸数组")]
        [ConfigurationProperty("smallPicSize", IsRequired = true)]
        public string smallPicSize
        {
            get { return this["smallPicSize"].ToString(); }
            set { this["smallPicSize"] = value; }
        }
        /// <summary>
        /// 是否打水印
        /// </summary>
        [Description("是否打水印")]
        [ConfigurationProperty("addWaterMark", IsRequired = true)]
        public string addWaterMark
        {
            get { return this["addWaterMark"].ToString(); }
            set { this["addWaterMark"] = value; }
        }
        /// <summary>
        /// 水印类型 (image 图片水印  或者 text 文字水印)
        /// </summary>
        [Description("水印类型 (image 图片水印  或者 text 文字水印)")]
        [ConfigurationProperty("waterMarkType", IsRequired = true)]
        public string waterMarkType
        {
            get { return this["waterMarkType"].ToString(); }
            set { this["waterMarkType"] = value; }
        }
        /// <summary>
        /// 如果图片水印,则填写水印图片路径, 如果文字水印,填写文字内容.
        /// </summary>
        [Description("如果图片水印,则填写水印图片路径, 如果文字水印,填写文字内容.")]
        [ConfigurationProperty("waterMarkImgOrTxt", IsRequired = true)]
        public string waterMarkImgOrTxt
        {
            get { return this["waterMarkImgOrTxt"].ToString(); }
            set { this["waterMarkImgOrTxt"] = value; }
        }
        /// <summary>
        /// 水印透明度 (0-1) float型.
        /// </summary>
        [Description("水印透明度 (0-1) float型")]
        [ConfigurationProperty("transparency", IsRequired = true)]
        public string transparency
        {
            get { return this["transparency"].ToString(); }
            set { this["transparency"] = value; }
        }

    }
}
