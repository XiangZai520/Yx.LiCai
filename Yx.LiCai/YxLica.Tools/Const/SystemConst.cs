using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using YxLiCai.Tools.Extension;
using System.Globalization;
using System.Xml;
using NPOI.SS.Util;
using YxLiCai.Tools.Util;

namespace YxLiCai.Tools.Const
{
    /// <summary>
    /// 产品类型
    /// </summary>
    public partial class ProductCategory
    {
        public ProductCategory()
        { }
        #region Model
        private int _id;
        private string _categoryname;
        private decimal? _yiedbase;
        private decimal? _yiedincrease;
        private decimal _yieldtop;
        private decimal? _withdrawfee = 0.0000M;
        private int _duration = 0;
        /// <summary>
        /// 产品类型id
        /// </summary>
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 类型名称
        /// </summary>
        public string CategoryName
        {
            set { _categoryname = value; }
            get { return _categoryname; }
        }
        /// <summary>
        /// 起始利率
        /// </summary>
        public decimal? YiedBase
        {
            set { _yiedbase = value; }
            get { return _yiedbase; }
        }
        /// <summary>
        /// 增长利率
        /// </summary>
        public decimal? YiedIncrease
        {
            set { _yiedincrease = value; }
            get { return _yiedincrease; }
        }
        /// <summary>
        /// 最高利率
        /// </summary>
        public decimal YieldTop
        {
            set { _yieldtop = value; }
            get { return _yieldtop; }
        }
        /// <summary>
        /// 手续费
        /// </summary>
        public decimal? WithdrawFee
        {
            set { _withdrawfee = value; }
            get { return _withdrawfee; }
        }
        /// <summary>
        /// 产品期限
        /// </summary>
        public int Duration
        {
            set { _duration = value; }
            get { return _duration; }
        }
        #endregion Model

    }
    /// <summary>
    /// 系统级别常量
    /// </summary>
    public class SystemConst
    {
        /// <summary>
        /// md5干扰码
        /// </summary>
        public static string Md5Random = ConfigurationManager.AppSettings["md5random"];

        /// <summary>
        /// 易宝回调页面
        /// </summary>
        public static string YeePaytCallBackUrl = ConfigurationManager.AppSettings["YeePaytCallBackUrl"];

        public static string LLPayNotifyUrl=ConfigurationManager.AppSettings["LLPayNotifyUrl"];

        public static string LLPayUrlReturn=ConfigurationManager.AppSettings["LLPayUrlReturn"];

        /// <summary>
        /// 是否使用压缩文件
        /// </summary>
        public static int IsMin = Util.ParseHelper.ToInt(ConfigurationManager.AppSettings["ismin"]);

        /// <summary>
        /// 加息券最大额度
        /// </summary>
        public static decimal AddRateMax = Util.ParseHelper.ToDecimal(ConfigurationManager.AppSettings["AddRateMax"]);
        /// <summary>
        /// 赎回手续费
        /// </summary>
        public static decimal RedemptionRate = Util.ParseHelper.ToDecimal(ConfigurationManager.AppSettings["RedemptionRate"]);
        /// <summary>
        /// 获取Email服务器地址
        /// </summary>
        public static string EmailFromAdress
        {
            get { return ConfigurationManager.AppSettings["EmailFromAddress"]; }
        }
        /// <summary>
        /// 获取Email服务器密码
        /// </summary>
        public static string EmailPwd
        {
            get { return ConfigurationManager.AppSettings["EmailFromPwd"]; }
        }
        /// <summary>
        /// 接收人邮件地址
        /// </summary>
        public static string EmailToAdress
        {
            get { return ConfigurationManager.AppSettings["EmailToAddress"]; }
        }

        /// <summary>
        /// 是否发送邮件
        /// </summary>
        public static string IsSendMail
        {
            get { return ConfigurationManager.AppSettings["IsSendMail"]; }
        }

        /// <summary>
        /// 用户cookie
        /// </summary>
        public static string userInfoCookieName = ConfigurationManager.AppSettings["userInfoCookieName"];
        /// <summary>
        /// 用户cookie(融资方平台)
        /// </summary>
        public static string FinauserInfoCookieName = ConfigurationManager.AppSettings["finauserInfoCookieName"];

        /// <summary>
        /// 可逆加密密钥
        /// </summary>
        public static string encrypt_des = ConfigurationManager.AppSettings["encrypt_des"];


        /// <summary>
        /// 验证码
        /// </summary>
        public const string VALID_CODE_SESSION_NAME = "Validate_Code";

        /// <summary>
        /// 获取产品类型名称 1专业项目2债券转让3优选计划4风险租赁
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static string GetProductCateType(string tid)
        {
            if (tid == "1")
                return "月";
            if (tid == "2")
                return "3个月";
            if (tid == "3")
                return "6个月";
            if (tid == "4")
                return "年";
            return "月";
        }

