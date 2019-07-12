using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.UserRaise
{  
    /// <summary>
    /// UserCountYear:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class UserCountYearModel
    {
        public UserCountYearModel()
        { }
        #region Model
        private int _id;
        private int _userid = 0;
        private decimal _allmoney;
        private decimal _allinterest = 0.00M;
        private decimal? _raisemoney = 0.00M;
        private decimal? _haveinterest = 0.00M;
        private decimal? _notinterest = 0.00M;
        /// <summary>
        /// 
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
        /// 年账户总资产
        /// </summary>
        public decimal AllMoney
        {
            set { _allmoney = value; }
            get { return _allmoney; }
        }
        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal AllInterest
        {
            set { _allinterest = value; }
            get { return _allinterest; }
        }
        /// <summary>
        /// 投资本金
        /// </summary>
        public decimal? RaiseMoney
        {
            set { _raisemoney = value; }
            get { return _raisemoney; }
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
        /// 待收利息
        /// </summary>
        public decimal? NotInterest
        {
            set { _notinterest = value; }
            get { return _notinterest; }
        }
        #endregion Model

    }
}
