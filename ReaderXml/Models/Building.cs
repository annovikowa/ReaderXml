namespace ReaderXml.Models
{
    public class Building : CadastralObject
    {
        /// <summary>
        /// Кадастровый номер.
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Площадь.
        /// </summary>
        public string Area { get; set; }
    }
}