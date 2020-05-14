using ReaderXml.ECPT;
using ReaderXml.KPT;
using ReaderXml.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReaderXml.Fillers
{
    public static class FillerFactory
    {
        public static IFiller<CadastralPlanTerritory> GetFiller(string root)
        {
            switch (root)
            {
                case "KPT":
                    {
                        return new KPTFiller();
                    }
                case "extract_cadastral_plan_territory":
                    {
                        return new ECPTFiller();
                    } 
                default:
                    {
                        return null;
                    }
            }
        }
    }
}
