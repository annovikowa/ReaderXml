using ConverterXlsx.DB;
using ConverterXlsx.DB.Models;
using ConverterXlsxLibrary.ExelSheets;
using ConverterXlsxLibrary;
using ConverterXlsxLibrary.Models;
using System.Linq;
using System.Collections.Generic;
using System;
using System.IO;

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
                var number = KPT.CadastralBlocks.First().CadastralNumber.Replace(":", "_");
                DateTime dateConvertion = DateTime.Now;
                string pathOutput = $"~/{id}/{number}_{dateConvertion.ToString("dd-MM-yyyy")}_kpt{KPT.Version}"; //указать папку
                File.Move(pathInput, $"{pathOutput}.xml");

                var exelFiller = new ExelFiller(KPT);
                exelFiller.XlWorkbook.SaveAs($"{pathOutput}.xlsx");

                using (var database = ConverterXlsxRepository.GetInstance())
                {
                    convertions.PathInput = $"{pathOutput}.xlsx";
                    convertions.PathOutput = $"{pathOutput}.xml";
                    database.SaveChanges();
                }
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