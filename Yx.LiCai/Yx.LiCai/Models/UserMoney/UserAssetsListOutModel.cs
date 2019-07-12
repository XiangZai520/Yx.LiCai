using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yx.LiCai.Models.UserMoney
{
    public class UserAssetsListOutModel
    {
        //public UserAssetsListOutModel()
        //{ }
        public UserAssetsListOutModel(long Id, int ProductType)
        {
            this.ShouYI = this.GetShouYI(Id, ProductType);
            this.ProductType = ProductType;
            this.CurrentPage = 1;
        }
        public int ProductType { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public decimal ShouYI { get; set; }
        public List<UserMonthViewModel> list1 { get; set; }
        public List<UserSeanViewModel> list2 { get; set; }
        public List<UserYearViewModel> list3 { get; set; }
        public decimal GetShouYI(long Id, int ProductType)
        {
            decimal ZiMoney = 0;
            switch (ProductType)
            {
                case 1:
                    ZiMoney = new YxLiCai.Server.UserAccumulatedEarnings.UserAccumulatedEarningsService().GetUserMonth(Id);
                    break;
                case 2:
                    ZiMoney = new YxLiCai.Server.UserAccumulatedEarnings.UserAccumulatedEarningsService().GetUserSeason(Id,"0");
                    break;
                case 3:
                    ZiMoney = new YxLiCai.Server.UserAccumulatedEarnings.UserAccumulatedEarningsService().GetUserSeason(Id,"1");
                    break;
                case 4:
                    ZiMoney = new YxLiCai.Server.UserAccumulatedEarnings.UserAccumulatedEarningsService().GetUserYear(Id);
                    break;
            }

            return ZiMoney;
        }

    }
}