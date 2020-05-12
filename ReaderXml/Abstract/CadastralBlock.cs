using ReaderXml.KPTv10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml.Abstract
{
    public abstract class CadastralBlock : CadastralObject
    {
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

        protected void AddNew<T>(ICollection<T> collection, XmlReader reader, XsdClassifiers dictionary = null) where T : class, ICadastralObject
        {
            //Рефлексия. Пока оставлю этот вариант, но подумаю над другими.
            //В целом использовать рефлексию не самый лучший подход, она медленная.
            //Возможно стоит сделать абстрактный класс фабрики, от неё пронаследовать 2 разных фабрики и держать их в соответствующих классах.
            var type = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().Where(x => x.BaseType == typeof(T)).FirstOrDefault(x => x.Namespace == this.GetType().Namespace);
            var obj = Activator.CreateInstance(type) as T;
            obj.Init(reader, dictionary);
            reader.Close();
            collection.Add(obj);
        }
    }
}
