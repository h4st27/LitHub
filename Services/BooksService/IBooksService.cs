
namespace MyApp.Services.BooksService
{
    public interface IBooksService
    {
        bool AddBook(string book);
        string GetRandomBook();
        bool RemoveBook(string book);
        bool ReplaceBook(string oldBook, string newBook);
        List<string> RetrieveBooks();
        bool ValidateBook(string book);
    }
}