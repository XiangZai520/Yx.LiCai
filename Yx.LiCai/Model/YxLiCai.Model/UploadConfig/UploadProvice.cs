﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace YxLiCai.Model.UploadConfig
{
    public class UploadProvice
    {
        private static UploadConfig uploadConfig;
        static UploadProvice()
        {
            uploadConfig = ConfigurationManager.GetSection("UploadConfig") as UploadConfig;
        }
        public static UploadConfig Instance()
        {
            return uploadConfig;
        }
    }
}
