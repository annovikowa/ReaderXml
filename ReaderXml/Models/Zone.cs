namespace ReaderXml.Models
{
    public class Zone : CadastralObject
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