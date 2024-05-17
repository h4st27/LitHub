using MyApp.Models;

namespace MyApp.Services.LibraryService
{
    public interface ILibraryService
    {
        List<LibraryData> GetLibrary();
        bool AddDefinitionToBook(string book, LibraryData definition);
        bool RemoveDefinitionFromBook(string book);
        bool ChangeDefinitionOfBook(string book,string definition);
    }
}
