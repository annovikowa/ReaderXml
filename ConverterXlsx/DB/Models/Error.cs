﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConverterXlsx.DB.Models
{
    public class Error
    {
        // int или long выбрать? - лучше long, в теории сообщений может быть больше чем загрузок
        public long Id { get; set; }

        public string ConversionId { get; set; }

        public virtual Conversion Conversion { get; set; }

        public string ErrorDescription { get; set; }
    }
}