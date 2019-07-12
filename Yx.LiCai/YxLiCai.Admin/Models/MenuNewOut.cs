using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YxLiCai.Model.Authority;

namespace YxLiCai.Admin.Models
{
    public class MenuNewOut
    {
        public MenuNewOut()
        {
        }
        public int ParParID { get; set; }
        public int ParID { get; set; }
        public int ParMenuType { get; set; }
        public string ParMenuName { get; set; }
        public string ParUrl { get;set; }
        public List<AuthorityMenuModel> list_menu { get; set; }
    }
}