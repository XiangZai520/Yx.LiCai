using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLica.Tools.Dic
{
    public class CommonDic
    {
        //产品分类
        public static IDictionary<int, string> ProductType = new Dictionary<int, string>() 
        { 
            {1,"月月盈"},
            {2,"季季享3个月"},
            {3,"季季享6个月"},
            {4,"年年丰"}
        };
        //活动类型
        public static IDictionary<int, string> ActType = new Dictionary<int, string>() 
        { 
            {0,"注册"},
            {1,"邀请"},
            {2,"购买"}
        };
        //提现状态
        public static IDictionary<int, string> WithdrawStatus = new Dictionary<int, string>() 
        { 
            {0,"未审核"},
            {1,"审核通过"},
            {2,"已提现"},
            {3,"审核未通过"},
            {4,"支付失败"}
        };
        public static IDictionary<int, string> RechargeType = new Dictionary<int, string>() 
        { 
            {1,"易宝"},
            {2,"连连"}
        };

    }
}
