using System.Collections;
using System.Collections.Generic;

namespace ReaderXml.Models
{
    /// <summary>
    /// Кадастровый квартал.
    /// </summary>
    public class CadastralBlock : CadastralObject
    {
        /// <summary>
        /// Кадастровый номер.
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Площадь.
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Сведения о земельных участках
        /// </summary>
        public ICollection<Parcel> Parcels { get; } = new List<Parcel>();

        /// <summary>
        /// Здания
        /// </summary>
        public ICollection<Building> Buildings { get; } = new List<Building>();

        /// <summary>
        /// Сооружение
        /// </summary>
        public ICollection<Construction> Constructions { get; } = new List<Construction>();

        /// <summary>
        /// ОНС
        /// </summary>
        public ICollection<Uncompleted> Uncompleteds { get; } = new List<Uncompleted>();

        /// <summary>
        /// Границы между субъектами РФ, границы населенных пунктов, муниципальных образований, расположенных в кадастровом квартале
        /// </summary>
        public ICollection<Bound> Bounds { get; } = new List<Bound>();

        /// <summary>
        /// Зоны
        /// </summary>
        public ICollection<Zone> Zones { get; } = new List<Zone>();

        /// <summary>
        /// Сведения о проектах межевания.
        /// </summary>
        public ICollection<SurveyingProject> SurveyingProjects { get; } = new List<SurveyingProject>();

        /// <summary>
        /// Сведения о пунктах ОМС
        /// </summary>
        public ICollection<OMSPoint> OMSPoints { get; } = new List<OMSPoint>();
    }
}