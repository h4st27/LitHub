using System.Net;
using System.Text.RegularExpressions;


namespace MyApp.Services.BooksService
{
    public class BooksService : IBooksService
    {
        string pattern = "^[a-zA-Z]+$";
        static private List<string> books = new List<string>
        {
            "Harry Potter", "Lord of the Rings", "The Blade Itself", "Witcher", "It",
            "If We Were Villains", "A Little Life", "The Little Prince", "Looking for Alaska", "Atomic Habits",
            "Barbie", "Happy Place", "Be Happy", "Have a Good Time", "Everything I Know About Love"
        };
        public string GetRandomBook()
        {
            Random rand = new Random();
            int randomIndex = rand.Next(books.Count);
            return books[randomIndex];
        }
        public bool AddBook(string book)
        {
            if (!books.Contains(book.Trim().ToLower()))
            {
                books.Add(book.Trim().ToLower());
                return true;
            }
            return false;
        }
        public bool RemoveBook(string book)
        {
            return books.Remove(book.Trim().ToLower());
        }
        public bool ReplaceBook(string oldBook, string newBook)
        {
            int index = books.IndexOf(oldBook.Trim().ToLower());
            if (index != -1)
            {
                books[index] = newBook.Trim().ToLower();
                return true;
            }
            return false;
        }
        public bool ValidateBook(string book)
        {
            string trimmedBook = book.Trim().ToLower();
            if (Regex.IsMatch(trimmedBook, pattern) && trimmedBook.Length != 0)
            {
                return true;
            }
            return false;
        }
        public List<string> RetrieveBooks()
        {
            return books;
        }
    }
}
