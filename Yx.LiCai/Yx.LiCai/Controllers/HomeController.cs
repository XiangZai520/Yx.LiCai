using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YxLiCai.Tools.Const;
using Yx.LiCai.Models;
using YxLiCai.Tools.Util;
using YxLiCai.Model.Product;
using YxLiCai.Model;

namespace Yx.LiCai.Controllers
{
    
    public class HomeController : Controller
    {
        private YxLiCai.Server.Product.ProductManager _iproduct = new YxLiCai.Server.Product.ProductManager();
         
        public ActionResult Index()
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
            }
            else
            {
                ViewBag.Month_Id = 0;
                ViewBag.Month_RaisedRate = 100;
                ViewBag.Month_EnableDate = "";
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
            }
            else
            {
                ViewBag.Quarter3_Id = 0;
                ViewBag.Quarter3_RaisedRate = 100;
                ViewBag.Quarter3_EnableDate = "";
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
            }
            else
            {
                ViewBag.Quarter6_Id = 0;
                ViewBag.Quarter6_RaisedRate = 100;
                ViewBag.Quarter6_EnableDate = "";
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
            }
            else
            {
                ViewBag.Year_Id = 0;
                ViewBag.Year_RaisedRate = 100;
                ViewBag.Year_EnableDate = "";
            }


            #endregion

            //ViewBag.NowTime = DateTime.Now.ToString("r");
            ViewBag.NowTime = DateTime.Now.AddHours(-8).ToString("r");

            return View();
        }

        /// <summary>
        /// 常见问题页
        /// </summary>
        /// <returns></returns>
        public ActionResult FAQ()
        {
            return View();
        }
        /// <summary>
        /// e休理财页
        /// </summary>
        /// <returns></returns>
        public ActionResult Introduction()
        {
            return View();
        }
        /// <summary>
        /// 查看协议
        /// </summary>
        /// <returns></returns>
        public ActionResult Protocol()
        {
            return View();
        }

        /// <summary>
        /// Controller 出错方法
        /// 跳转到错误页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
    }
}
