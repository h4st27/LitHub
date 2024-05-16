using ClosedXML.Excel;
using Libra.Services.RandomDataService;
using System;
using System.IO;
using System.Threading.Tasks;

public class RandomDataService : IRandomDataService
{
    //v1
    public int GetIntegerValue()
    {
        Random random = new Random();
        return random.Next();
    }
    //v2
    public string GetTextValue()
    {
        return "Some text";
    }

    //v3
    public byte[] GenerateExcelFile()
    {
        using (var ms = new MemoryStream())
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Sheet1");
                worksheet.Cell("A1").Value = "Yaroslav";
                worksheet.Cell("B1").Value = "Popov";
                workbook.SaveAs(ms);
                return ms.ToArray();
            }
        }
    }
}
