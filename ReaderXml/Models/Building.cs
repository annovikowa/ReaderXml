namespace ReaderXml.Models
{
    public class Building : CadastralObjectInBlock
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
        /// Площадь.
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Назначение здания.
        /// </summary>
        public string ObjectType { get; set; }

        // <summary>
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