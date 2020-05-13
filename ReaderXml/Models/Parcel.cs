namespace ReaderXml.Models
{
    public class Parcel : CadastralObject
    {
        /// <summary>
        /// Кадастровый номер.
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Площадь.
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Наименование участка.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Кадастровый номер земельного участка - Единого землепользования.
        /// </summary>
        public string ParentCadastralNumbers { get; set; }

        /// <summary>
        /// Категория земель.
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Вид разрешенного использования.
        /// </summary>
        public string Utilization { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Сведения о величине кадастровой стоимости.
        /// </summary>
        public string CadastralCost { get; set; }
    }
}