﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YxLiCai.Tools.Util
{
    public class EnumItem : IEnumItem, IValueTextEntry
    {
        public string Name { get; set; }

        public string Text { get; set; }

        public int Value { get; set; }
    }
}
