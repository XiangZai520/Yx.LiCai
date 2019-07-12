using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yx.LiCai.Models
{
    public class ConfirmAtoneOutModel
    {
        public string ProjectNames { get; set; }
        public decimal AtoneBenJin { get; set; }
        public decimal AtoneLiXi { get; set; }
        public decimal AtoneWeiYueJin { get;set; }
        public string ConfirmUrl { get; set; }
    }
}