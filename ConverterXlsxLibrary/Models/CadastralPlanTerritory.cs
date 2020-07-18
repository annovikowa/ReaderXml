﻿using System;
using System.Collections.Generic;

namespace ConverterXlsxLibrary.Models
{
    /// <summary>
    /// Кадастровый план территории.
    /// </summary>
    public class CadastralPlanTerritory
    {
        /// <summary>
        /// Версия КПТ
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Название файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Наименование органа кадастрового учета.
        /// </summary>
        public string OrganRegistrRights { get; set; }

        /// <summary>
        /// Дата.
        /// </summary>
        public DateTime DateFormation { get; set; }

        /// <summary>
        /// Номер документа.
        /// </summary>
        public string RegistrationNumber { get; set; }

        /// <summary>
        /// Должностное лицо.
        /// </summary>
        public string Official { get; set; }

        /// <summary>
        /// Сведения о кадастровых объектах.
        /// </summary>
        public ICollection<CadastralBlock> CadastralBlocks { get; } = new List<CadastralBlock>();

        /// <summary>
        /// Список ошибок.
        /// </summary>
        public ICollection<string> Errors { get; set; } = new List<string>();
    }
}
