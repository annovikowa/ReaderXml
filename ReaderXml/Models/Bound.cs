namespace ReaderXml.Models
{
    public class Bound : CadastralObjectInBlock
    {
        /// <summary>
        /// Учетный номер.
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Дата постановки на учет.
        /// </summary>
        public string RegistrationDate { get; set; }

        /// <summary>
        /// Вид границы.
        /// </summary>
        public string TypeBoundary { get; set; }

        /// <summary>
        /// Наименование.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дополнительная информация.
        /// </summary>
        public string AdditionalInformation { get; set; }
    }
}