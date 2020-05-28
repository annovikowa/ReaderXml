using ReaderXml.Fillers;
using ReaderXml.Fillers.KPT;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ReaderXml.KPT
{
    /// <summary>
    /// Кадастровый квартал.
    /// </summary>
    public class CadastralBlockFiller : IFiller<CadastralBlock>
    {
        private Dictionary<string, string> _coordSystem = new Dictionary<string, string>();

        public void Fill(CadastralBlock model, XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "Total":
                            {
                                model.Area = $"{reader.ReadElementContentAsString()} Га";
                            }
                            break;
                        case "CadastralBlock":
                            {
                                reader.MoveToAttribute("CadastralNumber");
                                model.CadastralNumber = reader.Value;
                            }
                            break;
                        case "Parcel":
                            {
                                AddNew(model.Parcels, reader.ReadSubtree(), model);
                            }
                            break;
                        case "Building":
                            {
                                AddNew(model.Buildings, reader.ReadSubtree(), model);
                            }
                            break;
                        case "Construction":
                            {
                                AddNew(model.Constructions, reader.ReadSubtree(), model);
                            }
                            break;
                        case "Uncompleted":
                            {
                                AddNew(model.Uncompleteds, reader.ReadSubtree(), model);
                            }
                            break;
                        case "Bound":
                            {
                                AddNew(model.Bounds, reader.ReadSubtree(), model);
                            }
                            break;
                        case "Zone":
                            {
                                AddNew(model.Zones, reader.ReadSubtree(), model);
                            }
                            break;
                        case "OMSPoint":
                            {
                                AddNew(model.OMSPoints, reader.ReadSubtree(), model);
                            }
                            break;
                        case "CoordSystems":
                            {
                                _coordSystem = FillCoordSystems(reader.ReadSubtree());
                            }
                            break;
                        case "Ordinate":
                            {
                                model.HasCoordinates = true;
                            }
                            break;

                    }
                }
            }
            var allObjects = model.Parcels.Union<CadastralObject>(model.Buildings).Union(model.Constructions).Union(model.Uncompleteds).Union(model.Bounds).Union(model.Zones);
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
        private void AddNew<T>(ICollection<T> collection, XmlReader reader, CadastralBlock model) where T : CadastralObjectInBlock, new()
        {
            var obj = new T();
            obj.SetCadastralBlock(model);
            var filler = KPTFillerFactory.GetFiller(obj);
            if (filler == null)
            {
                return;
            }
            filler.Fill(obj, reader);
            reader.Close();
            collection.Add(obj);
        }
        
        /// <summary>
        /// Заполнение свойства "Система координат" у каждого кадастрового объекта.
        /// </summary>
        /// <param name="objects">Перечисление кадастровых объектов.</param>
        private void DefiningCoordinateSystem(IEnumerable<CadastralObject> objects)
        {
            foreach (var p in objects)
            {
                if (!string.IsNullOrEmpty(p.CoorSys))
                {
                    if (_coordSystem.TryGetValue(p.CoorSys, out var entSys))
                        p.CoorSys = entSys;
                }
            }
        }
    }
}
