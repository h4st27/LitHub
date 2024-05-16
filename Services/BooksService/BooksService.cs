using System.Net;
using System.Text.RegularExpressions;


namespace MyApp.Services.BooksService
{
    public class BooksService : IBooksService
    {
        string pattern = "^[a-zA-Z]+$";
        static private List<string> books = new List<string>
        {
            "beautiful", "happy", "exciting", "creative", "wonderful",
            "amazing", "fantastic", "awesome", "brilliant", "joyful",
            "lovely", "delightful", "fun", "inspiring", "peaceful"
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
