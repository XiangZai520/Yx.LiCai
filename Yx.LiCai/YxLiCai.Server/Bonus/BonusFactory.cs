using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Server.Bonus
{
    /// <summary>
    /// 加息券工厂类
    /// </summary>
    public class BonusFactory
    {
        /// <summary>
        /// 注册奖励
        /// </summary>
        /// <returns></returns>
        public IBonus RegisterBonus()
        {
            return new  RegisterBonus();
        }
    }
}
