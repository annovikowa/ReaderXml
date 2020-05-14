namespace ReaderXml.Models
{
    public class Zone : CadastralObject
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
        /// Вид зоны.
        /// </summary>
        public string TypeZone { get; set; }

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