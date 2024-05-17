namespace MyApp.Services.BooksService
{
    public interface IBooksService
    {
        string GetRandomBook();
        bool AddBook(string book);
        bool ReplaceBook(string oldBook, string newBook);
        bool RemoveBook(string book);
        HashSet<string> RetrieveBooks();
    }
}