        /// <summary>
        /// 获取产品类型名称 1专业项目2债券转让3优选计划4风险租赁
        /// </summary>
        /// <param name="tid"></param>
        /// <returns></returns>
        public static string GetProductType(int tid)
        {
            if (tid == 1)
                return "月月盈";
            if (tid == 2)
                return "季季享-3个月";
            if (tid == 3)
                return "季季享-6个月";
            if (tid == 4)
                return "年年丰";
            return "月月盈";
        }
        /// <summary>
        /// 项目期限类型(1年，2月，3日)
        /// </summary>
        /// <returns></returns>
        public static string GetDurationRate(string ProjectDuration, string Duration)
        {
            string dur = Duration == "1" ? "年" : Duration == "2" ? "個月" : "天";
            return ProjectDuration + dur;
        }

        /// <summary>
        /// 获取产品类型名称 1.月月赢 2.季季享3个月 3.六个月 4.年年丰
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public static ProductCategory GetProductCategory(int cid)
        {
            ProductCategory cate = new ProductCategory();
            if (cid == 1)
            {
                cate.CategoryName = "月月盈";
                cate.WithdrawFee = 0;
                cate.YieldTop = 0.12m;
                cate.YiedBase = 0.07m;
                cate.YiedIncrease = 0.005m;
                cate.ID = 1;
                cate.Duration = 0;
            }
            if (cid == 2)
            {
                cate.CategoryName = "季季享-三个月";
                cate.WithdrawFee = 0.02m;
                cate.YieldTop = 0.09m;
                cate.YiedBase = 0.09m;
                cate.YiedIncrease = 0;
                cate.Duration = 90;
                cate.ID = 2;
            }
            if (cid == 3)
            {
                cate.CategoryName = "季季享-六个月";
                cate.WithdrawFee = 0.02m;
                cate.YieldTop = 0.11m;
                cate.YiedBase = 0.11m;
                cate.YiedIncrease = 0;
                cate.Duration = 180;
                cate.ID = 3;
            }
            if (cid == 4)
            {
                cate.CategoryName = "年年丰";
                cate.WithdrawFee = 0.02m;
                cate.YieldTop = 0.13m;
                cate.YiedBase = 0.13m;
                cate.YiedIncrease = 0;
                cate.Duration = 365;
                cate.ID = 3;
            }
            return cate;
        }


