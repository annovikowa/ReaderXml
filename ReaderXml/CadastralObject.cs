namespace ReaderXml
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
    }
}
