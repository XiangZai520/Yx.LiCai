
namespace YxLiCai.Model.Charge
{
    /// <summary>
    /// 充值管理Model
    /// add by lxm 2015年7月2日
    /// </summary>
    public class ChargeModel
    {
        public ChargeModel()
        {
        }
        /// <summary>
        /// 账户id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Balance { get; set; }
        /// <summary>
        /// 账户应付金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 账户类型：0融资方；1保理；2平台/活动
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 预警值
        /// </summary>
        public decimal AlertValue { get; set; }
    }
}
