namespace Libra.Services.RandomDataService
{
    public interface IRandomDataService
    {
        int GetIntegerValue(); // v1

        string GetTextValue(); // v2

        byte[] GenerateExcelFile(); //v3
    }
}
