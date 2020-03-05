using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ReaderXml
{
    public class KPT : CadastralObject
    {
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
        /// Сведения о земельных участках
        /// </summary>
        public List<Parcel> Parcels
        {
            get
            {
                if (_Parcels == null)
                    _Parcels = new List<Parcel>();
                return _Parcels;
            }
            set
            {
                _Parcels = value;
            }
        }

        /// <summary>
        /// Здания
        /// </summary>
        public List<Building> Buildings
        {
            get
            {
                if (_Buildings == null)
                    _Buildings = new List<Building>();
                return _Buildings;
            }
            set
            {
                _Buildings = value;
            }
        }

        /// <summary>
        /// Сооружение
        /// </summary>
        public List<Construction> Constructions
        {
            get
            {
                if (_Constructions == null)
                    _Constructions = new List<Construction>();
                return _Constructions;
            }
            set
            {
                _Constructions = value;
            }
        }

        /// <summary>
        /// ОНС
        /// </summary>
        public List<Uncompleted> Uncompleteds
        {
            get
            {
                if (_Uncompleted == null)
                    _Uncompleted = new List<Uncompleted>();
                return _Uncompleted;
            }
            set
            {
                _Uncompleted = value;
            }
        }

        /// <summary>
        /// Границы между субъектами РФ, границы населенных пунктов, муниципальных образований, расположенных в кадастровом квартале
        /// </summary>
        public List<Bound> Bounds
        {
            get
            {
                if (_Bounds == null)
                    _Bounds = new List<Bound>();
                return _Bounds;
            }
            set
            {
                _Bounds = value;
            }
        }

        /// <summary>
        /// Зоны
        /// </summary>
        public List<Zone> Zones
        {
            get
            {
                if (_Zones == null)
                    _Zones = new List<Zone>();
                return _Zones;
            }
            set
            {
                _Zones = value;
            }
        }

        /// <summary>
        /// Сведения о пунктах ОМС
        /// </summary>
        public List<OMSPoint> OMSPoints
        {
            get
            {
                if (_OMSPoint == null)
                    _OMSPoint = new List<OMSPoint>();
                return _OMSPoint;
            }
            set
            {
                _OMSPoint = value;
            }
        }

        private List<Parcel> _Parcels;
        private List<Building> _Buildings;
        private List<Construction> _Constructions;
        private List<Uncompleted> _Uncompleted;
        private List<Bound> _Bounds;
        private List<Zone> _Zones;
        private List<OMSPoint> _OMSPoint;
        public KPT(string fileName)
        {
            using (var reader = XmlReader.Create(fileName, new XmlReaderSettings() { DtdProcessing = DtdProcessing.Parse }))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        switch (reader.Name)
                        {
                            case "Total":
                                {
                                    Area = reader.ReadElementContentAsString();
                                }
                                break;
                            case "Unit":
                                {
                                    Unit = reader.ReadElementContentAsString();
                                }
                                break;
                            case "CadastralBlock":
                                {
                                    reader.MoveToAttribute(0);
                                    CadastralNumber = reader.Value;
                                }
                                break;
                            case "Parcel":
                                {
                                    var inner = reader.ReadSubtree();
                                    Parcel p = new Parcel(inner);
                                    Parcels.Add(p);
                                    inner.Close();
                                }
                                break;
                            case "Building":
                                {
                                    var inner = reader.ReadSubtree();
                                    Building b = new Building(inner);
                                    Buildings.Add(b);
                                    inner.Close();
                                }
                                break;
                            case "Construction":
                                {
                                    var inner = reader.ReadSubtree();
                                    Construction c = new Construction(inner);
                                    Constructions.Add(c);
                                    inner.Close();
                                }
                                break;
                            case "Uncompleted":
                                {
                                    var inner = reader.ReadSubtree();
                                    Uncompleted u = new Uncompleted(inner);
                                    Uncompleteds.Add(u);
                                    inner.Close();
                                }
                                break;
                            case "Bound":
                                {
                                    var inner = reader.ReadSubtree();
                                    Bound b = new Bound(inner);
                                    Bounds.Add(b);
                                    inner.Close();
                                }
                                break;
                            case "Zone":
                                {
                                    var inner = reader.ReadSubtree();
                                    Zone z = new Zone(inner);
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
                            case "ns6:Organization":
                                {
                                    Organization = reader.ReadElementContentAsString();
                                }
                                break;
                            case "ns6:Date":
                                {
                                    Date = reader.ReadElementContentAsDateTime();
                                }
                                break;
                            case "ns6:Number":
                                {
                                    Number = reader.ReadElementContentAsString();
                                }
                                break;
                            case "ns6:Appointment":
                                {
                                    Official += reader.ReadElementContentAsString() + ", ";
                                }
                                break;
                            case "ns7:FamilyName":
                                {
                                    Official += reader.ReadElementContentAsString() + " ";
                                }
                                break;
                            case "ns7:FirstName":
                                {
                                    Official += reader.ReadElementContentAsString() + " ";
                                }
                                break;
                            case "ns7:Patronymic":
                                {
                                    Official += reader.ReadElementContentAsString() + " ";
                                }
                                break;
                        }
                    }
                }
            }
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

        /// <summary>
        /// Единица измерения - квадратный метр
        /// </summary>
        public string Unit { get; set; }
    }

    public abstract class Address
    {
        public string AddressOrLocation { get; set; }
        /// <summary>
        /// ОКАТО
        /// </summary>
        public string OKATO { get; set; }

        /// <summary>
        /// КЛАДР
        /// </summary>
        public string KLADR { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public string OKTMO { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Код региона
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Наименование района
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// Тип района
        /// </summary>
        public string DistrictType { get; set; }

        /// <summary>
        /// Наименование муниципального образования
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Тип муниципального образования
        /// </summary>
        public string CityType { get; set; }

        /// <summary>
        /// Наименование городского района
        /// </summary>
        public string UrbanDistrictName { get; set; }

        /// <summary>
        /// Тип городского района
        /// </summary>
        public string UrbanDistrictType { get; set; }

        /// <summary>
        /// Наименование сельсовета
        /// </summary>
        public string SovietVillageName { get; set; }

        /// <summary>
        /// Тип сельсовета
        /// </summary>
        public string SovietVillageType { get; set; }

        /// <summary>
        /// Наименование населённого пункта
        /// </summary>
        public string LocalityName { get; set; }

        /// <summary>
        /// Тип населённого пункта
        /// </summary>
        public string LocalityType { get; set; }

        /// <summary>
        /// Наименование улицы
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// Тип улицы
        /// </summary>
        public string StreetType { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public string Level1Type { get; set; }

        /// <summary>
        /// Значение дома
        /// </summary>
        public string Level1Value { get; set; }

        /// <summary>
        /// Тип корпуса
        /// </summary>
        public string Level2Type { get; set; }

        /// <summary>
        /// Значение корпуса
        /// </summary>
        public string Level2Value { get; set; }

        /// <summary>
        /// Тип строения
        /// </summary>
        public string Level3Type { get; set; }

        /// <summary>
        /// Значение строения
        /// </summary>
        public string Level3Value { get; set; }

        /// <summary>
        /// Тип квартиры
        /// </summary>
        public string ApartmentType { get; set; }

        /// <summary>
        /// Значение квартиры
        /// </summary>
        public string ApartmentValue { get; set; }

        /// <summary>
        /// Иное
        /// </summary>
        public string Other { get; set; }

        /// <summary>
        /// Неформализованное описание
        /// </summary>
        public string Note { get; set; }
    }

    // Вопрос по EntitySpatial, можно ли всё не извлекать, просто проверить наличие
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
        /// Уточнение местоположения и адрес (описание местоположения) земельного участка
        /// </summary>
        public Location Location;

        /// <summary>
        /// Сведения о величине кадастровой стоимости
        /// </summary>
        public string CadastralCost { get; set; }

        #endregion
        public Parcel(XmlReader reader)
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
                    switch (reader.Name)
                    {
                        case "Area":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("Area");
                                Area = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Unit":
                            {
                                Unit = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Name":
                            {
                                //Это должно красиво выглядеть
                                Name = reader.ReadElementContentAsString();
                            }
                            break;
                        case "Location":
                            {
                                var inner = reader.ReadSubtree();
                                Location l = new Location(inner);
                                Location = l;
                            }
                            break;
                        case "Category":
                            {
                                //Это должно красиво выглядеть
                                Category = reader.ReadElementContentAsString();
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
                                EntSys = Convert.ToString(reader.Value);
                                reader.MoveToContent();
                                if (reader.ReadToDescendant("ns3:Ordinate"))
                                    isCoordinates = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    // Следующие четыре класса, кроме Elaboration, доделать  Адрес  
    public class Location : Address
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
        public Elaboration Elaboration;

        // ---------------- ВОПРОС ПО АДРЕСУ -------------------
        public Location(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
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
                                var inner = reader.ReadSubtree();
                                Elaboration e = new Elaboration(reader);
                                Elaboration = e;
                            }
                            break;
                        case "Address":
                            {
                                //Как лучше извлекать?
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

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
                    switch (reader.Name)
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

    public class Building : Address
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
        /// Площадь
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// Назначение здания
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion
        public Building(XmlReader reader)
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
                    switch (reader.Name)
                    {
                        case "Area":
                            {
                                Area = reader.ReadElementContentAsString();
                            }
                            break;
                        case "ObjectType":
                            {
                                ObjectType = reader.ReadElementContentAsString();
                            }
                            break;
                        //------------- Как извлекать -----------------
                        case "Address":
                            {

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
                                EntSys = Convert.ToString(reader.Value);
                                //Как лучше извлекать?
                                reader.MoveToContent();
                                reader.ReadToDescendant("ns3:Ordinate");
                                isCoordinates = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Construction : Address
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
        /// Основные характеристики сооружения, тип характеристики
        /// </summary>
        public string KeyParametersType { get; set; }

        /// <summary>
        /// Основные характеристики сооружения, значение
        /// </summary>
        public string KeyParametersValue { get; set; }

        /// <summary>
        /// Вид объекта недвижимости
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion
        public Construction(XmlReader reader)
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
                    switch (reader.Name)
                    {
                        case "ns4:KeyParameter":
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    switch (reader.Name)
                                    {
                                        case "Type":
                                            KeyParametersType = reader.Value;
                                            break;
                                        case "Value":
                                            KeyParametersValue = reader.Value;
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            break;
                        case "ObjectType":
                            {
                                ObjectType = reader.ReadElementContentAsString();
                            }
                            break;
                        //------------- Как извлекать -----------------
                        case "Address":
                            {

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
                                EntSys = Convert.ToString(reader.Value);
                                //Как лучше извлекать?
                                reader.MoveToContent();
                                reader.ReadToDescendant("ns3:Ordinate");
                                isCoordinates = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Uncompleted : Address
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
        /// Вид объекта недвижимости
        /// </summary>
        public string ObjectType { get; set; }

        /// <summary>
        /// Кадастровая стоимость
        /// </summary>
        public string CadastralCost { get; set; }
        #endregion

        public Uncompleted(XmlReader reader)
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
                    switch (reader.Name)
                    {
                        case "ObjectType":
                            {
                                ObjectType = reader.ReadElementContentAsString();
                            }
                            break;
                        //------------- Как извлекать -----------------
                        case "Address":
                            {

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
                                EntSys = Convert.ToString(reader.Value);
                                //Как лучше извлекать?
                                reader.MoveToContent();
                                reader.ReadToDescendant("ns3:Ordinate");
                                isCoordinates = true;
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
        /// Граница между субъектами Российской Федерации (наименование субъектов РФ)
        /// </summary>
        public string SubjectsBoundary { get; set; }

        /// <summary>
        /// Граница муниципального образования (наименование муниципального образования)
        /// </summary>
        public string MunicipalBoundary { get; set; }

        /// <summary>
        /// Граница населенного пункта (наименование населенного пункта)
        /// </summary>
        public string InhabitedLocalityBoundary { get; set; }

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
        #endregion
        public Bound(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
                    {
                        case "AccountNumber":
                            {
                                AccountNumber = reader.ReadElementContentAsString();
                            }
                            break;
                        case "SubjectsBoundary":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("NameNeighbours");
                                SubjectsBoundary = reader.ReadElementContentAsString();
                            }
                            break;
                        case "MunicipalBoundary":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("Name");
                                MunicipalBoundary = reader.ReadElementContentAsString();
                            }
                            break;
                        case "InhabitedLocalityBoundary":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("Name");
                                InhabitedLocalityBoundary = reader.ReadElementContentAsString();
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
                                EntSys = Convert.ToString(reader.Value);
                                //Как лучше извлекать?
                                reader.MoveToContent();
                                reader.ReadToDescendant("ns3:Ordinate");
                                isCoordinates = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    public class Zone
    {
        #region
        /// <summary>
        /// Учетный номер
        /// </summary>
        public string AccountNumber { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Зона с особыми условиями использования территорий
        /// </summary>
        public string SpecialZone { get; set; }

        /// <summary>
        /// Координаты
        /// </summary>
        public bool isCoordinates { get; set; }

        /// <summary>
        /// Система координат
        /// </summary>
        public string EntSys { get; set; }
        #endregion

        public Zone(XmlReader reader)
        {
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    switch (reader.Name)
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
                        case "SpecialZone":
                            {
                                reader.MoveToContent();
                                reader.ReadToDescendant("ContentRestrictions");
                                SpecialZone = reader.ReadElementContentAsString();
                            }
                            break;
                        case "EntitySpatial":
                            {
                                reader.MoveToAttribute("EntSys");
                                EntSys = Convert.ToString(reader.Value);
                                //Как лучше извлекать?
                                reader.MoveToContent();
                                reader.ReadToDescendant("ns3:Ordinate");
                                isCoordinates = true;
                            }
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
                    switch (reader.Name)
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

}
