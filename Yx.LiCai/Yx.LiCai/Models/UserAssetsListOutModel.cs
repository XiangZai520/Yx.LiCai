using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yx.LiCai.Models
{
    public class UserAssetsListOutModel
    {
        //public UserAssetsListOutModel()
        //{ }
        public UserAssetsListOutModel(long Id,int ProductType)
        {
            this.Zi_Money = this.GetZiMoney(Id,ProductType);
            this.Zong_Money = this.GetZongMoney(Id);
            this.ProductType = ProductType;
            this.CurrentPage = 1;
        }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int ProductType { get; set; }
        public decimal Zi_Money { get; set; }
        public decimal Zong_Money { get; set; }
        public List<UserAssetsViewModel> list { get; set; }

        public decimal GetZiMoney(long Id, int ProductType)
        {
            decimal ZiMoney = 0;
            switch (ProductType)
            {
                case 1:
                    ZiMoney = new YxLiCai.Server.User.UserCountMonthService().GetZiMoney(Id).Data;
                    break;
                case 2:
                    ZiMoney = new YxLiCai.Server.User.UserCountSeasonService().GetZiMoney(Id).Data;
                    break;
                case 3:
                    ZiMoney = new YxLiCai.Server.User.UserCountSeasonService().GetZiMoney(Id).Data;
                    break;
                case 4:
                    ZiMoney = new YxLiCai.Server.User.UserCountYearService().GetZiMoney(Id).Data;
                    break;
            }

            return ZiMoney;
        }
        public decimal GetZongMoney(long Id)
        {
            return new YxLiCai.Server.User.UserCountService().GetZongMoney(Id).Data;
        }

    }
}