using System;

namespace YxLiCai.Model.Product
{
    /// <summary>
    /// 在售产品实体类
    /// </summary>
    public class SallProductModel : ProductModel
    {
        // Id,ProductName,ProductCategory,IsAutoEnable,EnableDate,
        //ProductAmount,B.AvailableAmount,B.PurchasedMemberSum,B.RaisedAmount
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int ProductCategory { get; set; }
        public bool IsAutoEnable { get; set; }
        public DateTime ExpectedEnableDate { get; set; }
        public decimal ProductAmount { get; set; }
        public decimal AvailableAmount { get; set; }
        public int PurchasedMemberSum { get; set; }
        public decimal RaisedAmount { get; set; }
    }
}
