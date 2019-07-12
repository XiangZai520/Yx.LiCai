using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YxLiCai.Model.ExtendModel;

namespace YxLiCai.Admin.Models
{
    public class MemberDetailOutModel
    {
        public YxLiCai.Model.User.UserInfoModel userInfo { get; set; }
        /// <summary>
        /// 投资记录
        /// </summary>
        public List<PurchaseRecordEx> PurchaseRecordList { get; set; }
        /// <summary>
        /// 提现记录
        /// </summary>
        public List<WithdrawRecordEx> WithdrawRecordList { get; set; }
        /// <summary>
        /// 赎回记录
        /// </summary>
        public List<RedemptionRecordEx> RedemptionRecordList { get; set; }


        /// <summary>
        /// 银行名称
        /// </summary>
        private string _BankName = "";
        public string BankName
        {
            get 
            {
                return _BankName;
            }
            set
            {
                this._BankName = value;
            }
        }
        /// <summary>
        /// 银行卡号
        /// </summary>
        private string _BankCardNo = "";
        public string BankCardNo
        {
            get
            {
                return _BankCardNo;
            }
            set
            {
                this._BankCardNo = value;
            }
        }
        /// <summary>
        /// 账户总资产
        /// </summary>
        public decimal TotalMoney { get; set; }
        /// <summary>
        /// 自有资产
        /// </summary>
        public decimal ZiYouMoney { get; set; }
        /// <summary>
        /// 特享资产
        /// </summary>
        public decimal TeXiangMoney { get; set; }
        ////账户余额
        //public decimal ZhangHuMoney { get; set; }
        //可用余额
        //public decimal KeYongMoney { get; set; }
        ////冻结金额
        //public decimal DongJieMoney { get; set; }
        /// <summary>
        /// 可提现金额
        /// </summary>
        public decimal KeTiXianMoney { get; set; }
        /// <summary>
        /// 可赎回金额
        /// </summary>
        public decimal KeShuHuiMoney { get; set; }
        /// <summary>
        /// 累计收益
        /// </summary>
        public decimal TotalIncome { get; set; }
        /// <summary>
        /// 已获红包
        /// </summary>
        public decimal TotalHongBao { get; set; }

    }
}