﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YxLiCai.Tools.Expand
{
    public class ActionStatusAttribute : Attribute
    {
        private Type _type;
        public ActionStatusAttribute(Type status)
        {

        }
    }
}
