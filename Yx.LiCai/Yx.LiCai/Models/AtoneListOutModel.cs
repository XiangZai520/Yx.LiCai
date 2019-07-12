using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yx.LiCai.Models
{
    public class AtoneListOutModel
    {
        public List<AtoneDetaiModel> list { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
    }
}