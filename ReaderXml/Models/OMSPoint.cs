namespace ReaderXml.Models
{
    public class OMSPoint : CadastralObjectInBlock
    {
        /// <summary>
        /// Номер пункта опорной межевой сети на плане.
        /// </summary>
        public string PNmb { get; set; }

        /// <summary>
        /// Название и тип пункта.
        /// </summary>
        public string PName { get; set; }

        /// <summary>
        /// Класс геодезической сети.
        /// </summary>
        public string PKlass { get; set; }

        /// <summary>
        /// Координата Х.
        /// </summary>
        public decimal OrdX { get; set; }

        /// <summary>
        /// Координата У.
        /// </summary>
        public decimal OrdY { get; set; }
    }
}