        /// <summary>
        /// 转换手机号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string PhoneCut(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "***";
            }
            int len = str.Length;
            if (len < 6) return "***";
            return str.Replace(str.Substring(3, len - 6), "****");
        }
        /// <summary>
        /// 转换身份证
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string IDCardCut(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "***";
            }
            int len = str.Length;
            return str.Replace(str.Substring(6, len - 10), "*******");
        }
        /// <summary>
        /// 转换银行卡
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string BankIDCut(string str)
        {
            if (str.Trim() == "" || str == "0")
            { return ""; }

            if (string.IsNullOrEmpty(str))
            {
                return "***";
            }

            int len = str.Length;
            if (len < 12)
            { return str.Replace(str.Substring(6, len - 9), "*******"); }
            return str.Replace(str.Substring(6, len - 10), "*******");
        }
        /// <summary>
        /// 姓名
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string NameCut(string str)
        {
            if (str.Trim() == "")
            { return ""; }
            str = str.Trim();
            str = str.Replace(" ", "");
            return "*" + str.Substring(1, str.Length - 1);
        }
        /// <summary>
        /// 转换收益率
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static string GetRate(decimal rate)
        {
            decimal r = (rate * 100);
            return r.ToString().Substring(0, 4) + "%";
        }
        /// <summary>
        /// 进度
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static string GetRaiseProgress(decimal rate)
        {
            decimal r = (rate * 100);
            return Math.Ceiling(r).ToString();
        }


        /// <summary>
        /// 利率转换
        /// </summary>
        /// <param name="fmoney"></param>
        /// <returns></returns>
        public static decimal RateConvert(decimal fmoney)
        {
            return Math.Round(fmoney, 1);
        }

        /// <summary> 
        /// 输入Float格式数字，将其转换为货币表达方式 
        /// </summary> 
        /// <param name="ftype">货币表达类型：0=人民币；1=港币；2=美钞；3=英镑；4=不带货币;其它=不带货币表达方式</param> 
        /// <param name="fmoney">传入的int数字</param> 
        /// <returns>返回转换的货币表达形式</returns> 
        public static decimal MoenyConvert(decimal fmoney)
        {
            if (fmoney == 0)
            {
                return 0.00m;
            }
            string result = "";
            string fmoney_str = fmoney.ToString();
            if (fmoney_str.IndexOf('.') >= 0)
            {
                string[] array = fmoney_str.Split('.');
                if (array[1].Length > 2)
                {
                    result = array[0] + "." + array[1].Substring(0, 2);
                    return Convert.ToDecimal(result);
                }
                else
                {
                    return fmoney;
                }
            }
            else
            {
                return fmoney;
            }
        }

        /// <summary>
        /// 只截取整数部分
        /// </summary>
        /// <param name="fmoney"></param>
        /// <returns></returns>
        public static string MoenyConvertString(decimal fmoney)
        {
            string money = fmoney.ToString();
            int length = money.IndexOf('.');
            money = money.Substring(0, length);
            return money;
        }

        /// <summary> 
        /// 金额转换
        /// </summary> 
        /// <param name="ftype">货币表达类型：0=人民币；1=港币；2=美钞；3=英镑；4=不带货币;其它=不带货币表达方式</param> 
        /// <param name="fmoney">传入的int数字</param> 
        /// <returns>返回转换的货币表达形式</returns> 
        public static string ConvertMoeny(decimal fmoney)
        {
            var f = fmoney / 10000;
            return f.ToString("c") + "萬";
        }


        /// <summary>
        /// 计算利息
        /// </summary>
        /// <param name="dr">1年2月3日</param>
        /// <param name="fmoney"></param>
        /// <param name="dd">期限</param>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static decimal CalcRate(string dr, decimal fmoney, int dd, decimal rate)
        {
            if (dr == "1")
            {
                return Math.Ceiling(fmoney * rate * dd);
            }
            else if (dr == "2")
            {
                return Math.Ceiling(fmoney * (rate / 12) * dd);
            }
            else if (dr == "3")
            {
                return Math.Ceiling(fmoney * (rate / 365) * dd);
            }
            return 0;
        }


        /// <summary>  
        /// 输入Float格式数字，将其转换为货币表达方式  
        /// </summary>  
        /// <param name="ftype">货币表达类型：0=人民币；1=港币；2=美钞；3=英镑；4=不带货币;其它=不带货币表达方式</param>  
        /// <param name="fmoney">传入的int数字</param>  
        /// <returns>返回转换的货币表达形式</returns>  
        public static string ConvertCurrency(decimal fmoney, int ftype)
        {
            CultureInfo cul = null;
            string _rmoney = string.Empty;
            var f = fmoney / 10000;
            try
            {
                switch (ftype)
                {
                    case 0:
                        cul = new CultureInfo("zh-CN");//中国大陆  
                        _rmoney = f.ToString("c", cul);
                        break;

                    case 1:
                        cul = new CultureInfo("zh-HK");//香港  
                        _rmoney = f.ToString("c", cul);
                        break;
                    case 2:
                        cul = new CultureInfo("en-US");//美国  
                        _rmoney = f.ToString("c", cul);
                        break;
                    case 3:
                        cul = new CultureInfo("en-GB");//英国  
                        _rmoney = f.ToString("c", cul);
                        break;
                    case 4:
                        _rmoney = string.Format("{0:n}", f);//没有货币符号  
                        break;

                    default:
                        _rmoney = string.Format("{0:n}", f);
                        break;
                }
            }
            catch
            {
                _rmoney = "";
            }

            return _rmoney + "萬"; ;
        }


        /// <summary>
        /// 对手机号进行加密
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static string ConvertPhoneNum(string phone)
        {
            if(string.IsNullOrEmpty(phone))
            {
                return "";
            }

            long phonenum = long.Parse(phone);
            int sd = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day;
            phonenum = phonenum + sd;
            return ETS.Util.EncodeAndDecode.EncodeBase64(phonenum.ToString());
        }

        /// <summary>
        /// 对手机号进行解密
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static string ConvertPhoneNum2(string phone)
        { 
            if (string.IsNullOrEmpty(phone))
            {
                return "";
            }
            long phonenum = 0;
            try
            {
             phone = ETS.Util.EncodeAndDecode.DecodeBase64(phone);
             if (!ValidateHelper.IsPhoneNum(phone))
                {
                    return phone;
                }
                phonenum = long.Parse(phone);
            int sd = DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day;
            phonenum = (phonenum - sd); 
            }
            catch (Exception)
            {
                phonenum = 0;
                throw;
            }
       
            return phonenum.ToString();
        }



        /// <summary>
        /// 投资进度
        /// </summary>
        /// <param name="rate"></param>
        /// <returns></returns>
        public static string GetJinDu(decimal rate)
        {
            decimal r = (rate * 100);
            return r.ToString() + "%";

        }

        /// <summary>
        /// 根据类型获取发送短信的信息  By张浩然  2015-06-26 18：12：00
        /// </summary>
        /// <param name="type">发送消息类别</param>
        /// <returns></returns>
        public static string MsgContentByType(string type)
        {
            string msg = string.Empty;
            string sFullPath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/MsgContentMenu.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(sFullPath);
            XmlElement rootElem = doc.DocumentElement;   //获取根节点  
            XmlNodeList personNodes = rootElem.GetElementsByTagName("Msg"); //获取person子节点集合  
            foreach (XmlNode node in personNodes)
            {
                string strType = node.ChildNodes[0].InnerText;
                if (type == strType)
                {
                    msg = node.ChildNodes[2].InnerText;
                }
            }

            return msg;
        }


    }
}
