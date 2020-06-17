using ReaderXml.ExelSheets;

namespace ReaderXml
{
    class Program
    {
        static void Main(string[] args)
        {
            string file = @"D:\ReaderXml\ReaderXml\КПТ\Примеры\extract_cadastral_plan_territory\84_01_0010104_2017-05-31_kpt11.xml";
            var reader = new CadastralPlanTerritoryReader();
            var KPT = reader.Read(file);

            ExelFiller exelFiller = new ExelFiller(KPT);
            exelFiller.XlWorkbook.SaveAs("test.xlsx");
            System.Diagnostics.Process.Start("test.xlsx");            
        }
    }
}
