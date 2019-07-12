using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Model.Product
{
    public class ProductModel : ProductInfo
    {
        public string ProductStr { get; set; }
        public ProductCount ProductCount { get; set; }
        public List<string> ProjectList { get; set; }
        /// <summary>
        /// 自动上线
        /// </summary>
        public string AutoEnable { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string PStatus { get; set; }
        /// <summary>
        /// 类型 1.月月盈；2.季季享3个月；3.季季享6个月；4.年年丰
        /// </summary>
        public string PCategory { get; set; }
        /// <summary>
        /// 是否预警 0否,1是
        /// </summary>
        public string IsAlert { get; set; }
        /// <summary>
        /// 是否是可售商品
        /// </summary>
        public string IsSellPdt { get; set; }
    }
    public partial class ProductInfo
    {
        public ProductInfo()
        { }
        #region Model
        private int _id;
        private string _productname;
        private int _productcategory = 0;
        private int? _productstatus;
        private int? _creatorid;
        private decimal? _yieldbase;
        private decimal? _yieldincrease;
        private decimal? _yieldtop;
        private DateTime? _createdate;
        private DateTime? _auditdate;
        private int? _auditorid;
        private int? _productorder;
        private bool _isdeleted = false;
        private DateTime? _enabledate;
        private DateTime? _enddate;
        private decimal? _productamount = 0.0000M;
        private string _remark;

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ProductName
        {
            set { _productname = value; }
            get { return _productname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ProductCategory
        {
            set { _productcategory = value; }
            get { return _productcategory; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductStatus
        {
            set { _productstatus = value; }
            get { return _productstatus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? CreatorID
        {
            set { _creatorid = value; }
            get { return _creatorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? YieldBase
        {
            set { _yieldbase = value; }
            get { return _yieldbase; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? YieldIncrease
        {
            set { _yieldincrease = value; }
            get { return _yieldincrease; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? YieldTop
        {
            set { _yieldtop = value; }
            get { return _yieldtop; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AuditDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? AuditorID
        {
            set { _auditorid = value; }
            get { return _auditorid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ProductOrder
        {
            set { _productorder = value; }
            get { return _productorder; }
        }
        /// <summary>
        /// 是否自动生效
        /// </summary>
        public int IsAutoEnable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EnableDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? ProductAmount
        {
            set { _productamount = value; }
            get { return _productamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        /// <summary>
        /// 预期开始日期
        /// </summary>
        public DateTime ExpectedEnableDate { get; set; }
        /// <summary>
        /// 产品期限
        /// </summary>
        public int ProductDuration { get; set; }
        #endregion Model
    }
    public partial class ProductCount
    {
        public ProductCount()
        { }
        #region Model
        private int _productid;
        private decimal _raisedamount = 0.00M;
        private decimal _availableamount = 0.00M;
        private decimal _paidinterest = 0.00M;
        private decimal _accruedinterest = 0.00M;
        /// <summary>
        /// 购买人数
        /// </summary>
        public string PurchasedMemberSum { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public int ProductID
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal RaisedAmount
        {
            set { _raisedamount = value; }
            get { return _raisedamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal AvailableAmount
        {
            set { _availableamount = value; }
            get { return _availableamount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal PaidInterest
        {
            set { _paidinterest = value; }
            get { return _paidinterest; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal AccruedInterest
        {
            set { _accruedinterest = value; }
            get { return _accruedinterest; }
        }
        #endregion Model

    }
}
