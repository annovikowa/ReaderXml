using ConverterXlsx.DB;
using ConverterXlsx.DB.Models;
using ConverterXlsxLibrary.ExelSheets;
using ConverterXlsxLibrary;
using ConverterXlsxLibrary.Models;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConverterXlsx
{
    public class ConverterHelper
    {
        public void Convertions(string pathInput, string id)
        {
            Conversion convertions;
            using (var database = ConverterXlsxRepository.GetInstance())
            {
                convertions = database.GetConversion(id);
                convertions.Status = Status.InProcess;
                database.SaveChanges();
            }

            var reader = new CadastralPlanTerritoryReader();
            var KPT = reader.Read(pathInput);
            SaveErrors(KPT, convertions);

            if (KPT != null)
            {
                string pathOutput = $"D:\\{KPT.FileName}.xlsx"; //указать папку
                var exelFiller = new ExelFiller(KPT);
                exelFiller.XlWorkbook.SaveAs(pathOutput);

                using (var database = ConverterXlsxRepository.GetInstance())
                {
                    convertions = database.GetConversion(id); //надо искать по id,иначе нет связи этого экземпляра с бд
                    convertions.PathOutput = pathOutput;
                    database.SaveChanges();
                }
                return;
            }
        }

        private void SaveErrors(CadastralPlanTerritory KPT, Conversion conversion)
        {
            IEnumerable<string> allErrors = null;
            foreach (var cadastralBlock in KPT.CadastralBlocks)
            {
                allErrors = KPT.Errors.Union(cadastralBlock.Errors);
            }
            if (allErrors.Count() > 0)
            {
                using (var database = ConverterXlsxRepository.GetInstance())
                {
                    foreach (var errorDescriptions in allErrors)
                    {
                        database.SaveError(errorDescriptions, conversion);
                    }
                }
            }

        }
    }
}