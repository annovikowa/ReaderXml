namespace ConverterXlsxLibrary
{
    public abstract class CadastralObject
    {
        /// <summary>
        /// Наличие координат.
        /// </summary>
        public bool HasCoordinates { get; set; }
        /// <summary>
        /// Система координат.
        /// </summary>
        public string CoorSys { get; set; }
        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string Error { get; set; }
    }
}
