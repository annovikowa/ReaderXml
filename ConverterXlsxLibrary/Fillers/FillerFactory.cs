using ConverterXlsxLibrary.Fillers.ECPT;
using ConverterXlsxLibrary.Fillers.KPT;
using ConverterXlsxLibrary.Models;

namespace ConverterXlsxLibrary.Fillers
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
