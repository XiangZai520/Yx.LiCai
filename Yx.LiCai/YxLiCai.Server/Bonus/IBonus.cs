using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Server.Bonus
{
    /// <summary>
    /// 加息券接口
    /// </summary>
    public interface IBonus
    {
        /// <summary>
        /// 生成加息券
        /// </summary>
        /// <returns></returns>
        bool GenerateInterestBonus();
        /// <summary>
        /// 生成红包
        /// </summary>
        /// <returns></returns>
        bool GenerateLuckyMoney();
        /// <summary>
        /// 生成特享金
        /// </summary>
        /// <returns></returns>
        bool GenerateSpecialPrincipal();
    }
}
