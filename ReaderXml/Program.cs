using ReaderXml.ExelSheets;

namespace ReaderXml
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"D:\ReaderXml\ReaderXml\КПТ\Примеры\KPT (v10)\kpt10_doc27877366.xml";
            var reader = new CadastralPlanTerritoryReader();
            var KPT = reader.Read(file);

            ExelFiller exelFiller = new ExelFiller(KPT);
            exelFiller.XlWorkbook.SaveAs("test.xlsx");
            System.Diagnostics.Process.Start("test.xlsx");            
        }
    }
}
