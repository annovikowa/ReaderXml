using ConverterXlsxLibrary.Models;

namespace ConverterXlsxLibrary.Fillers.KPT
{
    public static class KPTFillerFactory
    {
        public static IFiller<T> GetFiller<T>(T model) where T : CadastralObject
        {
            //аналогично добавить остальные филлеры. вообще такой подход не совсем правильный, зато удобный)
            switch (model)
            {
                case Parcel _:
                    {
                        return (IFiller<T>)new ParcelFiller();
                    }
                case Building _:
                    {
                        return (IFiller<T>)new BuildingFiller();
                    }
                case Construction _:
                    {
                        return (IFiller<T>)new ConstructionFiller();
                    }
                case Uncompleted _:
                    {
                        return (IFiller<T>)new UncompletedFiller();
                    }
                case Bound _:
                    {
                        return (IFiller<T>)new BoundFiller();
                    }
                case Zone _:
                    {
                        return (IFiller<T>)new ZoneFiller();
                    }
                case OMSPoint _:
                    {
                        return (IFiller<T>)new OMSPointFiller();
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
