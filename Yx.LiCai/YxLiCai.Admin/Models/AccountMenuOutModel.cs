using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YxLiCai.Model.Authority;
using YxLiCai.Model.ExtendModel;

namespace YxLiCai.Admin.Models
{
    public class AccountMenuOutModel
    {
        public List<AccountMenuEx> list_AccountMenu { get; set; }
        public List<AuthorityMenuModel> list_AllMenu_Fir { get; set; }
        public List<AuthorityMenuModel> list_AllMenu_Sec { get; set; }
        public List<AuthorityMenuModel> list_AllMenu_Tir { get; set; }
    }
}