using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YxLiCai.Tools.Const
{
    public class RedisCacheKey
    {

        /// <summary>
        /// 后台登录验证码缓存key
        /// </summary>
        public const string CaptchaCodeAdminLogin = "CaptchaCodeAdminLogin_{0}";

        /// <summary>
        /// 前台用户手机验证码
        /// </summary>
        public const string CaptchaCodeYIxiu = "CaptchaCodeYIxiu_{0}";
        /// <summary>
        /// 前台用户手登录验证码
        /// </summary>
        public const string CaptchaCodeYIxiuUserCode = "CaptchaCodeYIxiuUserCode_{0}";
        /// <summary>
        /// 前台用户银行卡绑定唯一吗
        /// </summary>
        public const string CaptchaCodeSendBoundBankCard = "{0}_SendBoundBankCard";
        /// <summary>
        /// 用户简单实体信息放到Reades
        /// </summary>
        public const string CaptchaUserReadsModel = "UserReadsModel_{0}";
        /// <summary>
        /// 图片自增Key
        /// </summary>
        public const string ImageIdentity = "ImageIdentity";

        /// <summary>
        /// 用户提现次数
        /// </summary>
        public const string WithdrawalsCount = "WithdrawalsCount_{0}";

        /// <summary>
        /// 用户购买次数
        /// </summary>
        public const string UserBuyCount = "UserBuyCount_{0}";

        /// <summary>
        /// 用户电话号码缓存
        /// </summary>
        public const string Phone = "Phone_{0}";

        /// <summary>
        /// 用户银行卡临时缓存
        /// </summary>
        public const string UserBankInfoCache = "UserBankInfoCache_{0}";


        #region 融资方Key
        /// <summary>
        /// 用户简单实体信息放到Reades（融资方简单信息）
        /// </summary>
        public const string UserFinancingReadsModel = "UserFinancingReadsModel_{0}";
        /// <summary>
        /// 融资方用户银行卡绑定唯一吗
        /// </summary>
        public const string UserFinancingCaptchaCodeSendBoundBankCard = "{0}_UserFinancingSendBoundBankCard";
        /// <summary>
        /// 融资方手机验码证Key
        /// </summary>
        public const string FinancingCodeYIxiu = "FinancingCodeYIxiu_{0}";
        /// <summary>
        /// 融资方手机验码证防刷Key
        /// </summary>
        public const string FinancingCodeYIxiuFS = "FinancingCodeYIxiuFS_{0}";
        #endregion


    }
}
