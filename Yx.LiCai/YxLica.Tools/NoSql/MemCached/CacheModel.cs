using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YxLiCai.Tools.NoSql.MemCached
{
   public class CacheModel
    {
        /// <summary>
        /// 键值
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 设置时间
        /// </summary>
        public int CacheTime { get; set; }
        /// <summary>
        /// 设置类型
        /// </summary>
        public int Cachetype { get; set; }
        /// <summary>
        /// 指定到具体时间过期
        /// </summary>
        public DateTime ExpiredTime { get; set; }
    }
}
