using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.Abstract
{
    public abstract class CadastralPlanTerritory
    {
        /// <summary>
        /// Название файла.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Наименование органа кадастрового учета.
        /// </summary>
        public string OrganRegistrRights { get; set; }

        /// <summary>
        /// Дата выдачи.
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
        /// Сведения о кадастровых кварталах.
        /// </summary>
        public ICollection<CadastralBlock> CadastralBlocks { get; } = new List<CadastralBlock>();
    }
}
