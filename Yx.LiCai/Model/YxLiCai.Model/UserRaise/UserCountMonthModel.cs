using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserRaise
{
    /// <summary>
    /// UserCountMonth:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UserCountMonthModel
    {
        public UserCountMonthModel()
        { }
        #region Model
        private int _id;
        private int _userid = 0;
        private decimal _mymoney = 0.00M;
        private decimal _lockmoney = 0.00M;
        private decimal _raisemoney = 0.00M;
        private decimal _allinterest = 0.00M;
        private decimal? _haveinterest = 0.00M;
        private decimal? _traisemoney = 0.00M;
        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 月账号总资产
        /// </summary>
        public decimal MyMoney
        {
            set { _mymoney = value; }
            get { return _mymoney; }
        }
        /// <summary>
        /// 月账号总资产
        /// </summary>
        public decimal Lockmoney
        {
            set { _lockmoney = value; }
            get { return _lockmoney; }
        }
        /// <summary>
        /// 月账户总投资本金
        /// </summary>
        public decimal RaiseMoney
        {
            set { _raisemoney = value; }
            get { return _raisemoney; }
        }
        /// <summary>
        /// 月账户总累计收益
        /// </summary>
        public decimal AllInterest
        {
            set { _allinterest = value; }
            get { return _allinterest; }
        }
        /// <summary>
        /// 已收利息
        /// </summary>
        public decimal? HaveInterest
        {
            set { _haveinterest = value; }
            get { return _haveinterest; }
        }
        /// <summary>
        /// 特享金本金
        /// </summary>
        public decimal? TRaiseMoney
        {
            set { _traisemoney = value; }
            get { return _traisemoney; }
        }
        #endregion Model

    }
}
