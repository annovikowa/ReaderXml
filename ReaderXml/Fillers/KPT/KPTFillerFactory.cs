using ReaderXml.KPT;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.Fillers.KPT
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
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
