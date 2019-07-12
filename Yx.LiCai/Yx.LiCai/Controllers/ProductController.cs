using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yx.LiCai.Models;
using Yx.LiCai.App_Start;
using YxLiCai.Model;
using YxLiCai.Model.Product;

namespace Yx.LiCai.Controllers
{ 
    public class ProductController : Controller
    {
        private YxLiCai.Server.Product.ProductManager _iproduct = new YxLiCai.Server.Product.ProductManager();
        //
        // GET: /Product/ 
        public ActionResult Index(string id)
        {
            #region 月月赢
            ResultInfo<SallProductModel> sallMonthProductResult = _iproduct.GetSallProduct(1);
            if (sallMonthProductResult.Result && sallMonthProductResult.Data != null)
            {
                ViewBag.Month_Id = sallMonthProductResult.Data.Id;
                decimal Month_RaisedAmount = sallMonthProductResult.Data.RaisedAmount;
                decimal Month_ProductAmount = sallMonthProductResult.Data.ProductAmount;
                ViewBag.Month_RaisedRate = Math.Round(Month_RaisedAmount / Month_ProductAmount * 100);
                ViewBag.Month_EnableDate = sallMonthProductResult.Data.ExpectedEnableDate;
                //ViewBag.Month_PurchasedMemberSum = sallMonthProductResult.Data.PurchasedMemberSum + 10000;
            }
            else
            {
                ViewBag.Month_Id = 0;
                ViewBag.Month_RaisedRate = 100;
                ViewBag.Month_EnableDate = "";
                //ViewBag.Month_PurchasedMemberSum = 10000;
            }
            ResultInfo<int> sallMonthProductCount = _iproduct.pdtSellCountByptype(1);
            if (sallMonthProductCount.Result && sallMonthProductCount.Data != 0)
            {
                ViewBag.Month_PurchasedMemberSum = sallMonthProductCount.Data+10000;
            }
            else
            {
                ViewBag.Month_PurchasedMemberSum = 10000;
            }

            #endregion

            #region 季季赢-3个月
            ResultInfo<SallProductModel> sallQuarter3ProductResult = _iproduct.GetSallProduct(2);
            if (sallQuarter3ProductResult.Result && sallQuarter3ProductResult.Data != null)
            {
                ViewBag.Quarter3_Id = sallQuarter3ProductResult.Data.Id;
                decimal Quarter3_RaisedAmount = sallQuarter3ProductResult.Data.RaisedAmount;
                decimal Quarter3_ProductAmount = sallQuarter3ProductResult.Data.ProductAmount;
                ViewBag.Quarter3_RaisedRate = Math.Round(Quarter3_RaisedAmount / Quarter3_ProductAmount * 100);
                ViewBag.Quarter3_EnableDate = sallQuarter3ProductResult.Data.ExpectedEnableDate;
                //ViewBag.Quarter3_PurchasedMemberSum = sallQuarter3ProductResult.Data.PurchasedMemberSum + 10000;
            }
            else
            {
                ViewBag.Quarter3_Id = 0;
                ViewBag.Quarter3_RaisedRate = 100;
                ViewBag.Quarter3_EnableDate = "";
                //ViewBag.Quarter3_PurchasedMemberSum = 10000;
            }
            ResultInfo<int> sallQuarter3ProductCount = _iproduct.pdtSellCountByptype(2);
            if (sallQuarter3ProductCount.Result && sallQuarter3ProductCount.Data != 0)
            {
                ViewBag.Quarter3_PurchasedMemberSum = sallQuarter3ProductCount.Data + 10000;
            }
            else
            {
                ViewBag.Quarter3_PurchasedMemberSum = 10000;
            }
            #endregion

            #region 季季赢-6个月
            ResultInfo<SallProductModel> sallQuarter6ProductResult = _iproduct.GetSallProduct(3);
            if (sallQuarter6ProductResult.Result && sallQuarter6ProductResult.Data != null)
            {
                ViewBag.Quarter6_Id = sallQuarter6ProductResult.Data.Id;
                decimal Quarter6_RaisedAmount = sallQuarter6ProductResult.Data.RaisedAmount;
                decimal Quarter6_ProductAmount = sallQuarter6ProductResult.Data.ProductAmount;
                ViewBag.Quarter6_RaisedRate = Math.Round(Quarter6_RaisedAmount / Quarter6_ProductAmount * 100);
                ViewBag.Quarter6_EnableDate = sallQuarter6ProductResult.Data.ExpectedEnableDate;
                //ViewBag.Quarter6_PurchasedMemberSum = sallQuarter6ProductResult.Data.PurchasedMemberSum + 10000;
            }
            else
            {
                ViewBag.Quarter6_Id = 0;
                ViewBag.Quarter6_RaisedRate = 100;
                ViewBag.Quarter6_EnableDate = "";
                //ViewBag.Quarter6_PurchasedMemberSum =10000;
            }
            ResultInfo<int> sallQuarter6ProductCount = _iproduct.pdtSellCountByptype(3);
            if (sallQuarter6ProductCount.Result && sallQuarter6ProductCount.Data != 0)
            {
                ViewBag.Quarter6_PurchasedMemberSum = sallQuarter6ProductCount.Data + 10000;
            }
            else
            {
                ViewBag.Quarter6_PurchasedMemberSum = 10000;
            }
            #endregion

            #region 年年丰
            ResultInfo<SallProductModel> sallYearProductResult = _iproduct.GetSallProduct(4);
            if (sallYearProductResult.Result && sallYearProductResult.Data != null)
            {
                ViewBag.Year_Id = sallYearProductResult.Data.Id;
                decimal Year_RaisedAmount = sallYearProductResult.Data.RaisedAmount;
                decimal Year_ProductAmount = sallYearProductResult.Data.ProductAmount;
                ViewBag.Year_RaisedRate = Math.Round(Year_RaisedAmount / Year_ProductAmount * 100);
                ViewBag.Year_EnableDate = sallYearProductResult.Data.ExpectedEnableDate;
                //ViewBag.Year_PurchasedMemberSum = sallYearProductResult.Data.PurchasedMemberSum + 10000;
            }
            else
            {
                ViewBag.Year_Id = 0;
                ViewBag.Year_RaisedRate = 100;
                ViewBag.Year_EnableDate = "";
                //ViewBag.Year_PurchasedMemberSum = 10000;
            }
            ResultInfo<int> sallYearProductCount = _iproduct.pdtSellCountByptype(4);
            if (sallYearProductCount.Result && sallYearProductCount.Data != 0)
            {
                ViewBag.Year_PurchasedMemberSum = sallYearProductCount.Data + 10000;
            }
            else
            {
                ViewBag.Year_PurchasedMemberSum = 10000;
            }

            #endregion

            ViewBag.ID = id;
            ViewBag.IsRealCard = UserContext.simpleUserInfoModel.IsRealCard;
            //ViewBag.NowTime = DateTime.Now.Year.ToString() + ","
            //    + DateTime.Now.Month.ToString() + ","
            //    + DateTime.Now.Day.ToString() + ","
            //    + DateTime.Now.Hour.ToString() + ","
            //    + DateTime.Now.Minute.ToString() + ","
            //    + DateTime.Now.Second.ToString();
            //ViewBag.NowTime = DateTime.Now.ToString("r");
            ViewBag.NowTime = DateTime.Now.AddHours(-8).ToString("r");
            return View();
        }
        /// <summary>
        /// 月月赢详情
        /// </summary>
        /// <returns></returns>
        public ActionResult _DetailMonth()
        {
            return View();
        }
        /// <summary>
        /// 季季享三个月 详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult _DetailQ3()
        {
            return View();
        }
        /// <summary>
        /// 季季享6个月 详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult _DetailQ6()
        {
            return View();
        }
        /// <summary>
        /// 年年丰 详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult _DetailYear()
        {
            return View();
        }
    }
}
