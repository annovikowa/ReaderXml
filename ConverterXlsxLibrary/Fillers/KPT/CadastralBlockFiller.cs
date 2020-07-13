using ConverterXlsxLibrary.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace ConverterXlsxLibrary.Fillers.KPT
{
    /// <summary>
    /// Кадастровый квартал.
    /// </summary>
    public class CadastralBlockFiller : IFiller<CadastralBlock>
    {
        private Dictionary<string, string> _coordSystem = new Dictionary<string, string>();

        public void Fill(CadastralBlock model, XmlReader reader)
        {
            try
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
                                    foreach (var er in model.Parcels)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "Building":
                                {
                                    AddNew(model.Buildings, reader.ReadSubtree(), model);
                                    foreach (var er in model.Buildings)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "Construction":
                                {
                                    AddNew(model.Constructions, reader.ReadSubtree(), model);
                                    foreach (var er in model.Constructions)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "Uncompleted":
                                {
                                    AddNew(model.Uncompleteds, reader.ReadSubtree(), model);
                                    foreach (var er in model.Uncompleteds)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "Bound":
                                {
                                    AddNew(model.Bounds, reader.ReadSubtree(), model);
                                    foreach (var er in model.Bounds)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                    
                                }
                                break;
                            case "Zone":
                                {
                                    AddNew(model.Zones, reader.ReadSubtree(), model);
                                    foreach (var er in model.Zones)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "OMSPoint":
                                {
                                    AddNew(model.OMSPoints, reader.ReadSubtree(), model);
                                    foreach (var er in model.OMSPoints)
                                    {
                                        model.Errors.Add(er.Error);
                                    }
                                }
                                break;
                            case "CoordSystems":
                                {
                                    _coordSystem = FillCoordSystems(reader.ReadSubtree(), model.Errors);
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
            catch (System.Exception ex)
            {
                model.Errors.Add(ex.Message);
            }       
        }

        /// <summary>
        /// Заполение словаря системами координат из кадастрового квартала.
        /// </summary>
        /// <param name="reader">XmlReader узла систем координат.</param>
        /// <returns></returns>
        private Dictionary<string, string> FillCoordSystems(XmlReader reader, ICollection<string> errors)
        {
            try
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
            catch (System.Exception ex)
            {

                errors.Add(ex.Message);
                return null;
            }
            
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
