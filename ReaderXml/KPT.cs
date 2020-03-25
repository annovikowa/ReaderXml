using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace ReaderXml
{
    public class KPT : CadastralObject
    {
        #region
        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Наименование органа кадастрового учета
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Должностное лицо
        /// </summary>
        public string Official { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Сведения о земельных участках
        /// </summary>
        public List<Parcel> Parcels { get; } = new List<Parcel>();

        /// <summary>
        /// Здания
        /// </summary>
        public List<Building> Buildings { get; } = new List<Building>();

        /// <summary>
        /// Сооружение
        /// </summary>
        public List<Construction> Constructions { get; } = new List<Construction>();

        /// <summary>
        /// ОНС
        /// </summary>
        public List<Uncompleted> Uncompleteds { get; } = new List<Uncompleted>();

        /// <summary>
        /// Границы между субъектами РФ, границы населенных пунктов, муниципальных образований, расположенных в кадастровом квартале
        /// </summary>
        public List<Bound> Bounds { get; } = new List<Bound>();

        /// <summary>
        /// Зоны
        /// </summary>
        public List<Zone> Zones { get; } = new List<Zone>();

        /// <summary>
        /// Сведения о пунктах ОМС
        /// </summary>
        public List<OMSPoint> OMSPoints { get; } = new List<OMSPoint>();
        #endregion
        public KPT(string fileName)
        {
            var dictionary = FillDictionary(fileName);
            
            using (var reader = XmlReader.Create(fileName, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.LocalName)
                        {
                            case "Total":
                                {
                                    Area = reader.ReadElementContentAsString() + "Га";
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
                                    var inner = reader.ReadSubtree();
                                    Parcel p = new Parcel(inner, dictionary);
                                    Parcels.Add(p);
                                    inner.Close();
                                }
                                break;
                            case "Building":
                                {
                                    var inner = reader.ReadSubtree();
                                    Building b = new Building(inner, dictionary);
                                    Buildings.Add(b);
                                    inner.Close();
                                }
                                break;
                            case "Construction":
                                {
                                    var inner = reader.ReadSubtree();
                                    Construction c = new Construction(inner, dictionary);
                                    Constructions.Add(c);
                                    inner.Close();
                                }
                                break;
                            case "Uncompleted":
                                {
                                    var inner = reader.ReadSubtree();
                                    Uncompleted u = new Uncompleted(inner, dictionary);
                                    Uncompleteds.Add(u);
                                    inner.Close();
                                }
                                break;
                            case "Bound":
                                {
                                    var inner = reader.ReadSubtree();
                                    Bound b = new Bound(inner, dictionary);
                                    Bounds.Add(b);
                                    inner.Close();
                                }
                                break;
                            case "Zone":
                                {
                                    var inner = reader.ReadSubtree();
                                    Zone z = new Zone(inner, dictionary);
                                    Zones.Add(z);
                                    inner.Close();
                                }
                                break;
                            case "OMSPoint":
                                {
                                    var inner = reader.ReadSubtree();
                                    OMSPoint oms = new OMSPoint(inner);
                                    OMSPoints.Add(oms);
                                    inner.Close();
                                }
                                break;
                            case "Organization":
                                {
                                    Organization = reader.ReadElementContentAsString();
                                }
                                break;
                            case "Ordinate":
                                {
                                    isCoordinates = true;
                                }
                                break;
                            case "Date":
                                {
                                    Date = reader.ReadElementContentAsDateTime();
                                }
                                break;
                            case "Number":
                                {
                                    Number = reader.ReadElementContentAsString();
                                }
                                break;
                            case "Appointment":
                                {
                                    Official += reader.ReadElementContentAsString() + ", ";
                                }
                                break;
                            case "FamilyName":
                                {
                                    Official += reader.ReadElementContentAsString() + " ";
                                }
                                break;
                            case "FirstName":
                                {
                                    Official += reader.ReadElementContentAsString() + " ";
                                }
                                break;
                            case "Patronymic":
                                {
                                    Official += reader.ReadElementContentAsString() + " ";
                                }
                                break;
                        }
                    }
                }
            }
        }

        private Dictionary FillDictionary(string fileName)
        {
            Dictionary dictionary = new Dictionary();
            var nsManager = new XmlNamespaceManager(new NameTable());
            nsManager.AddNamespace("xs", "http://www.w3.org/2001/XMLSchema");

            var xsdAddressRegion = XElement.Load(@"D:\ReaderXml\ReaderXml\КПТ\Схемы\KPT_v10\SchemaCommon\dRegionsRF_v01.xsd");
            dictionary.AddressRegion = xsdAddressRegion.XPathSelectElements(".//xs:enumeration", nsManager)
                .ToDictionary(x => x.XPathEvaluate("string(@value)").ToString(),
                x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());

            var xsdParcelsName = XElement.Load(@"D:\ReaderXml\ReaderXml\КПТ\Схемы\KPT_v10\SchemaCommon\dParcels_v01.xsd");
            dictionary.ParcelsName = xsdParcelsName.XPathSelectElements(".//xs:enumeration", nsManager)
                .ToDictionary(x => x.XPathEvaluate("string(@value)").ToString(),
                x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());

            var xsdParcelsCategory = XElement.Load(@"D:\ReaderXml\ReaderXml\КПТ\Схемы\KPT_v10\SchemaCommon\dCategories_v01.xsd");
            dictionary.ParcelsCategory = xsdParcelsCategory.XPathSelectElements(".//xs:enumeration", nsManager)
                .ToDictionary(x => x.XPathEvaluate("string(@value)").ToString(),
                x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());

            var xsdParcelsUtilization = XElement.Load(@"D:\ReaderXml\ReaderXml\КПТ\Схемы\KPT_v10\SchemaCommon\dUtilizations_v01.xsd");
            dictionary.ParcelsUtilization = xsdParcelsUtilization.XPathSelectElements(".//xs:enumeration", nsManager)
                .ToDictionary(x => x.XPathEvaluate("string(@value)").ToString(),
                x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());

            var xsdObjectType = XElement.Load(@"D:\ReaderXml\ReaderXml\КПТ\Схемы\KPT_v10\SchemaCommon\dRealty_v03.xsd");
            dictionary.ObjectType = xsdObjectType.XPathSelectElements(".//xs:enumeration", nsManager)
                .ToDictionary(x => x.XPathEvaluate("string(@value)").ToString(),
                x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());

            var xsdKeyParameters = XElement.Load(@"D:\ReaderXml\ReaderXml\КПТ\Схемы\KPT_v10\SchemaCommon\dTypeParameter_v01.xsd");
            dictionary.KeyParameters = xsdKeyParameters.XPathSelectElements(".//xs:enumeration", nsManager)
                .ToDictionary(x => x.XPathEvaluate("string(@value)").ToString(),
                x => x.XPathEvaluate("string(./xs:annotation/xs:documentation)", nsManager).ToString());

            using (var reader = XmlReader.Create(fileName, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
            {
                reader.ReadToDescendant("CoordSystems");
                while (reader.Read())
                {
                    if (reader.LocalName == "CoordSystem")
                    {
                        string CsId = "";
                        reader.MoveToAttribute("CsId").ToString();
                        CsId = reader.Value;
                        reader.MoveToAttribute("Name");
                        dictionary.CoordSystems.Add(CsId, reader.Value);
                    }
                }
            }
            return dictionary;
        }
    }

    public abstract class CadastralObject
    {
        /// <summary>
        /// Кадастровый номер замельного участка
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Площадь земельного участка
        /// </summary>
        public string Area { get; set; }
    }

    public class Address
    {
        #region
        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Код региона
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Городской район
        /// </summary>
        public string UrbanDistrict { get; set; }

        /// <summary>
        /// Сельсовет
        /// </summary>
        public string SovietVillage { get; set; }

        /// <summary>
        /// Населённый пункт
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public string Street { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public string Level1 { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public string Level2 { get; set; }

        /// <summary>
        /// Строение
        /// </summary>
        public string Level3 { get; set; }

        /// <summary>
        /// Квартира
        /// </summary>
        public string Apartment { get; set; }

        /// <summary>
        /// Иное
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// Неформализованное описание
        /// </summary>
        public string Note { get; set; }
        #endregion

        public Address(XmlReader reader, Dictionary<string, string> dictionary)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "PostalCode":
                            {
                                PostalCode = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Region":
                            {
                                string region = "";
                                if (dictionary.TryGetValue(reader.ReadElementContentAsString(), out region))
                                    Region = region;
                            }
                            break;
                        case "District":
                            {
                                reader.MoveToAttribute("Type");
                                District = reader.Value + " ";
                                reader.MoveToAttribute("Name");
                                District += reader.Value;
                            }
                            break;
                        case "City":
                            {
                                reader.MoveToAttribute("Type");
                                City = reader.Value + " ";
                                reader.MoveToAttribute("Name");
                                City += reader.Value;
                            }
                            break;
                        case "UrbanDistrict":
                            {
                                reader.MoveToAttribute("Type");
                                UrbanDistrict = reader.Value + " ";
                                reader.MoveToAttribute("Name");
                                UrbanDistrict += reader.Value;
                            }
                            break;
                        case "SovietVillage":
                            {
                                reader.MoveToAttribute("Type");
                                SovietVillage = reader.Value + " ";
                                reader.MoveToAttribute("Name");
                                SovietVillage += reader.Value;
                            }
                            break;
                        case "Locality":
                            {
                                reader.MoveToAttribute("Type");
                                Locality = reader.Value + " ";
                                reader.MoveToAttribute("Name");
                                Locality += reader.Value;
                            }
                            break;
                        case "Street":
                            {
                                reader.MoveToAttribute("Type");
                                Street = reader.Value + " ";
                                reader.MoveToAttribute("Name");
                                Street += reader.Value;
                            }
                            break;
                        case "Level1":
                            {
                                reader.MoveToAttribute("Type");
                                Level1 = reader.Value + " ";
                                reader.MoveToAttribute("Value");
                                Level1 += reader.Value;
                            }
                            break;
                        case "Level2":
                            {
                                reader.MoveToAttribute("Type");
                                Level2 = reader.Value + " ";
                                reader.MoveToAttribute("Value");
                                Level2 += reader.Value;
                            }
                            break;
                        case "Level3":
                            {
                                reader.MoveToAttribute("Type");
                                Level3 = reader.Value + " ";
                                reader.MoveToAttribute("Value");
                                Level3 += reader.Value;
                            }
                            break;
                        case "Apartment":
                            {
                                reader.MoveToAttribute("Type");
                                Apartment = reader.Value + " ";
                                reader.MoveToAttribute("Value");
                                Apartment += reader.Value;
                            }
                            break;
                        case "Other":
                            {
                                Other = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Note":
                            {
                                Note = reader.ReadElementContentAsString();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Parcel : CadastralObject
    {
        #region Свойства

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }

        /// <summary>
        /// Наименование участка
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Кадастровый номер земельного участка - Единого землепользования
        /// </summary>
        public string ParentCadastralNumbers { get; set; }

        /// <summary>
        /// Категория земель
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Вид разрешенного использования
        /// </summary>
        public string Utilization { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Сведения о величине кадастровой стоимости
        /// </summary>
        public string CadastralCost { get; set; }

        #endregion
        public Parcel(XmlReader reader, Dictionary dictionary)
        {
            reader.Read();
            #region Присваиваем атрибуты Parcel
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "CadastralNumber":
                        CadastralNumber = reader.Value;
                        break;
                    default:
                        break;
                }
            }

            #endregion
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "Area":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("Area");
                                Area = reader.ReadElementContentAsString() + "кв. м.";
                            }
                            break;
                        case "Name":
                            {
                                string name = "";
                                if (dictionary.ParcelsName.TryGetValue(reader.ReadElementContentAsString(), out name))
                                    Name = name;
                            }
                            break;
                        case "Location":
                            {
                                Location l = new Location(reader.ReadSubtree(), dictionary.AddressRegion);
                                Address = l.GetAddress(true);
                            }
                            break;
                        case "Category":
                            {
                                string category = "";
                                if (dictionary.ParcelsCategory.TryGetValue(reader.ReadElementContentAsString(), out category))
                                    Category = category;
                            }
                            break;
                        case "Utilization":
                            {
                                if (reader.MoveToAttribute("LandUse"))
                                    Utilization = reader.Value;

                                if (Utilization == "" || Utilization == null || Utilization == "-")
                                {
                                    reader.MoveToAttribute("ByDoc");
                                    Utilization = reader.Value;
                                }

                                if (Utilization == "" || Utilization == null || Utilization == "-")
                                {
                                    reader.MoveToAttribute("Utilization");
                                    Utilization = reader.Value;
                                }
                            }
                            break;
                        case "ParentCadastralNumbers":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("CadastralNumber");
                                ParentCadastralNumbers = reader.ReadElementContentAsString();
                            }
                            break;
                        case "CadastralCost":
                            {
                                reader.MoveToAttribute("Value");
                                CadastralCost = Convert.ToString(reader.Value) + " руб.";
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                string entSys = "";
                                if (dictionary.CoordSystems.TryGetValue(Convert.ToString(reader.Value), out entSys))
                                    EntSys = entSys;
                            }
                            break;
                        case "Ordinate":
                            isCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Location
    {
        /// <summary>
        /// В границах
        /// </summary>
        public string inBounds { get; set; }

        /// <summary>
        /// Положение ДКК
        /// </summary>
        public string Placed { get; set; }

        /// <summary>
        /// Уточнение местоположения
        /// </summary>
        public Elaboration Elaboration { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public Address Address { get; set; }

        public Location(XmlReader reader, Dictionary<string, string> dictionary)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "inBounds":
                            {
                                inBounds = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Placed":
                            {
                                Placed = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Elaboration":
                            {
                                Elaboration e = new Elaboration(reader.ReadSubtree());
                                Elaboration = e;
                            }
                            break;
                        case "Address":
                            {
                                Address a = new Address(reader.ReadSubtree(), dictionary);
                                Address = a;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

        }

        public string GetAddress(bool isParcel)
        {
            string address;
            if (isParcel == true)
            {
                if (inBounds == "" || inBounds == "2")
                {
                    address = Address.Note != null || Address.Note != "" ? Address.Note : OutputAddress();
                }
                else
                {
                    address = inBounds == "1" ? OutputAddress(true) : OutputAddress(false);
                }
            }
            else
                address = OutputAddress();
            return address;

        }
        private string OutputAddress()
        {
            return $"{(Address.PostalCode == null || Address.PostalCode == "" ? "" : $"{Address.PostalCode}, ")}" +
                        $"{(Address.Region == null || Address.Region == "" ? "" : $"{Address.Region}, ")}" +
                        $"{(Address.District == null || Address.District == "" ? "" : $"{Address.District}, ")}" +
                        $"{(Address.City == null || Address.City == "" ? "" : $"{Address.City}, ")}" +
                        $"{(Address.UrbanDistrict == null || Address.UrbanDistrict == "" ? "" : $"{Address.UrbanDistrict}, ")}" +
                        $"{(Address.SovietVillage == null || Address.SovietVillage == "" ? "" : $"{Address.SovietVillage}, ")}" +
                        $"{(Address.Locality == null || Address.Locality == "" ? "" : $"{Address.Locality}, ")}" +
                        $"{(Address.Street == null || Address.Street == "" ? "" : $"{Address.Street}, ")}" +
                        $"{(Address.Level1 == null || Address.Level1 == "" ? "" : $"{Address.Level1}, ")}" +
                        $"{(Address.Level2 == null || Address.Level2 == "" ? "" : $"{Address.Level2}, ")}" +
                        $"{(Address.Level3 == null || Address.Level3 == "" ? "" : $"{Address.Level3}, ")}" +
                        $"{(Address.Apartment == null || Address.Apartment == "" ? "" : $"{Address.Apartment}, ")}" +
                        $"{(Address.Other == null || Address.Other == "" ? "" : $"{Address.Other}, ")}";
        }
        private string OutputAddress(bool inBounds)
        {
            return $"Местоположение установлено относительно ориентира, расположенного {(inBounds == true ? "в границах участка. " : "за пределами участка. ")} " +
                        $"Ориентир {(Elaboration == null ? "-" : Elaboration.ReferenceMark)}. " +
                        $"Участок находится примерно в {(Elaboration == null ? "-" : Elaboration.Distance)} " +
                        $"от ориентира по направлению на {(Elaboration == null ? "-" : Elaboration.Direction)}. " +
                        $"Почтовый адрес ориентира: {(Address.Note != null || Address.Note != "" ? Address.Note : OutputAddress())}";
        }
    }

    public class Elaboration
    {
        /// <summary>
        /// Наименование ориентира
        /// </summary>
        public string ReferenceMark { get; set; }

        /// <summary>
        /// Расстояние
        /// </summary>
        public string Distance { get; set; }

        /// <summary>
        /// Направление
        /// </summary>
        public string Direction { get; set; }

        public Elaboration(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "ReferenceMark":
                            {
                                ReferenceMark = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Distance":
                            {
                                Distance = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Direction":
                            {
                                Direction = reader.ReadElementContentAsString();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Building : CadastralObject
    {
        #region Свойства

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }

        /// <summary>
        /// Назначение здания
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion
        public Building(XmlReader reader, Dictionary dictionary)
        {
            reader.Read();
            #region Присваиваем атрибуты Building
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "CadastralNumber":
                        CadastralNumber = reader.Value;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "Area":
                            {
                                Area = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ObjectType":
                            {
                                string objectType = "";
                                if (dictionary.ObjectType.TryGetValue(reader.ReadElementContentAsString(), out objectType))
                                    ObjectType = objectType;
                            }
                            break;
                        case "Address":
                            {
                                Location l = new Location(reader.ReadSubtree(), dictionary.AddressRegion);
                                Address = l.GetAddress(false);
                            }
                            break;
                        case "CadastralCost":
                            {
                                reader.MoveToAttribute("Value");
                                CadastralCost = Convert.ToString(reader.Value) + " руб.";
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                string entSys = "";
                                if (dictionary.CoordSystems.TryGetValue(Convert.ToString(reader.Value), out entSys))
                                    EntSys = entSys;
                            }
                            break;
                        case "Ordinate":
                            isCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Construction
    {
        #region Свойства
        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }

        /// <summary>
        /// Основные характеристики сооружения
        /// </summary>
        public string KeyParameters { get; set; }

        /// <summary>
        /// Вид объекта недвижимости
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion
        public Construction(XmlReader reader, Dictionary dictionary)
        {
            reader.Read();
            #region Присваиваем атрибуты Construction
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "CadastralNumber":
                        CadastralNumber = reader.Value;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "ObjectType":
                            {
                                string objectType = "";
                                if (dictionary.ObjectType.TryGetValue(reader.ReadElementContentAsString(), out objectType))
                                    ObjectType = objectType;
                            }
                            break;
                        case "Address":
                            {
                                Location l = new Location(reader.ReadSubtree(), dictionary.AddressRegion);
                                Address = l.GetAddress(false);
                            }
                            break;
                        case "CadastralCost":
                            {
                                reader.MoveToAttribute("Value");
                                CadastralCost = Convert.ToString(reader.Value) + " руб.";
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                string entSys = "";
                                if (dictionary.CoordSystems.TryGetValue(Convert.ToString(reader.Value), out entSys))
                                    EntSys = entSys;
                            }
                            break;
                        case "Ordinate":
                            isCoordinates = true;
                            break;
                        case "KeyParameter":
                            {
                                string keyParamenter = "";
                                reader.MoveToAttribute("Type");
                                if (dictionary.KeyParameters.TryGetValue(reader.Value, out keyParamenter))
                                {
                                    
                                    if (reader.Value == "05" || reader.Value == "06")
                                    {
                                        reader.MoveToAttribute("Value");
                                        KeyParameters = keyParamenter + ": " + reader.Value + " кв.м.";
                                    }
                                    else if(reader.Value == "03")
                                    {
                                        reader.MoveToAttribute("Value");
                                        KeyParameters = keyParamenter + ": " + reader.Value + " куб.м.";
                                    }
                                    else
                                    {
                                        reader.MoveToAttribute("Value");
                                        KeyParameters = keyParamenter + ": " + reader.Value + " м.";
                                    }
                                }                                
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Uncompleted
    {
        #region
        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public string CadastralNumber { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }

        /// <summary>
        /// Основные характеристики ОНС
        /// </summary>
        public string KeyParameters { get; set; }

        /// <summary>
        /// Вид объекта недвижимости
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion

        public Uncompleted(XmlReader reader, Dictionary dictionary)
        {
            reader.Read();
            #region Присваиваем атрибуты Construction
            while (reader.MoveToNextAttribute())
            {
                switch (reader.Name)
                {
                    case "CadastralNumber":
                        CadastralNumber = reader.Value;
                        break;
                    default:
                        break;
                }
            }
            #endregion
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "ObjectType":
                            {
                                string objectType = "";
                                if (dictionary.ObjectType.TryGetValue(reader.ReadElementContentAsString(), out objectType))
                                    ObjectType = objectType;
                            }
                            break;
                        case "Address":
                            {
                                Location l = new Location(reader.ReadSubtree(), dictionary.AddressRegion);
                                Address = l.GetAddress(false);
                            }
                            break;
                        case "CadastralCost":
                            {
                                reader.MoveToAttribute("Value");
                                CadastralCost = Convert.ToString(reader.Value) + " руб.";
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                string entSys = "";
                                if (dictionary.CoordSystems.TryGetValue(Convert.ToString(reader.Value), out entSys))
                                    EntSys = entSys;
                            }
                            break;
                        case "Ordinate":
                            isCoordinates = true;
                            break;
                        case "KeyParameter":
                            {
                                string keyParamenter = "";
                                reader.MoveToAttribute("Type");
                                if (dictionary.KeyParameters.TryGetValue(reader.Value, out keyParamenter))
                                {

                                    if (reader.Value == "05" || reader.Value == "06")
                                    {
                                        reader.MoveToAttribute("Value");
                                        KeyParameters = keyParamenter + ": " + reader.Value + " кв.м.";
                                    }
                                    else if (reader.Value == "03")
                                    {
                                        reader.MoveToAttribute("Value");
                                        KeyParameters = keyParamenter + ": " + reader.Value + " куб.м.";
                                    }
                                    else
                                    {
                                        reader.MoveToAttribute("Value");
                                        KeyParameters = keyParamenter + ": " + reader.Value + " м.";
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Bound
    {
        #region
        /// <summary>
        /// Учетный номер
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Вид границы
        /// </summary>
        public string TypeBoundary { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public string AdditionalInformation { get; set; }
        #endregion

        public Bound(XmlReader reader, Dictionary dictionary)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "AccountNumber":
                            {
                                AccountNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "NameNeighbours":
                            {
                                TypeBoundary = "Граница между субъектами Российской Федерации";
                                if (AdditionalInformation != null || AdditionalInformation != "")
                                    AdditionalInformation += reader.ReadElementContentAsString();
                                else
                                    AdditionalInformation += ", " + reader.ReadElementContentAsString();
                            }
                            break;
                        case "MunicipalBoundary":
                            {
                                TypeBoundary = "Граница муниципального образования";
                                reader.MoveToContent();
                                reader.ReadToDescendant("Name");
                                if (AdditionalInformation != null || AdditionalInformation != "")
                                    AdditionalInformation += reader.ReadElementContentAsString();
                                else
                                    AdditionalInformation += ", " + reader.ReadElementContentAsString();
                            }
                            break;
                        case "InhabitedLocalityBoundary":
                            {
                                TypeBoundary = "Граница населенного пункта";
                                reader.MoveToContent();
                                reader.ReadToDescendant("Name");
                                if (AdditionalInformation != null || AdditionalInformation != "")
                                    AdditionalInformation += reader.ReadElementContentAsString();
                                else
                                    AdditionalInformation += ", " + reader.ReadElementContentAsString();
                            }
                            break;
                        case "Description":
                            {
                                Description = reader.ReadElementContentAsString();
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                string entSys = "";
                                if (dictionary.CoordSystems.TryGetValue(Convert.ToString(reader.Value), out entSys))
                                    EntSys = entSys;
                            }
                            break;
                        case "Ordinate":
                            isCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    //Дополнительная информация. PermittedUse не понятно 
    public class Zone
    {
        #region
        /// <summary>
        /// Учетный номер
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Вид зоны
        /// </summary>
        public string TypeZone { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public string AdditionalInformation { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }
        #endregion

        public Zone(XmlReader reader, Dictionary dictionary)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "AccountNumber":
                            {
                                AccountNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Description":
                            {
                                Description = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ContentRestrictions":
                            {
                                AdditionalInformation = reader.ReadElementContentAsString();
                            }
                            break;
                        case "TerritorialZone":
                            {
                                TypeZone = "Территориальная зона";
                            }
                            break;
                        case "SpecialZone":
                            {
                                TypeZone = "Зона с особыми условиями использования территорий";
                            }
                            break;
                        //case "PermittedUse":
                        //    {
                        //        
                        //    }
                        //    break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                string entSys = "";
                                if (dictionary.CoordSystems.TryGetValue(Convert.ToString(reader.Value), out entSys))
                                    EntSys = entSys;
                            }
                            break;
                        case "Ordinate":
                            isCoordinates = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class OMSPoint
    {
        #region
        /// <summary>
        /// Номер пункта опорной межевой сети на плане
        /// </summary>
        public string PNmb { get; set; }

        /// <summary>
        /// Название и тип пункта
        /// </summary>
        public string PName { get; set; }

        /// <summary>
        /// Класс геодезической сети
        /// </summary>
        public string PKlass { get; set; }

        /// <summary>
        /// Координата Х
        /// </summary>
        public decimal OrdX { get; set; }

        /// <summary>
        /// Координата У
        /// </summary>
        public decimal OrdY { get; set; }
        #endregion

        public OMSPoint(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.LocalName)
                    {
                        case "PNmb":
                            {
                                PNmb = reader.ReadElementContentAsString();
                            }
                            break;
                        case "PName":
                            {
                                PName = reader.ReadElementContentAsString();
                            }
                            break;
                        case "PKlass":
                            {
                                PKlass = reader.ReadElementContentAsString();
                            }
                            break;
                        case "OrdX":
                            {
                                OrdX = reader.ReadElementContentAsDecimal();
                            }
                            break;
                        case "OrdY":
                            {
                                OrdY = reader.ReadElementContentAsDecimal();
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Dictionary
    {
        public Dictionary<string, string> AddressRegion { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ParcelsName { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ParcelsCategory { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ParcelsUtilization { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> ObjectType { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> KeyParameters { get; set; } = new Dictionary<string, string>();
        public Dictionary<string, string> CoordSystems { get; set; } = new Dictionary<string, string>();
    }
}
