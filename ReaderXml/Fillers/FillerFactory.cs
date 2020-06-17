using ReaderXml.ECPT;
using ReaderXml.KPT;
using ReaderXml.Models;

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
