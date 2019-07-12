using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YxLiCai.Admin.Models
{
    public class LLPayOutModel
    {
        private string _version="1.0";
        private string _oid_partner = YxLica.Tools.Pay.LLWYPay.PartnerConfig.OID_PARTNER;
        private string _user_id;
        private string _timestamp = YxLica.Tools.Pay.LLWYPay.YinTongUtil.getCurrentDateTimeStr();
        private string _sign_type="MD5";
        private string _sign;
        private string _busi_partner = "101001";
        private string _no_order;
        private string _dt_order;
        private string _name_goods="保理账户充值";
        private string _money_order;
        private string _notify_url;
        private string _url_return;
        private string _bank_code="";
        private string _pay_type="1";
        private string _risk_item;


        public string version
        {
            get
            {
                return this._version;
            }
            set
            {
                this._version = value;
            }
        }
        public string oid_partner
        {
            get
            {
                return this._oid_partner;
            }
            set
            {
                this._oid_partner = value;
            }
        }
        public string user_id
        {
            get
            {
                return this._user_id;
            }
            set
            {
                this._user_id = value;
            }
        }
        public string timestamp
        {
            get
            {
                return this._timestamp;
            }
            set
            {
                this._timestamp = value;
            }
        }
        public string sign_type
        {
            get
            {
                return this._sign_type;
            }
            set
            {
                this._sign_type = value;
            }
        }
        public string sign
        {
            get
            {
                return this._sign;
            }
            set
            {
                this._sign = value;
            }
        }
        public string busi_partner
        {
            get
            {
                return this._busi_partner;
            }
            set
            {
                this._busi_partner = value;
            }
        }
        public string no_order
        {
            get
            {
                return this._no_order;
            }
            set
            {
                this._no_order = value;
            }
        }
        public string dt_order
        {
            get
            {
                return this._dt_order;
            }
            set
            {
                this._dt_order = value;
            }
        }
        public string name_goods
        {
            get
            {
                return this._name_goods;
            }
            set
            {
                this._name_goods = value;
            }
        }
        public string money_order
        {
            get
            {
                return this._money_order;
            }
            set
            {
                this._money_order = value;
            }
        }
        public string notify_url
        {
            get
            {
                return this._notify_url;
            }
            set
            {
                this._notify_url = value;
            }
        }
        public string url_return
        {
            get
            {
                return this._url_return;
            }
            set
            {
                this._url_return = value;
            }
        }
        public string bank_code
        {
            get
            {
                return this._bank_code;
            }
            set
            {
                this._bank_code = value;
            }
        }
        public string pay_type
        {
            get
            {
                return this._pay_type;
            }
            set
            {
                this._pay_type = value;
            }
        }
        public string risk_item
        {
            get
            {
                return this._risk_item;
            }
            set
            {
                this._risk_item = value;
            }
        }

    }
}