namespace ReaderXml.Models
{
    public class SurveyingProject : CadastralObjectInBlock
    {
        /// <summary>
        /// Учетный номер ПМТ.
        /// </summary>
        public string SurveyProjectNum { get; set; }

        /// <summary>
        /// Условный номер ЗУ.
        /// </summary>
        public string NominalNumber { get; set; }
    }
}