namespace ReaderXml.Models
{
    public class Construction : CadastralObject
    {
        /// <summary>
        /// Кадастровый номер.
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Кадастровый номер ЕНК.
        /// </summary>
        public string UnitedCadNumbers { get; set; }

        /// <summary>
        /// Основные характеристики сооружения.
        /// </summary>
        public string KeyParameters { get; set; }

        /// <summary>
        /// Вид объекта недвижимости.
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Вид разрешенного использования.
        /// </summary>
        public string PermittedUse { get; set; }

        /// <summary>
        /// Адрес.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Кадастровая стоимость.
        /// </summary>
        public string CadastralCost { get; set; }
    }
}