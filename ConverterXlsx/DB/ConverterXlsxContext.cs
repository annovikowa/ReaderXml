﻿using ConverterXlsx.DB.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ConverterXlsx.DB
{
    /// <summary>
    /// Контекст подключения к БД
    /// </summary>
    public class ConverterXlsxContext : DbContext
    {
        public ConverterXlsxContext() : base("name=ServerDB")
        {

        }

        public virtual DbSet<Conversion> Conversions { get; set; }
        public virtual DbSet<Error> Errors { get; set; }
    }
}