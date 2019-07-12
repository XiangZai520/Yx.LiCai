using System;

namespace YxLiCai.Model.Loan
{
    /// <summary>
    /// 放款操作model
    /// </summary>
    public class LoanModel
    {
        public long id { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public int pjid { get; set; }
        /// <summary>
        /// 生成日期
        /// </summary>
        public DateTime ctime { get; set; }
        /// <summary>
        /// 应放金额
        /// </summary>
        public decimal amount_expect { get; set; }
        /// <summary>
        /// 实放金额
        /// </summary>
        public decimal amount_actual { get; set; }
        /// <summary>
        /// 状态：0.未处理；3.全部放款；4.部分放款；5.放款失败
        /// </summary>
        public int status { get; set; }
        /// <summary>
        /// 审核人ID
        /// </summary>
        public int auditorID { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime auditTime { get; set; }
        /// <summary>
        /// 放款人ID
        /// </summary>
        public int adminID { get; set; }
        /// <summary>
        /// 放款时间
        /// </summary>
        public DateTime loanTime { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 借款方（融资方）名称
        /// </summary>
        public string MerchantName { get; set; }
        /// <summary>
        /// 融资方法人姓名
        /// </summary>
        public string MerchantLegalManName { get; set; }
        /// <summary>
        /// 融资方法人联系电话
        /// </summary>
        public string MerchantLegalManPhone { get; set; }
        /// <summary>
        /// 融资方id
        /// </summary>
        public int MerchantID { get; set; }
    }
    public class LoanModelExtend : LoanModel
    {
        /// <summary>
        /// 项目信息
        /// </summary>
        public YxLiCai.Model.Project.ProjectModel project { get; set; }
        /// <summary>
        /// 银行卡信息
        /// </summary>
        public YxLiCai.Model.User.UserBankCardModel BankCardInfo { get; set; }
    }
}
