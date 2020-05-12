using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ReaderXml.KPTv10
{
    /// <summary>
    /// Кадастровый квартал.
    /// </summary>
    public class CadastralBlock : Abstract.CadastralBlock
    {
        private Dictionary<string, string> _сoordSystems;

        /// <summary>
        /// Инициализация нового экземпляра класса CadastralBlock.
        /// </summary>
        /// <param name="reader">XmlReader узла кадастрового квартала.</param>
        /// <param name="dictionary">Экзепляр словарей для перевода кодов в значения по схеме.</param>
        public CadastralBlock(XmlReader reader, XsdClassifiers dictionary)
        {
            Init(reader, dictionary);
        }

        public override void Init(XmlReader reader, XsdClassifiers dictionary)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "Total":
                            {
                                Area = $"{reader.ReadElementContentAsString()} Га";
                            }
                            break;
                        case "CadastralBlock":
                            {
                                reader.MoveToAttribute("CadastralNumber");
                                CadastralNumber = reader.Value;
                            }
                            break;
                        case "Parcel":
                            {
                                AddNew(Parcels, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Building":
                            {
                                AddNew(Buildings, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Construction":
                            {
                                AddNew(Constructions, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Uncompleted":
                            {
                                AddNew(Uncompleteds, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Bound":
                            {
                                AddNew(Bounds, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "Zone":
                            {
                                AddNew(Zones, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "OMSPoint":
                            {
                                AddNew(OMSPoints, reader.ReadSubtree(), dictionary);
                            }
                            break;
                        case "CoordSystems":
                            {
                                this._сoordSystems = FillCoordSystems(reader.ReadSubtree());
                            }
                            break;
                        case "Ordinate":
                            {
                                isCoordinates = true;
                            }
                            break;

                    }
                }
            }
            var allObjects = Parcels.Union<ICadastralObject>(Buildings).Union(Constructions).Union(Uncompleteds).Union(Bounds).Union(Zones);
            DefiningCoordinateSystem(allObjects);
        }

        /// <summary>
        /// Заполение словаря системами координат из кадастрового квартала.
        /// </summary>
        /// <param name="reader">XmlReader узла систем координат.</param>
        /// <returns></returns>
        private Dictionary<string, string> FillCoordSystems(XmlReader reader)
        {
            var dictionary = new Dictionary<string, string>();
            reader.ReadToDescendant("CoordSystems");
            while (reader.Read())
            {
                if (reader.LocalName == "CoordSystem")
                {
                    reader.MoveToAttribute("CsId").ToString();
                    string CsId = reader.Value;
                    reader.MoveToAttribute("Name");
                    dictionary.Add(CsId, reader.Value);
                }
            }
            return dictionary;
        }

        
        /// <summary>
        /// Заполнение свойства "Система координат" у каждого кадастрового объекта.
        /// </summary>
        /// <param name="objects">Перечисление кадастровых объектов.</param>
        private void DefiningCoordinateSystem(IEnumerable<ICadastralObject> objects)
        {
            foreach (var p in objects)
            {
                if (!String.IsNullOrEmpty(p.CoorSys))
                {
                    if (_сoordSystems.TryGetValue(p.CoorSys, out var entSys))
                        p.CoorSys = entSys;
                }
            }
        }
    }
}
