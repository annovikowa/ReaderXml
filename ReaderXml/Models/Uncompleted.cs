namespace ReaderXml.Models
{
    public class Uncompleted : CadastralObjectInBlock
    {
        /// <summary>
        /// Кадастровый номер.
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Основные характеристики ОНС.
        /// </summary>
        public string KeyParameters { get; set; }

        /// <summary>
        /// Вид объекта недвижимости.
        /// </summary>
        public string ObjectType { get; set; }

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